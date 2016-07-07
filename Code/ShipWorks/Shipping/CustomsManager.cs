using System;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using Autofac;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore;
using ShipWorks.Data;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content;

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
        public static void LoadCustomsItems(ShipmentEntity shipment, bool reloadIfPresent, SqlAdapter adapter)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment);
            MethodConditions.EnsureArgumentIsNotNull(adapter);

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
                    shipment.CustomsItems.Clear();
                    shipment.CustomsItems.AddRange(DataProvider.GetRelatedEntities(shipment.ShipmentID, EntityType.ShipmentCustomsItemEntity).Cast<ShipmentCustomsItemEntity>());

                    // Set the removed tracker for tracking deletions in the UI until saved
                    shipment.CustomsItems.RemovedEntitiesTracker = new ShipmentCustomsItemCollection();
                }
            }
            // Not already generated, have to create
            else
            {
                // Make sure that these new customs items get persisted
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    GenerateCustomsItems(shipment);

                    adapter.SaveAndRefetch(shipment);

                    scope.Complete();
                }
            }

            // Consider them loaded.  This is an in-memory field
            shipment.CustomsItemsLoaded = true;
        }

        /// <summary>
        /// Generate customs for a shipment.  If the shipment is processed, or doesn't require customs,
        /// or customs have already been generated, nothing will be done.
        /// 
        /// Customs items are not persisted to the database, as that is the caller's responsibility.
        /// </summary>
        public static void GenerateCustomsItems(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment);

            // If custom's aren't required, then forget it
            if (shipment.Processed || shipment.CustomsGenerated || !IsCustomsRequired(shipment))
            {
                return;
            }

            shipment.CustomsItems.Clear();

            decimal customsValue = 0m;

            // By default create one content item representing each item in the order
            foreach (OrderItemEntity item in shipment.Order.OrderItems)
            {
                decimal attributePrice = item.OrderItemAttributes.Sum(oia => oia.UnitPrice);

                decimal priceAndValue = item.UnitPrice + attributePrice;

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

                customsValue += ((decimal)customsItem.Quantity * customsItem.UnitValue);
            }

            shipment.CustomsValue = customsValue;
            shipment.CustomsGenerated = true;

            // Set the removed tracker for tracking deletions in the UI until saved
            shipment.CustomsItems.RemovedEntitiesTracker = new ShipmentCustomsItemCollection();

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
