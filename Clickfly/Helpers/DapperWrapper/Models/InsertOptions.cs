using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace clickfly.Data
{
    public class InsertOptions : QueryExtensions
    {
        public object Data { get; set; }
        public List<string> Exclude { get; set; }
        public IDbTransaction Transaction { get; set; }

        public InsertOptions()
        {
            Exclude = new List<string>();
        }
    }
}