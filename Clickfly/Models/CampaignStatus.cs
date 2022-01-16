using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clickfly.Models
{
    [Table("campaign_status")]
    public class CampaignStatus : BaseEntity
    {
        public string type { get; set; }
        public string campaign_id { get; set; }

        [ForeignKey("campaign_id")]
        public Campaign campaign { get; set; }
    }
}
