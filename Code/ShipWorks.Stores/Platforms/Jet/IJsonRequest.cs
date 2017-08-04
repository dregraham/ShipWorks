using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore.Logging;

namespace ShipWorks.Stores.Platforms.Jet
{
    public interface IJsonRequest
    {
        T ProcessRequest<T>(string action, ApiLogSource logSource, IHttpRequestSubmitter request);
    }
}