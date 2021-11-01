using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;
using clickfly.Repositories;
using clickfly.Exceptions;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace clickfly.Services
{
    public class AirTaxiService : BaseService, IAirTaxiService
    {
        private readonly IAirTaxiRepository _airTaxiRepository;
        private readonly IAccessTokenRepository _accessTokenRepository;

        public AirTaxiService(IAirTaxiRepository airTaxiRepository, IOptions<AppSettings> appSettings, IUtils utils, IAccessTokenRepository accessTokenRepository) : base(appSettings, utils)
        {
            _airTaxiRepository = airTaxiRepository;
            _accessTokenRepository = accessTokenRepository;
        }

        public async Task<AirTaxi> GetByAccessToken(string token)
        {
            AccessToken accessToken = await _accessTokenRepository.GetByToken(token);

            if(accessToken == null)
            {
                throw new UnauthorizedException("Token de acesso inválido.");
            }

            string airTaxiId = accessToken.resource_id;

            List<string> fields = new List<string>();
            fields.Add("id"); // <-- Obrigatório informar o Id para o método GetById
            fields.Add("name");

            AirTaxi airTaxi = await _airTaxiRepository.GetById(airTaxiId, fields.ToArray());
            airTaxi.id = null; // <-- Remover Id antes de mandar para o cliente

            return airTaxi;
        }

        public Task<AirTaxi> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<AirTaxi> Save(AirTaxi airTaxi)
        {
            bool update = airTaxi.id != "";

            if(update)
            {
                
            }
            else
            {
                airTaxi = await _airTaxiRepository.Create(airTaxi);

                AccessToken accessToken = new AccessToken();
                accessToken.token = _utils.RandomBytes(30);
                accessToken.resource_id = airTaxi.id;

                accessToken = await _accessTokenRepository.Create(accessToken);
                
                string airTaxiName = _utils.RemoveWhiteSpaces(airTaxi.name);
                airTaxi.dashboard_url = $"{_appSettings.DashboardUrl}/login?ref={airTaxiName}&token={accessToken.token}";
            }

            return airTaxi;
        }
    }
}