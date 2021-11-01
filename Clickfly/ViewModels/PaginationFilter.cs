using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace clickfly.ViewModels
{
    public class PaginationFilter
    {
        public int page_number { get; set; }
        public int page_size { get; set; }
        public string text { get; set; }
        public string order { get; set; }
        public string order_by { get; set; }

        // other params
        public string customer_id { get; set; }
        public string flight_id { get; set; }
        public int selected_seats { get; set; }
        public PaginationFilter()
        {
            this.page_number = 1;
            this.page_size = 10;
        }
        public PaginationFilter(int page_number,int page_size)
        {
            this.page_number = page_number < 1 ? 1 : page_number;
            this.page_size = page_size > 10 ? 10 : page_size;
        }
    }
}