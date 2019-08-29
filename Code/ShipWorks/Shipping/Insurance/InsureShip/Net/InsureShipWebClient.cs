﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using log4net;
using Newtonsoft.Json;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Insurance.InsureShip.Net.Insure;

namespace ShipWorks.Shipping.Insurance.InsureShip.Net
{
    /// <summary>
    /// Simple web client for InsureShip
    /// </summary>
    [Component]
    public class InsureShipWebClient : IInsureShipWebClient
    {
        private readonly ILog log;
        private readonly IInsureShipSettings settings;
        private readonly ILogEntryFactory logEntryFactory;
        private readonly IHttpRequestSubmitterFactory requestSubmitterFactory;
        private readonly IInsureShipCredentialRetriever credentialRetriever;

        /// <summary>
        /// Constructor
        /// </summary>
        public InsureShipWebClient(
            IInsureShipSettings settings,
            IHttpRequestSubmitterFactory requestSubmitterFactory,
            IInsureShipCredentialRetriever credentialRetriever,
            ILogEntryFactory logEntryFactory,
            Func<Type, ILog> createLog)
        {
            this.credentialRetriever = credentialRetriever;
            this.requestSubmitterFactory = requestSubmitterFactory;
            this.settings = settings;
            this.logEntryFactory = logEntryFactory;
            log = createLog(GetType());
        }

        /// <summary>
        /// Submits this request to InsureShip
        /// </summary>
        public GenericResult<T> Submit<T>(string endpoint, IStoreEntity store, Dictionary<string, string> postData)
        {
#if !DEBUG
            // Confirm that the connection with InsureShip has not been compromised
            EnsureSecureConnection();
#endif

            Uri uri = new Uri(settings.ApiUrl.AbsoluteUri + endpoint);
            var requestSubmitter = ConfigureNewRequestSubmitter(store, uri);

            foreach (string key in postData.Keys)
            {
                requestSubmitter.AddVariable(key, postData[key]);
            }

            var logEntry = logEntryFactory.GetLogEntry(ApiLogSource.InsureShip, endpoint, LogActionType.Other);

            try
            {
                logEntry.LogRequest(requestSubmitter);

                var response = requestSubmitter.GetResponse();
                var responseText = response.ReadResult();

                LogInsureShipResponse(logEntry, response.HttpWebResponse, responseText);

                var deserializedResponse = JsonConvert.DeserializeObject<T>(responseText);

                // Log telemetry when insuring a shipment
                if (typeof(T) == typeof(InsureShipNewPolicyResponse))
                {
                    LogTelemetry(response.HttpWebResponse, deserializedResponse as InsureShipNewPolicyResponse);
                }

                return deserializedResponse;
            }
            catch (WebException ex)
            {
                log.Error(ex);

                HttpWebResponse httpWebResponse = ex.Response as HttpWebResponse;
                var responseText = ReadResponse(httpWebResponse);

                LogInsureShipResponse(logEntry, httpWebResponse, responseText);
                return ex;
            }
            catch (ArgumentNullException ex)
            {
                // Try to deserialize null
                return ex;
            }
            catch (JsonException ex)
            {
                return ex;
            }
        }

        /// <summary>
        /// Configures a new request submitter for the given URI, setting up the common headers and
        /// the allowable HTTP status codes.
        /// </summary>
        /// <param name="uri">The URI.</param>
        protected IHttpVariableRequestSubmitter ConfigureNewRequestSubmitter(IStoreEntity store, Uri uri)
        {
            var requestSubmitter = requestSubmitterFactory.GetHttpVariableRequestSubmitter();

            requestSubmitter.Uri = uri;
            requestSubmitter.AllowHttpStatusCodes(GetAllowedCodes());

            var credentials = credentialRetriever.Get(store);

            return requestSubmitter
                .AddHeader("Accept", "application/json")
                .AddHeader("X-ShipWorks-Version", TangoWebClient.Version)
                .AddVariable("client_id", credentials.ClientID)
                .AddVariable("api_key", credentials.ApiKey);
        }

        /// <summary>
        /// Ensures that a secure connection is made with InsureShip.
        /// </summary>
        /// <exception cref="InsureShipException"></exception>
        protected virtual void EnsureSecureConnection()
        {
            if (!IsTrustedCertificate())
            {
                const string message = "A trusted connection to InsureShip could not be established.";

                log.Error(message);
                throw new InsureShipException(message);
            }
        }

        /// <summary>
        /// Determines whether the connection to InsureShip is secure by inspecting the certificate.
        /// </summary>
        /// <returns></returns>
        private bool IsTrustedCertificate()
        {
            ICertificateInspector certificateInspector = new CertificateInspector(TangoCredentialStore.Instance.InsureShipCertificateVerificationData);
            CertificateRequest request = new CertificateRequest(settings.CertificateUrl, certificateInspector);

            return request.Submit() == CertificateSecurityLevel.Trusted;
        }

        /// <summary>
        /// Get the allowed HTTP status codes to the request based on the response
        /// codes we are expecting from the InsureShip API.
        /// </summary>
        private static HttpStatusCode[] GetAllowedCodes() =>
            Enum.GetValues(typeof(InsureShipResponseCode))
                .OfType<Enum>()
                .Select(EnumHelper.GetApiValue)
                .Select(int.Parse)
                .Cast<HttpStatusCode>()
                .ToArray();

        /// <summary>
        /// Gets the content of the response.
        /// </summary>
        protected string ReadResponse(HttpWebResponse response)
        {
            using (Stream responseStream = response?.GetResponseStream())
            {
                if (responseStream?.CanRead == true)
                {
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Logs the response from InsureShip.
        /// </summary>
        /// <param name="response">The response.</param>
        protected void LogInsureShipResponse(IApiLogEntry logEntry, HttpWebResponse response, string content)
        {
            if (response != null)
            {
                StringBuilder responseText = new StringBuilder();

                responseText.AppendLine(string.Format("{0} {1}", (int) response.StatusCode, response.StatusCode.ToString()));
                responseText.AppendLine(response.Headers.ToString());
                responseText.AppendLine(content);

                logEntry.LogResponse(responseText.ToString(), "log");
            }
        }

        /// <summary>
        /// Log telemetry when insuring a shipment
        /// </summary>
        private void LogTelemetry(HttpWebResponse response, InsureShipNewPolicyResponse responseObject)
        {
            using (ITrackedEvent telemetricEvent = new TrackedEvent("Shipping.InsureShip.Response"))
            {
                telemetricEvent.AddProperty("Status", responseObject.Status);
                telemetricEvent.AddProperty("HttpStatusCode", response.StatusCode.ToString());
                telemetricEvent.AddProperty("PolicyNumber", responseObject.PolicyID.ToString());
            }
        }
    }
}
