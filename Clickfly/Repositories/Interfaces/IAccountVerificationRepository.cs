using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.ViewModels;
using clickfly.Models;

namespace clickfly.Repositories
{
    public interface IAccountVerificationRepository
    {
        Task<AccountVerification> Create(AccountVerification accountVerification);
        Task<AccountVerification> GetByToken(string token);
        Task<bool> TokenIsValid(string token);
        Task Delete(string id);
    }
}
