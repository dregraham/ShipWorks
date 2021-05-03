using System.Net;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;

namespace Interapptive.Shared.Net
{
    /// <summary>
    /// HTTP Validator
    /// </summary>
    [Component]
    public class HttpValidator : IHttpValidator
    {
        private const int MinPort = 1024;
        private const int MaxPort = 65535;

        /// <summary>
        /// Validate Port
        /// </summary>
        public GenericResult<long> ValidatePort(string port) =>
            long.TryParse(port.Trim().TrimStart('0'), out long portNumber) && portNumber >= MinPort && portNumber <= MaxPort
                ? GenericResult.FromSuccess(portNumber)
                : GenericResult.FromError<long>($"Please enter a valid port number between {MinPort} and {MaxPort}.");

        /// <summary>
        /// Validate IP Address
        /// </summary>
        public GenericResult<string> ValidateIPAddress(string ipAddress) =>
            IPAddress.TryParse(ipAddress.Trim(), out IPAddress validIPAddress)
                ? GenericResult.FromSuccess(validIPAddress.ToString())
                : GenericResult.FromError<string>("Please enter a valid IP Address.");
    }
}