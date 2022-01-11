using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.Repositories;
using clickfly.Helpers;
using Microsoft.Extensions.Options;
using clickfly.ViewModels;

namespace clickfly.Services
{
    public class AccountVerificationService : BaseService, IAccountVerificationService
    {
        private readonly IAccountVerificationRepository _accountVerificationRepository;

        public AccountVerificationService(
            IOptions<AppSettings> appSettings, 
            ISystemLogRepository systemLogRepository,
            IPermissionRepository permissionRepository,
            INotificator notificator, 
            IInformer informer,
            IUtils utils, 
            IAccountVerificationRepository accountVerificationRepository
        ) : base(
            appSettings,
            systemLogRepository,
            permissionRepository,
            notificator,
            informer,
            utils
        )
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
