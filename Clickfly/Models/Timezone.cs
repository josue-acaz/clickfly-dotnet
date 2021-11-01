using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clickfly.Models
{
    [Table("timezones")]
    public class Timezone : BaseEntity
    {
        public int gmt { get; set; }
    }
}
