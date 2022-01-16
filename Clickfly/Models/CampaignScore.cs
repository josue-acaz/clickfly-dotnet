using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clickfly.Models
{
    [Table("campaign_scores")]
    public class CampaignScore : BaseEntity
    {
        public string campaign_id { get; set; }

        [ForeignKey("campaign_id")]
        public Campaign campaign { get; set; }

        public string customer_id { get; set; }

        [ForeignKey("customer_id")]
        public Customer customer { get; set; }

        [NotMapped]
        public int completed_points { get; set; }
    }
}
