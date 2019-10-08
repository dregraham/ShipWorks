﻿using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.Security;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.SFP.Api.DTOs;
using ShipWorks.Stores.Platforms.Amazon.Mws;

namespace ShipWorks.Shipping.Carriers.Amazon.SFP.Api
{
    /// <summary>
    /// Amazon shipping api client
    /// </summary>
    [Component]
    public class AmazonSFPShippingWebClient : IAmazonSFPShippingWebClient
    {
        private readonly Func<IHttpVariableRequestSubmitter> createVariableRequestSubmitter;
        private readonly IEncryptionProvider encryptionProvider;
        private readonly ILogEntryFactory createApiLogEntry;
        private readonly IAmazonMwsWebClientSettingsFactory settingsFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonSFPShippingWebClient(IEncryptionProviderFactory encryptionFactory,
            Func<IHttpVariableRequestSubmitter> createVariableRequestSubmitter,
            ILogEntryFactory createApiLogEntry, IAmazonMwsWebClientSettingsFactory settingsFactory)
        {
            this.createApiLogEntry = createApiLogEntry;
            this.settingsFactory = settingsFactory;
            this.createVariableRequestSubmitter = createVariableRequestSubmitter;
            encryptionProvider = encryptionFactory.CreateSecureTextEncryptionProvider("Interapptive");
        }

        /// <summary>
        /// Validate the given credentials
        /// </summary>
        public AmazonValidateCredentialsResponse ValidateCredentials(IAmazonMwsWebClientSettings mwsSettings)
        {
            try
            {
                // Request a list of marketplaces to test credentials
                ExecuteRequest(createVariableRequestSubmitter(), AmazonMwsApiCall.ListMarketplaceParticipations, mwsSettings);
                return AmazonValidateCredentialsResponse.Succeeded();
            }
            catch (AmazonSFPShippingException ex)
            {
                // Something must be wrong with the credentials
                return AmazonValidateCredentialsResponse.Failed(ex.Message);
            }
        }

        /// <summary>
        /// Gets rates for the given ShipmentRequestDetails
        /// </summary>
        public GetEligibleShippingServicesResponse GetRates(ShipmentRequestDetails requestDetails, AmazonSFPShipmentEntity shipment)
        {
            AmazonMwsApiCall call = AmazonMwsApiCall.GetEligibleShippingServices;

            IHttpVariableRequestSubmitter request = createVariableRequestSubmitter();

            // Add Shipment Information XML
            AddShipmentRequestDetails(request, requestDetails);

            // Get Response
            IHttpResponseReader response = ExecuteRequest(request, call, settingsFactory.Create(shipment));

            // Deserialize
            return DeserializeResponse<GetEligibleShippingServicesResponse>(response.ReadResult());
        }

        /// <summary>
        /// Create a shipment for the given ShipmentRequestDetails
        /// </summary>
        public AmazonShipment CreateShipment(ShipmentRequestDetails requestDetails, AmazonSFPShipmentEntity shipment, TelemetricResult<IDownloadedLabelData> telemetricResult)
        {
            AmazonMwsApiCall call = AmazonMwsApiCall.CreateShipment;

            IHttpVariableRequestSubmitter request = createVariableRequestSubmitter();

            // Add the service
            request.Variables.Add("ShippingServiceId", shipment.ShippingServiceID);

            // Add Shipment Information XML
            AddShipmentRequestDetails(request, requestDetails);

            // Get Response
            IHttpResponseReader response = ExecuteRequest(request, call, settingsFactory.Create(shipment));
            telemetricResult.AddEntry(TelemetricEventType.GetLabel, response.ResponseTimeInMs);

            // Deserialize
            CreateShipmentResponse createShipmentResponse = DeserializeResponse<CreateShipmentResponse>(response.ReadResult());

            return ValidateCreateShipmentResponse(createShipmentResponse);
        }

