using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;

namespace clickfly.Repositories
{
    public interface ISystemLogRepository
    {
        Task<SystemLog> Create(SystemLog systemLog);
    }
}
