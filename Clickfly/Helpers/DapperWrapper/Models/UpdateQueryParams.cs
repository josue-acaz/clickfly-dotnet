using System;
using System.Collections.Generic;

namespace clickfly.Data
{
    public class UpdateQueryParams {
        public string Where { get; set; }
        public List<string> Exclude { get; set; }

        public UpdateQueryParams()
        {
            Exclude = new List<string>();
        }
    };
}
