using System;
using System.Collections.Generic;

namespace clickfly.Data
{
    public class InsertQueryParams {
        public List<string> Exclude { get; set; }

        public InsertQueryParams()
        {
            Exclude = new List<string>();
        }
    };
}
