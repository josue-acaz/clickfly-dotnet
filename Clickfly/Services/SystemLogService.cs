using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.Repositories;
using Microsoft.Extensions.Options;
using clickfly.Helpers;

namespace clickfly.Services
{
    public class SystemLogService : BaseService, ISystemLogService
    {
        public SystemLogService(
            IOptions<AppSettings> appSettings, 
            ISystemLogRepository systemLogRepository,
            IPermissionRepository permissionRepository,
            INotificator notificator, 
            IInformer informer,
            IUtils utils
        ) : base(
            appSettings,
            systemLogRepository,
            permissionRepository,
            notificator,
            informer,
            utils
        )
        {

        }

        public async Task<SystemLog> Save(SystemLog systemLog)
        {
            bool update = systemLog.id != "";
            User user = _informer.GetValue<User>(UserTypes.User);

            if(update)
            {
                
            }
            else
            {
                systemLog.created_by = user.id;
                systemLog = await _systemLogRepository.Create(systemLog);
            }

            return systemLog;
        }
    }
}