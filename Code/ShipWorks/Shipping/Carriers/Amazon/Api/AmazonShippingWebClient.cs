using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Stores.Platforms.Amazon;
using ShipWorks.Stores.Platforms.Amazon.Mws;
using Interapptive.Shared.Net;
using System.Linq;
using System.Net;
using System.Reflection;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Logging;
using log4net;

namespace ShipWorks.Shipping.Carriers.Amazon.Api
{
    /// <summary>
    /// Amazon shipping api client
    /// </summary>
    public class AmazonShippingWebClient : IAmazonShippingWebClient
    {
        static readonly ILog log = LogManager.GetLogger(typeof(AmazonMwsClient));
        
        /// <summary>
        /// Validate the given credentials
        /// </summary>
        public AmazonValidateCredentialsResponse ValidateCredentials(string merchantId, string authToken)
        {
            // Create a fake store instance because the current
            // AmazonMwsClient requires a store to be passed
            // use US api 
            AmazonStoreEntity fakeStore = new AmazonStoreEntity() 
            {
                MerchantID = merchantId, 
                AuthToken = authToken,
                AmazonApiRegion = "US"
            };

            using (AmazonMwsClient client = new AmazonMwsClient(fakeStore))
            {
                try
                {
                    // Request a list of marketplaces to test credentials
                    client.GetMarketplaces();
                    return AmazonValidateCredentialsResponse.Succeeded();
                }
                catch (AmazonException ex)
                {
                    // Something must be wrong with the credentails 
                    return AmazonValidateCredentialsResponse.Failed(ex.Message);
                }
            }

        }

        /// <summary>
        /// Gets the rates.
        /// </summary>
        public GetEligibleShippingServices GetRates(ShipmentRequestDetails requestDetails, IAmazonMwsWebClientSettings mwsSettings)
        {
            throw new System.NotImplementedException();
        }
        
        /// <summary>
        /// Executes a request 
        /// </summary>
        private IHttpResponseReader ExecuteRequest(HttpVariableRequestSubmitter request, AmazonMwsApiCall amazonMwsApiCall, AmazonMwsWebClientSettings mwsSettings)
        {
            try
            {
                DateTime timestamp = DateTime.UtcNow;

                string endpointPath = mwsSettings.GetApiEndpointPath(amazonMwsApiCall);

                request.Uri = new Uri(mwsSettings.Endpoint + endpointPath);
                request.VariableEncodingCasing = QueryStringEncodingCasing.Upper;

                request.Variables.Add("Action", mwsSettings.GetActionName(amazonMwsApiCall));


                request.Variables.Add("SellerId", mwsSettings.Connection.MerchantId);
                request.Variables.Add("MWSAuthToken", mwsSettings.Connection.AuthToken);

                request.Variables.Add("SignatureMethod", "HmacSHA256");
                request.Variables.Add("SignatureVersion", "2");
                request.Variables.Add("Timestamp", FormatDate(timestamp));
                request.Variables.Add("Version", mwsSettings.GetApiVersion(amazonMwsApiCall));
                request.Variables.Add("AWSAccessKeyId", Decrypt(mwsSettings.InterapptiveAccessKeyID));

                // now construct the signature parameter
                string verbString = request.Verb == HttpVerb.Get ? "GET" : "POST";
                string queryString = QueryStringUtility.GetQueryString(
                    request.Variables.OrderBy(v => v.Name, StringComparer.Ordinal),
                    QueryStringEncodingCasing.Upper);

                string parameterString = String.Format("{0}\n{1}\n{2}\n{3}", verbString, request.Uri.Host, endpointPath, queryString);

                // sign the string and add it to the request
                string signature = RequestSignature.CreateRequestSignature(parameterString, Decrypt(mwsSettings.InterapptiveSecretKey), SigningAlgorithm.SHA256);
                request.Variables.Add("Signature", signature);

                // add a User Agent header
                request.Headers.Add("x-amazon-user-agent", String.Format("ShipWorks/{0} (Language=.NET)", Assembly.GetExecutingAssembly().GetName().Version));

                // business logic failures are handled through status codes
                request.AllowHttpStatusCodes(new HttpStatusCode[] { HttpStatusCode.BadRequest });

                log.InfoFormat("Submitting request for {0}", amazonMwsApiCall);

                ApiLogEntry logger = new ApiLogEntry(ApiLogSource.Amazon, mwsSettings.GetActionName(amazonMwsApiCall));

                // log the request
                logger.LogRequest(request);

                // Feed uploads are a combination of querystring params AND POST data, which isn't handled by typical request logging
                AmazonMwsFeedRequestSubmitter feedRequest = request as AmazonMwsFeedRequestSubmitter;
                if (feedRequest != null)
                {
                    logger.LogRequestSupplement(feedRequest.GetPostContent(), "FeedDocument", "xml");
                }

                using (IHttpResponseReader response = request.GetResponse())
                {
                    // log the response
                    logger.LogResponse(response.ReadResult());

                    // check response for errors
                    AmazonMwsResponseHandler.RaiseErrors(amazonMwsApiCall, response, mwsSettings);

                    return response;
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(AmazonException));
            }
        }

        /// <summary>
        /// Formats a date to make it appropriate/safe for Amazon
        /// </summary>
        private static string FormatDate(DateTime dateTime)
        {
            // not including milliseconds
            return dateTime.ToString("yyyy-MM-ddTHH:mm:ssZ");
        }

        /// <summary>
        /// Returns the decrypted Interapptive Developer Access Key
        /// </summary>
        private static string Decrypt(string encrypted)
        {
            return SecureText.Decrypt(encrypted, "Interapptive");
        }
    }
}