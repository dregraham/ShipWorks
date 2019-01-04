using System;
using System.Net;
using System.Reactive;
using System.Security.Cryptography.X509Certificates;
using Interapptive.Shared.ComponentRegistration;
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
        /// <remarks>
        /// First validate that we are connecting to interapptive, and not a fake redirect to steal passwords and such.  Doing this pre-call
        /// also prevents stealing the headers user\pass with fiddler
        /// </remarks>
        public Result ValidateSecureConnection(TelemetricResult<Unit> telemetricResult, Uri uri)
        {
            if (nextSecureConnectionValidation < dateTimeProvider.UtcNow)
            {
                return ForceValidateSecureConnection(telemetricResult, uri)
                    .Do(() => nextSecureConnectionValidation = dateTimeProvider.UtcNow.AddMinutes(throttlePeriod));
            }

            return Result.FromSuccess();
        }

        /// <summary>
        /// Validate a secure connection
        /// </summary>
        /// <remarks>
        /// First validate that we are connecting to interapptive, and not a fake redirect to steal passwords and such.  Doing this pre-call
        /// also prevents stealing the headers user\pass with fiddler
        /// </remarks>
        public Result ForceValidateSecureConnection(TelemetricResult<Unit> telemetricResult, Uri uri) =>
            PerformValidation("ValidateSecureConnection", telemetricResult, () => ValidateSecureConnection(uri));

        /// <summary>
        /// Validate the certificate of a response
        /// </summary>
        public Result ValidateCertificate(TelemetricResult<Unit> telemetricResult, HttpWebRequest request) =>
            PerformValidation("ValidateInterapptiveCertificate", telemetricResult, () => ValidateInterapptiveCertificate(request));

        /// <summary>
        /// Perform the validation on an action that returns a response
        /// </summary>
        /// <param name="checkName"></param>
        /// <param name="telemetricResult"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        private Result PerformValidation(string checkName, TelemetricResult<Unit> telemetricResult, Func<Result> func) =>
            telemetricResult
                .RunTimedEvent(checkName, func)
                .Do(() => telemetricResult.AddProperty($"{checkName}.IsValidCertificate", "Yes"))
                .OnFailure(_ => telemetricResult.AddProperty($"{checkName}.IsValidCertificate", "No"))
                .OnFailure(ex => nextSecureConnectionValidation = DateTime.MinValue);

        /// <summary>
        /// Ensure the connection to the given URI is a valid interapptive secure connection
        /// </summary>
        public Result ValidateSecureConnection(Uri uri)
        {
#if !DEBUG
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(uri);
            request.KeepAlive = false;
            request.UserAgent = "shipworks";

            return Using(request.GetResponse(), _ => ValidateInterapptiveCertificate(request));
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
