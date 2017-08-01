using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Jet.DTO;

namespace ShipWorks.Stores.Platforms.Jet
{
    /// <summary>
    /// Loads Jet Order Items into entities
    /// </summary>
    public interface IJetOrderItemLoader
    {
        /// <summary>
        /// Load the Jet order items into the order entity
        /// </summary>
        void LoadItems(JetOrderEntity orderEntity, JetOrderDetailsResult orderDto);
    }
}