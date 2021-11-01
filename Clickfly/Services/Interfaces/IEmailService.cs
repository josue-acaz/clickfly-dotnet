using System;
using System.Threading.Tasks;
using clickfly.ViewModels;

namespace clickfly.Services
{
    public interface IEmailService
    {
        void SendEmail(EmailRequest emailRequest);
    }
}
