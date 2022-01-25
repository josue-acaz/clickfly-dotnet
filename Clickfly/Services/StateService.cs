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
    public class StateService : BaseService, IStateService
    {
        private readonly IStateRepository _stateRepository;

        public StateService(
            IOptions<AppSettings> appSettings, 
            ISystemLogRepository systemLogRepository,
            IPermissionRepository permissionRepository,
            INotificator notificator, 
            IInformer informer,
            IUtils utils,
            IStateRepository stateRepository
        ) : base(
            appSettings,
            systemLogRepository,
            permissionRepository,
            notificator,
            informer,
            utils
        )
        {
            _stateRepository = stateRepository;
        }

        public async Task Delete(string id)
        {
            await _stateRepository.Delete(id);
        }

        public async Task<State> GetById(string id)
        {
            State state = await _stateRepository.GetById(id);
            return state;
        }

        public async Task<State> GetByPrefix(string prefix)
        {
            State state = await _stateRepository.GetByPrefix(prefix);
            return state;
        }

        public async Task<PaginationResult<State>> Pagination(PaginationFilter filter)
        {
            PaginationResult<State> paginationResult = await _stateRepository.Pagination(filter);
            return paginationResult;
        }

        public async Task<IEnumerable<State>> Autocomplete(AutocompleteParams autocompleteParams)
        {
            PaginationFilter filter = new PaginationFilter();
            filter.page_size = 10;
            filter.page_number = 1;
            filter.order = "DESC";
            filter.order_by = "created_at";
            filter.text = autocompleteParams.text;

            PaginationResult<State> paginationResult = await _stateRepository.Pagination(filter);
            List<State> states = paginationResult.data;

            return states;
        }

        public async Task<State> Save(State state)
        {
            bool update = state.id != "";

            if(update)
            {
                state = await _stateRepository.Update(state);
            }
            else
            {
                state = await _stateRepository.Create(state);
            }

            return state;
        }
    }
}