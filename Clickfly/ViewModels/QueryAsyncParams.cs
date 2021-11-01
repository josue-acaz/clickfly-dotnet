using System;
using System.Collections.Generic;

namespace clickfly.ViewModels
{
    public class Include
    {
        public string tableName { get; set; }
        public string[] attributes { get; set; }
        public string relationshipName { get; set; }
        public string foreignKey { get; set; }
        public List<Include> includes { get; set; }

        public Include()
        {
            includes = new List<Include>();
        }
    }

    public class QueryAsyncParams
    {
        public string querySql { get; set; }
        public string tableName { get; set; }
        public string relationshipName { get; set; }
        public List<Include> includes { get; set; }
        public string[] attributes { get; set; }
        public Dictionary<string, object> queryParams { get; set; }
        public string foreignKey { get; set; }

        public QueryAsyncParams()
        {
            includes = new List<Include>();
        }
    }
}