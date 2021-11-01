using System;

namespace clickfly.ViewModels
{
    public class UploadResponse
    {
        public string Key { get; set; }
        public long Size { get; set; }
        public string Url { get; set; }
        public string MimeType { get; set; }
        public string Name { get; set; }
    }
}