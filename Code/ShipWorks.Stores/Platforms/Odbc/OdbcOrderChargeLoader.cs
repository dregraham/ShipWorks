using Interapptive.Shared.Utility;
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
            MethodConditions.EnsureArgumentIsNotNull(map, "map");

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

            charge.Type = GetChargeType(chargeEntry.ShipWorksField.DisplayName);
            charge.Description = chargeEntry.ExternalField.Column.Name;
            charge.Amount = (decimal)chargeEntry.ShipWorksField.Value;
        }

        /// <summary>
        /// Gets the Charge Type from the display name
        /// </summary>
        private string GetChargeType(string displayName)
        {
            if (displayName == "Shipping Amount")
            {
                return "SHIPPING";
            }

            if (displayName == "Tax Amount")
            {
                return "TAX";
            }

            if (displayName == "Insurance Amount")
            {
                return "INSURANCE";
            }

            return "ADJUST";
        }
    }
}