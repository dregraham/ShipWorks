using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.BigCommerce.OnlineUpdating
{
    /// <summary>
    /// Item loader for BigCommerce online updater
    /// </summary>
    public interface IItemLoader
    {
        /// <summary>
        /// Load items
        /// </summary>
        Task<GenericResult<OnlineItems>> LoadItems(
            IEnumerable<IOrderItemEntity> orderItems,
            string orderNumberComplete,
            long orderNumber,
            IBigCommerceWebClient webClient);

        /// <summary>
        /// Get the shipping method used for a shipment
        /// </summary>
        Tuple<string, string> GetShippingMethod(ShipmentEntity shipment);
    }
}