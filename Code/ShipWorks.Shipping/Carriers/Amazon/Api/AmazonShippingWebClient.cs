using System;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Stores.Platforms.Amazon;
using ShipWorks.Stores.Platforms.Amazon.Mws;
using Interapptive.Shared.Net;
using System.Linq;
using System.Net;
using System.Reflection;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Logging;

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
            catch (AmazonShipperException ex)
            {
                // Something must be wrong with the credentails 
                return AmazonValidateCredentialsResponse.Failed(ex.Message);
            }
        }

        /// <summary>
        /// Gets Rates
        /// </summary>
        /// <param name="requestDetails"></param>
        /// <param name="mwsSettings"></param>
        /// <returns></returns>
        public GetEligibleShippingServicesResponse GetRates(ShipmentRequestDetails requestDetails, AmazonMwsWebClientSettings mwsSettings)
        {
            AmazonMwsApiCall call = AmazonMwsApiCall.GetEligibleShippingServices;

            HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();
            
            // Add Shipment Information XML
            AddShipmentRequestDetails(request, requestDetails);
            
            // Get Response
            IHttpResponseReader response = ExecuteRequest(request, call, mwsSettings);
            
            // Deserialize 
            return DeserializeResponse<GetEligibleShippingServicesResponse>(response.ReadResult());
        }

        /// <summary>
        /// Deserializes the response XML
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        private static T DeserializeResponse<T>(string xml)
        {
            try
            {
                return SerializationUtility.DeserializeFromXml<T>(xml);
            }
            catch (Exception ex)
            {
                throw new AmazonShipperException(ex.Message, (AmazonShipperException)ex.InnerException);
            }
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
                string orderItemIdParameter = $"ShipmentRequestDetails.ItemList.Item.{i}.OrderItemId";
                string quantityParameter = $"ShipmentRequestDetails.ItemList.Item.{i}.Quantity";

                request.Variables.Add(orderItemIdParameter,item.OrderItemId);
                request.Variables.Add(quantityParameter,item.Quantity.ToString());

                i++;
            }

            // From Address Info
            if (!string.IsNullOrWhiteSpace(requestDetails.ShipFromAddress.Name))
            {
                request.Variables.Add("ShipmentRequestDetails.ShipFromAddress.Name", requestDetails.ShipFromAddress.Name);
            }
            if (!string.IsNullOrWhiteSpace(requestDetails.ShipFromAddress.AddressLine1))
            {
                request.Variables.Add("ShipmentRequestDetails.ShipFromAddress.AddressLine1", requestDetails.ShipFromAddress.AddressLine1);
            }
            if (!string.IsNullOrWhiteSpace(requestDetails.ShipFromAddress.AddressLine2))
            {
                request.Variables.Add("ShipmentRequestDetails.ShipFromAddress.AddressLine2", requestDetails.ShipFromAddress.AddressLine2);
            }

            if (!string.IsNullOrWhiteSpace(requestDetails.ShipFromAddress.AddressLine3))
            {
                request.Variables.Add("ShipmentRequestDetails.ShipFromAddress.AddressLine3", requestDetails.ShipFromAddress.AddressLine3);
            }

            request.Variables.Add("ShipmentRequestDetails.ShipFromAddress.City", requestDetails.ShipFromAddress.City);
            request.Variables.Add("ShipmentRequestDetails.ShipFromAddress.StateOrProvinceCode", requestDetails.ShipFromAddress.StateOrProvinceCode);
            request.Variables.Add("ShipmentRequestDetails.ShipFromAddress.PostalCode", requestDetails.ShipFromAddress.PostalCode);
            request.Variables.Add("ShipmentRequestDetails.ShipFromAddress.CountryCode", requestDetails.ShipFromAddress.CountryCode);
            if (!string.IsNullOrWhiteSpace(requestDetails.ShipFromAddress.Email))
            {
                request.Variables.Add("ShipmentRequestDetails.ShipFromAddress.Email", requestDetails.ShipFromAddress.Email);
            }
            if (!string.IsNullOrWhiteSpace(requestDetails.ShipFromAddress.Phone))
            {
                request.Variables.Add("ShipmentRequestDetails.ShipFromAddress.Phone", requestDetails.ShipFromAddress.Phone);
            }
            
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

            if (requestDetails.MustArriveByDate != null && requestDetails.SendDateMustArriveBy)
            {
                request.Variables.Add("ShipmentRequestDetails.MustArriveByDate", FormatDate(requestDetails.MustArriveByDate.Value));
            }
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
                .Select(v => v.Name + "=" + AmazonMwsSignature.Encode(v.Value, false))
                .Aggregate((x,y) => x + "&" + y);

            string parameterString = $"{verbString}\n{request.Uri.Host}\n{endpointPath}\n{queryString}";

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
            
            // add a User Agent header
            request.Headers.Add("x-amazon-user-agent",
                $"ShipWorks/{Assembly.GetExecutingAssembly().GetName().Version} (Language=.NET)");

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
            catch (AmazonException ex)
            {
                // Found an error in the respons, throw it as an AmazonShipperException
                throw new AmazonShipperException(ex.Message, (AmazonShipperException)ex.InnerException);
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