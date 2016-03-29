using System;
using System.Collections.Generic;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.AddressValidation;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.ExecutionMode;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
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
        private readonly IValidatedAddressManager validatedAddressManager;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="shipmentAdapterFactor"></param>
        public ShippingManagerWrapper(
            ICarrierShipmentAdapterFactory shipmentAdapterFactor,
            IValidatedAddressScope validatedAddressScope,
            IValidatedAddressManager validatedAddressManager)
        {
            this.shipmentAdapterFactory = shipmentAdapterFactor;
            this.validatedAddressScope = validatedAddressScope;
            this.validatedAddressManager = validatedAddressManager;
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
        /// Ensure the specified shipment is fully loaded
        /// </summary>
        public ShipmentEntity EnsureShipmentLoaded(ShipmentEntity shipment)
        {
            ShippingManager.EnsureShipmentLoaded(shipment);
            return shipment;
        }

        /// <summary>
        /// Get the shipment of the specified ID.  The Order will be attached.
        /// </summary>
        public ICarrierShipmentAdapter GetShipment(long shipmentID)
        {
            ShipmentEntity shipment = ShippingManager.GetShipment(shipmentID);
            EnsureShipmentLoaded(shipment);
            return shipmentAdapterFactory.Get(shipment);
        }

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
                    shipment.ResetDirtyFlagOnUnchangedEntityFields();

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
        /// Void the given shipment.  If the shipment is already voided, then no action is taken and no error is reported.  The fact that
        /// it was voided is logged to tango.
        /// </summary>
        public GenericResult<ICarrierShipmentAdapter> VoidShipment(long shipmentID, IShippingErrorManager errorManager)
        {
            try
            {
                ShipmentEntity shipment = ShippingManager.VoidShipment(shipmentID);
                return GenericResult.FromSuccess(shipmentAdapterFactory.Get(shipment));
            }
            catch (Exception ex) when (ex is ORMConcurrencyException ||
                                        ex is ObjectDeletedException ||
                                        ex is SqlForeignKeyException)
            {
                return GenericResult.FromError<ICarrierShipmentAdapter>(errorManager.SetShipmentErrorMessage(shipmentID, ex, "voided"));
            }
            catch (ShippingException ex)
            {
                return GenericResult.FromError<ICarrierShipmentAdapter>(errorManager.SetShipmentErrorMessage(shipmentID, ex));
            }
        }

        /// <summary>
        /// Indicates if the shipment type of the given type code has gone through the full setup wizard \ configuration
        /// </summary>
        public bool IsShipmentTypeConfigured(ShipmentTypeCode shipmentTypeCode) =>
            ShippingManager.IsShipmentTypeConfigured(shipmentTypeCode);

        /// <summary>
        /// Create a new shipment for the given order
        /// </summary>
        public ShipmentEntity CreateShipment(OrderEntity order) => ShippingManager.CreateShipment(order);

        /// <summary>
        /// Create a shipment as a copy of an existing shipment
        /// </summary>
        public ShipmentEntity CreateShipmentCopy(ShipmentEntity shipment) => CreateShipmentCopy(shipment, null);

        /// <summary>
        /// Create a shipment as a copy of an existing shipment
        /// </summary>
        public ShipmentEntity CreateShipmentCopy(ShipmentEntity shipment, Action<ShipmentEntity> configuration)
        {
            ShipmentEntity copy = ShippingManager.CreateShipmentCopy(shipment);

            configuration?.Invoke(copy);

            // save
            ShippingManager.SaveShipment(copy);

            using (SqlAdapter sqlAdapter = new SqlAdapter(true))
            {
                validatedAddressManager.CopyValidatedAddresses(sqlAdapter, shipment.ShipmentID, "Ship", copy.ShipmentID, "Ship");

                sqlAdapter.Commit();
            }

            return copy;
        }

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