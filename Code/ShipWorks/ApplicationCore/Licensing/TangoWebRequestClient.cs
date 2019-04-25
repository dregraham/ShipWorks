﻿using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Xml;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Licensing.WebClientEnvironments;
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
        private readonly ILogEntryFactory logEntryFactory;
        private readonly ITangoSecurityValidator securityValidator;
        private readonly WebClientEnvironment webClientEnvironment;

        /// <summary>
        /// Constructor
        /// </summary>
        public TangoWebRequestClient(
            ILogEntryFactory logEntryFactory,
            WebClientEnvironmentFactory webClientEnvironmentFactory)
        {
            this.logEntryFactory = logEntryFactory;
            webClientEnvironment = webClientEnvironmentFactory.SelectedEnvironment;
            securityValidator = webClientEnvironment.TangoSecurityValidator;
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
        public GenericResult<T> ProcessXmlRequest<T>(IHttpVariableRequestSubmitter postRequest, string logEntryName, bool collectTelemetry) =>
            ProcessRequest(postRequest, logEntryName, collectTelemetry)
                .Bind(DeserializeXml<T>);

        /// <summary>
        /// Process the given request against the interapptive license server
        /// </summary>
        public GenericResult<string> ProcessRequest(IHttpVariableRequestSubmitter postRequest, string logEntryName, bool collectTelemetry)
        {
            ApiLogEntry logEntry = new ApiLogEntry(ApiLogSource.ShipWorks, logEntryName);
            ConfigureRequest(postRequest, logEntry);

            string getActionName() => postRequest.Variables["action"] ?? logEntryName;
            TelemetricResult<Unit> telemetricResult = new TelemetricResult<Unit>("Tango.Request");

            try
            {
                return Using(
                    GetTelemetryEvent(getActionName, collectTelemetry, telemetricResult),
                    _ => PerformRequest(postRequest, logEntry, telemetricResult));
            }
            catch (Exception ex)
            {
                if (WebHelper.IsWebException(ex))
                {
                    return new TangoException("An error occurred connecting to the ShipWorks server:\n\n" + ex.Message, ex);
                }

                return ex;
            }
        }

        /// <summary>
        /// Get the telemetry event
        /// </summary>
        private static IDisposable GetTelemetryEvent(Func<string> getActionName, bool collectTelemetry, TelemetricResult<Unit> telemetryResult) =>
            collectTelemetry ? BuildTrackedDurationEvent(getActionName, telemetryResult) : null;

        /// <summary>
        /// Build the tracked duration event for telemetry
        /// </summary>
        private static IDisposable BuildTrackedDurationEvent(Func<string> getActionName, TelemetricResult<Unit> telemetryResult)
        {
            var trackedDurationEvent = new TrackedDurationEvent("Tango.Request");
            trackedDurationEvent?.AddProperty("Tango.Request.Action", getActionName());

            return Disposable.Create(() =>
            {
                telemetryResult.WriteTo(trackedDurationEvent);
                trackedDurationEvent.Dispose();
            });
        }

        /// <summary>
        /// Perform the actual request
        /// </summary>
        private GenericResult<string> PerformRequest(IHttpVariableRequestSubmitter postRequest, ApiLogEntry logEntry, TelemetricResult<Unit> telemetricResult) =>
            securityValidator
                .ValidateSecureConnection(telemetricResult, postRequest.Uri)
                .Map(() => telemetricResult.RunTimedEvent("ActualRequest", postRequest.GetResponse))
                .Bind(result => ParseValidatedResponse(telemetricResult, result))
                .Do(response => telemetricResult.RunTimedEvent("LogResponse", () => logEntry.LogResponse(response)));

        /// <summary>
        /// Parse the validated response
        /// </summary>
        private GenericResult<string> ParseValidatedResponse(TelemetricResult<Unit> telemetricResult, IHttpResponseReader responseReader) =>
            Using(responseReader,
                x =>
                {
                    var result = x.ReadResult().Trim();
                    return securityValidator.ValidateCertificate(telemetricResult, x.HttpWebRequest)
                        .Map(() => result);
                });

        /// <summary>
        /// Validate a secure connection, forcing it if necessary
        /// </summary>
        private Result ValidateSecureConnection(TelemetricResult<Unit> telemetricResult, IHttpVariableRequestSubmitter postRequest) =>
            postRequest.ForcePreCallCertificateValidation ?
                securityValidator.ForceValidateSecureConnection(telemetricResult, postRequest.Uri) :
                securityValidator.ValidateSecureConnection(telemetricResult, postRequest.Uri);

        /// <summary>
        /// configure the post request
        /// </summary>
        private void ConfigureRequest(IHttpVariableRequestSubmitter postRequest, ApiLogEntry logEntry)
        {
            postRequest.Timeout = TimeSpan.FromSeconds(60);
            postRequest.Uri = new Uri(webClientEnvironment.TangoUrl);
            postRequest.ForcePreCallCertificateValidation = webClientEnvironment.ForcePreCallCertificationValidation;

            logEntry.LogRequest(postRequest);

            postRequest.RequestSubmitting += delegate (object sender, HttpRequestSubmittingEventArgs e)
            {
                e.HttpWebRequest.KeepAlive = false;
                e.HttpWebRequest.UserAgent = "shipworks";
                e.HttpWebRequest.Headers.Add("X-SHIPWORKS-VERSION", TangoWebClient.Version);
                e.HttpWebRequest.Headers.Add("X-SHIPWORKS-USER", webClientEnvironment.HeaderShipWorksUsername);
                e.HttpWebRequest.Headers.Add("X-SHIPWORKS-PASS", webClientEnvironment.HeaderShipWorksPassword);
                e.HttpWebRequest.Headers.Add("SOAPAction", webClientEnvironment.SoapAction);
            };
        }
    }
}
