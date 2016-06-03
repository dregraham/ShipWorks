using System.Collections.Generic;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Import;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.Odbc.Mapping;

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
        public void Load(IOdbcFieldMap map, IEntity2 entity, IOrderElementFactory orderElementFactory)
        {
            OrderEntity order = entity as OrderEntity;

            if (order != null)
            {
                IEnumerable<IOdbcFieldMapEntry> chargeEntries = map.FindEntriesBy(OrderChargeFields.Amount);

                foreach (IOdbcFieldMapEntry chargeEntry in chargeEntries)
                {
                    AddChargeToOrder(order, chargeEntry, orderElementFactory);
                }
            }
        }

        /// <summary>
        /// Add the individual charge to the order entity
        /// </summary>
        private void AddChargeToOrder(OrderEntity order, IOdbcFieldMapEntry chargeEntry, IOrderElementFactory factory)
        {
            OrderChargeEntity charge = factory.CreateCharge(order);

            charge.Type = chargeEntry.ShipWorksField.DisplayName;
            charge.Amount = (decimal)chargeEntry.ShipWorksField.Value;
        }
    }
}