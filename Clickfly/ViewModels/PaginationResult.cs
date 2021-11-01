using System;
using System.Collections.Generic;

namespace clickfly.ViewModels
{
    public class PaginationResult<Type>
    {
        public int page_number { get; set; }
        public int page_size { get; set; }
        public int total_pages { get; set; }
        public int total_records { get; set; }
        public List<Type> data { get; set; }

        public PaginationResult(List<Type> data, int page_number , int page_size)
        {
            this.page_number = page_number;
            this.page_size = page_size;
            this.data = data;
        }
    }
}
