﻿using Newtonsoft.Json;
using ShipEngine.ApiClient.Model;

namespace ShipWorks.ShipEngine.AddStoreRequests
{
    /// <summary>
    /// The add store request for adding volusion to ShipEngine
    /// </summary>
    public class VolusionAddStoreRequest : ApiOrderSourceAccountInformationRequest
    {
        /// <summary>
        /// The full url of the volusion store.
        /// Includes the Username and Password
        /// </summary>
        [JsonProperty("api_url")]
        public string ApiUrl { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary
        public VolusionAddStoreRequest() { }

        /// <summary>
        /// Constructor for configuring the ApiUrl
        /// </summary
        public VolusionAddStoreRequest(string username, string encryptedPassword, string baseUrl)
        {
            if(baseUrl[baseUrl.Length - 1] == '/')
            {
                baseUrl = baseUrl.Substring(0, baseUrl.Length - 1);
            }

            ApiUrl = $"{baseUrl}/net/WebService.aspx?Login={username}&EncryptedPassword={encryptedPassword}&EDI_Name=Generic\\Orders";
        }
    }
}
