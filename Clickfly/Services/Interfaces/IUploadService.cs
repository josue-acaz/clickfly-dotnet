using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using clickfly.ViewModels;

namespace clickfly.Services
{
    public interface IUploadService
    {
        Task<UploadResponse> UploadFileAsync (IFormFile file);
    }
}