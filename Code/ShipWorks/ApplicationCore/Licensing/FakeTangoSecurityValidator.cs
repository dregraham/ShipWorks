using System;
using System.Net;
using System.Reactive;
using Interapptive.Shared.Utility;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Fake Tango security validator that always returns success
    /// </summary>
    public class FakeTangoSecurityValidator : ITangoSecurityValidator
    {
        /// <summary>
        /// Validate a secure connection
        /// </summary>
        /// <remarks>
        /// Always returns success
        /// </remarks>
        public Result ValidateSecureConnection(TelemetricResult<Unit> telemetricResult, Uri uri)
        {
            return Result.FromSuccess();
        }

        /// <summary>
        /// Validate a secure connection
        /// </summary>
        /// <remarks>
        /// Always returns success
        /// </remarks>
        public Result ForceValidateSecureConnection(TelemetricResult<Unit> telemetricResult, Uri uri)
        {
            return Result.FromSuccess();
        }

        /// <summary>
        /// Validate a certificate
        /// </summary>
        /// <remarks>
        /// Always returns success
        /// </remarks>
        public Result ValidateCertificate(TelemetricResult<Unit> telemetricResult, HttpWebRequest request)
        {
            return Result.FromSuccess();
        }
    }
}
