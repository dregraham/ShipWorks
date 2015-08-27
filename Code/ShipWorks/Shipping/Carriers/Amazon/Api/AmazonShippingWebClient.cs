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
using System.Text.RegularExpressions;
using System.Text;
using System.Collections.Generic;

namespace ShipWorks.Shipping.Carriers.Amazon.Api
{
    /// <summary>
    /// Amazon shipping api client
    /// </summary>
    public class AmazonShippingWebClient : IAmazonShippingWebClient
    {
        /// <summary>
        /// Validate the given credentials
        /// </summary>
        public AmazonValidateCredentialsResponse ValidateCredentials(AmazonMwsWebClientSettings mwsSettings)
        {
            try
            {
                // Request a list of marketplaces to test credentials
                ExecuteRequest(new HttpVariableRequestSubmitter(), AmazonMwsApiCall.ListMarketplaceParticipations, mwsSettings);
                return AmazonValidateCredentialsResponse.Succeeded();
            }
            catch (AmazonException ex)
            {
                // Something must be wrong with the credentails 
                return AmazonValidateCredentialsResponse.Failed(ex.Message);
            }
        }

        /// <summary>
        /// Gets the rates.
        /// </summary>
        public GetEligibleShippingServices GetRates(ShipmentRequestDetails requestDetails, AmazonMwsWebClientSettings mwsSettings)
        {
            HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();
            
            AddShipmentRequestDetails(request, requestDetails);
                        
            ExecuteRequest(request, AmazonMwsApiCall.GetEligibleShippingServices, mwsSettings);

            throw new NotImplementedException();
        }


        /// <summary>
        /// Configures the request with required parameters
        /// </summary>
        /// <param name="request"></param>
        /// <param name="amazonMwsApiCall"></param>
        /// <param name="mwsSettings"></param>
        private static void ConfigureRequest(HttpVariableRequestSubmitter request, AmazonMwsApiCall amazonMwsApiCall,  AmazonMwsWebClientSettings mwsSettings)
        {
            string endpointPath = mwsSettings.GetApiEndpointPath(amazonMwsApiCall);

            request.Uri = new Uri(mwsSettings.Endpoint + endpointPath);
            request.VariableEncodingCasing = QueryStringEncodingCasing.Upper;
            
            request.Variables.Add("AWSAccessKeyId", Decrypt(mwsSettings.InterapptiveAccessKeyID));
            request.Variables.Add("Action", mwsSettings.GetActionName(amazonMwsApiCall));
            request.Variables.Add("MWSAuthToken", mwsSettings.Connection.AuthToken);
            request.Variables.Add("SellerId", mwsSettings.Connection.MerchantId);
        }
        
