using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Enums;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data;
using ShipWorks.Data.Model;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Model.HelperClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Stores;

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
            // Some carts have an international shipping program in place that allow
            // sellers to ship international orders to a domestic facility meaning 
            // customs is not required despite the international shipping address, so 
            // let the store take a look at the shipment as well to determine if customs
            // are required in addition to the just looking at the shipping address.

            bool requiresCustoms = IsCustomsRequiredByShipment(shipment);

            if (requiresCustoms)
            {
                // This shipment requires customs based on the shipping address
                // but allow the store to have the final say
                OrderHeader orderHeader = DataProvider.GetOrderHeader(shipment.OrderID);
                StoreType storeType = StoreTypeManager.GetType(StoreManager.GetStore(orderHeader.StoreID));

                // Pass a true value indicating customs is required based on the shipping addresss
                requiresCustoms = storeType.IsCustomsRequired(shipment, true);
            }

            return requiresCustoms;
        }

        /// <summary>
        /// Indicates if customs forms may be required to ship the shipment based on the
        /// shipping address.
        /// </summary>
        private static bool IsCustomsRequiredByShipment(ShipmentEntity shipment)
        {
            bool requiresCustoms = !ShipmentType.IsDomestic(shipment);

            if (shipment.ShipCountryCode == "US")
            {
                if (PostalUtility.IsMilitaryState(shipment.ShipStateProvCode))
                {
                    requiresCustoms = true;
                }

                // Foreign US territories
                if (shipment.ShipPostalCode.StartsWith("96910") ||
                    shipment.ShipPostalCode.StartsWith("96950") ||
                    shipment.ShipPostalCode.StartsWith("96960") ||
                    shipment.ShipPostalCode.StartsWith("96970") ||
                    shipment.ShipPostalCode.StartsWith("96799"))
                {
                    requiresCustoms = true;
                }
            }

            if (shipment.OriginCountryCode == "US")
            {
                if (PostalUtility.IsPostalShipmentType((ShipmentTypeCode)shipment.ShipmentType) && PostalUtility.IsDomesticCountry(shipment.ShipCountryCode))
                {
                    if (shipment.ShipCountryCode != "GU")
                    {
                        // Even though Guam is USPS domestic services, it still requires customs. We don't need
                        // customs when shipping with USPS to a domestic country that is not Guam, so flip this
                        // back to false (because it was set to true by IsDomestic method due to origin country
                        // and ship country differ)
                        requiresCustoms = false;
                    }
                }
                // i-Parcel allows customers to upload their SKUs and customs info, so we don't need to enter it in ShipWorks
                // So Customs is never required.
                if (ShipmentTypeManager.IsiParcel((ShipmentTypeCode) shipment.ShipmentType))
                {
                    requiresCustoms = false;
                }

            }

            return requiresCustoms;
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
                    using (SqlAdapter adapter = new SqlAdapter(true))
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
