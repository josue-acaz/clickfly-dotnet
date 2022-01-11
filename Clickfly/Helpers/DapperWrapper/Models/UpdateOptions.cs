using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace clickfly.Data
{
    public class UpdateOptions : QueryExtensions
    {
        public object Data { get; set; }
        public string Where { get; set; }
        public List<string> Exclude { get; set; }
        public IDbTransaction Transaction { get; set; }

        public UpdateOptions()
        {
            Exclude = new List<string>();
        }
    }
}