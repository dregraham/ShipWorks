using System.Net;
using System.Text.RegularExpressions;
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
        private const string IpRegEx = @"^(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])$";


        /// <summary>
        /// Validate Port
        /// </summary>
        public GenericResult<long> ValidatePort(string port) =>
            !string.IsNullOrWhiteSpace(port) && long.TryParse(port.Trim().TrimStart('0'), out long portNumber) && portNumber >= MinPort && portNumber <= MaxPort
                ? GenericResult.FromSuccess(portNumber)
                : GenericResult.FromError<long>($"Please enter a valid port number between {MinPort} and {MaxPort}.");

        /// <summary>
        /// Validate IP Address
        /// </summary>
        public GenericResult<string> ValidateIPAddress(string ipAddress)
        {
            var isValid = !string.IsNullOrWhiteSpace(ipAddress);

            if (isValid && !Regex.IsMatch(ipAddress.Trim(), IpRegEx))
            {
                isValid = false;
            }
            
            if (isValid && IPAddress.TryParse(ipAddress.Trim(), out IPAddress validIPAddress))
            {
                return GenericResult.FromSuccess(validIPAddress.ToString());
            }

            return GenericResult.FromError<string>("Please enter a valid IP Address.");
        }
    }
}