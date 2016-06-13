using System;
using System.Collections.Generic;
using System.Linq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using Interapptive.Shared.Utility;

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
            IEnumerable<IShipWorksOdbcMappableField> shipworksFields = map.FindEntriesBy(entityField, false).Select(f => f.ShipWorksField);

            IShipWorksOdbcMappableField singleShipworksField = shipworksFields.SingleOrDefault(f => f.DisplayName == EnumHelper.GetDescription(unitDescription));

            if (singleShipworksField != null)
            {
                return Convert.ToDecimal(singleShipworksField.Value);
            }

            IShipWorksOdbcMappableField totalShipWorksField = shipworksFields.SingleOrDefault(f => f.DisplayName == EnumHelper.GetDescription(totalDescription));
            if (totalShipWorksField == null || Math.Abs(item.Quantity) < .01)
            {
                return 0;
            }

            return (Convert.ToDecimal(totalShipWorksField.Value))/Convert.ToDecimal(item.Quantity);
        }
    }
}