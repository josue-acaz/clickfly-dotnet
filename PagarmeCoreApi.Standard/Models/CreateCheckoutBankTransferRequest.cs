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
    public class CreateCheckoutBankTransferRequest : BaseModel 
    {
        // These fields hold the values for the public properties.
        private List<string> bank;
        private int retries;

        /// <summary>
        /// Bank
        /// </summary>
        [JsonProperty("bank")]
        public List<string> Bank 
        { 
            get 
            {
                return this.bank; 
            } 
            set 
            {
                this.bank = value;
                onPropertyChanged("Bank");
            }
        }

        /// <summary>
        /// Number of retries for processing
        /// </summary>
        [JsonProperty("retries")]
        public int Retries 
        { 
            get 
            {
                return this.retries; 
            } 
            set 
            {
                this.retries = value;
                onPropertyChanged("Retries");
            }
        }
    }
} 