using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;
using clickfly.Repositories;
using Microsoft.AspNetCore.Http;

namespace clickfly.Services
{
    public class AircraftImageService : IAircraftImageService
    {
        private readonly IAircraftImageRepository _aircraftImageRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IUploadService _uploadService;

        public AircraftImageService(IAircraftImageRepository aircraftImageRepository, IFileRepository fileRepository, IUploadService uploadService)
        {
            _aircraftImageRepository = aircraftImageRepository;
            _fileRepository = fileRepository;
            _uploadService = uploadService;
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginationResult<AircraftImage>> Pagination(AircraftImagePaginationFilter filter)
        {
            PaginationResult<AircraftImage> paginationResult = await _aircraftImageRepository.Pagination(filter);
            return paginationResult;
        }

        public async Task<AircraftImage> Save(AircraftImage aircraftImage)
        {
            IFormFile file = aircraftImage.file;
            UploadResponse uploadResponse = await _uploadService.UploadFileAsync(file);

            aircraftImage = await _aircraftImageRepository.Create(aircraftImage);

            File createFile = new File();
            createFile.resource_id = aircraftImage.id;
            createFile.resource = Resources.AircraftImages;
            createFile.key = uploadResponse.Key;
            createFile.name = uploadResponse.Name;
            createFile.size = uploadResponse.Size;
            createFile.url = uploadResponse.Url;
            createFile.mimetype = uploadResponse.MimeType;
            createFile.field_name = "aircraft_image";

            createFile = await _fileRepository.Create(createFile);
            aircraftImage.url = createFile.url;
            aircraftImage.file = null;
    
            return aircraftImage;
        }
    }
}
