using System;
using System.Reactive;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Throttle tango security checks
    /// </summary>
    public interface ITangoSecurityValidator
    {
        /// <summary>
        /// Validate a secure connection
        /// </summary>
        Result ValidateSecureConnection(TelemetricResult<Unit> telemetricResult, Uri uri);

        /// <summary>
        /// Validate a secure connection
        /// </summary>
        Result ForceValidateSecureConnection(TelemetricResult<Unit> telemetricResult, Uri uri);

        /// <summary>
        /// Ensure the connection to the given URI is a valid interapptive secure connection
        /// </summary>
        GenericResult<IHttpResponseReader> ValidateCertificate(TelemetricResult<Unit> telemetricResult, IHttpResponseReader responseReader);
    }
}