        /// <summary>
        /// Adds Shipment Details to the request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestDetails"></param>
        private static void AddShipmentRequestDetails(HttpVariableRequestSubmitter request, ShipmentRequestDetails requestDetails)
        {
            // Order ID
            request.Variables.Add("ShipmentRequestDetails.AmazonOrderId", requestDetails.AmazonOrderId);
            
            // Item Info
            int i = 1;
            foreach (Item item in requestDetails.ItemList)
            {
                string orderItemIdParameter = String.Format("ShipmentRequestDetails.ItemList.Item.{0}.OrderItemId", i);
                string quantityParameter = String.Format("ShipmentRequestDetails.ItemList.Item.{0}.Quantity", i);

                request.Variables.Add(orderItemIdParameter,item.OrderItemId);
                request.Variables.Add(quantityParameter,item.Quantity.ToString());

                i++;
            }
            
            // From Address Info
            request.Variables.Add("ShipmentRequestDetails.ShipFromAddress.Name", requestDetails.ShipFromAddress.Name);
            request.Variables.Add("ShipmentRequestDetails.ShipFromAddress.AddressLine1", requestDetails.ShipFromAddress.AddressLine1);

            if (!string.IsNullOrWhiteSpace(requestDetails.ShipFromAddress.AddressLine2))
            {
                request.Variables.Add("ShipmentRequestDetails.ShipFromAddress.AddressLine2", requestDetails.ShipFromAddress.AddressLine2);
            }

            if (!string.IsNullOrWhiteSpace(requestDetails.ShipFromAddress.AddressLine3))
            {
                request.Variables.Add("ShipmentRequestDetails.ShipFromAddress.AddressLine3", requestDetails.ShipFromAddress.AddressLine3);
            }

            request.Variables.Add("ShipmentRequestDetails.ShipFromAddress.City", requestDetails.ShipFromAddress.City);
            //request.Variables.Add("ShipmentRequestDetails.ShipFromAddress.StateOrProvinceCode", requestDetails.ShipFromAddress.StateOrProvinceCode);
            request.Variables.Add("ShipmentRequestDetails.ShipFromAddress.PostalCode", requestDetails.ShipFromAddress.PostalCode);
            request.Variables.Add("ShipmentRequestDetails.ShipFromAddress.CountryCode", requestDetails.ShipFromAddress.CountryCode);
            //request.Variables.Add("ShipmentRequestDetails.ShipFromAddress.Phone", requestDetails.ShipFromAddress.Phone);

            // Package Info
            request.Variables.Add("ShipmentRequestDetails.PackageDimensions.Length", requestDetails.PackageDimensions.Length.ToString());
            request.Variables.Add("ShipmentRequestDetails.PackageDimensions.Width", requestDetails.PackageDimensions.Width.ToString());
            request.Variables.Add("ShipmentRequestDetails.PackageDimensions.Height", requestDetails.PackageDimensions.Height.ToString());
            request.Variables.Add("ShipmentRequestDetails.PackageDimensions.Unit", "inches");
            request.Variables.Add("ShipmentRequestDetails.Weight.Value", (requestDetails.Weight * 16).ToString());
            request.Variables.Add("ShipmentRequestDetails.Weight.Unit", "ounces");

            // ShippingServiceOptions
            request.Variables.Add("ShipmentRequestDetails.ShippingServiceOptions.CarrierWillPickUp", requestDetails.ShippingServiceOptions.CarrierWillPickUp.ToString().ToLower());
            request.Variables.Add("ShipmentRequestDetails.ShippingServiceOptions.DeliveryExperience", requestDetails.ShippingServiceOptions.DeliveryExperience);

            //if (requestDetails.MustArriveByDate != null)
            //{
            //    request.Variables.Add("ShipmentRequestDetails.MustArriveByDate", FormatDate(requestDetails.MustArriveByDate.Value));
            //}
        }

        /// <summary>
        /// Adds Signature to the request
        /// Required by Amazon MWS Api
        /// </summary>
        /// <param name="request"></param>
        /// <param name="amazonMwsApiCall"></param>
        /// <param name="mwsSettings"></param>
        private static void AddSignature(HttpVariableRequestSubmitter request, AmazonMwsApiCall amazonMwsApiCall, AmazonMwsWebClientSettings mwsSettings)
        {
            request.Variables.Add("SignatureMethod", "HmacSHA256");
            request.Variables.Add("SignatureVersion", "2");
            request.Variables.Add("Timestamp", FormatDate(DateTime.UtcNow));
            request.Variables.Add("Version", mwsSettings.GetApiVersion(amazonMwsApiCall));

            string endpointPath = mwsSettings.GetApiEndpointPath(amazonMwsApiCall);

            // now construct the signature parameter
            string verbString = request.Verb == HttpVerb.Get ? "GET" : "POST";
            string queryString = request.Variables
                .OrderBy(v => v.Name, StringComparer.Ordinal)
                .Select(v => v.Name + "=" + AmazonMwsUtility.SignatureEncode(v.Value, false))
                .Aggregate((x,y) => x + "&" + y);

            string parameterString = String.Format("{0}\n{1}\n{2}\n{3}", verbString, request.Uri.Host, endpointPath, queryString);

            // sign the string and add it to the request
            string signature = RequestSignature.CreateRequestSignature(parameterString, Decrypt(mwsSettings.InterapptiveSecretKey), SigningAlgorithm.SHA256);
            request.Variables.Add("Signature", signature);
        }

        /// <summary>
        /// Executes a request 
        /// </summary>
        private IHttpResponseReader ExecuteRequest(HttpVariableRequestSubmitter request, AmazonMwsApiCall amazonMwsApiCall, AmazonMwsWebClientSettings mwsSettings)
        {
            // Adds our amazon credentials and other parameters
            // required for each api call
            ConfigureRequest(request, amazonMwsApiCall, mwsSettings);

            // Signes the request
            AddSignature(request, amazonMwsApiCall, mwsSettings);

            //request.Variables.Sort(v => v.Name);

            // add a User Agent header
            request.Headers.Add("x-amazon-user-agent", String.Format("ShipWorks/{0} (Language=.NET)", Assembly.GetExecutingAssembly().GetName().Version));

            // business logic failures are handled through status codes
            request.AllowHttpStatusCodes(new HttpStatusCode[] { HttpStatusCode.BadRequest });

            try
            {
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
                throw WebHelper.TranslateWebException(ex, typeof(AmazonShipperException));
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