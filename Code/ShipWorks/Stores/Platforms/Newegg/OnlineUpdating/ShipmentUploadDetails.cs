using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Collections;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.Newegg.OnlineUpdating
{
    /// <summary>
    /// Shipment details upload data
    /// </summary>
    public class ShipmentUploadDetails
    {
        private readonly OrderEntity order;
        private readonly IDictionary<long, IEnumerable<ItemDetails>> items;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentUploadDetails(OrderEntity order, IEnumerable<OrderUploadDetail> identifiers, IDictionary<long, IEnumerable<ItemDetails>> items)
        {
            this.order = order;
            Identifiers = identifiers.ToReadOnly();
            this.items = items;
        }

        /// <summary>
        /// All the orders are manual
        /// </summary>
        public bool AllOrdersManual => Identifiers.All(x => x.IsManual);

        /// <summary>
        /// Original order requested to be updated
        /// </summary>
        public IOrderEntity Order => order;

        /// <summary>
        /// Get all the identifiers for the order
        /// </summary>
        public IEnumerable<OrderUploadDetail> Identifiers { get; }

        /// <summary>
        /// Get items for the given identifier
        /// </summary>
        public IEnumerable<ItemDetails> GetItemsFor(OrderUploadDetail detail) =>
            items.ContainsKey(detail.OrderID) ?
                items[detail.OrderID] :
                Enumerable.Empty<ItemDetails>();
    }
}