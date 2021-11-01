/*
 * PagarmeCoreApi.Standard
 *
 * This file was automatically generated by APIMATIC v2.0 ( https://apimatic.io ).
 */
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PagarmeCoreApi.Standard;
using PagarmeCoreApi.Standard.Utilities;


namespace PagarmeCoreApi.Standard.Models
{
    public class UpdateChargeDueDateRequest : BaseModel 
    {
        // These fields hold the values for the public properties.
        private DateTime? dueAt;

        /// <summary>
        /// The charge's new due date
        /// </summary>
        [JsonConverter(typeof(IsoDateTimeConverter))]
        [JsonProperty("due_at")]
        public DateTime? DueAt 
        { 
            get 
            {
                return this.dueAt; 
            } 
            set 
            {
                this.dueAt = value;
                onPropertyChanged("DueAt");
            }
        }
    }
} 