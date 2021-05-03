using Interapptive.Shared.Utility;

namespace Interapptive.Shared.Net
{
    public interface IHttpValidator
    {
        GenericResult<long> ValidatePort(string port);
        
        GenericResult<string> ValidateIPAddress(string ipAddress);
    }
}