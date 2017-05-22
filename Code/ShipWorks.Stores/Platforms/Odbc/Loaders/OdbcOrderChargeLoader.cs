﻿using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System.Collections.Generic;
using System.Linq;

namespace ShipWorks.Stores.Platforms.Odbc.Loaders
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
            MethodConditions.EnsureArgumentIsNotNull(map, nameof(map));
            MethodConditions.EnsureArgumentIsNotNull(order, nameof(order));

            if (order.IsNew)
            {
                // Filter out 0 amount charges.
                IEnumerable<IOdbcFieldMapEntry> chargeEntries = map.FindEntriesBy(OrderChargeFields.Amount, false)
                    .Where(e => e.ShipWorksField.Value is decimal);

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
            string desc = chargeEntry.ShipWorksField.DisplayName.Split(' ').First();

            // Don't let resharper make you an object initializer here. SonarLint will complain...
            OrderChargeEntity charge = new OrderChargeEntity();
            charge.Order = order;
            charge.Type = desc.ToUpperInvariant();
            charge.Description = desc;
            charge.Amount = (decimal)chargeEntry.ShipWorksField.Value;
        }
    }
}