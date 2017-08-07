using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Jet
{
    public interface IJetAuthenticatedRequest
    {
        GenericResult<T> ProcessRequest<T>(string action, IHttpRequestSubmitter request, JetStoreEntity store, bool generateNewTokenIfExpired = true);
    }
}