using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clickfly.Models
{
    [Table("newsletter_subscribers")]
    public class NewsletterSubscriber : BaseEntity
    {
        public string newsletter_id { get; set; }
        public string subscriber_id { get; set; }

        [ForeignKey("newsletter_id")]
        public Newsletter newsletter { get; set; }

        [ForeignKey("subscriber_id")]
        public Subscriber subscriber { get; set; }
    }
}
