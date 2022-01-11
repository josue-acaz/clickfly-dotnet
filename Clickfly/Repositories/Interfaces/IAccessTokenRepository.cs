using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;

namespace clickfly.Repositories
{
    public interface IAccessTokenRepository
    {
        Task<AccessToken> Create(AccessToken accessToken);
        Task<AccessToken> GetByToken(string token);
        Task<bool> TokenIsValid(string token);
        Task Delete(string id);
    }
}
