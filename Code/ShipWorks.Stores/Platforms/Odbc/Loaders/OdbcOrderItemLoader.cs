using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;

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

                item.UnitPrice = GetUnitAmount(clonedMap, item, OrderItemFields.UnitPrice, OdbcOrderFieldDescription.ItemUnitPrice, OdbcOrderFieldDescription.ItemTotalPrice);
                item.UnitCost = GetUnitAmount(clonedMap, item, OrderItemFields.UnitCost, OdbcOrderFieldDescription.ItemUnitCost, OdbcOrderFieldDescription.ItemTotalCost);
                item.Weight = (double) GetUnitAmount(clonedMap, item, OrderItemFields.Weight, OdbcOrderFieldDescription.ItemUnitWeight, OdbcOrderFieldDescription.ItemTotalWeight);

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
            EntityField2 entityField,
            OdbcOrderFieldDescription unitDescription,
            OdbcOrderFieldDescription totalDescription)
        {
            IEnumerable<IShipWorksOdbcMappableField> shipworksFields =
                map.FindEntriesBy(entityField, false).Select(f => f.ShipWorksField).ToList();

            IShipWorksOdbcMappableField unitAmountField =
                shipworksFields.SingleOrDefault(f => f.DisplayName == EnumHelper.GetDescription(unitDescription));

            // If we have the field for the unit amount, we should use its value.
            if (unitAmountField != null)
            {
                return Convert.ToDecimal(unitAmountField.Value);
            }

            // If we don't have a total amount or the quantity is 0, we can't determine the unit amount, so return 0.
            IShipWorksOdbcMappableField totalAmountField =
                shipworksFields.SingleOrDefault(f => f.DisplayName == EnumHelper.GetDescription(totalDescription));
            if (totalAmountField == null || Math.Abs(item.Quantity) < .01)
            {
                return 0;
            }

            // Return total amount / quantity orderred.
            return (Convert.ToDecimal(totalAmountField.Value))/Convert.ToDecimal(item.Quantity);
        }
    }
}