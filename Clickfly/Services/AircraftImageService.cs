using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.Helpers;
using clickfly.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using clickfly.ViewModels;

namespace clickfly.Services
{
    public class AircraftImageService : BaseService, IAircraftImageService
    {
        private readonly IAircraftImageRepository _aircraftImageRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IUploadService _uploadService;

        public AircraftImageService(
            IOptions<AppSettings> appSettings, 
            ISystemLogRepository systemLogRepository,
            IPermissionRepository permissionRepository,
            INotificator notificator, 
            IInformer informer,
            IUtils utils,
            IAircraftImageRepository aircraftImageRepository, 
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
            _aircraftImageRepository = aircraftImageRepository;
            _fileRepository = fileRepository;
            _uploadService = uploadService;
        }

        public async Task Delete(string id)
        {
            await _aircraftImageRepository.Delete(id);
        }

        public async Task<PaginationResult<AircraftImage>> Pagination(AircraftImagePaginationFilter filter)
        {
            PaginationResult<AircraftImage> paginationResult = await _aircraftImageRepository.Pagination(filter);
            return paginationResult;
        }

        public async Task<AircraftImage> Save(AircraftImage aircraftImage)
        {
            IFormFile file = aircraftImage.file;
            string user_id = _informer.GetValue(UserIdTypes.UserId);

            string keyName = _utils.RandomBytes(20);
            await _uploadService.UploadFileAsync(file, keyName);
            aircraftImage = await _aircraftImageRepository.Create(aircraftImage);

            File createFile = new File();
            createFile.resource_id = aircraftImage.id;
            createFile.resource = Resources.AircraftImages;
            createFile.key = keyName;
            createFile.name = file.Name;
            createFile.size = file.Length;
            createFile.mimetype = file.ContentType;
            createFile.field_name = FieldNames.AircraftImage;
            createFile.created_by = user_id;

            createFile = await _fileRepository.Create(createFile);
            aircraftImage.url = _uploadService.GetPreSignedUrl(keyName);
            aircraftImage.file = null;
    
            return aircraftImage;
        }
    }
}
