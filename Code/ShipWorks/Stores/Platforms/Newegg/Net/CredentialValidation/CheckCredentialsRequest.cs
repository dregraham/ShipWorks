using System.Collections.Generic;
using System.Linq;
using System.Net;
using Interapptive.Shared.Net;
using ShipWorks.Stores.Platforms.Newegg.Enums;


namespace ShipWorks.Stores.Platforms.Newegg.Net.CredentialValidation
{
    /// <summary>
    /// An implementation of the ICheckCredentialRequest that hits the Newegg API.
    /// </summary>
    public class CheckCredentialsRequest : ICheckCredentialRequest
    {
        // We could really just use any URL here since we're just going to bounce a request off the 
        // Newegg API to see that we don't get an authentication error
        private const string RequestUrl = "{0}/ordermgmt/servicestatus?sellerid={1}";
        private INeweggRequest request;

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckCredentialsRequest"/> class.
        /// </summary>
        public CheckCredentialsRequest()
            : this(new NeweggHttpRequest())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckCredentialsRequest"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        public CheckCredentialsRequest(INeweggRequest request)
        {
            this.request = request;
        }
        /// <summary>
        /// Makes a request to determine if the credentials are valid.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <returns>
        /// Returns true if the credentials are valid; otherwise false.
        /// </returns>
        public bool AreCredentialsValid(Credentials credentials)
        {
            NeweggResponse response = SubmitRequest(credentials);
            return response.ResponseErrors.Count() == 0;
        }


        private NeweggResponse SubmitRequest(Credentials credentials)
        {
            // API URL depends on which marketplace the seller selected 
            string marketplace = "";

            switch (credentials.Channel)
            {
                case NeweggChannelType.US:
                    break;
                case NeweggChannelType.Business:
                    marketplace = "/b2b";
                    break;
                case NeweggChannelType.Canada:
                    marketplace = "/can";
                    break;
                default:
                    break;
            }

            // Format our request URL with the value of the seller ID and configure the request
            string formattedUrl = string.Format(RequestUrl, marketplace, credentials.SellerId);
            RequestConfiguration requestConfig = new RequestConfiguration("Check Credentials", formattedUrl) 
            { 
                Method = HttpVerb.Get, 
                Body = string.Empty 
            };

            string rawResponseData = this.request.SubmitRequest(credentials, requestConfig);

            // The response data should contain the XML containing the result of our credential check
            NeweggResponse credentialResponse = new NeweggResponse(rawResponseData, new CheckCredentialsResponseSerializer());
            return credentialResponse;
        }


    }
}
