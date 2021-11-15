using System;
using System.Collections.Generic;

namespace clickfly.ViewModels
{
    public class CreateQueryParams {
        public string As { get; set; }
        public string TableName { get; set; }
        public string[] Attributes { get; set; }
        public string Where { get; set; }
        public List<IncludeModel> Includes { get; set; }
        public List<RawAttribute> RawAttributes { get; set; }

        public CreateQueryParams()
        {
            Attributes = new string[0];
            Includes = new List<IncludeModel>();
            RawAttributes = new List<RawAttribute>();
        }
    };
}
