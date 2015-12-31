using System;
using System.Linq;
using Autofac;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore;
using ShipWorks.Data;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Helps with customs stuff
    /// </summary>
    public static class CustomsManager
    {
        /// <summary>
        /// Indicates if customs forms may be required to ship the shipment.
        /// </summary>
        public static bool IsCustomsRequired(ShipmentEntity shipment)
        {
            // Defer to the shipment type to inspect the shipment to determine whether
            // customs is required based on any carrier-specific logic (i.e. best-rate)
            ShipmentType shipmpentType = ShipmentTypeManager.GetType(shipment);
            return shipmpentType.IsCustomsRequired(shipment);
        }

        /// <summary>
        /// Ensure custom's contents for the given shipment have been created
        /// </summary>
        public static void LoadCustomsItems(ShipmentEntity shipment, bool reloadIfPresent)
        {
            // If custom's aren't required, then forget it
            if (!IsCustomsRequired(shipment))
            {
                return;
            }

            // If its already been generated, then just check if its loaded
            if (shipment.CustomsGenerated)
            {
                if (reloadIfPresent || !shipment.CustomsItemsLoaded)
                {
                    using (SqlAdapter adapter = new SqlAdapter())
                    {
                        shipment.CustomsItems.Clear();
                        shipment.CustomsItems.AddRange(DataProvider.GetRelatedEntities(shipment.ShipmentID, EntityType.ShipmentCustomsItemEntity).Cast<ShipmentCustomsItemEntity>());
                        // Add FedExShipmentCustomsItems from database
                    }

                    // Set the removed tracker for tracking deletions in the UI until saved
                    shipment.CustomsItems.RemovedEntitiesTracker = new ShipmentCustomsItemCollection();
                }
                // if fedex, check to see if fedex customs items have been loaded. If not, create them
            }

            // Not already generated, have to create
            else
            {
                // If its been processed we don't mess with it
                if (!shipment.Processed)
                {
                    using (SqlAdapter adapter = SqlAdapter.Create(true))
                    {
                        decimal customsValue = 0m;

                        // By default create one content item representing each item in the order
                        foreach (OrderItemEntity item in DataProvider.GetRelatedEntities(shipment.OrderID, EntityType.OrderItemEntity))
                        {
                            object attributePrice = adapter.GetScalar(OrderItemAttributeFields.UnitPrice, null, AggregateFunction.Sum, OrderItemAttributeFields.OrderItemID == item.OrderItemID);

                            decimal priceAndValue = item.UnitPrice + ((attributePrice is DBNull) ? 0M : Convert.ToDecimal(attributePrice));

                            ShipmentCustomsItemEntity customsItem = new ShipmentCustomsItemEntity
                            {
                                Shipment = shipment,
                                Description = item.Name,
                                Quantity = item.Quantity,
                                Weight = item.Weight,
                                UnitValue = priceAndValue,
                                CountryOfOrigin = "US",
                                HarmonizedCode = "",
                                NumberOfPieces = 0,
                                UnitPriceAmount = priceAndValue
                            };

                            adapter.SaveAndRefetch(customsItem);

                            customsValue += ((decimal) customsItem.Quantity * customsItem.UnitValue);
                        }

                        shipment.CustomsValue = customsValue;
                        shipment.CustomsGenerated = true;

                        adapter.SaveAndRefetch(shipment);

                        adapter.Commit();
                    }

                    // Set the removed tracker for tracking deletions in the UI until saved
                    shipment.CustomsItems.RemovedEntitiesTracker = new ShipmentCustomsItemCollection();
                }
            }

            // Consider them loaded.  This is an in-memory field
            shipment.CustomsItemsLoaded = true;
        }

        /// <summary>
        /// Create a new ShipmentCustomsItemEntity for the given shipment, filled in with defaults
        /// </summary>
        public static ShipmentCustomsItemEntity CreateCustomsItem(ShipmentEntity shipment)
        {
            ShipmentCustomsItemEntity customsItem = new ShipmentCustomsItemEntity();
            customsItem.Shipment = shipment;

            customsItem.Description = "New Item";
            customsItem.Quantity = 1;
            customsItem.Weight = 0;
            customsItem.UnitValue = 0;
            customsItem.CountryOfOrigin = "US";
            customsItem.HarmonizedCode = "";
            customsItem.UnitPriceAmount = 0;
            customsItem.NumberOfPieces = 0;

            return customsItem;
        }
    }
}
