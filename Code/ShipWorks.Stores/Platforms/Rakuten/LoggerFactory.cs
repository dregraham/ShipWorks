using RestSharp;
using ShipWorks.ApplicationCore.Logging;

namespace ShipWorks.Stores.Platforms.Rakuten
{
    public class LoggerFactory
    {
        private readonly ApiLogEntry log;

        public LoggerFactory(ApiLogSource source, string name)
        {
            log = new ApiLogEntry(source, name);
        }
        public void LogRequest(IRestRequest request, IRestClient client, string extension)
        {
            log.LogRequest(request, client, extension);
        }
        public void LogResponse(IRestResponse response, string extension)
        {
            log.LogResponse(response, extension);
        }
    }
}