        /// <summary>
        /// Cancel Shipment
        /// </summary>
        public CancelShipmentResponse CancelShipment(AmazonSFPShipmentEntity amazonShipment)
        {
            AmazonMwsApiCall call = AmazonMwsApiCall.CancelShipment;

            IHttpVariableRequestSubmitter request = createVariableRequestSubmitter();

            // Add the service
            request.Variables.Add("ShipmentId", amazonShipment.AmazonUniqueShipmentID);

            try
            {
                // Get Response
                IHttpResponseReader response = ExecuteRequest(request, call, settingsFactory.Create(amazonShipment));

                // Deserialize
                return DeserializeResponse<CancelShipmentResponse>(response.ReadResult());
            }
            catch (AmazonSFPShippingException ex)
                when (ex.Code == "InvalidState" &&
                      amazonShipment.CarrierName.StartsWith("dynamex", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new AmazonSFPShippingException("Dynamex shipments cannot be voided electronically. Please contact Dynamex at 855-DYNAMEX or https://www.dynamex.com/contact-us");
            }
        }

        /// <summary>
        /// Validate the CreateShipmentResponse to ensure that it contains a label
        /// </summary>
        private AmazonShipment ValidateCreateShipmentResponse(CreateShipmentResponse createShipmentResponse)
        {
            if (createShipmentResponse?.CreateShipmentResult?.AmazonShipment?.Label?.FileContents == null)
            {
                throw new AmazonSFPShippingException("Amazon failed to return a label for the Shipment.");
            }

            return createShipmentResponse.CreateShipmentResult.AmazonShipment;
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
                    throw new AmazonSFPShippingException(errorResponse.Error.Message, ex);
                }

                throw new AmazonSFPShippingException($"Error Deserializing {typeof(T).Name}", ex);
            }
        }

        /// <summary>
        /// Configures the request with required parameters
        /// </summary>
        /// <param name="request"></param>
        /// <param name="amazonMwsApiCall"></param>
        /// <param name="mwsSettings"></param>
        private void ConfigureRequest(IHttpVariableRequestSubmitter request, AmazonMwsApiCall amazonMwsApiCall, IAmazonMwsWebClientSettings mwsSettings)
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
        private static void AddShipmentRequestDetails(IHttpVariableRequestSubmitter request, ShipmentRequestDetails requestDetails)
        {
            // Order ID
            request.Variables.Add("ShipmentRequestDetails.AmazonOrderId", requestDetails.AmazonOrderId);
            request.Variables.Add("ShipmentRequestDetails.ShipDate", FormatDate(requestDetails.ShipDate));

            AddItemInfo(request, requestDetails);
            AddFromAddressInfo(request, requestDetails);
            AddPackageInfo(request, requestDetails);
            AddShippingServiceOptions(request, requestDetails);
            AddLabelCustomizations(request, requestDetails);
        }

        /// <summary>
        /// Add the shipping service options to the request
        /// </summary>
        private static void AddShippingServiceOptions(IHttpVariableRequestSubmitter request, ShipmentRequestDetails requestDetails)
        {
            // ShippingServiceOptions
            request.Variables.Add("ShipmentRequestDetails.ShippingServiceOptions.CarrierWillPickUp",
                requestDetails.ShippingServiceOptions.CarrierWillPickUp.ToString().ToLowerInvariant());
            request.Variables.Add("ShipmentRequestDetails.ShippingServiceOptions.DeliveryExperience",
                requestDetails.ShippingServiceOptions.DeliveryExperience);

            request.Variables.Add("ShipmentRequestDetails.ShippingServiceOptions.DeclaredValue.Amount",
                requestDetails.ShippingServiceOptions.DeclaredValue.Amount.ToString(CultureInfo.InvariantCulture));
            request.Variables.Add("ShipmentRequestDetails.ShippingServiceOptions.DeclaredValue.CurrencyCode",
                requestDetails.ShippingServiceOptions.DeclaredValue.CurrencyCode);

            // Only request thermal if that is what was selected.  Otherwise, let Amazon figure it out (like we used to do)
            if (requestDetails.ShippingServiceOptions.LabelFormat == "ZPL203")
            {
                request.Variables.Add("ShipmentRequestDetails.ShippingServiceOptions.LabelFormat", requestDetails.ShippingServiceOptions.LabelFormat);
            }
        }

