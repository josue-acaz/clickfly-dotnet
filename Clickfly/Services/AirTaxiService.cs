using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using clickfly.Models;
using clickfly.Repositories;
using clickfly.Exceptions;
using clickfly.Helpers;
using clickfly.ViewModels;

namespace clickfly.Services
{
    public class AirTaxiService : BaseService, IAirTaxiService
    {
        private readonly IAirTaxiRepository _airTaxiRepository;
        private readonly IAccessTokenRepository _accessTokenRepository;

        public AirTaxiService(
            IAirTaxiRepository airTaxiRepository, 
            IOptions<AppSettings> appSettings, 
            ISystemLogRepository systemLogRepository,
            IPermissionRepository permissionRepository,
            INotificator notificator, 
            IInformer informer,
            IUtils utils, 
            IAccessTokenRepository accessTokenRepository
        ) : base(
            appSettings,
            systemLogRepository,
            permissionRepository, 
            notificator, 
            informer,
            utils
        )
        {
            _airTaxiRepository = airTaxiRepository;
            _accessTokenRepository = accessTokenRepository;
        }

        public async Task Delete(string id)
        {
            await _airTaxiRepository.Delete(id);
        }

        public async Task<AirTaxi> GetByAccessToken(string token)
        {
            AccessToken accessToken = await _accessTokenRepository.GetByToken(token);

            if(accessToken == null)
            {
                throw new UnauthorizedException("Token de acesso inválido.");
            }

            string airTaxiId = accessToken.resource_id;

            AirTaxi airTaxi = await _airTaxiRepository.GetById(airTaxiId);
            airTaxi.id = null; // <-- Remover Id antes de mandar para o cliente

            return airTaxi;
        }

        public async Task<AirTaxi> GetById(string id)
        {
            AirTaxi airTaxi = await _airTaxiRepository.GetById(id);
            return airTaxi;
        }

        public async Task<PaginationResult<AirTaxi>> Pagination(PaginationFilter filter)
        {
            PaginationResult<AirTaxi> paginationResult = await _airTaxiRepository.Pagination(filter);
            return paginationResult;
        }

        public async Task<AirTaxi> Save(AirTaxi airTaxi)
        {
            bool update = airTaxi.id != "";

            if(update)
            {
                airTaxi = await _airTaxiRepository.Update(airTaxi);
            }
            else
            {
                airTaxi = await _airTaxiRepository.Create(airTaxi);

                AccessToken accessToken = new AccessToken();
                accessToken.token = _utils.RandomBytes(30);
                accessToken.resource_id = airTaxi.id;

                accessToken = await _accessTokenRepository.Create(accessToken);
                airTaxi.dashboard_url = $"{_appSettings.DashboardUrl}/login?token={accessToken.token}";
            }

            return airTaxi;
        }
    }
}