using System;
using System.Threading.Tasks;
using clickfly.ViewModels;
using clickfly.Models;

namespace clickfly.Services
{
    public interface IAirTaxiService
    {
        Task<AirTaxi> Save(AirTaxi airTaxi);
        Task<AirTaxi> GetById(string id);
        Task<AirTaxi> GetByAccessToken(string token);
    }
}