using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;
using clickfly.Repositories;

namespace clickfly.Services
{
    public class TimezoneService : ITimezoneService
    {
        private readonly ITimezoneRepository _timezoneRepository;

        public TimezoneService(ITimezoneRepository timezoneRepository)
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