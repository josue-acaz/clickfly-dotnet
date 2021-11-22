using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RazorEngine;
using RazorEngine.Templating;
using System.IO;
using clickfly.ViewModels;
using Microsoft.Extensions.Options;
using clickfly.Helpers;

namespace clickfly.Services
{
    public class EmailService : BaseService, IEmailService
    {
        private string TEMPLATES_PATH { get; set; }
        private string SMTP_USERNAME { get; set; }
        private string SMTP_PASSWORD { get; set; }
        private string HOST { get; set; }
        private int PORT { get; set; } = 587;

        public EmailService(
            IOptions<AppSettings> appSettings, 
            INotificator notificator, 
            IInformer informer,
            IUtils utils, 
            HttpClient client
        ) : base(
            appSettings, 
            notificator, 
            informer,
            utils
        )
        {   
            HOST = _appSettings.SmtpHost;
            PORT = _appSettings.SmtpPort;
            SMTP_USERNAME = _appSettings.SmtpUsername;
            SMTP_PASSWORD = _appSettings.SmtpPassword;
            TEMPLATES_PATH = _appSettings.TemplatesFolder;
        }

        private string ReadHtmlAsString(string path)
        {
            string html = File.ReadAllText(path, System.Text.Encoding.UTF8);
            return html;
        }

        public void SendEmail(EmailRequest emailRequest)
        {
            string templateName = emailRequest.templateName;
            string templatePath = $"{TEMPLATES_PATH}/{templateName}.cshtml";

            string key = _utils.RandomBytes(20);
            string html = ReadHtmlAsString(templatePath);

            Type modelType = emailRequest.modelType;
            object model = emailRequest.model;

            String TO = emailRequest.to;
            String FROM = emailRequest.from;
            String FROMNAME = emailRequest.fromName;
            String SUBJECT = emailRequest.subject;
            String BODY = Engine.Razor.RunCompile(html, key, modelType, model);

            MailMessage message = new MailMessage();
            message.IsBodyHtml = true;
            message.From = new MailAddress(FROM, FROMNAME);
            message.To.Add(new MailAddress(TO));
            message.Subject = SUBJECT;
            message.Body = BODY;

            using (var client = new System.Net.Mail.SmtpClient(HOST, PORT))
            {
                // Pass SMTP credentials
                client.Credentials = new NetworkCredential(SMTP_USERNAME, SMTP_PASSWORD);

                // Enable SSL encryption
                client.EnableSsl = true;

                // Try to send the message. Show status in console.
                client.Send(message);
            }
        }
    }
}
