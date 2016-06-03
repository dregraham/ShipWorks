using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Loads order charges from the map to the order entity
    /// </summary>
    public class OdbcOrderChargeLoader : IOdbcOrderDetailLoader
    {
        /// <summary>
        /// Load the order charges from the given map into the given entity
        /// </summary>
        public void Load(IOdbcFieldMap map, OrderEntity order)
        {
            if (order != null)
            {
                IEnumerable<IOdbcFieldMapEntry> chargeEntries = map.FindEntriesBy(OrderChargeFields.Amount);

                foreach (IOdbcFieldMapEntry chargeEntry in chargeEntries)
                {
                    AddChargeToOrder(order, chargeEntry);
                }
            }
        }

        /// <summary>
        /// Add the individual charge to the order entity
        /// </summary>
        private void AddChargeToOrder(OrderEntity order, IOdbcFieldMapEntry chargeEntry)
        {
            OrderChargeEntity charge = new OrderChargeEntity();
            charge.Order = order;

            charge.Type = chargeEntry.ShipWorksField.DisplayName;
            charge.Amount = (decimal)chargeEntry.ShipWorksField.Value;
        }
    }
}