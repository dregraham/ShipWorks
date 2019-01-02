using System;
using System.Linq;
using System.Reactive;
using System.Xml;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Net;
using Interapptive.Shared.Security;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Logging;
using static Interapptive.Shared.Utility.Functional;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Client for making Tango web requests
    /// </summary>
    [Component(RegistrationType.Self)]
    public class TangoWebRequestClient : ITangoWebRequestClient
    {
        private readonly IEncryptionProvider encryptionProvider;
        private readonly ILogEntryFactory logEntryFactory;
        private readonly ITangoSecurityValidator securityValidator;

        /// <summary>
        /// Constructor
        /// </summary>
        public TangoWebRequestClient(
            IEncryptionProviderFactory encryptionProviderFactory,
            ILogEntryFactory logEntryFactory,
            ITangoSecurityValidator securityValidator)
        {
            this.securityValidator = securityValidator;
            this.logEntryFactory = logEntryFactory;
            encryptionProvider = encryptionProviderFactory.CreateSecureTextEncryptionProvider("interapptive");
        }

        /// <summary>
        /// Process the given request against the interapptive license server
        /// </summary>
        public GenericResult<XmlDocument> ProcessXmlRequest(IHttpVariableRequestSubmitter postRequest, string logEntryName, bool collectTelemetry) =>
            ProcessRequest(postRequest, logEntryName, collectTelemetry)
                .Bind(ParseXml);

        /// <summary>
        /// Process the given request against the interapptive license server
        /// </summary>
        public GenericResult<string> ProcessRequest(IHttpVariableRequestSubmitter postRequest, string logEntryName, bool collectTelemetry)
        {
            ApiLogEntry logEntry = new ApiLogEntry(ApiLogSource.ShipWorks, logEntryName);
            ConfigureRequest(postRequest, logEntry);

            IHttpResponseReader postResponse = null;
            TrackedDurationEvent telemetryEvent = null;

            try
            {
                if (collectTelemetry)
                {
                    telemetryEvent = new TrackedDurationEvent("Tango.Request");
                }

                TelemetricResult<Unit> telemetricResult = new TelemetricResult<Unit>("Tango.Request");

                string action = postRequest.Variables.FirstOrDefault(v => v.Name.Equals("action", StringComparison.InvariantCultureIgnoreCase))?.Value ?? logEntryName;
                telemetryEvent?.AddProperty("Tango.Request.Action", action);

                securityValidator.ValidateSecureConnection(telemetricResult, postRequest.Uri);

                telemetricResult.RunTimedEvent("ActualRequest", () => postResponse = postRequest.GetResponse());

                // Ensure the site has a valid interapptive secure certificate
                securityValidator.ValidateCertificate(telemetricResult, postResponse.HttpWebRequest);

                string result = postResponse.ReadResult().Trim();

                telemetricResult.RunTimedEvent("LogResponse", () => logEntry.LogResponse(result));

                telemetricResult.WriteTo(telemetryEvent);

                return result;
            }
            catch (Exception ex)
            {
                if (WebHelper.IsWebException(ex))
                {
                    return new TangoException("An error occurred connecting to the ShipWorks server:\n\n" + ex.Message, ex);
                }

                return ex;
            }
            finally
            {
                postResponse?.Dispose();
                telemetryEvent?.Dispose();
            }
        }

        /// <summary>
        /// configure the post request
        /// </summary>
        private void ConfigureRequest(IHttpVariableRequestSubmitter postRequest, ApiLogEntry logEntry)
        {
            postRequest.Timeout = TimeSpan.FromSeconds(60);
            postRequest.Uri = new Uri("https://www.interapptive.com/ShipWorksNet/ShipWorksV1.svc/account/shipworks");

            logEntry.LogRequest(postRequest);

            postRequest.RequestSubmitting += delegate (object sender, HttpRequestSubmittingEventArgs e)
            {
                e.HttpWebRequest.KeepAlive = false;

                e.HttpWebRequest.UserAgent = "shipworks";
                e.HttpWebRequest.Headers.Add("X-SHIPWORKS-VERSION", TangoWebClient.Version);

                e.HttpWebRequest.Headers.Add("X-SHIPWORKS-USER", encryptionProvider.Decrypt("C5NOiKdNaM/324R7sIjFUA=="));
                e.HttpWebRequest.Headers.Add("X-SHIPWORKS-PASS", encryptionProvider.Decrypt("lavEgsQoKGM="));
                e.HttpWebRequest.Headers.Add("SOAPAction", "http://stamps.com/xml/namespace/2015/06/shipworks/shipworksv1/IShipWorks/ShipworksPost");
            };
        }
    }
}
