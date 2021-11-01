using System;
using System.Threading.Tasks;
using clickfly.ViewModels;
using Microsoft.AspNetCore.Http;

namespace clickfly.Services
{
    public interface IUploadService
    {
        Task<UploadResponse> UploadFileAsync (IFormFile file);
    }
}