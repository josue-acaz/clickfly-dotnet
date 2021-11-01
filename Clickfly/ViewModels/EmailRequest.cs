using System;

namespace clickfly.ViewModels
{
    public class EmailRequest
    {
        public string from { get; set; }
        public string fromName { get; set; }
        public string to { get; set; }
        public string subject { get; set; }
        public string templateName { get; set; }
        public Type modelType { get; set; }
        public object model { get; set; }
    }
}