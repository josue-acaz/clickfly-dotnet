using System;

namespace clickfly
{
    public class AWS {
        public string Profile { get; set; }
        public string Region { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string BucketName { get; set; }
        public string PathName { get; set; }
    }

    public class AppSettings
    {
        public string Secret { get; set; }
        public string DBConnection { get; set; }
        public AWS AWS { get; set; }
        public string SupportEmail { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }
        public string TemplatesFolder { get; set; }
        public string CepApiUrl { get; set; }
        public Guid OneSignalAppId { get; set; }
        public string OneSignalAppKey { get; set; }
        public string DashboardUrl { get; set; }
        public string WebsiteUrl { get; set; }
    }
}


/*

MANAGER -> Grupo de permissão e/ou alterar permissões
FUNCIONARIO -> Grupo de permissão e/ou alterar permissões
ADMINISTRADOR -> Grupo de permissão e/ou alterar permissões

-- PERMISSÕES ADICIONAIS
1. Permissão para criar x recursos
2. Permissão para criar y recursos
3. Permissão para criar z recursos
*/