using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.Repositories;
using clickfly.Helpers;
using Microsoft.Extensions.Options;
using clickfly.ViewModels;
using System.Collections.Generic;

namespace clickfly.Services
{
    public class DoubleCheckService : BaseService, IDoubleCheckService
    {
        private readonly IDoubleCheckRepository _doubleCheckRepository;

        public DoubleCheckService(
            IOptions<AppSettings> appSettings, 
            ISystemLogRepository systemLogRepository,
            IPermissionRepository permissionRepository,
            IDoubleCheckRepository doubleCheckRepository,
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
            _doubleCheckRepository = doubleCheckRepository;
        }

        public Task<IEnumerable<DoubleCheck>> Autocomplete(AutocompleteParams autocompleteParams)
        {
            throw new NotImplementedException();
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<DoubleCheck> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginationResult<DoubleCheck>> Pagination(PaginationFilter filter)
        {
            PaginationResult<DoubleCheck> paginationResult = await _doubleCheckRepository.Pagination(filter);
            return paginationResult;
        }

        public Task<DoubleCheck> Save(DoubleCheck doubleCheck)
        {
            throw new NotImplementedException();
        }

        public Task<DoubleCheck> SaveExternal(DoubleCheck doubleCheck)
        {
            throw new NotImplementedException();
        }
    }
}
