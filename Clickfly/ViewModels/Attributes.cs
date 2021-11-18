using System;
using System.Collections.Generic;

namespace clickfly.ViewModels
{
    public class Attributes {
        public List<string> Include { get; set; }
        public List<string> Exclude { get; set; }

        public Attributes()
        {
            Include = new List<string>();
            Exclude = new List<string>();
        }
    };
}