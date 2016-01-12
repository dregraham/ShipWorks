using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.AddressValidation;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.ExecutionMode;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Wraps the ShippingManager
    /// </summary>
    public class ShippingManagerWrapper : IShippingManager, IInitializeForCurrentDatabase
    {
        private readonly ICarrierShipmentAdapterFactory shipmentAdapterFactory;
        private readonly IValidatedAddressScope validatedAddressScope;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="shipmentAdapterFactor"></param>
        public ShippingManagerWrapper(
            ICarrierShipmentAdapterFactory shipmentAdapterFactor,
            IValidatedAddressScope validatedAddressScope)
        {
            this.shipmentAdapterFactory = shipmentAdapterFactor;
            this.validatedAddressScope = validatedAddressScope;
        }

        /// <summary>
        /// Refresh the data for the given shipment, including the carrier specific data.  The order and the other siblings are not touched.
        /// If the shipment has been deleted, an ObjectDeletedException is thrown.
        /// </summary>
        public void RefreshShipment(ShipmentEntity shipment) =>
            ShippingManager.RefreshShipment(shipment);

        /// <summary>
        /// Update the label format of any unprocessed shipment with the given shipment type code
        /// </summary>
        public void UpdateLabelFormatOfUnprocessedShipments(ShipmentTypeCode shipmentTypeCode) =>
            ShippingManager.UpdateLabelFormatOfUnprocessedShipments(shipmentTypeCode);


        /// <summary>
        /// Get the list of shipments that correspond to the given order key.  If no shipment exists for the order,
        /// one will be created if autoCreate is true.  An OrderEntity will be attached to each shipment.
        /// </summary>
        public IEnumerable<ICarrierShipmentAdapter> GetShipments(long orderID, bool createIfNone)
        {
            OrderEntity order = LoadFullShipmentGraph(orderID);

            if (!order.Shipments.Any() && createIfNone)
            {
                ShippingManager.CreateShipment(order);
            }

            return LoadFullShipmentGraph(orderID)
                .Shipments
                .Select(shipmentAdapterFactory.Get);
        }

        /// <summary>
        /// Ensure the specified shipment is fully loaded
        /// </summary>
        public ShipmentEntity EnsureShipmentLoaded(ShipmentEntity shipment)
        {
            ShippingManager.EnsureShipmentLoaded(shipment);
            return shipment;
        }

        /// <summary>
        /// Ensure the specified shipment is fully loaded
        /// </summary>
        public OrderEntity LoadFullShipmentGraph(long orderId)
        {
            IPrefetchPath2 orderPrefetchPath = new PrefetchPath2(EntityType.OrderEntity);
            orderPrefetchPath.Add(OrderEntity.PrefetchPathStore);
            orderPrefetchPath.Add(OrderEntity.PrefetchPathOrderItems);
            IPrefetchPathElement2 shipmentsPath = orderPrefetchPath.Add(OrderEntity.PrefetchPathShipments);

            IPrefetchPathElement2 upsShipmentPath = shipmentsPath.SubPath.Add(ShipmentEntity.PrefetchPathUps);
            upsShipmentPath.SubPath.Add(UpsShipmentEntity.PrefetchPathPackages);

            IPrefetchPathElement2 postalShipmentPath = shipmentsPath.SubPath.Add(ShipmentEntity.PrefetchPathPostal);
            postalShipmentPath.SubPath.Add(PostalShipmentEntity.PrefetchPathUsps);
            postalShipmentPath.SubPath.Add(PostalShipmentEntity.PrefetchPathEndicia);

            IPrefetchPathElement2 iParcelShipmentPath = shipmentsPath.SubPath.Add(ShipmentEntity.PrefetchPathIParcel);
            iParcelShipmentPath.SubPath.Add(IParcelShipmentEntity.PrefetchPathPackages);

            shipmentsPath.SubPath.Add(ShipmentEntity.PrefetchPathOnTrac);

            shipmentsPath.SubPath.Add(ShipmentEntity.PrefetchPathAmazon);

            shipmentsPath.SubPath.Add(ShipmentEntity.PrefetchPathBestRate);

            IPrefetchPathElement2 fedexShipmentPath = shipmentsPath.SubPath.Add(ShipmentEntity.PrefetchPathFedEx);
            fedexShipmentPath.SubPath.Add(FedExShipmentEntity.PrefetchPathPackages);

            shipmentsPath.SubPath.Add(ShipmentEntity.PrefetchPathOther);

            shipmentsPath.SubPath.Add(ShipmentEntity.PrefetchPathInsurancePolicy);
            shipmentsPath.SubPath.Add(ShipmentEntity.PrefetchPathCustomsItems);
            shipmentsPath.SubPath.Add(ShipmentEntity.PrefetchPathValidatedAddress);

            OrderEntity order = new OrderEntity(orderId);

            using (SqlAdapter sqlAdapter = SqlAdapter.Create(true))
            {
                sqlAdapter.FetchEntity(order, orderPrefetchPath);
            }

            return order;
        }

        /// <summary>
        /// Get the shipment of the specified ID.  The Order will be attached.
        /// </summary>
        public ShipmentEntity GetShipment(long shipmentID)
        {
            ShipmentEntity shipment = ShippingManager.GetShipment(shipmentID);
            EnsureShipmentLoaded(shipment);
            return shipment;
        }

        /// <summary>
        /// Get rates for the given shipment using the appropriate ShipmentType
        /// </summary>
        public RateGroup GetRates(ShipmentEntity shipment, ShipmentType shipmentType)
        {
            if (!shipment.Processed)
            {
                ShippingManager.GetRates(shipment, shipmentType);
            }

            return new RateGroup(Enumerable.Empty<RateResult>());
        }

        /// <summary>
        /// Get rates for the given shipment using the appropriate ShipmentType
        /// </summary>
        public Task<RateGroup> GetRatesAsync(ShipmentEntity shipment, ShipmentType shipmentType, CancellationToken token) =>
            TaskEx.Run(() => GetRates(shipment, shipmentType), token);

        /// <summary>
        /// Removes the specified shipment from the cache
        /// </summary>
        /// <param name="shipment">Shipment that should be removed from cache</param>
        /// <returns></returns>
        public void RemoveShipmentFromRatesCache(ShipmentEntity shipment)
        {
            ShippingManager.RemoveShipmentFromRatesCache(shipment);
        }

        /// <summary>
        /// Change the shipment type of the provided shipment and return it's shipment adapter
        /// </summary>
        public ICarrierShipmentAdapter ChangeShipmentType(ShipmentTypeCode shipmentTypeCode, ShipmentEntity shipment)
        {
            shipment.ShipmentTypeCode = shipmentTypeCode;
            EnsureShipmentLoaded(shipment);
            return shipmentAdapterFactory.Get(shipment);
        }

        /// <summary>
        /// Save the shipment to the database
        /// </summary>
        public IDictionary<ShipmentEntity, Exception> SaveShipmentToDatabase(ShipmentEntity shipment, bool forceSave) =>
            SaveShipmentsToDatabase(new[] { shipment }, forceSave);

        /// <summary>
        /// Save the shipments to the database
        /// </summary>
        public IDictionary<ShipmentEntity, Exception> SaveShipmentsToDatabase(IEnumerable<ShipmentEntity> shipments, bool forceSave)
        {
            if (shipments == null)
            {
                return new Dictionary<ShipmentEntity, Exception>();
            }

            Dictionary<ShipmentEntity, Exception> errors = new Dictionary<ShipmentEntity, Exception>();

            foreach (ShipmentEntity shipment in shipments)
            {
                try
                {
                    // Force the shipment to look dirty so it's forced to save. This is to make sure that if any other
                    // changes had been made by other users we pick up the concurrency violation.
                    if (forceSave && !shipment.IsDirty)
                    {
                        shipment.Fields[(int) ShipmentFieldIndex.ShipmentType].IsChanged = true;
                        shipment.Fields.IsDirty = true;
                    }

                    ShippingManager.SaveShipment(shipment);

                    using (SqlAdapter sqlAdapter = new SqlAdapter(true))
                    {
                        validatedAddressScope?.FlushAddressesToDatabase(sqlAdapter, shipment.ShipmentID, "Ship");
                        sqlAdapter.Commit();
                    }
                }
                catch (ObjectDeletedException ex)
                {
                    errors[shipment] = ex;
                }
                catch (SqlForeignKeyException ex)
                {
                    errors[shipment] = ex;
                }
                catch (ORMConcurrencyException ex)
                {
                    errors[shipment] = ex;
                }
            }

            return errors;
        }

        /// <summary>
        /// Gets the overridden store shipment.
        /// </summary>
        public ShipmentEntity GetOverriddenStoreShipment(ShipmentEntity shipment) =>
            ShippingManager.GetOverriddenStoreShipment(shipment);

        /// <summary>
        /// Indicates if the given shipment type code is enabled for selection in the shipping window
        /// </summary>
        public bool IsShipmentTypeEnabled(ShipmentTypeCode shipmentTypeCode) =>
            ShippingManager.IsShipmentTypeEnabled(shipmentTypeCode);

        /// <summary>
        /// Get rates for the given shipment using the appropriate ShipmentType
        /// </summary>
        public RateGroup GetRates(ShipmentEntity shipment) => ShippingManager.GetRates(shipment);

        /// <summary>
        /// Void the given shipment.  If the shipment is already voided, then no action is taken and no error is reported.  The fact that
        /// it was voided is logged to tango.
        /// </summary>
        public void VoidShipment(long shipmentID) => ShippingManager.VoidShipment(shipmentID);

        /// <summary>
        /// Indicates if the shipment type of the given type code has gone through the full setup wizard \ configuration
        /// </summary>
        public bool IsShipmentTypeConfigured(ShipmentTypeCode shipmentTypeCode) =>
            ShippingManager.IsShipmentTypeConfigured(shipmentTypeCode);

        /// <summary>
        /// Gets the service used.
        /// </summary>
        public string GetServiceUsed(ShipmentEntity shipment) =>
            ShippingManager.GetServiceUsed(shipment);

        /// <summary>
        /// Gets the service used.
        /// </summary>
        public string GetCarrierName(ShipmentTypeCode shipmentTypeCode) =>
            ShippingManager.GetCarrierName(shipmentTypeCode);

        /// <summary>
        /// Initialize service for the given database
        /// </summary>
        public void InitializeForCurrentDatabase(ExecutionMode executionMode) =>
            ShippingManager.InitializeForCurrentDatabase();
    }
}