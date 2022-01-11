using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.Repositories;
using clickfly.Helpers;
using Microsoft.Extensions.Options;
using clickfly.ViewModels;

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

        public Task Delete(string id)
        {
            throw new NotImplementedException();
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

        public Task<PaginationResult<State>> Pagination(PaginationFilter filter)
        {
            throw new NotImplementedException();
        }

        public async Task<State> Save(State state)
        {
            bool update = state.id != "";

            if(update)
            {
                
            }
            else
            {
                state = await _stateRepository.Create(state);
            }

            return state;
        }
    }
}