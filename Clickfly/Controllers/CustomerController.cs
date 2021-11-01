using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using clickfly.Data;
using clickfly.Models;
using clickfly.Services;
using clickfly.ViewModels;

namespace clickfly.Controllers
{
    [Route("/customers")]
    public class CustomerController : BaseController
    {
        private readonly IDataContext _dataContext;
        private readonly ICustomerService _customerService;
        private readonly IAccountVerificationService _accountVerificationService;
        private readonly IEmailService _emailService;
        private readonly IUtils _utils;

        public CustomerController(IDataContext dataContext, ICustomerService customerService, IAccountVerificationService accountVerificationService, IEmailService emailService, IUtils utils)
        {
            _dataContext  = dataContext;
            _customerService = customerService;
            _accountVerificationService = accountVerificationService;
            _emailService = emailService;
            _utils = utils;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Save([FromBody]Customer customer)
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

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Aircraft>> GetById(string id)
        {
            Customer customer = await _customerService.GetById(id);
            return HttpResponse(customer);
        }
        
        [HttpPost]
        [Route("authenticate")]
        [AllowAnonymous]
        public async Task<ActionResult<Authenticated>> Authenticate([FromBody]AuthenticateParams authenticateParams)
        {
            Authenticated authenticated = await _customerService.Authenticate(authenticateParams);
            return authenticated;
        }

        [HttpPost("thumbnail")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Thumbnail([FromForm]ThumbnailRequest thumbnailRequest)
        {
            using var transaction = _dataContext.Database.BeginTransaction();

            string url = await _customerService.Thumbnail(thumbnailRequest);
            await transaction.CommitAsync();

            return HttpResponse(url);
        }
    }
}
