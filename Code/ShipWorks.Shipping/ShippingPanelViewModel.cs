﻿using System;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.UI;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Interapptive.Shared.Messaging;
using System.Collections.Generic;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Main view model for the shipment panel
    /// </summary>
    public class ShippingPanelViewModel : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private bool supportsMultiplePackages;
        private ShipmentTypeCode selectedShipmentType;
        private PropertyChangedHandler handler;
        private AddressViewModel origin;
        private AddressViewModel destination;
        private ShipmentViewModel shipment;
        private ILoader<ShippingPanelLoadedShipment> shipmentLoader;
        private ShippingPanelLoadedShipment loadedShipment;
        private IMessenger messenger;
        private bool isProcessed;
        private Func<ShipmentTypeCode, ShipmentType> shipmentTypeFactory;

        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        public ShippingPanelViewModel()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingPanelViewModel(ILoader<ShippingPanelLoadedShipment> shipmentLoader, IMessenger messenger, IShippingManager shipmentPersister, Func<ShipmentTypeCode, ShipmentType> shipmentTypeFactory)
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged, () => PropertyChanging);

            this.shipmentTypeFactory = shipmentTypeFactory;
            this.shipmentLoader = shipmentLoader;
            this.messenger = messenger;
        }

        /// <summary>
        /// Load the shipment from the given order
        /// </summary>
        public async Task LoadOrder(long orderID)
        {
            loadedShipment = await shipmentLoader.LoadAsync(orderID);

            if (loadedShipment.Shipment == null)
            {
                // No shipment was created.  Show a message and return.
                // TODO: Show a message

                return;
            }

            DisableRateCriteriaChanged();

            SelectedShipmentType = loadedShipment.Shipment.ShipmentTypeCode;
            Origin.Load(loadedShipment.Shipment.OriginPerson);

            Destination.Load(loadedShipment.Shipment.ShipPerson);

            Shipment.Load(loadedShipment.Shipment);

            IsProcessed = loadedShipment.Shipment.Processed;

            EnableRateCriteriaChanged();
        }

        /// <summary>
        /// Process the current shipment using the specified processor
        /// </summary>
        public async Task ProcessShipment(ShipmentProcessor shipmentProcessor, CarrierConfigurationShipmentRefresher refresher)
        {
            Save();
            IEnumerable<ShipmentEntity> shipments = await shipmentProcessor.Process(new[] { loadedShipment.Shipment }, refresher, null, null);
            await LoadOrder(loadedShipment.Shipment.OrderID);
            IsProcessed = shipments?.FirstOrDefault()?.Processed ?? false;
        }

        /// <summary>
        /// Selected shipment type for the current shipment
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ShipmentTypeCode SelectedShipmentType
        {
            get { return selectedShipmentType; }
            set
            {
                if (handler.Set(nameof(SelectedShipmentType), ref selectedShipmentType, value))
                {
                    SupportsMultiplePackages = shipmentTypeFactory(selectedShipmentType)?.SupportsMultiplePackages ?? false;
                }
            }
        }

        /// <summary>
        /// Are multiple packages supported
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool SupportsMultiplePackages
        {
            get { return supportsMultiplePackages; }
            set { handler.Set(nameof(SupportsMultiplePackages), ref supportsMultiplePackages, value); }
        }

        /// <summary>
        /// Are multiple packages supported
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool IsProcessed
        {
            get { return isProcessed; }
            set { handler.Set(nameof(IsProcessed), ref isProcessed, value); }
        }

        public AddressViewModel Origin => origin ?? (origin = new AddressViewModel());

        public AddressViewModel Destination => destination ?? (destination = new AddressViewModel());

        public ShipmentViewModel Shipment => shipment ?? (shipment = new ShipmentViewModel());

        /// <summary>
        /// Save the UI values to the shipment
        /// </summary>
        public void Save()
        {
            loadedShipment.Shipment.ShipmentTypeCode = SelectedShipmentType;
            Origin.SaveToEntity(loadedShipment.Shipment.OriginPerson);
            Destination.SaveToEntity(loadedShipment.Shipment.ShipPerson);
            Shipment.Save(loadedShipment.Shipment);
        }

        /// <summary>
        /// Raised when a view model field changes so that rates may be updated.
        /// </summary>
        private void OnRateCriteriaPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Only send the ShipmentChangedMessage message if the field that changed is a rating field.
            ShipmentType shipmentType = ShipmentTypeManager.GetType(loadedShipment.Shipment);

            // Since we have a generic AddressViewModel whose properties do not match entity feild names,
            // we need to translate the Ship, Origin, and Street properties to know if the changed field
            // is one rating cares about.
            string name = e.PropertyName;
            string shipName = string.Format("Ship{0}", e.PropertyName);
            string origName = string.Format("Origin{0}", e.PropertyName);

            if (shipmentType.RatingFields.FieldsContainName(name) ||
                shipmentType.RatingFields.FieldsContainName(shipName) ||
                shipmentType.RatingFields.FieldsContainName(origName) ||
                name.Equals("Street", StringComparison.InvariantCultureIgnoreCase))
            {
                // Save UI values to the shipment so we can send the new values to the rates panel
                Save();

                messenger.Send(new ShipmentChangedMessage(this, loadedShipment.Shipment));
            }
        }

        /// <summary>
        /// Wire up the rate criteria changed event to the view models.
        /// </summary>
        private void EnableRateCriteriaChanged()
        {
            Origin.PropertyChanged += OnRateCriteriaPropertyChanged;
            Destination.PropertyChanged += OnRateCriteriaPropertyChanged;
            Shipment.PropertyChanged += OnRateCriteriaPropertyChanged;
        }

        /// <summary>
        /// Remove the rate criteria changed event from the view models.
        /// </summary>
        private void DisableRateCriteriaChanged()
        {
            Origin.PropertyChanged -= OnRateCriteriaPropertyChanged;
            Destination.PropertyChanged -= OnRateCriteriaPropertyChanged;
            Shipment.PropertyChanged -= OnRateCriteriaPropertyChanged;
        }
    }
}
