using System.Threading.Tasks;
using ShipWorks.Stores.Platforms.ShipEngine.Apollo;

namespace ShipWorks.Stores.Platforms.ShipEngine
{
    public interface IPlatformOrderWebClient
    {
        Task<PaginatedPlatformServiceResponseOfOrderSourceApiSalesOrder> GetOrders(string orderSourceId, string continuationToken);
    }
}