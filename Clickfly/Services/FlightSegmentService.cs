using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.Repositories;
using Microsoft.Extensions.Options;
using clickfly.Helpers;
using OneSignal.RestAPIv3.Client.Resources.Notifications;
using OneSignal.RestAPIv3.Client.Resources;
using System.Collections.Generic;
using clickfly.ViewModels;

namespace clickfly.Services
{
    public class FlightSegmentService : BaseService, IFlightSegmentService
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IFlightSegmentRepository _flightSegmentRepository;

        public FlightSegmentService(
            IOptions<AppSettings> appSettings, 
            ISystemLogRepository systemLogRepository,
            IPermissionRepository permissionRepository,
            INotificator notificator,
            IInformer informer,
            IUtils utils, 
            IFlightRepository flightRepository, 
            IFlightSegmentRepository flightSegmentRepository
        ) : base(appSettings, systemLogRepository, permissionRepository, notificator, informer, utils)
        {
            _flightRepository = flightRepository;
            _flightSegmentRepository = flightSegmentRepository;
        }

        public async Task Delete(string id)
        {
            await _flightRepository.Delete(id);
        }

        public async Task<FlightSegment> GetById(string id)
        {
            FlightSegment flightSegment = await _flightSegmentRepository.GetById(id);
            return flightSegment;
        }

        public async Task<PaginationResult<FlightSegment>> Pagination(PaginationFilter filter)
        {
            PaginationResult<FlightSegment> paginationResult = await _flightSegmentRepository.Pagination(filter);
            return paginationResult;
        }

        public async Task<FlightSegment> Save(FlightSegment flightSegment)
        {
            bool update = flightSegment.id != "";
            User user = _informer.GetValue<User>(UserTypes.User);

            if(update)
            {
                flightSegment.updated_by = user.id;
                flightSegment = await _flightSegmentRepository.Update(flightSegment);
            }
            else
            {
                string flightId = flightSegment.flight_id;
                FlightSegment lastSegment = await _flightRepository.GetLastSegment(flightId);
                
                int number = 1;
                if(lastSegment != null)
                {
                    number += lastSegment.number;
                }
                
                flightSegment.number = number;
                flightSegment.created_by = user.id;
                flightSegment = await _flightSegmentRepository.Create(flightSegment);
            }

            return flightSegment;
        }

        public async Task SendNotification(string id)
        {
            FlightSegment flightSegment = await _flightSegmentRepository.GetById(id);

            string originCityName = flightSegment.origin_aerodrome.city.full_name;
            string destinationCityName = flightSegment.destination_aerodrome.city.full_name;
            string aircraftName = flightSegment.aircraft.model.name;
            string aircraftThumbnail = flightSegment.aircraft.thumbnail;

            NotificationCreateOptions notificationCreateOptions = new NotificationCreateOptions();
            
            List<string> includedSegments = new List<string>();
            includedSegments.Add("All");

            notificationCreateOptions.AppId = _appSettings.OneSignalAppId;
            notificationCreateOptions.Headings.Add(LanguageCodes.English, $"{originCityName} ✈️ {destinationCityName}");
            notificationCreateOptions.Contents.Add(LanguageCodes.English, $"{flightSegment.departure_datetime}, {flightSegment.price_per_seat} por pessoa no {aircraftName}, {flightSegment.total_seats} pass.");
            notificationCreateOptions.BigPictureForAndroid = aircraftThumbnail;
            notificationCreateOptions.IncludedSegments = includedSegments;
        
            await _oneSignalClient.Notifications.CreateAsync(notificationCreateOptions);
        }
    }
}