        /// <summary>
        /// Add the label customizations to the request
        /// </summary>
        private static void AddLabelCustomizations(IHttpVariableRequestSubmitter request, ShipmentRequestDetails requestDetails)
        {
            // LabelCustomization
            if (requestDetails.LabelCustomization?.CustomTextForLabel?.IsNullOrWhiteSpace() == false)
            {
                request.Variables.Add("ShipmentRequestDetails.LabelCustomization.CustomTextForLabel",
                    requestDetails.LabelCustomization.CustomTextForLabel.ToLowerInvariant());
            }
        }

        /// <summary>
        /// Add the package info to the request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestDetails"></param>
        private static void AddPackageInfo(IHttpVariableRequestSubmitter request, ShipmentRequestDetails requestDetails)
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
        private static void AddFromAddressInfo(IHttpVariableRequestSubmitter request, ShipmentRequestDetails requestDetails)
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
        private static void AddItemInfo(IHttpVariableRequestSubmitter request, ShipmentRequestDetails requestDetails)
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
        private void AddSignature(IHttpVariableRequestSubmitter request, AmazonMwsApiCall amazonMwsApiCall, IAmazonMwsWebClientSettings mwsSettings)
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
                .Select(v => v.Name + "=" + AmazonMwsSignature.Encode(v.Value ?? string.Empty, false))
                .Aggregate((x, y) => x + "&" + y);

            string parameterString = $"{verbString}\n{request.Uri.Host}\n{endpointPath}\n{queryString}";

            // sign the string and add it to the request
            string signature = RequestSignature.CreateRequestSignature(parameterString, Decrypt(mwsSettings.InterapptiveSecretKey), SigningAlgorithm.SHA256);
            request.Variables.Add("Signature", signature);
        }

        /// <summary>
        /// Executes a request
        /// </summary>
        private IHttpResponseReader ExecuteRequest(IHttpVariableRequestSubmitter request, AmazonMwsApiCall amazonMwsApiCall, IAmazonMwsWebClientSettings mwsSettings)
        {
            // Adds our amazon credentials and other parameters
            // required for each api call
            ConfigureRequest(request, amazonMwsApiCall, mwsSettings);

            // Signs the request
            AddSignature(request, amazonMwsApiCall, mwsSettings);

            // add a User Agent header
            request.Headers.Add("x-amazon-user-agent",
                $"ShipWorks/{GetType().Assembly.GetName().Version} (Language=.NET)");

            // business logic failures are handled through status codes
            request.AllowHttpStatusCodes(HttpStatusCode.BadRequest, HttpStatusCode.NotFound, HttpStatusCode.Forbidden);

            IHttpResponseReader response = null;

            try
            {
                IApiLogEntry logger = createApiLogEntry.GetLogEntry(ApiLogSource.Amazon, mwsSettings.GetActionName(amazonMwsApiCall), LogActionType.Other);

                // log the request
                logger.LogRequest(request);
                using (response = request.GetResponse())
                {
                    // log the response
                    logger.LogResponse(response.ReadResult());
                };

            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(AmazonSFPShippingException));
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
        private string Decrypt(string encrypted) => encryptionProvider.Decrypt(encrypted);

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
                                 Code = (string) e.Element(ns + "Code"),
                                 Message = (string) e.Element(ns + "Message")
                             }).FirstOrDefault();

                if (error != null)
                {
                    // No message was provided so we use the error code
                    throw new AmazonSFPShippingException(error.Message, error.Code);
                }
            }
            catch (XmlException)
            {
                // Throw error because the response is not valid
                throw new AmazonSFPShippingException($"ShipWorks was unable to get a valid response for {api}.");
            }
        }
    }
}