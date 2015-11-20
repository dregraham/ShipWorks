using System;
using System.Globalization;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Stores.Platforms.Amazon.Mws;
using Interapptive.Shared.Net;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
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
        public AmazonValidateCredentialsResponse ValidateCredentials(IAmazonMwsWebClientSettings mwsSettings)
        {
            try
            {
                // Request a list of marketplaces to test credentials
                ExecuteRequest(new HttpVariableRequestSubmitter(), AmazonMwsApiCall.ListMarketplaceParticipations, mwsSettings);
                return AmazonValidateCredentialsResponse.Succeeded();
            }
            catch (AmazonShippingException ex)
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
        public GetEligibleShippingServicesResponse GetRates(ShipmentRequestDetails requestDetails, IAmazonMwsWebClientSettings mwsSettings)
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
        /// Cancel Shipment
        /// </summary>
        public CancelShipmentResponse CancelShipment(IAmazonMwsWebClientSettings mwsSettings, string amazonShipmentId)
        {
            AmazonMwsApiCall call = AmazonMwsApiCall.CancelShipment;

            HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();

            // Add the service
            request.Variables.Add("ShipmentId", amazonShipmentId);

            // Get Response
            IHttpResponseReader response = ExecuteRequest(request, call, mwsSettings);

            // Deserialize
            return DeserializeResponse<CancelShipmentResponse>(response.ReadResult());
        }

        /// <summary>
        /// Create Shipment
        /// </summary>
        public CreateShipmentResponse CreateShipment(ShipmentRequestDetails requestDetails, IAmazonMwsWebClientSettings mwsSettings, string shippingServiceId)
        {
            AmazonMwsApiCall call = AmazonMwsApiCall.CreateShipment;

            HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();

            // Add the service
            request.Variables.Add("ShippingServiceId", shippingServiceId);

            // Add Shipment Information XML
            AddShipmentRequestDetails(request, requestDetails);

            // Get Response
            IHttpResponseReader response = ExecuteRequest(request, call, mwsSettings);

            // Deserialize
            CreateShipmentResponse createShipmentResponse = DeserializeResponse<CreateShipmentResponse>(response.ReadResult());

            return ValidateCreateShipmentResponse(createShipmentResponse);
        }

        /// <summary>
        /// Validate the CreateShipmentResponse to ensure that it contains a label
        /// </summary>
        public CreateShipmentResponse ValidateCreateShipmentResponse(CreateShipmentResponse createShipmentResponse)
        {
            if (createShipmentResponse?.CreateShipmentResult?.Shipment?.Label?.FileContents == null)
            {
                throw new AmazonShippingException("Amazon failed to return a label for the Shipment.");
            }
            return createShipmentResponse;
        }

        /// <summary>
        /// Deserializes the response XML
        /// </summary>
        private static T DeserializeResponse<T>(string xml)
        {
            try
            {
                return SerializationUtility.DeserializeFromXml<T>(xml);
            }
            catch (InvalidOperationException ex)
            {
                if (xml.Contains("ErrorResponse"))
                {
                    ErrorResponse errorResponse = SerializationUtility.DeserializeFromXml<ErrorResponse>(xml);
                    throw new AmazonShippingException(errorResponse.Error.Message, ex);
                }
                
                throw new AmazonShippingException($"Error Deserializing {typeof(T).Name}", ex);
            }
        }

        /// <summary>
        /// Configures the request with required parameters
        /// </summary>
        /// <param name="request"></param>
        /// <param name="amazonMwsApiCall"></param>
        /// <param name="mwsSettings"></param>
        private static void ConfigureRequest(HttpVariableRequestSubmitter request, AmazonMwsApiCall amazonMwsApiCall, IAmazonMwsWebClientSettings mwsSettings)
        {
            string endpointPath = mwsSettings.GetApiEndpointPath(amazonMwsApiCall);

            request.Uri = new Uri(mwsSettings.Endpoint + endpointPath);
            request.VariableEncodingCasing = QueryStringEncodingCasing.Upper;

            request.Variables.Add("AWSAccessKeyId", Decrypt(mwsSettings.InterapptiveAccessKeyID));
            request.Variables.Add("Action", mwsSettings.GetActionName(amazonMwsApiCall));
            request.Variables.Add("MWSAuthToken", mwsSettings.Credentials.AuthToken);
            request.Variables.Add("SellerId", mwsSettings.Credentials.MerchantID);
        }

        /// <summary>
        /// Adds Shipment Details to the request
        /// </summary>
        private static void AddShipmentRequestDetails(HttpVariableRequestSubmitter request, ShipmentRequestDetails requestDetails)
        {
            // Order ID
            request.Variables.Add("ShipmentRequestDetails.AmazonOrderId", requestDetails.AmazonOrderId);

            AddItemInfo(request,requestDetails);
            AddFromAddressInfo(request, requestDetails);
            AddPackageInfo(request, requestDetails);
            AddShippingServiceOptions(request, requestDetails);
        }

        /// <summary>
        /// Add the shipping service options to the request
        /// </summary>
        private static void AddShippingServiceOptions(HttpVariableRequestSubmitter request, ShipmentRequestDetails requestDetails)
        {
            // ShippingServiceOptions
            request.Variables.Add("ShipmentRequestDetails.ShippingServiceOptions.CarrierWillPickUp", requestDetails.ShippingServiceOptions.CarrierWillPickUp.ToString().ToLower());
            request.Variables.Add("ShipmentRequestDetails.ShippingServiceOptions.DeliveryExperience", requestDetails.ShippingServiceOptions.DeliveryExperience);

            request.Variables.Add("ShipmentRequestDetails.ShippingServiceOptions.DeclaredValue.Amount", requestDetails.ShippingServiceOptions.DeclaredValue.Amount.ToString(CultureInfo.InvariantCulture));
            request.Variables.Add("ShipmentRequestDetails.ShippingServiceOptions.DeclaredValue.CurrencyCode", requestDetails.ShippingServiceOptions.DeclaredValue.CurrencyCode);
        }

        /// <summary>
        /// Add the package info to the request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestDetails"></param>
        private static void AddPackageInfo(HttpVariableRequestSubmitter request, ShipmentRequestDetails requestDetails)
        {
            // Package Info
            request.Variables.Add("ShipmentRequestDetails.PackageDimensions.Length", requestDetails.PackageDimensions.Length.ToString(CultureInfo.InvariantCulture));
            request.Variables.Add("ShipmentRequestDetails.PackageDimensions.Width", requestDetails.PackageDimensions.Width.ToString(CultureInfo.InvariantCulture));
            request.Variables.Add("ShipmentRequestDetails.PackageDimensions.Height", requestDetails.PackageDimensions.Height.ToString(CultureInfo.InvariantCulture));
            request.Variables.Add("ShipmentRequestDetails.PackageDimensions.Unit", "inches");
            request.Variables.Add("ShipmentRequestDetails.Weight.Value", (requestDetails.Weight * 16).ToString(CultureInfo.InvariantCulture));
            request.Variables.Add("ShipmentRequestDetails.Weight.Unit", "ounces");
        }

        /// <summary>
        /// Add the from address to the request
        /// </summary>
        private static void AddFromAddressInfo(HttpVariableRequestSubmitter request, ShipmentRequestDetails requestDetails)
        {
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
        }

        /// <summary>
        /// Adds item information to the request
        /// </summary>
        private static void AddItemInfo(HttpVariableRequestSubmitter request, ShipmentRequestDetails requestDetails)
        {
            // Item Info
            int i = 1;
            foreach (Item item in requestDetails.ItemList)
            {
                string orderItemIdParameter = $"ShipmentRequestDetails.ItemList.Item.{i}.OrderItemId";
                string quantityParameter = $"ShipmentRequestDetails.ItemList.Item.{i}.Quantity";

                request.Variables.Add(orderItemIdParameter, item.OrderItemId);
                request.Variables.Add(quantityParameter, item.Quantity.ToString());

                i++;
            }
        }

        /// <summary>
        /// Adds Signature to the request
        /// Required by Amazon MWS Api
        /// </summary>
        /// <param name="request"></param>
        /// <param name="amazonMwsApiCall"></param>
        /// <param name="mwsSettings"></param>
        private static void AddSignature(HttpVariableRequestSubmitter request, AmazonMwsApiCall amazonMwsApiCall, IAmazonMwsWebClientSettings mwsSettings)
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
        private IHttpResponseReader ExecuteRequest(HttpVariableRequestSubmitter request, AmazonMwsApiCall amazonMwsApiCall, IAmazonMwsWebClientSettings mwsSettings)
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
            request.AllowHttpStatusCodes(HttpStatusCode.BadRequest, HttpStatusCode.NotFound, HttpStatusCode.Forbidden);

            IHttpResponseReader response;

            try
            {
                ApiLogEntry logger = new ApiLogEntry(ApiLogSource.Amazon, mwsSettings.GetActionName(amazonMwsApiCall));

                // log the request
                logger.LogRequest(request);

                using (response = request.GetResponse())
                {
                    // log the response
                    logger.LogResponse(response.ReadResult());
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(AmazonShippingException));
            }

            // check response for errors
            RaiseErrors(amazonMwsApiCall, response, mwsSettings);

            return response;
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

        /// <summary>
        /// Raise AmazonShipperException for errors returned to us in the response XML
        /// </summary>
        private static void RaiseErrors([Obfuscation(Exclude = true)] AmazonMwsApiCall api, IHttpResponseReader reader, IAmazonMwsWebClientSettings mwsSettings)
        {
            XNamespace ns = mwsSettings.GetApiNamespace(api);

            string responseText = reader.ReadResult();

            // Try to parse the response and look for Errors
            try
            {
                XDocument xdoc = XDocument.Parse(responseText);

                var error = (from e in xdoc.Descendants(ns + "Error")
                             select new
                             {
                                 Code = (string)e.Element(ns + "Code"),
                                 Message = (string)e.Element(ns + "Message")
                             }).FirstOrDefault();

                if (error != null)
                {
                    // No message was provided so we use the error code
                    throw new AmazonShippingException(error.Message, error.Code);
                }
            }
            catch (XmlException)
            {
                // Throw error because the response is not valid
                throw new AmazonShippingException($"ShipWorks was unable to get a valid response for {api}.");
            }
        }
    }
}