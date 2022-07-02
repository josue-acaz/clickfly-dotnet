using System;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using clickfly.Helpers;
using clickfly.ViewModels;
using clickfly.Repositories;

namespace clickfly.Services
{
    public class UploadService : BaseService, IUploadService
    {
        private readonly RegionEndpoint regionEndpoint = RegionEndpoint.SAEast1;

        public UploadService(
            IOptions<AppSettings> appSettings, 
            ISystemLogRepository systemLogRepository,
            IPermissionRepository permissionRepository,
            INotificator notificator, 
            IInformer informer,
            IUtils utils
        ) : base(
            appSettings,
            systemLogRepository,
            permissionRepository, 
            notificator, 
            informer,
            utils
        )
        {
            
        }

        public async Task UploadFileAsync (IFormFile file, string keyName)
        {
            string accessKey = _appSettings.AWS.AccessKey;
            string secretKey = _appSettings.AWS.SecretKey;
            string bucketName = _appSettings.AWS.BucketName;

            BasicAWSCredentials credentials = new BasicAWSCredentials(accessKey, secretKey);
            AmazonS3Config config = new AmazonS3Config
            {
                RegionEndpoint = regionEndpoint
            };
            using var client = new AmazonS3Client(credentials, config);

            await using var newMemoryStream = new MemoryStream();
            await file.CopyToAsync(newMemoryStream);
            
            TransferUtilityUploadRequest uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = newMemoryStream,
                Key = keyName,
                BucketName = bucketName,
            };

            TransferUtility fileTransferUtility = new TransferUtility(client);
            await fileTransferUtility.UploadAsync(uploadRequest);
        }

        public string GetPreSignedUrl(string key)
        {
            string accessKey = _appSettings.AWS.AccessKey;
            string secretKey = _appSettings.AWS.SecretKey;
            string bucketName = _appSettings.AWS.BucketName;
            
            BasicAWSCredentials credentials = new BasicAWSCredentials(accessKey, secretKey);
            AmazonS3Config config = new AmazonS3Config
            {
                RegionEndpoint = Amazon.RegionEndpoint.USEast1
            };
            using var client = new AmazonS3Client(credentials, config);

            GetPreSignedUrlRequest request1 = new GetPreSignedUrlRequest
            {
                BucketName = bucketName,
                Key = key,
                Expires = DateTime.UtcNow.AddHours(1)
            };

            string urlString = client.GetPreSignedURL(request1);
            return urlString;
        }
    }
}