using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.Repositories;
using clickfly.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using clickfly.Helpers;
using Microsoft.Extensions.Options;
using clickfly.ViewModels;

namespace clickfly.Services
{
    public class AircraftService : BaseService, IAircraftService
    {
        private readonly IAircraftRepository _aircraftRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IUploadService _uploadService;

        public AircraftService(
            IOptions<AppSettings> appSettings, 
            ISystemLogRepository systemLogRepository,
            IPermissionRepository permissionRepository,
            INotificator notificator, 
            IInformer informer,
            IUtils utils,
            IAircraftRepository aircraftRepository, 
            IFileRepository fileRepository, 
            IUploadService uploadService
        ) : base(
            appSettings,
            systemLogRepository,
            permissionRepository,
            notificator,
            informer,
            utils
        )
        {
            _aircraftRepository = aircraftRepository;
            _fileRepository = fileRepository;
            _uploadService = uploadService;
        }

        public async Task<Aircraft> GetById(string id)
        {
            Aircraft aircraft = await _aircraftRepository.GetById(id);
            return aircraft;
        }

        public async Task<string> GetThumbnail(GetThumbnailRequest thumbnailRequest)
        {
            string type = thumbnailRequest.type;

            if(
                type != AircraftThumbnailTypes.Thumbnail &&
                type != AircraftThumbnailTypes.SeatingMap
            )
            {
                throw new BadRequestException("Some arguments are invalid.");
            }

            string thumbnail = await _aircraftRepository.GetThumbnail(thumbnailRequest);
            return thumbnail;
        }

        public async Task<IEnumerable<Aircraft>> Autocomplete(AutocompleteParams autocompleteParams)
        {
            User user = _informer.GetValue<User>(UserTypes.User);

            PaginationFilter filter = new PaginationFilter();
            filter.page_size = 10;
            filter.page_number = 1;
            filter.order = "DESC";
            filter.order_by = "created_at";
            filter.text = autocompleteParams.text;
            filter.air_taxi_id = user.air_taxi_id;

            PaginationResult<Aircraft> paginationResult = await _aircraftRepository.Pagination(filter);
            List<Aircraft> aircrafts = paginationResult.data;

            return aircrafts;
        }

        public async Task<PaginationResult<Aircraft>> Pagination(PaginationFilter filter)
        {
            PaginationResult<Aircraft> paginationResult = await _aircraftRepository.Pagination(filter);
            return paginationResult;
        }

        public async Task<Aircraft> Save(Aircraft aircraft)
        {
            bool update = aircraft.id != "";

            if(update)
            {
                aircraft = await _aircraftRepository.Update(aircraft);
            }
            else
            {
                User user = _informer.GetValue<User>(UserTypes.User); 
                aircraft.air_taxi_id = user.air_taxi_id;

                aircraft = await _aircraftRepository.Create(aircraft);
            }

            return aircraft;
        }

        public async Task<string> Thumbnail(ThumbnailRequest thumbnailRequest)
        {
            IFormFile file = thumbnailRequest.file;
            string aircraft_id = thumbnailRequest.aircraft_id;
            string user_id = _informer.GetValue(UserIdTypes.UserId);

            string keyName = _utils.RandomBytes(20);
            await _uploadService.UploadFileAsync(file, keyName);

            File createFile = new File();
            createFile.resource_id = aircraft_id;
            createFile.resource = Resources.Aircrafts;
            createFile.key = keyName;
            createFile.name = file.Name;
            createFile.size = file.Length;
            createFile.mimetype = file.ContentType;
            createFile.field_name = thumbnailRequest.type;
            createFile.created_by = user_id;

            await _fileRepository.Create(createFile);
            return _uploadService.GetPreSignedUrl(keyName);
        }

        public async Task Delete(string id)
        {
            await _aircraftRepository.Delete(id);
        }
    }
}
