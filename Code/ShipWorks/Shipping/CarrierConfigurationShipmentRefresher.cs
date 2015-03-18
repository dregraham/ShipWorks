using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Messaging;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Profiles;
using Interapptive.Shared.Collections;

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
        private readonly List<ShipmentEntity> shipmentsProcessing = new List<ShipmentEntity>();

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
            IEnumerable<ShipmentEntity> unprocessedShipments = shippingDialog.FetchShipmentsFromShipmentControl()
                .Except(shipmentsProcessing, x => x.ShipmentID)
                .Where(x => !x.Processed);
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
            shippingManager.UpdateLabelFormatOfUnprocessedShipments(message.ShipmentTypeCode);

            IEnumerable<ShipmentEntity> shipmentsToRefresh = shippingDialog.FetchShipmentsFromShipmentControl()
                .Except(shipmentsProcessing, x => x.ShipmentID)
                .Where(s => !s.Processed && !shippingDialog.ShipmentHasError(s.ShipmentID));

            int? requestedLabelFormat = shippingProfileManager.GetDefaultProfile(message.ShipmentTypeCode).RequestedLabelFormat;
            if (!requestedLabelFormat.HasValue)
            {
                return;
            }

            UpdateShipments(shipmentsToRefresh, message.ShipmentTypeCode, requestedLabelFormat.Value);
            UpdateShipments(shipmentsProcessing, message.ShipmentTypeCode, requestedLabelFormat.Value);
        }

        /// <summary>
        /// Tell the refresher which shipments are currently being processed so it can react accordingly
        /// </summary>
        public void ProcessingShipments(IEnumerable<ShipmentEntity> shipments)
        {
            shipmentsProcessing.Clear();
            shipmentsProcessing.AddRange(shipments);
        }

        /// <summary>
        /// Tell the refresher that processing is finished so it can behave normally
        /// </summary>
        public void FinishProcessing()
        {
            shipmentsProcessing.Clear();
        }

        /// <summary>
        /// Update the requested label format of the collection of shipments
        /// </summary>
        private void UpdateShipments(IEnumerable<ShipmentEntity> shipments, ShipmentTypeCode shipmentTypeCode, int requestedLabelFormat)
        {
            foreach (ShipmentEntity shipment in shipments.Where(x => x.ShipmentType == (int)shipmentTypeCode))
            {
                shippingManager.RefreshShipment(shipment);
                shipment.RequestedLabelFormat = requestedLabelFormat;
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