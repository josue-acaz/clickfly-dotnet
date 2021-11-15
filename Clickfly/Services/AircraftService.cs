using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;
using clickfly.Repositories;
using clickfly.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace clickfly.Services
{
    public class AircraftService : IAircraftService
    {
        private readonly IAircraftRepository _aircraftRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IUploadService _uploadService;

        public AircraftService(IAircraftRepository aircraftRepository, IFileRepository fileRepository, IUploadService uploadService)
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
            PaginationFilter filter = new PaginationFilter();
            filter.page_size = 10;
            filter.page_number = 1;
            filter.order = "DESC";
            filter.order_by = "created_at";
            filter.text = autocompleteParams.text;
            filter.air_taxi_id = autocompleteParams.air_taxi_id;

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
                aircraft = await _aircraftRepository.Create(aircraft);
            }

            return aircraft;
        }

        public async Task<string> Thumbnail(ThumbnailRequest thumbnailRequest)
        {
            string type = thumbnailRequest.type;
            IFormFile file = thumbnailRequest.file;
            string aircraftId = thumbnailRequest.aircraft_id;

            UploadResponse uploadResponse = await _uploadService.UploadFileAsync(file);

            File createFile = new File();
            createFile.resource_id = aircraftId;
            createFile.resource = Resources.Aircrafts;
            createFile.key = uploadResponse.Key;
            createFile.name = uploadResponse.Name;
            createFile.size = uploadResponse.Size;
            createFile.url = uploadResponse.Url;
            createFile.mimetype = uploadResponse.MimeType;
            createFile.field_name = type;

            createFile = await _fileRepository.Create(createFile);
            string url = createFile.url;

            return url;
        }
    }
}
