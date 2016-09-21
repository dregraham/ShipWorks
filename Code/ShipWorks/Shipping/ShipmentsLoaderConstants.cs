using System.Collections.Generic;
using System.Linq;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Constants for the shipment loader
    /// </summary>
    public static class ShipmentsLoaderConstants
    {
        private static readonly IEnumerable<int> maxAllowedOrderOptions = new[] { 500, 1000, 5000, 10000, 50000, 100000 };

        /// <summary>
        /// The maximum number of orders that we support loading at a time.
        /// </summary>
        public static int MaxAllowedOrders
        {
            get
            {
                int? maxAllowedOrders = ShippingSettings.FetchReadOnly()?.ShipmentEditLimit;

                return maxAllowedOrders.HasValue && MaxAllowedOrderOptions.Contains(maxAllowedOrders.Value) ?
                    maxAllowedOrders.Value :
                    DefaultMaxAllowedOrders;
            }
        }

        /// <summary>
        /// Default max allowed orders
        /// </summary>
        public static int DefaultMaxAllowedOrders => MaxAllowedOrderOptions.Last();

        /// <summary>
        /// Valid options for the max allowed orders
        /// </summary>
        public static IEnumerable<int> MaxAllowedOrderOptions => maxAllowedOrderOptions;
    }
}
