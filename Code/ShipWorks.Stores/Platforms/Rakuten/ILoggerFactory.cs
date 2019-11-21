using RestSharp;
using ShipWorks.ApplicationCore.Logging;

namespace ShipWorks.Stores.Platforms.Rakuten
{
    public interface ILoggerFactory
    {
        void LogRequest(IRestRequest request, IRestClient client, string extension);
        void LogResponse(IRestResponse response, string extension);
    }
}
