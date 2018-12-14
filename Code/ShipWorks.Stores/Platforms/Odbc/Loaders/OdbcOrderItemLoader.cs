using Interapptive.Shared.Utility;
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

            int maxIndex = clonedMap.Entries.Max(e => e.Index);

            foreach (OdbcRecord odbcRecord in odbcRecords)
            {
                clonedMap.ApplyValues(odbcRecord);

                for (int i = 0; i <= maxIndex; i++)
                {
                    int itemIndex = i;

                    OrderItemEntity item = new OrderItemEntity();
                    item.InitializeNullsToDefault();
                    clonedMap.CopyToEntity(item, itemIndex);

                    SetCost(clonedMap, item, itemIndex);
                    SetPrice(clonedMap, item, itemIndex);
                    SetWeight(clonedMap, item, itemIndex);

                    attributeLoader.Load(clonedMap, item, itemIndex);

                    if (item.IsDirty)
                    {
                        item.Order = order;
                    }
                }
            }
        }

        /// <summary>
        /// Sets the weight of the order item
        /// </summary>
        private void SetWeight(IOdbcFieldMap clonedMap, OrderItemEntity item, int itemIndex)
        {
            IEnumerable<IOdbcFieldMapEntry> unitWeightFields =
                clonedMap.FindEntriesBy(OrderItemFields.Weight, false).Where(e => e.Index == itemIndex);

            item.Weight = (double) GetUnitAmount(unitWeightFields, item, OdbcOrderFieldDescription.ItemUnitWeight,
                OdbcOrderFieldDescription.ItemTotalWeight);
        }

        /// <summary>
        /// Sets the cost of the order item.
        /// </summary>
        private void SetCost(IOdbcFieldMap clonedMap, OrderItemEntity item, int itemIndex)
        {
            IEnumerable<IOdbcFieldMapEntry> costFields =
                clonedMap.FindEntriesBy(OrderItemFields.UnitCost, false).Where(e => e.Index == itemIndex);

            item.UnitCost = GetUnitAmount(costFields, item, OdbcOrderFieldDescription.ItemUnitCost,
                OdbcOrderFieldDescription.ItemTotalCost);
        }

        /// <summary>
        /// Sets the price of the order item.
        /// </summary>
        private void SetPrice(IOdbcFieldMap clonedMap, OrderItemEntity item, int itemIndex)
        {
            IEnumerable<IOdbcFieldMapEntry> unitPriceFields =
                clonedMap.FindEntriesBy(OrderItemFields.UnitPrice, false).Where(e => e.Index == itemIndex);

            item.UnitPrice = GetUnitAmount(unitPriceFields, item, OdbcOrderFieldDescription.ItemUnitPrice,
                OdbcOrderFieldDescription.ItemTotalPrice);
        }

        /// <summary>
        /// Evaluates the unit amount, total amount and quantity to determine the unit amount.
        /// </summary>
        /// <remarks>
        /// Defaults to 0 if cannot be determined.
        /// </remarks>
        private decimal GetUnitAmount(IEnumerable<IOdbcFieldMapEntry> applicableEntries,
            OrderItemEntity item,
            OdbcOrderFieldDescription unitDescription,
            OdbcOrderFieldDescription totalDescription)
        {
            List<IShipWorksOdbcMappableField> shipworksFields = applicableEntries.Select(f => f.ShipWorksField).ToList();

            // If we have the field for the unit amount, we should use its value.
            decimal? unitAmount = GetAmount(shipworksFields, unitDescription);
            if (unitAmount != null)
            {
                return unitAmount.Value;
            }

            // Return total amount / quantity orderred.
            decimal? totalAmount = GetAmount(shipworksFields, totalDescription);

            if (totalAmount != null && Math.Abs(item.Quantity) > .01)
            {
                return totalAmount.Value / Convert.ToDecimal(item.Quantity);
            }

            return 0;
        }

        /// <summary>
        /// Gets the amount - returns 0 if null.
        /// </summary>
        private decimal? GetAmount(IEnumerable<IShipWorksOdbcMappableField> applicableEntries, OdbcOrderFieldDescription fieldDescription)
        {
            object value = applicableEntries.SingleOrDefault(f => f.DisplayName == EnumHelper.GetDescription(fieldDescription))?.Value;

            if (value == null)
            {
                return null;
            }

            return Convert.ToDecimal(value);
        }
    }
}