using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;

namespace clickfly.Repositories
{
    public interface ISystemLogService
    {
        Task<SystemLog> Save(SystemLog systemLog);
    }
}
