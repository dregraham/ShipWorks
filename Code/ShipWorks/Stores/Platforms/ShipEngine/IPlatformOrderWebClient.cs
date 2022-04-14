using System.Threading.Tasks;

namespace ShipWorks.Stores.Platforms.ShipEngine
{
    public interface IPlatformOrderWebClient
    {
        Task<object> GetOrders(string orderSourceId, string continuationToken);
    }
}