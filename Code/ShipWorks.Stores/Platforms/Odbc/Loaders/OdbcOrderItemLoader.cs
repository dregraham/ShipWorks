using System;
using System.Collections.Generic;
using System.Linq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.Odbc.Mapping;

namespace ShipWorks.Stores.Platforms.Odbc.Loaders
{
    /// <summary>
    /// Loads order item information
    /// </summary>
    public class OdbcOrderItemLoader : IOdbcOrderItemLoader
    {
        private readonly IOdbcItemAttributeLoader attributeLoader;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcOrderItemLoader"/> class.
        /// </summary>
        public OdbcOrderItemLoader(IOdbcItemAttributeLoader attributeLoader)
        {
            this.attributeLoader = attributeLoader;
        }

        /// <summary>
        /// Loads items from odbcRecords into the order
        /// </summary>
        public void Load(IOdbcFieldMap map, OrderEntity order, IEnumerable<OdbcRecord> odbcRecords)
        {
            IOdbcFieldMap clonedMap = map.Clone();

            foreach (OdbcRecord odbcRecord in odbcRecords)
            {
                clonedMap.ApplyValues(odbcRecord);

                OrderItemEntity item = new OrderItemEntity(order);
                clonedMap.CopyToEntity(item);

                item.UnitPrice = GetUnitAmount(clonedMap, item, OrderItemFields.UnitPrice, ShipWorksOdbcMappableField.UnitPriceDisplayName, ShipWorksOdbcMappableField.TotalPriceDisplayName);
                item.UnitCost = GetUnitAmount(clonedMap, item, OrderItemFields.UnitCost, ShipWorksOdbcMappableField.UnitCostDisplayName, ShipWorksOdbcMappableField.TotalCostDisplayName);
                item.Weight = (double) GetUnitAmount(clonedMap, item, OrderItemFields.Weight, ShipWorksOdbcMappableField.UnitWeightDisplayName, ShipWorksOdbcMappableField.TotalWeightDisplayName);

                attributeLoader.Load(clonedMap, item);
            }
        }

        /// <summary>
        /// Evaluates the unit amount, total amount and quantity to determine the unit amount.
        /// </summary>
        /// <remarks>
        /// Defaults to 0 if cannot be determined.
        /// </remarks>
        private decimal GetUnitAmount(IOdbcFieldMap map,
            OrderItemEntity item,
            EntityField2 amountField,
            string unitDisplayName,
            string totalDisplayName)
        {
            IEnumerable<IShipWorksOdbcMappableField> priceFields = map.FindEntriesBy(amountField, false)
                .Select(f => f.ShipWorksField).ToList();

            IShipWorksOdbcMappableField unitPriceField =
                priceFields.SingleOrDefault(f => f.DisplayName == unitDisplayName);

            if (unitPriceField != null)
            {
                return Convert.ToDecimal(unitPriceField.Value);
            }

            IShipWorksOdbcMappableField totalPriceField =
                priceFields.SingleOrDefault(f => f.DisplayName == totalDisplayName);
            if (totalPriceField == null || Math.Abs(item.Quantity) < .01)
            {
                return 0;
            }

            return (Convert.ToDecimal(totalPriceField.Value))/Convert.ToDecimal(item.Quantity);
        }
    }
}