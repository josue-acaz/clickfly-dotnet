using System;
using System.Collections.Generic;

namespace clickfly.Data
{
    public class SelectQueryParams {
        public string PK { get; set; }
        public string As { get; set; }
        public string TableName { get; set; }
        public List<string> Attributes { get; set; }
        public string Where { get; set; }
        public List<IncludeModel> Includes { get; set; }
        public List<RawAttribute> RawAttributes { get; set; }

        public SelectQueryParams()
        {
            Attributes = new List<string>();
            Includes = new List<IncludeModel>();
            RawAttributes = new List<RawAttribute>();
        }
    };
}
