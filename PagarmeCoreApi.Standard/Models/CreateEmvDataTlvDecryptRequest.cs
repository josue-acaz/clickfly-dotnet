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
    public class CreateEmvDataTlvDecryptRequest : BaseModel 
    {
        // These fields hold the values for the public properties.
        private string tag;
        private string lenght;
        private string mvalue;

        /// <summary>
        /// Emv tag
        /// </summary>
        [JsonProperty("tag")]
        public string Tag 
        { 
            get 
            {
                return this.tag; 
            } 
            set 
            {
                this.tag = value;
                onPropertyChanged("Tag");
            }
        }

        /// <summary>
        /// Emv lenght
        /// </summary>
        [JsonProperty("lenght")]
        public string Lenght 
        { 
            get 
            {
                return this.lenght; 
            } 
            set 
            {
                this.lenght = value;
                onPropertyChanged("Lenght");
            }
        }

        /// <summary>
        /// Emv value
        /// </summary>
        [JsonProperty("value")]
        public string Value 
        { 
            get 
            {
                return this.mvalue; 
            } 
            set 
            {
                this.mvalue = value;
                onPropertyChanged("Value");
            }
        }
    }
} 