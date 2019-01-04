using System;
using System.Net;
using System.Reactive;
using System.Security.Cryptography.X509Certificates;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Throttle tango security checks
    /// </summary>
    [Component(SingleInstance = true)]
    public class TangoSecurityValidator : ITangoSecurityValidator
    {
        private const int throttlePeriod = 15;
        private DateTime nextSecureConnectionValidation = DateTime.MinValue;
        private readonly IDateTimeProvider dateTimeProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public TangoSecurityValidator(IDateTimeProvider dateTimeProvider)
        {
            this.dateTimeProvider = dateTimeProvider;
        }

        /// <summary>
        /// Validate a secure connection
        /// </summary>
        public Result ValidateSecureConnection(TelemetricResult<Unit> telemetricResult, Uri uri)
        {
            // First validate that we are connecting to interapptive, and not a fake redirect to steal passwords and such.  Doing this pre-call
            // also prevents stealing the headers user\pass with fiddler
            if (nextSecureConnectionValidation < dateTimeProvider.UtcNow)
            {
                return telemetricResult.RunTimedEvent("ValidateSecureConnection", () => ValidateSecureConnection(uri))
                    .Do(() => nextSecureConnectionValidation = dateTimeProvider.UtcNow.AddMinutes(throttlePeriod))
                    .Do(() => telemetricResult.AddProperty("ValidateSecureConnection.IsValidCertificate", "Yes"))
                    .OnFailure(_ => telemetricResult.AddProperty("ValidateSecureConnection.IsValidCertificate", "No"));
            }

            return Result.FromSuccess();
        }

        /// <summary>
        /// Validate the certificate of a response
        /// </summary>
        public GenericResult<IHttpResponseReader> ValidateCertificate(TelemetricResult<Unit> telemetricResult, IHttpResponseReader responseReader) =>
            telemetricResult
                .RunTimedEvent("ValidateInterapptiveCertificate", () => ValidateInterapptiveCertificate(responseReader.HttpWebRequest))
                .Do(() => telemetricResult.AddProperty("ValidateInterapptiveCertificate.IsValidCertificate", "Yes"))
                .OnFailure(_ => telemetricResult.AddProperty("ValidateInterapptiveCertificate.IsValidCertificate", "No"))
                .Map(() => responseReader);

        /// <summary>
        /// Ensure the connection to the given URI is a valid interapptive secure connection
        /// </summary>
        public static Result ValidateSecureConnection(Uri uri)
        {
#if !DEBUG
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(uri);
            request.KeepAlive = false;
            request.UserAgent = "shipworks";

            using (WebResponse response = request.GetResponse())
            {
                return ValidateInterapptiveCertificate(request);
            }
#else
            return Result.FromSuccess();
#endif
        }

        /// <summary>
        /// Validate that there is an accurate interapptive certificate attached to the web request
        /// </summary>
        private static Result ValidateInterapptiveCertificate(HttpWebRequest httpWebRequest)
        {
#pragma warning disable 168
            X509Certificate certificate;
#pragma warning restore 168

#if !DEBUG
            if (httpWebRequest.ServicePoint == null)
            {
                return new TangoException("The SSL certificate on the server is invalid.");
            }

            certificate = httpWebRequest.ServicePoint.Certificate;

            if (certificate == null)
            {
                return new TangoException("The SSL certificate on the server is invalid.");
            }

            if (certificate.Subject.IndexOf("www.interapptive.com") == -1 ||
                certificate.Subject.IndexOf("Interapptive, Inc") == -1)
            {
                return new TangoException("The SSL certificate on the server is invalid.");
            }
#endif
            return Result.FromSuccess();
        }
    }
}
