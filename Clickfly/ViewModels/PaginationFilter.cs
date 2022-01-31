using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace clickfly.ViewModels
{
    public class ExcludeFilterAttribute
    {
        public string name { get; set; }
        public string value { get; set; }
    };

    public class PaginationFilter
    {
        public int page_number { get; set; }
        public int page_size { get; set; }
        public string text { get; set; }
        public string order { get; set; }
        public string order_by { get; set; }

        // additional params
        public string user_id { get; set; }
        public string customer_id { get; set; }
        public string flight_id { get; set; }
        public int selected_seats { get; set; }
        public string origin_city_id { get; set; }
        public string destination_city_id { get; set; }
        public string flight_type { get; set; }
        public string flight_segment_type { get; set; }
        public string air_taxi_id { get; set; }
        public string booking_id { get; set; }
        public List<ExcludeFilterAttribute> exclude { get; set; }
        public PaginationFilter()
        {
            this.page_number = 1;
            this.page_size = 10;
            this.exclude = new List<ExcludeFilterAttribute>();
        }
        public PaginationFilter(int page_number,int page_size)
        {
            this.page_number = page_number < 1 ? 1 : page_number;
            this.page_size = page_size > 10 ? 10 : page_size;
        }
    }
}