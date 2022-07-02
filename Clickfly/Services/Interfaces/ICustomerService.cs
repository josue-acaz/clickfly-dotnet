using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;

namespace clickfly.Services
{
    public interface ICustomerService
    {
        Task<Customer> Save(Customer customer);
        Task<PaginationResult<Customer>> Pagination(PaginationFilter filter);
        Task<Customer> GetById(string id);
        Task<Authenticated> Authenticate(AuthenticateParams authenticateParams);
        Task ForgotPassword(string email);
        Task<bool> CheckResetPassword(string token);
        Task ResetPassword(ResetPasswordParams resetPasswordParams);
        Task ActivateAccount(string token);
        Task<string> Thumbnail(ThumbnailRequest thumbnailRequest);
        Task Delete(string id);
    }
}
