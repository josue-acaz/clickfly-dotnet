using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using clickfly.ViewModels;

namespace clickfly.Services
{
    public interface IUploadService
    {
        Task UploadFileAsync (IFormFile file, string keyName);
        string GetPreSignedUrl(string key);
    }
}