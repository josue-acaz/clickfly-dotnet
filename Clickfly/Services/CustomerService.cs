using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.Repositories;
using clickfly.ViewModels;
using clickfly.Exceptions;
using PagarmeCoreApi.Standard.Controllers;
using PagarmeCoreApi.Standard.Models;
using BCryptNet = BCrypt.Net.BCrypt;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;

namespace clickfly.Services
{
    public class CustomerService : BaseService, ICustomerService
    {

        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomersController _customersController;
        private readonly IFileRepository _fileRepository;
        private readonly IUploadService _uploadService;

        public CustomerService(IOptions<AppSettings> appSettings, IUtils utils, ICustomerRepository customerRepository, IFileRepository fileRepository, IUploadService uploadService, ICustomersController customersController) : base(appSettings, utils)
        {
            _customerRepository = customerRepository;
            _customersController = customersController;
            _fileRepository = fileRepository;
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

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task ForgotPassword(string email)
        {
            throw new NotImplementedException();
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

        public Task ResetPassword(ResetPasswordParams resetPasswordParams)
        {
            throw new NotImplementedException();
        }

        public async Task<Customer> Save(Customer customer)
        {
            bool update = customer.id != "";

            if(update)
            {
                customer = await _customerRepository.Update(customer);
            }
            else
            {
                Customer _customer = await _customerRepository.GetByEmail(customer.email);
                if(_customer != null)
                {
                    throw new ConflictException("Já existe uma conta associada a esse email.");
                }

                CreateCustomerRequest customerRequest = new CreateCustomerRequest();
                customerRequest.Code = customer.id;
                customerRequest.Name = customer.name;
                customerRequest.Email = customer.email;
                customerRequest.Type = customer.type;

                GetCustomerResponse customerResponse = await _customersController.CreateCustomerAsync(customerRequest);
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
                    new Claim(ClaimTypes.Name, customer.name.ToString()),
                    new Claim(ClaimTypes.Role, customer.role.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            return tokenHandler.WriteToken(token);
        }

        public async Task<string> Thumbnail(ThumbnailRequest thumbnailRequest)
        {
            IFormFile file = thumbnailRequest.file;
            string customerId = thumbnailRequest.customer_id;

            UploadResponse uploadResponse = await _uploadService.UploadFileAsync(file);

            File createFile = new File();
            createFile.resource_id = customerId;
            createFile.resource = Resources.Customers;
            createFile.key = uploadResponse.Key;
            createFile.name = uploadResponse.Name;
            createFile.size = uploadResponse.Size;
            createFile.url = uploadResponse.Url;
            createFile.mimetype = uploadResponse.MimeType;
            createFile.field_name = FieldNames.Thumbnail;

            createFile = await _fileRepository.Create(createFile);
            string url = createFile.url;

            return url;
        }
    }
}
