using System;
using System.Threading.Tasks;
using clickfly.ViewModels;

namespace clickfly.Services
{
    public interface ICepService
    {
        Task<ConsultCepResponse> ConsultCep(string cep);
    }
}
