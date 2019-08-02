using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.OrderLookup.ScanPack
{
    /// <summary>
    /// Represents a factory for creating scan pack items
    /// </summary>
    public interface IScanPackItemFactory
    {
        /// <summary>
        /// Create a collection of ScanPackItems based on an order
        /// </summary>
        Task<List<ScanPackItem>> Create(OrderEntity order);
    }
}
