using Interapptive.Shared.Utility;

namespace Interapptive.Shared.Net
{
    /// <summary>
    /// HTTP Validator
    /// </summary>
    public interface IHttpValidator
    {
        /// <summary>
        /// Validate Port
        /// </summary>
        GenericResult<long> ValidatePort(string port);
        
        /// <summary>
        /// Validate IP Address
        /// </summary>
        GenericResult<string> ValidateIPAddress(string ipAddress);
    }
}