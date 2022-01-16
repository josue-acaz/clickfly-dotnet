using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clickfly.Models
{
    [Table("score")]
    public class Score : BaseEntity
    {
        public int points { get; set; }
        public string flight_segment_id { get; set; }
        public string campaign_score_id { get; set; }

        [ForeignKey("campaign_score_id")]
        public CampaignScore campaign_score { get; set; }
    }
}
