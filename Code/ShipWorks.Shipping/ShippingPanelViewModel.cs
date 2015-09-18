using System;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.UI;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Interapptive.Shared.Messaging;
using System.Collections.Generic;
using ShipWorks.AddressValidation;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Main view model for the shipment panel
    /// </summary>
    public class ShippingPanelViewModel : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private ShippingPanelLoadedShipmentResult loadResult;
        private bool supportsMultiplePackages;
        private ShipmentTypeCode selectedShipmentType;
        private readonly PropertyChangedHandler handler;
        private readonly ILoader<ShippingPanelLoadedShipment> shipmentLoader;
        private ShippingPanelLoadedShipment loadedShipment;
        private readonly IMessenger messenger;
        private bool isProcessed;
        private readonly IShipmentTypeFactory shipmentTypeFactory;
        private readonly Func<ShipmentViewModel> shipmentViewModelFactory;
        private readonly IShippingManager shippingManager;
        private readonly ICustomsManager customsManager;

        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShippingPanelViewModel"/> class.
        /// </summary>
        public ShippingPanelViewModel()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingPanelViewModel(
            ILoader<ShippingPanelLoadedShipment> shipmentLoader,
            IMessenger messenger,
            IShippingManager shippingManager,
            IShipmentTypeFactory shipmentTypeFactory,
            ICustomsManager customsManager,
            Func<ShipmentViewModel> shipmentViewModelFactory) : this()
        {
            this.customsManager = customsManager;
            this.shippingManager = shippingManager;
            this.shipmentTypeFactory = shipmentTypeFactory;
            this.shipmentViewModelFactory = shipmentViewModelFactory;
            this.shipmentLoader = shipmentLoader;
            this.messenger = messenger;

            handler = new PropertyChangedHandler(this, () => PropertyChanged, () => PropertyChanging);
            Origin = new AddressViewModel();
            Destination = new AddressViewModel();

            ShipmentViewModel = shipmentViewModelFactory();
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
                    if (loadedShipment?.Shipment != null)
                    {
                        shippingManager.EnsureShipmentLoaded(loadedShipment.Shipment);
                    }

                    SupportsMultiplePackages = shipmentTypeFactory.Get(selectedShipmentType)?.SupportsMultiplePackages ?? false;
                    UpdateServices();
                    UpdatePackages();
                }
            }
        }

        /// <summary>
        /// Are multiple packages supported
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ShippingPanelLoadedShipmentResult LoadResult
        {
            get { return loadResult; }
            set { handler.Set(nameof(LoadResult), ref loadResult, value); }
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

        public AddressViewModel Origin { get; }

        public AddressViewModel Destination { get; }

        public ShipmentViewModel ShipmentViewModel { get; }

        /// <summary>
        /// Save the current shipment to the database
        /// </summary>
        public void SaveToDatabase()
        {
            if (loadedShipment?.Shipment == null)
            {
                return;
            }

            Save();
            shippingManager.SaveShipmentsToDatabase(new[] { loadedShipment.Shipment }, ValidatedAddressScope.Current, false);
        }

        /// <summary>
        /// Load the shipment from the given order
        /// </summary>
        public async Task LoadOrder(long orderID)
        {
            loadedShipment = await shipmentLoader.LoadAsync(orderID);

            LoadResult = loadedShipment.Result;

            if (loadedShipment.Shipment == null)
            {
                // No shipment was created.  Show a message and return.
                // TODO: Show a message

                return;
            }

            DisableRateCriteriaChanged();
            DisableNeedToUpdateServices();
            DisableNeedToUpdatePackages();

            SelectedShipmentType = loadedShipment.Shipment.ShipmentTypeCode;
            Origin.Load(loadedShipment.Shipment.OriginPerson);

            Destination.Load(loadedShipment.Shipment.ShipPerson);

            ShipmentViewModel.Load(loadedShipment.Shipment);

            IsProcessed = loadedShipment.Shipment.Processed;

            EnableRateCriteriaChanged();
            EnableNeedToUpdateServices();
            EnableNeedToUpdatePackages();
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
        /// Save the UI values to the shipment
        /// </summary>
        public void Save()
        {
            ShipmentType shipmentType = shipmentTypeFactory.Get(loadedShipment.Shipment);
            shipmentType.UpdateDynamicShipmentData(loadedShipment.Shipment);
            shipmentType.UpdateTotalWeight(loadedShipment.Shipment);

            loadedShipment.Shipment.ShipmentTypeCode = SelectedShipmentType;
            Origin.SaveToEntity(loadedShipment.Shipment.OriginPerson);
            Destination.SaveToEntity(loadedShipment.Shipment.ShipPerson);
            ShipmentViewModel.Save(loadedShipment.Shipment);

            customsManager.EnsureCustomsLoaded(new[] { loadedShipment.Shipment }, ValidatedAddressScope.Current);
        }

        /// <summary>
        /// Called when [need to update services].
        /// </summary>
        private void OnNeedToUpdateServices(object sender, PropertyChangedEventArgs e)
        {
            Save();
            UpdateServices();
        }

        /// <summary>
        /// Called when [need to update packages].
        /// </summary>
        private void OnNeedToUpdatePackages(object sender, PropertyChangedEventArgs e)
        {
            Save();
            UpdatePackages();
        }

        /// <summary>
        /// Updates the services.
        /// </summary>
        private void UpdateServices() => ShipmentViewModel.RefreshShipmentTypes(loadedShipment.Shipment);

        /// <summary>
        /// Updates the packages.
        /// </summary>
        private void UpdatePackages() => ShipmentViewModel.RefreshPackageTypes(loadedShipment.Shipment);

        /// <summary>
        /// Enables the need to update packages.
        /// </summary>
        private void EnableNeedToUpdatePackages()
        {
            Origin.PropertyChanged += OnNeedToUpdatePackages;
            Destination.PropertyChanged += OnNeedToUpdatePackages;
        }

        /// <summary>
        /// Enables NeedToUpdateServices event
        /// </summary>
        private void EnableNeedToUpdateServices()
        {
            Origin.PropertyChanged += OnNeedToUpdateServices;
            Destination.PropertyChanged += OnNeedToUpdateServices;
        }

        /// <summary>
        /// Disables the need to update packages.
        /// </summary>
        private void DisableNeedToUpdatePackages()
        {
            Origin.PropertyChanged -= OnNeedToUpdatePackages;
            Destination.PropertyChanged -= OnNeedToUpdatePackages;
        }

        /// <summary>
        /// Disables NeedToUpdateServices event
        /// </summary>
        private void DisableNeedToUpdateServices()
        {
            Origin.PropertyChanged -= OnNeedToUpdateServices;
            Destination.PropertyChanged -= OnNeedToUpdateServices;
        }

        /// <summary>
        /// Raised when a view model field changes so that rates may be updated.
        /// </summary>
        private void OnRateCriteriaPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Only send the ShipmentChangedMessage message if the field that changed is a rating field.
            ShipmentType shipmentType = shipmentTypeFactory.Get(loadedShipment.Shipment);

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
            ShipmentViewModel.PropertyChanged += OnRateCriteriaPropertyChanged;
        }

        /// <summary>
        /// Remove the rate criteria changed event from the view models.
        /// </summary>
        private void DisableRateCriteriaChanged()
        {
            Origin.PropertyChanged -= OnRateCriteriaPropertyChanged;
            Destination.PropertyChanged -= OnRateCriteriaPropertyChanged;
            ShipmentViewModel.PropertyChanged -= OnRateCriteriaPropertyChanged;
        }
    }
}
