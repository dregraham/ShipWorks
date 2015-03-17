using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Messaging;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Refresh shipments when a carrier is configured for the first time
    /// </summary>
    public sealed class CarrierConfigurationShipmentRefresher : IDisposable
    {
        private readonly IMessenger messenger;
        private readonly IShippingDialogInteraction shippingDialog;
        private readonly IShippingProfileManager shippingProfileManager;
        private readonly IShippingManager shippingManager;
        private readonly MessengerToken configuringCarrierToken;
        private readonly MessengerToken carrierConfiguredToken;

        /// <summary>
        /// Constructor
        /// </summary>
        public CarrierConfigurationShipmentRefresher(IMessenger messenger, IShippingDialogInteraction shippingDialog, 
            IShippingProfileManager shippingProfileManager, IShippingManager shippingManager)
        {
            this.messenger = MethodConditions.EnsureArgumentIsNotNull(messenger, "messenger");
            this.shippingDialog = MethodConditions.EnsureArgumentIsNotNull(shippingDialog, "shippingDialog");
            this.shippingProfileManager = MethodConditions.EnsureArgumentIsNotNull(shippingProfileManager, "shippingProfileManager");
            this.shippingManager = MethodConditions.EnsureArgumentIsNotNull(shippingManager, "shippingManager");

            configuringCarrierToken = messenger.Handle<ConfiguringCarrierMessage>(this, OnConfiguringCarrier);
            carrierConfiguredToken = messenger.Handle<CarrierConfiguredMessage>(this, OnCarrierConfigured);
        }

        /// <summary>
        /// A carrier is about to be configured
        /// </summary>
        private void OnConfiguringCarrier(ConfiguringCarrierMessage message)
        {
            // Save all loaded shipments in preparation for possibly changing the requested label type
            // This will cause any shipments that have been changed elsewhere to be noted
            IEnumerable<ShipmentEntity> unprocessedShipments = shippingDialog.FetchShipmentsFromShipmentControl().Where(x => !x.Processed);
            IDictionary<ShipmentEntity, Exception> errors = shippingDialog.SaveShipmentsToDatabase(unprocessedShipments, true);

            foreach (KeyValuePair<ShipmentEntity, Exception> error in errors)
            {
                shippingDialog.SetShipmentErrorMessage(error.Key.ShipmentID, error.Value, "updated");
            }
        }

        /// <summary>
        /// A carrier has just been configured
        /// </summary>
        /// <param name="message"></param>
        private void OnCarrierConfigured(CarrierConfiguredMessage message)
        {
            IEnumerable<ShipmentEntity> shipmentsToRefresh = shippingDialog.FetchShipmentsFromShipmentControl()
                .Where(s => !s.Processed && !shippingDialog.ShipmentHasError(s.ShipmentID));

            int? requestedLabelFormat = shippingProfileManager.GetDefaultProfile(message.ShipmentTypeCode).RequestedLabelFormat;
            if (!requestedLabelFormat.HasValue)
            {
                return;
            }

            foreach (ShipmentEntity shipment in shipmentsToRefresh)
            {
                shippingManager.RefreshShipment(shipment);
                shipment.RequestedLabelFormat = requestedLabelFormat.Value;
            }
        }

        /// <summary>
        /// Dispose any managed resources
        /// </summary>
        public void Dispose()
        {
            messenger.Remove(configuringCarrierToken);
            messenger.Remove(carrierConfiguredToken);
        }
    }
}