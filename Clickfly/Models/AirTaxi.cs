using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clickfly.Models
{
    [Table("air_taxis")]
    public class AirTaxi : BaseEntity
    {
        public string name { get; set; }
        public string email { get; set; }
        public string cnpj { get; set; }
        public string phone_number { get; set; }

        [NotMapped]
        public string dashboard_url { get; set; }
    }
}
