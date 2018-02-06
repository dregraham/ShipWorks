using ShipWorks.Stores.Orders.Combine;

namespace ShipWorks.Stores.Platforms.NetworkSolutions.OnlineUpdating
{
    /// <summary>
    /// Combined order search provider for NetworkSolutions
    /// </summary>
    public interface INetworkSolutionsCombineOrderSearchProvider : ICombineOrderSearchProvider<long>
    {
    }
}