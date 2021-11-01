using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;
using clickfly.Repositories;

namespace clickfly.Services
{
    public class AccountVerificationService : IAccountVerificationService
    {
        private readonly IAccountVerificationRepository _accountVerificationRepository;

        public AccountVerificationService(IAccountVerificationRepository accountVerificationRepository)
        {
            _accountVerificationRepository = accountVerificationRepository;
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<AccountVerification> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationResult<AccountVerification>> Pagination(PaginationFilter filter)
        {
            throw new NotImplementedException();
        }

        public async Task<AccountVerification> Save(AccountVerification accountVerification)
        {
            bool update = accountVerification.id != "";

            if(update)
            {
                
            }
            else
            {
                accountVerification = await _accountVerificationRepository.Create(accountVerification);
            }

            return accountVerification;
        }
    }
}
