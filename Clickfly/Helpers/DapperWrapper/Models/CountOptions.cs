using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace clickfly.Data
{
    public class CountOptions : QueryExtensions
    {
        public string As { get; set; }
        public object Params { get; set; }
        public string Where { get; set; }
        public IDbTransaction Transaction { get; set; }
    }
}