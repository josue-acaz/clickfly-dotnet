using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.Repositories;
using clickfly.Exceptions;
using clickfly.Helpers;
using PagarmeCoreApi.Standard.Controllers;
using PagarmeCoreApi.Standard.Models;
using BCryptNet = BCrypt.Net.BCrypt;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using clickfly.ViewModels;
using PagarmeCoreApi.Standard.Exceptions;
using System.Linq;

namespace clickfly.Services
{
    public class CustomerService : BaseService, ICustomerService
    {

        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomersController _customersController;
        private readonly IFileRepository _fileRepository;
        private readonly IUploadService _uploadService;
        private readonly IEmailService _emailService;

        public CustomerService(
            IOptions<AppSettings> appSettings, 
            ISystemLogRepository systemLogRepository,
            IPermissionRepository permissionRepository,
            IEmailService emailService,
            INotificator notificator, 
            IInformer informer,
            IUtils utils, 
            ICustomerRepository customerRepository, 
            IFileRepository fileRepository, 
            IUploadService uploadService, 
            ICustomersController customersController
        ) : base(appSettings, systemLogRepository, permissionRepository, notificator, informer, utils)
        {
            _customerRepository = customerRepository;
            _customersController = customersController;
            _fileRepository = fileRepository;
            _emailService = emailService;
            _uploadService = uploadService;
        }

        public Task ActivateAccount(string token)
        {
            throw new NotImplementedException();
        }

        public async Task<Authenticated> Authenticate(AuthenticateParams authenticateParams)
        {
            string username = authenticateParams.username;
            string password = authenticateParams.password;

            Customer customer = await _customerRepository.GetByEmail(username);

            if(customer == null)
            {
                throw new NotFoundException("Usuário ou senha inválidos.");
            }

            Console.WriteLine($"password: {password}");
            Console.WriteLine($"password_hash: {customer.password_hash}");
            bool passwordIsValid = BCryptNet.Verify(password, customer.password_hash);
            if(!passwordIsValid)
            {
                throw new NotFoundException("Usuário ou senha inválidos.");
            }

            string token = GenerateToken(customer);
            Authenticated authenticated = new Authenticated();

            authenticated.token = token;
            authenticated.auth_user = customer;

            return authenticated;
        }

        public async Task Delete(string id)
        {
            await _customerRepository.Delete(id);
        }

        public async Task ForgotPassword(string email)
        {
            Customer customer = await _customerRepository.GetByEmail(email);

            if(customer != null)
            {
                DateTime password_reset_expires = DateTime.Now;
                password_reset_expires.AddHours(5);

                customer.password_reset_expires = password_reset_expires;
                customer.password_reset_token = _utils.RandomHexString();

                await _customerRepository.Update(customer);

                // Enviar email
                ForgotPasswordParams forgotPasswordParams = new ForgotPasswordParams();
                forgotPasswordParams.customer_name = customer.name;
                forgotPasswordParams.reset_password_url = $"https://clickfly.app/reset-password?token={customer.password_reset_token}";

                EmailRequest emailRequest = new EmailRequest();
                emailRequest.subject = "Alteração de senha";
                emailRequest.to = customer.email;
                emailRequest.from = "noreply@clickfly.app";
                emailRequest.fromName = "Clickfly";
                emailRequest.templateName = "ForgotPassword";
                emailRequest.model = forgotPasswordParams;
                emailRequest.modelType = typeof(ForgotPasswordParams);

                _emailService.SendEmail(emailRequest);
            }
        }

        public async Task<Customer> GetById(string id)
        {
            Customer customer = await _customerRepository.GetById(id);
            return customer;
        }

        public Task<PaginationResult<Customer>> Pagination(PaginationFilter filter)
        {
            throw new NotImplementedException();
        }

        public async Task ResetPassword(ResetPasswordParams resetPasswordParams)
        {
            bool isValid = await _customerRepository.PasswordResetTokenIsValid(resetPasswordParams.token);
            if(!isValid)
            {
                Notify("Token é inválido ou expirou.");
                return;
            }

            if(resetPasswordParams.password != resetPasswordParams.confirm_password)
            {
                Notify("Senhas não conferem");
                return;
            }

            if(!_utils.IsStrongPassword(resetPasswordParams.password))
            {
                Notify("A senha deve ter tamanho mínimo de 6 e não pode ser uma sequência de letras ou números.");
                return;
            }

            Customer customer = await _customerRepository.GetByPasswordResetToken(resetPasswordParams.token);
            customer.password = resetPasswordParams.password;

            await _customerRepository.UpdatePassword(customer);
        }

