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
    public class GetAntifraudResponse : BaseModel 
    {
        // These fields hold the values for the public properties.
        private string status;
        private string returnCode;
        private string returnMessage;
        private string providerName;
        private string score;

        /// <summary>
        /// TODO: Write general description for this method
        /// </summary>
        [JsonProperty("status")]
        public string Status 
        { 
            get 
            {
                return this.status; 
            } 
            set 
            {
                this.status = value;
                onPropertyChanged("Status");
            }
        }

        /// <summary>
        /// TODO: Write general description for this method
        /// </summary>
        [JsonProperty("return_code")]
        public string ReturnCode 
        { 
            get 
            {
                return this.returnCode; 
            } 
            set 
            {
                this.returnCode = value;
                onPropertyChanged("ReturnCode");
            }
        }

        /// <summary>
        /// TODO: Write general description for this method
        /// </summary>
        [JsonProperty("return_message")]
        public string ReturnMessage 
        { 
            get 
            {
                return this.returnMessage; 
            } 
            set 
            {
                this.returnMessage = value;
                onPropertyChanged("ReturnMessage");
            }
        }

        /// <summary>
        /// TODO: Write general description for this method
        /// </summary>
        [JsonProperty("provider_name")]
        public string ProviderName 
        { 
            get 
            {
                return this.providerName; 
            } 
            set 
            {
                this.providerName = value;
                onPropertyChanged("ProviderName");
            }
        }

        /// <summary>
        /// TODO: Write general description for this method
        /// </summary>
        [JsonProperty("score")]
        public string Score 
        { 
            get 
            {
                return this.score; 
            } 
            set 
            {
                this.score = value;
                onPropertyChanged("Score");
            }
        }
    }
} 