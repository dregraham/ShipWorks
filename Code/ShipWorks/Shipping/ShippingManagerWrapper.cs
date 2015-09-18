using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using System;
using ShipWorks.AddressValidation;
using ShipWorks.Data.Model;
using System.Linq;
using ShipWorks.Data.Connection;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Wraps the ShippingManager
    /// </summary>
    public class ShippingManagerWrapper : IShippingManager
    {
        /// <summary>
        /// Refresh the data for the given shipment, including the carrier specific data.  The order and the other siblings are not touched.
        /// If the shipment has been deleted, an ObjectDeletedException is thrown.
        /// </summary>
        public void RefreshShipment(ShipmentEntity shipment)
        {
            ShippingManager.RefreshShipment(shipment);
        }

        /// <summary>
        /// Update the label format of any unprocessed shipment with the given shipment type code
        /// </summary>
        public void UpdateLabelFormatOfUnprocessedShipments(ShipmentTypeCode shipmentTypeCode)
        {
            ShippingManager.UpdateLabelFormatOfUnprocessedShipments(shipmentTypeCode);
        }

        /// <summary>
        /// Get the list of shipments that correspond to the given order key.  If no shipment exists for the order,
        /// one will be created if autoCreate is true.  An OrderEntity will be attached to each shipment.
        /// </summary>
        public List<ShipmentEntity> GetShipments(long orderID, bool createIfNone) =>
            ShippingManager.GetShipments(orderID, createIfNone);

        /// <summary>
        /// Ensure the specified shipment is fully loaded
        /// </summary>
        public void EnsureShipmentLoaded(ShipmentEntity shipment) => 
            ShippingManager.EnsureShipmentLoaded(shipment);

        /// <summary>
        /// Get the shipment of the specified ID.  The Order will be attached.
        /// </summary>
        public ShipmentEntity GetShipment(long shipmentID)
        {
            return ShippingManager.GetShipment(shipmentID);
        }

        /// <summary>
        /// Get rates for the given shipment using the appropriate ShipmentType
        /// </summary>
        public RateGroup GetRates(ShipmentEntity shipment, ShipmentType shipmentType)
        {
            return ShippingManager.GetRates(shipment, shipmentType);
        }

        /// <summary>
        /// Removes the specified shipment from the cache
        /// </summary>
        /// <param name="shipment">Shipment that should be removed from cache</param>
        /// <returns></returns>
        public void RemoveShipmentFromRatesCache(ShipmentEntity shipment)
        {
            RemoveShipmentFromRatesCache(shipment);
        }

        /// <summary>
        /// Save the shipments to the database
        /// </summary>
        public IDictionary<ShipmentEntity, Exception> SaveShipmentsToDatabase(IEnumerable<ShipmentEntity> shipments, ValidatedAddressScope validatedAddressScope, bool forceSave)
        {
            if (shipments == null || !shipments.Any())
            {
                return new Dictionary<ShipmentEntity, Exception>();
            }
            
            Dictionary<ShipmentEntity, Exception> errors = new Dictionary<ShipmentEntity, Exception>();

            foreach (ShipmentEntity shipment in shipments)
            {
                try
                {
                    // Force the shipment to look dirty to its forced to save.  This is to make sure that if any other
                    // changes had been made by other users we pick up the concurrency violation.
                    if (forceSave && !shipment.IsDirty)
                    {
                        shipment.Fields[(int)ShipmentFieldIndex.ShipmentType].IsChanged = true;
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
    }
}