using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.Repositories;
using Microsoft.Extensions.Options;
using clickfly.Helpers;
using clickfly.ViewModels;
using System.Collections.Generic;

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

        public async Task Delete(string id)
        {
            await _timezoneRepository.Delete(id);
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

        public async Task<IEnumerable<Timezone>> Autocomplete(AutocompleteParams autocompleteParams)
        {
            PaginationFilter filter = new PaginationFilter();
            filter.page_size = 10;
            filter.page_number = 1;
            filter.order = "DESC";
            filter.order_by = "created_at";
            filter.text = autocompleteParams.text;

            PaginationResult<Timezone> paginationResult = await _timezoneRepository.Pagination(filter);
            List<Timezone> timezones = paginationResult.data;

            return timezones;
        }
    }
}