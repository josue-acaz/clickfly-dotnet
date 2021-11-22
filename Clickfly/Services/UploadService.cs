using System;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using clickfly.ViewModels;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using clickfly.Helpers;

namespace clickfly.Services
{
    public class UploadService : BaseService, IUploadService
    {
        private readonly RegionEndpoint regionEndpoint = RegionEndpoint.SAEast1;

        public UploadService(
            IOptions<AppSettings> appSettings, 
            INotificator notificator, 
            IInformer informer,
            IUtils utils
        ) : base(
            appSettings, 
            notificator, 
            informer,
            utils
        )
        {
            
        }

        public async Task<UploadResponse> UploadFileAsync (IFormFile file)
        {
            try
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


                var image = Image.Load(file.OpenReadStream());
                image.Mutate(x => x.Resize(256, 256));
                await using var newMemoryStream = new MemoryStream();
                image.SaveAsPng(newMemoryStream);
                
                //await file.CopyToAsync(newMemoryStream);

                string originalName = file.FileName;
                string fileExtension = Path.GetExtension(originalName);
                string key = $"{_utils.RandomBytes(20)}-{originalName}";

                // URL for Accessing Document for Demo
                string location = $"https://{bucketName}.s3.amazonaws.com/{key}";

                TransferUtilityUploadRequest uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = newMemoryStream,
                    Key = key,
                    BucketName = bucketName,
                    CannedACL = S3CannedACL.PublicRead
                };

                TransferUtility fileTransferUtility = new TransferUtility(client);
                await fileTransferUtility.UploadAsync(uploadRequest);

                UploadResponse uploadResponse = new UploadResponse();
                uploadResponse.MimeType = file.ContentType;
                uploadResponse.Key = key;
                uploadResponse.Name = originalName;
                uploadResponse.Url = location;
                uploadResponse.Size = file.Length;

                return uploadResponse;
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null
                    && (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") || amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    throw new Exception("Check the provided AWS Credentials.");
                }
                else
                {
                    throw new Exception("Error occurred: " + amazonS3Exception.Message);
                }
            }
        }
    
        /*
        public async Task<IActionResult> DownloadFileAsync(int id)
        {
            try
            {
                var getdocument = _documentdata.GetDocumentbyDocumentId(id);
                var credentials = new BasicAWSCredentials(_appSettings.AccessKey, _appSettings.SecretKey);
                var config = new AmazonS3Config
                {
                    RegionEndpoint = Amazon.RegionEndpoint.APSouth1
                };
                using var client = new AmazonS3Client(credentials, config);
                var fileTransferUtility = new TransferUtility(client);

                var objectResponse = await fileTransferUtility.S3Client.GetObjectAsync(new GetObjectRequest()
                {
                    BucketName = _appSettings.BucketName,
                    Key = getdocument.DocumentName
                });

                if (objectResponse.ResponseStream == null)
                {
                    return NotFound();
                }
                return File(objectResponse.ResponseStream, objectResponse.Headers.ContentType, getdocument.DocumentName);
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null
                    && (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") || amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    throw new Exception("Check the provided AWS Credentials.");
                }
                else
                {
                    throw new Exception("Error occurred: " + amazonS3Exception.Message);
                }
            }

        }
    
        public async Task<IActionResult> DeleteFileAsync(int id)
        {
            try
            {
                var getdocument = _documentdata.GetDocumentbyDocumentId(id);
                _documentdata.Delete(getdocument);

                var credentials = new BasicAWSCredentials(_appSettings.AccessKey, _appSettings.SecretKey);
                var config = new AmazonS3Config
                {
                    RegionEndpoint = Amazon.RegionEndpoint.APSouth1
                };
                using var client = new AmazonS3Client(credentials, config);
                var fileTransferUtility = new TransferUtility(client);
                await fileTransferUtility.S3Client.DeleteObjectAsync(new DeleteObjectRequest()
                {
                    BucketName = _appSettings.BucketName,
                    Key = getdocument.DocumentName
                });

            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null
                    && (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") || amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    throw new Exception("Check the provided AWS Credentials.");
                }
                else
                {
                    throw new Exception("Error occurred: " + amazonS3Exception.Message);
                }
            }
            return RedirectToAction("AllFiles");
        }
    
        */
    }
}