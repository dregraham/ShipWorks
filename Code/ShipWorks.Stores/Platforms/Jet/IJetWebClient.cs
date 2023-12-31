﻿using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
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
        GenericResult<JetOrderResponse> GetOrders(IJetStoreEntity store);

        /// <summary>
        /// Get a jet product for the given item
        /// </summary>
        GenericResult<JetProduct> GetProduct(JetOrderItem item, IJetStoreEntity store);

        /// <summary>
        /// Acknowledge the given order
        /// </summary>
        void Acknowledge(JetOrderDetailsResult order, IJetStoreEntity store);

        /// <summary>
        /// Get order details for the given order
        /// </summary>
        GenericResult<JetOrderDetailsResult> GetOrderDetails(string orderUrl, IJetStoreEntity store);

        /// <summary>
        /// Uploads the shipment details.
        /// </summary>
        void UploadShipmentDetails(string merchantOrderId, ShipmentEntity shipment, IJetStoreEntity store);
    }
}
