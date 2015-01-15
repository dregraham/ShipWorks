using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Interapptive.Shared.Business;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Shipping.ShipSense;
using ShipWorks.Stores;
using ShipWorks.Users;
using ShipWorks.Users.Security;

namespace ShipWorks.Tests.Integration.MSTest.Utilities
{
    public class ShipWorksDataMethods
    {
        public static EntityBase2 GetEntity(long entityID)
        {
            EntityType entityType = EntityUtility.GetEntityType(entityID);
            IEntityField2 pkField = EntityUtility.GetPrimaryKeyField(entityType);

            Stopwatch sw = Stopwatch.StartNew();

            EntityBase2 entity = (EntityBase2)SqlAdapter.Default.FetchNewEntity(
                GeneralEntityFactory.Create(entityType).GetEntityFactory(),
                new RelationPredicateBucket(new FieldCompareValuePredicate(pkField, null, ComparisonOperator.Equal, entityID)),
                GetPrefetch(entityType));

            return entity;
        }

        /// <summary>
        /// Get the prefetch to use for the given entity, or null if none has been configured
        /// </summary>
        static Dictionary<EntityType, PrefetchPath2> prefetchPaths = new Dictionary<EntityType, PrefetchPath2>();
        public static PrefetchPath2 GetPrefetch(EntityType entityType)
        {
            PrefetchPath2 prefetch = null;
            if (prefetchPaths != null)
            {
                prefetchPaths.TryGetValue(entityType, out prefetch);
            }

            return prefetch;
        }

        /// <summary>
        /// Create a shipment for the given order.  The order\shipment reference is created between the two objects.
        /// </summary>
        /// <param name="order">The order for which to create the shipment.</param>
        /// <param name="numberOfItems">Number of items to create.  This should equal the number of packages.</param>
        /// <param name="itemWeight">Weight of each item.  This should be the total weight divided by number of packages.</param>
        /// <param name="weightUnits">Lbs, KGs, etc...</param>
        public static ShipmentEntity InternalCreateShipment(OrderEntity order, ShipmentTypeCode shipmentTypeCode, int numberOfItems, double itemWeight, string weightUnits)
        {
            UserSession.Security.DemandPermission(PermissionType.ShipmentsCreateEditProcess, order.OrderID);

            // Create the shipment
            ShipmentEntity shipment = new ShipmentEntity();

            // It goes with the order
            shipment.Order = order;

            // Set some defaults
            shipment.ShipDate = DateTime.Now.Date.AddHours(12);
            shipment.ShipmentType = (int)shipmentTypeCode;
            shipment.Processed = false;
            shipment.Voided = false;
            shipment.ShipmentCost = 0;
            shipment.TrackingNumber = "";
            shipment.ResidentialDetermination = (int)ResidentialDeterminationType.CommercialIfCompany;
            shipment.ResidentialResult = true;
            shipment.ReturnShipment = false;
            shipment.Insurance = false;
            shipment.InsuranceProvider = (int)InsuranceProvider.Carrier;
            shipment.BestRateEvents = (int)BestRateEventTypes.None;
            shipment.ShipSenseStatus = (int) ShipSenseStatus.NotApplied;
            shipment.ShipSenseEntry = new byte[] {};
            shipment.ShipSenseChangeSets = "<ChangeSets/>";
            shipment.OnlineShipmentID = string.Empty;
            shipment.RequestedLabelFormat = (int) ThermalLanguage.None;

            List<EntityBase2> orderItems = new List<EntityBase2>();
            for (int i = 0; i < numberOfItems; i++)
            {
                // We have to get the order items to calculate the weight
                OrderItemEntity orderItemEntity = new OrderItemEntity
                    {
                        OrderID = order.OrderID,
                        Name = "Test Item",
                        Code = "12345678",
                        SKU = "12345678",
                        ISBN = string.Empty,
                        UPC = string.Empty,
                        Description = "Test Item",
                        Location = string.Empty,
                        Image = string.Empty,
                        Thumbnail = string.Empty,
                        UnitPrice = 3,
                        UnitCost = 0,
                        Weight = itemWeight,
                        Quantity = 1,
                        LocalStatus = "",
                        IsManual = false
                    };
                orderItems.Add(orderItemEntity);
            }

            // Set the initial weights
            shipment.ContentWeight = orderItems.OfType<OrderItemEntity>().Sum(i => i.Quantity * i.Weight);
            shipment.TotalWeight = shipment.ContentWeight;

            shipment.BilledWeight = shipment.TotalWeight;
            shipment.BilledType = (int)BilledType.Unknown;

            // Content items arent generated until they are needed
            shipment.CustomsGenerated = false;
            shipment.CustomsValue = 0;

            // Initialize the to address
            PersonAdapter.Copy(order, shipment, "Ship");

            shipment.OriginOriginID = (int)ShipmentOriginSource.Store;

            // The from address will be dependant on the specific service type, but we'll default it to that of the store
            StoreEntity store = StoreManager.GetStore(order.StoreID);
            PersonAdapter.Copy(store, "", shipment, "Origin");
            shipment.OriginFirstName = store.StoreName;

            // Save the record
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                // Apply the determined shipment type
                shipment.ShipmentType = (int) shipmentTypeCode;

                //foreach (var orderItem in orderItems)
                //{
                //    shipment.Order.OrderItems.Add(orderItem as OrderItemEntity);
                //}

                // Save the shipment
                adapter.SaveAndRefetch(shipment);

                // If the type is not none, apply defaults
                if (shipmentTypeCode != 0)
                {
                    ShipmentType shipmentType = ShipmentTypeManager.GetType(shipment);
                    shipmentType.LoadShipmentData(shipment, false);
                    shipmentType.UpdateDynamicShipmentData(shipment);

                    adapter.SaveAndRefetch(shipment);
                }

                adapter.Commit();
            }

            return shipment;
        }
    }
}
