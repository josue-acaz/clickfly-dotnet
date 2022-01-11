using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.Repositories;
using Microsoft.Extensions.Options;
using clickfly.Helpers;
using clickfly.ViewModels;

namespace clickfly.Services
{
    public class TimezoneService : BaseService, ITimezoneService
    {
        private readonly ITimezoneRepository _timezoneRepository;

        public TimezoneService(
            IOptions<AppSettings> appSettings, 
            ISystemLogRepository systemLogRepository,
            IPermissionRepository permissionRepository,
            INotificator notificator, 
            IInformer informer,
            IUtils utils,
            ITimezoneRepository timezoneRepository
        ) : base(
            appSettings,
            systemLogRepository,
            permissionRepository,
            notificator,
            informer,
            utils
        )
        {
            _timezoneRepository = timezoneRepository;
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Timezone> GetByGmt(int gmt)
        {
            Timezone timezone = await _timezoneRepository.GetByGmt(gmt);
            return timezone;
        }

        public async Task<Timezone> Save(Timezone timezone)
        {
            bool update = timezone.id != "";

            if(update)
            {
                
            }
            else
            {
                timezone = await _timezoneRepository.Create(timezone);
            }

            return timezone;
        }
    }
}