using System;
using System.Text;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using clickfly.ViewModels;
using Microsoft.Extensions.Options;
using clickfly.Helpers;

namespace clickfly.Services
{
    public class CepService : BaseService, ICepService
    {
        private readonly HttpClient _httClient;
        private string _format = "json";

        public CepService(
            IOptions<AppSettings> appSettings, 
            INotificator notificator, 
            IInformer informer,
            IUtils utils, 
            HttpClient client
        ) : base(appSettings, notificator, informer, utils)
        {
            client.BaseAddress = new Uri(_appSettings.CepApiUrl);
            _httClient = client;
        }

        public async Task<ConsultCepResponse> ConsultCep(string cep)
        {
            HttpResponseMessage httpResponse = await _httClient.GetAsync($"/ws/{cep}/{_format}");
            string httpContent = await httpResponse.Content.ReadAsStringAsync();
            
            ConsultCepResponse consultCepResponse = JsonSerializer.Deserialize<ConsultCepResponse>(httpContent);

            return consultCepResponse;
        }
    }
}
