using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;

namespace Interapptive.Shared.Net
{
    [Component]
    public class HttpValidator : IHttpValidator
    {
        private const int MinPort = 1024;
        private const int MaxPort = 65535;

        public GenericResult<long> ValidatePort(string port)
        {
            string trimmedPortNumber = port.Trim().TrimStart('0');
            
            return long.TryParse(trimmedPortNumber, out long portNumber) && portNumber >= MinPort && portNumber <= MaxPort
                ? GenericResult.FromSuccess(portNumber)
                : GenericResult.FromError<long>($"Please enter a valid port number between {MinPort} and {MaxPort}.");
        }

        public GenericResult<string> ValidateIPAddress(string ipAddress)
        {
            string trimmedIPAddress = ipAddress.Trim();
            var chunks = trimmedIPAddress.Split('.');

            return chunks.Length == 4 && chunks.All(x => byte.TryParse(x, out byte _))
                ? GenericResult.FromSuccess(ipAddress)
                : GenericResult.FromError<string>("Please enter a valid IP Address.");
        }
    }
}