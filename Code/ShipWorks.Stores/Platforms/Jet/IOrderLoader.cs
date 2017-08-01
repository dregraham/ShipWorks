using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Jet.DTO;

namespace ShipWorks.Stores.Platforms.Jet
{
    /// <summary>
    /// Interface to load order data from jet
    /// </summary>
    public interface IOrderLoader
    {
        /// <summary>
        /// Load the OrderEntity with the JetOrderDetails
        /// </summary>
        void LoadOrder(OrderEntity order, JetOrderDetail jetOrder);
    }
}