        public async Task<Customer> Save(Customer customer)
        {
            bool update = customer.id != "";

            if(update)
            {
                customer = await _customerRepository.Update(customer);

                UpdateCustomerRequest updateCustomerRequest = new UpdateCustomerRequest();
                updateCustomerRequest.Name = customer.name;
                updateCustomerRequest.Email = customer.email;
                updateCustomerRequest.Code = customer.id;
                updateCustomerRequest.Document = customer.document;
                updateCustomerRequest.DocumentType = customer.document_type;
                updateCustomerRequest.Type = customer.type;

                if(customer.phone_number.Length == 13 || customer.phone_number.Length == 14)
                {
                    string[] phone_number = _utils.GetPhoneNumber(customer.phone_number);
                    CreatePhonesRequest phonesRequest = new CreatePhonesRequest();
                    CreatePhoneRequest phoneRequest = new CreatePhoneRequest();
                    phoneRequest.CountryCode = "55";
                    phoneRequest.AreaCode = phone_number[0];
                    phoneRequest.Number = phone_number[1];
                    phonesRequest.MobilePhone = phoneRequest;
                    updateCustomerRequest.Phones = phonesRequest;   
                }

                try
                {
                    GetCustomerResponse customerResponse = await _customersController.UpdateCustomerAsync(customer.customer_id, updateCustomerRequest);
                }
                catch(ErrorException ex)
                {
                    Console.WriteLine(ex.Errors);
                }
            }
            else
            {
                Customer _customer = await _customerRepository.GetByEmail(customer.email);
                if(_customer != null)
                {
                    throw new ConflictException("Já existe uma conta associada a esse email.");
                }

                string customerId = Guid.NewGuid().ToString();
                CreateCustomerRequest customerRequest = new CreateCustomerRequest();
                customerRequest.Code = customerId;
                customerRequest.Name = customer.name;
                customerRequest.Email = customer.email;
                customerRequest.Type = customer.type;

                if(customer.type == CustomerTypes.Individual)
                {
                    customer.document_type = "CPF";
                }
                else
                {
                    customer.document_type = "CNPJ";
                    customer.document = customer.document;
                    customerRequest.Document = customer.document;
                }

                if(customer.password != customer.confirm_password)
                {
                    Notify("Senhas não conferem");
                    return null;
                }

                if(!_utils.IsStrongPassword(customer.password))
                {
                    Notify("A senha deve ter tamanho mínimo de 6 e não pode ser uma sequência de letras ou números.");
                    return null;
                }

                customerRequest.DocumentType = customer.document_type;
                customer.password_hash = BCryptNet.HashPassword(customer.password, 12);

                GetCustomerResponse customerResponse = await _customersController.CreateCustomerAsync(customerRequest);
                
                customer.id = customerId;
                customer.customer_id = customerResponse.Id;
                customer = await _customerRepository.Create(customer);
            }

            return customer;
        }

        private string GenerateToken(Customer customer)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(UserIdTypes.CustomerId, customer.id),
                    new Claim(ClaimTypes.Name, customer.name.ToString()),
                    new Claim(ClaimTypes.Role, "customer"),
                    new Claim(UserTypes.Customer, JsonConvert.SerializeObject(customer))
                }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var encodedToken = tokenHandler.CreateToken(tokenDescriptor);
            
            return tokenHandler.WriteToken(encodedToken);
        }

        public async Task<string> Thumbnail(ThumbnailRequest thumbnailRequest)
        {
            string customer_id = _informer.GetValue(UserIdTypes.CustomerId);
            IFormFile file = thumbnailRequest.file;

            string keyName = _utils.RandomBytes(20);
            await _uploadService.UploadFileAsync(file, keyName);

            File createFile = new File();
            createFile.resource_id = customer_id;
            createFile.resource = Resources.Customers;
            createFile.key = keyName;
            createFile.name = file.Name;
            createFile.size = file.Length;
            createFile.mimetype = file.ContentType;
            createFile.field_name = FieldNames.Thumbnail;
            createFile.created_by = customer_id;

            await _fileRepository.Create(createFile);
            return _uploadService.GetPreSignedUrl(keyName);
        }

        public async Task<bool> CheckResetPassword(string token)
        {
            bool isValid = await _customerRepository.PasswordResetTokenIsValid(token);
            return isValid;
        }
    }
}
