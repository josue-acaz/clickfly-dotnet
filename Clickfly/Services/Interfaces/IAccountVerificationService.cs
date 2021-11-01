using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.ViewModels;
using clickfly.Models;

namespace clickfly.Services
{
    public interface IAccountVerificationService
    {
        Task<AccountVerification> Save(AccountVerification accountVerification);
        Task<AccountVerification> GetById(string id);
        Task Delete(string id);
        Task<PaginationResult<AccountVerification>> Pagination(PaginationFilter filter);
    }
}
