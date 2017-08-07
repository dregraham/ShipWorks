using System.Collections.Generic;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Jet.DTO;

namespace ShipWorks.Stores.Platforms.Jet
{
    /// <summary>
    /// Interface for JetWebClient
    /// </summary>
    public interface IJetWebClient
    {
        /// <summary>
        /// Get orders jet
        /// </summary>
        GenericResult<JetOrderResponse> GetOrders(JetStoreEntity store);

        /// <summary>
        /// Get a jet product for the given item
        /// </summary>
        GenericResult<JetProduct> GetProduct(JetOrderItem item, JetStoreEntity store);

        /// <summary>
        /// Acknowledge the given order
        /// </summary>
        void Acknowledge(JetOrderEntity order, JetStoreEntity store);

        /// <summary>
        /// Get order details for the given order
        /// </summary>
        GenericResult<JetOrderDetailsResult> GetOrderDetails(string orderUrl, JetStoreEntity store);
    }
}
