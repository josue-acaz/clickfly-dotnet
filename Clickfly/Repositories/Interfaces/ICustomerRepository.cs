using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;

namespace clickfly.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer> Create(Customer customer);
        Task<Customer> GetById(string id);
        Task<Customer> GetByEmail(string email);
        Task<Customer> GetByPasswordResetToken(string password_reset_token);
        Task<bool> PasswordResetTokenIsValid(string password_reset_token);
        Task MarkAsVerified(string id);
        Task<Customer> Update(Customer customer);
        Task Delete(string id);
        Task<PaginationResult<Customer>> Pagination(PaginationFilter filter);
    }
}
