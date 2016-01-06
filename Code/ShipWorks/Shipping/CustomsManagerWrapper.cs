using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Wrapper for the static customs manager class
    /// </summary>
    public class CustomsManagerWrapper : ICustomsManager
    {
        private static readonly IEnumerable<EntityField2> fields = new[]
        {
            ShipmentFields.ShipStateProvCode, ShipmentFields.ShipCountryCode,
            ShipmentFields.OriginStateProvCode, ShipmentFields.OriginCountryCode
        };

        private readonly IShippingManager shippingManager;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="shippingManager"></param>
        public CustomsManagerWrapper(IShippingManager shippingManager)
        {
            this.shippingManager = shippingManager;
        }

        /// <summary>
        /// Ensure custom's contents for the given shipment have been created
        /// </summary>
        public void LoadCustomsItems(ShipmentEntity shipment, bool reloadIfPresent) =>
            CustomsManager.LoadCustomsItems(shipment, reloadIfPresent);
        
        /// <summary>
        /// Ensire customs items are loaded if the address or shipment type has changed
        /// </summary>
        public IDictionary<ShipmentEntity, Exception> EnsureCustomsLoaded(IEnumerable<ShipmentEntity> shipments)
        {
            // We also need to save changes to any whose state\country has changed, since that affects customs items requirements
            List<ShipmentEntity> destinationChangeNeedsSaved = shipments.Where(s => fields.Any(x => s.Fields[x.FieldIndex].IsChanged)).ToList();

            // We need to show the user if anything went wrong while doing that
            IDictionary<ShipmentEntity, Exception> errors = shippingManager.SaveShipmentsToDatabase(destinationChangeNeedsSaved, false);

            // When the destination address is changed we save the affected shipments to the database right away to ensure that any concurrency errors that could
            // occur while generating customs items are caught right away.  This is why we do the Load here - if the customs items already exist, then this will do
            // nothing.  Which means that this will do nothing unless the shipment changed from being a domestic to an international.
            foreach (ShipmentEntity shipment in destinationChangeNeedsSaved.Except(errors.Keys))
            {
                try
                {
                    LoadCustomsItems(shipment, false);
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

            // Go through each one not known to be deleted and try to get it completely refreshed so we can update it's UI display
            foreach (ShipmentEntity shipment in errors.Select(x => x.Key).Where(x => !x.DeletedFromDatabase))
            {
                // It we don't think its deleted, and it had a concurrency exception, try to refresh it
                try
                {
                    shippingManager.RefreshShipment(shipment);
                }
                catch (ObjectDeletedException)
                {
                    // We can ignore this for now - we'll deal with the deleted list in a minute
                }
            }

            return errors;
        }

        /// <summary>
        /// Create a new ShipmentCustomsItemEntity for the given shipment, filled in with defaults
        /// </summary>
        public ShipmentCustomsItemEntity CreateCustomsItem(ShipmentEntity shipment)
        {
            return CustomsManager.CreateCustomsItem(shipment);
        }
    }
}
