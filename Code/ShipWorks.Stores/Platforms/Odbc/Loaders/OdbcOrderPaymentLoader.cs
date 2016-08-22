using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System.Collections.Generic;
using System.Linq;

namespace ShipWorks.Stores.Platforms.Odbc.Loaders
{
    /// <summary>
    /// Loads payment details into an order
    /// </summary>
    public class OdbcOrderPaymentLoader : IOdbcOrderDetailLoader
    {
        /// <summary>
        /// Load the order payment details from the given map into the given order entity
        /// </summary>
        public void Load(IOdbcFieldMap map, OrderEntity order)
        {
            MethodConditions.EnsureArgumentIsNotNull(map, "map");
            MethodConditions.EnsureArgumentIsNotNull(order, "order");

            if (order.IsNew)
            {
                IEnumerable<IOdbcFieldMapEntry> paymentEntries = map.FindEntriesBy(OrderPaymentDetailFields.Value, false)
                    .Where(e => !string.IsNullOrWhiteSpace((string) e.ShipWorksField.Value));

                foreach (IOdbcFieldMapEntry entry in paymentEntries)
                {
                    AddPaymentDetailToOrder(order, entry);
                }
            }
        }

        /// <summary>
        /// Add the individual payment detail to the order entity
        /// </summary>
        private void AddPaymentDetailToOrder(OrderEntity order, IOdbcFieldMapEntry chargeEntry)
        {
            OrderPaymentDetailEntity paymentDetail = new OrderPaymentDetailEntity();
            paymentDetail.Order = order;
            paymentDetail.Label = chargeEntry.ShipWorksField.DisplayName;
            paymentDetail.Value = chargeEntry.ShipWorksField.Value.ToString();
        }
    }
}