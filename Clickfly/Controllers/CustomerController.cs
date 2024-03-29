using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using clickfly.Data;
using clickfly.Models;
using clickfly.Helpers;
using clickfly.Services;
using clickfly.ViewModels;

namespace clickfly.Controllers
{
    [Authorize]
    [Route("/customers")]
    public class CustomerController : BaseController
    {
        private readonly ICustomerService _customerService;
        private readonly IAccountVerificationService _accountVerificationService;
        private readonly IEmailService _emailService;
        private readonly IUtils _utils;

        public CustomerController(
            IDataContext dataContext, 
            INotificator notificator,
            IInformer informer, 
            ICustomerService customerService, 
            IAccountVerificationService accountVerificationService, 
            IEmailService emailService, 
            IUtils utils
        ) : base(
            dataContext, 
            notificator,
            informer
        )
        {
            _customerService = customerService;
            _accountVerificationService = accountVerificationService;
            _emailService = emailService;
            _utils = utils;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Save([FromBody]Customer customer)
        {
            try
            {
                using var transaction = _dataContext.Database.BeginTransaction();

                string customerType = customer.type;
                if(customerType == CustomerTypes.Company)
                {
                    customer.document_type = DocumentTypes.CNPJ;
                }

                Customer _customer = await _customerService.Save(customer);

                string token = _utils.RandomBytes(30);

                AccountVerification accountVerification = new AccountVerification();
                accountVerification.token = token;
                accountVerification.expires = DateTime.Now;
                accountVerification.customer_id = _customer.id;

                accountVerification = await _accountVerificationService.Save(accountVerification);
                await transaction.CommitAsync();

                // Enviar email
                ActivateAccount activateAccount = new ActivateAccount();
                activateAccount.customer_name = _customer.name;
                activateAccount.activation_link = $"https://clickfly.app/activate-account?token={token}";

                EmailRequest emailRequest = new EmailRequest();
                emailRequest.from = "noreply@clickfly.app";
                emailRequest.fromName = "ClickFly";
                emailRequest.subject = "Ativação de conta";
                emailRequest.to = _customer.email;
                emailRequest.templateName = "ActivateAccount";
                emailRequest.modelType = typeof(ActivateAccount);
                emailRequest.model = activateAccount;

                //_emailService.SendEmail(emailRequest);

                return HttpResponse(_customer);
            }
            catch (Exception ex)
            {
                Notify(ex.ToString());
                return HttpResponse();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Aircraft>> GetById(string id)
        {
            try
            {
                Customer customer = await _customerService.GetById(id);
                return HttpResponse(customer);
            }
            catch (Exception ex)
            {
                Notify(ex.ToString());
                return HttpResponse();
            }
        }
        
        [HttpPost]
        [Route("authenticate")]
        [AllowAnonymous]
        public async Task<ActionResult<Authenticated>> Authenticate([FromBody]AuthenticateParams authenticateParams)
        {
            try
            {
                Authenticated authenticated = await _customerService.Authenticate(authenticateParams);
                return authenticated;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Notify(ex.ToString());
                return HttpResponse();
            }
        }

        [HttpPost("thumbnail")]
        public async Task<ActionResult<string>> Thumbnail([FromForm]ThumbnailRequest thumbnailRequest)
        {
            try
            {
                GetSessionInfo(Request.Headers["Authorization"], UserTypes.Customer);
                using var transaction = _dataContext.Database.BeginTransaction();

                string url = await _customerService.Thumbnail(thumbnailRequest);
                await transaction.CommitAsync();

                return HttpResponse(url);
            }
            catch (Exception ex)
            {
                Notify(ex.ToString());
                return HttpResponse();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                await _customerService.Delete(id);
                return HttpResponse();
            }
            catch (Exception ex)
            {
                Notify(ex.ToString());
                return HttpResponse();
            }
        }

        [HttpPost("forgot-password/{email}")]
        public async Task<ActionResult> ForgotPassword(string email)
        {
            try
            {
                await _customerService.ForgotPassword(email);
                return HttpResponse();
            }
            catch(Exception ex)
            {
                Notify(ex.ToString());
                return HttpResponse();
            }
        }

        [HttpGet("check-reset-password/{token}")]
        public async Task<ActionResult> CheckResetPassword(string token)
        {
            try
            {
                bool isValid = await _customerService.CheckResetPassword(token);
                return HttpResponse(isValid);
            }
            catch(Exception ex)
            {
                Notify(ex.ToString());
                return HttpResponse();
            }
        }

        [HttpPut("reset-password")]
        public async Task<ActionResult> ResetPassword([FromBody]ResetPasswordParams resetPasswordParams)
        {
            try
            {
                await _customerService.ResetPassword(resetPasswordParams);
                return HttpResponse();
            }
            catch(Exception ex)
            {
                Notify(ex.ToString());
                return HttpResponse();
            }
        }
    }
}
