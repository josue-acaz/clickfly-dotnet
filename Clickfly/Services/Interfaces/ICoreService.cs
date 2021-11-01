using System;
using CoordinateSharp;
using clickfly.ViewModels;
using System.Threading.Tasks;

namespace clickfly.Services
{
    public interface ICoreService
    {
        double GetDistanceBetweenCoordinates(Coordinate p1, Coordinate p2);
        Task<double> GetFlightTime(FlightTimeRequest flightTimeRequest);
        Task<Installment []> GetInstallments(InstallmentsRequest installmentsRequest);
        Task<float> GetFlightPrice(FlightPriceRequest flightPriceRequest);
    }
}