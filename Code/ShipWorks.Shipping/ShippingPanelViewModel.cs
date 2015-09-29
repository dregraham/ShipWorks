using System;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Interapptive.Shared.Messaging;
using System.Collections.Generic;
using System.Reactive.Linq;
using ShipWorks.AddressValidation;
using System.Windows.Input;
using Autofac.Features.OwnedInstances;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Main view model for the shipment panel
    /// </summary>
    public class ShippingPanelViewModel : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private ShippingPanelLoadedShipmentResult loadedShipmentResult;
        private bool supportsMultiplePackages;
        private ShipmentTypeCode selectedShipmentType;
        private ShipmentTypeCode initialShipmentTypeCode;
        private string requestedShippingMethod;
        private long originAddressType;
        private long initialOriginAddressType;
        private readonly PropertyChangedHandler handler;
        private readonly ILoader<ShippingPanelLoadedShipment> shipmentLoader;
        private ShippingPanelLoadedShipment loadedShipment;
        private readonly IMessenger messenger;
        private bool isProcessed;
        private readonly IShipmentTypeFactory shipmentTypeFactory;
        private readonly Func<ShipmentViewModel> shipmentViewModelFactory;
        private readonly IShippingManager shippingManager;
        private readonly ICustomsManager customsManager;
        private readonly IShipmentProcessor shipmentProcessor;
        private readonly Func<Owned<ICarrierConfigurationShipmentRefresher>> shipmentRefresherFactory;

        private bool listenForRateCriteriaChanged = false;
        private bool forceRateCriteriaChanged = false;

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
            IShipmentProcessor shipmentProcessor,
            Func<Owned<ICarrierConfigurationShipmentRefresher>> shipmentRefresherFactory,
            Func<ShipmentViewModel> shipmentViewModelFactory) : this()
        {
            this.shipmentProcessor = shipmentProcessor;
            this.customsManager = customsManager;
            this.shippingManager = shippingManager;
            this.shipmentTypeFactory = shipmentTypeFactory;
            this.shipmentLoader = shipmentLoader;
            this.messenger = messenger;
            listenForRateCriteriaChanged = false;

            messenger.Handle<ShipmentChangedMessage>(this, OnShipmentChanged);

            WireUpObservables();

            this.shipmentViewModelFactory = shipmentViewModelFactory;
            this.shipmentRefresherFactory = shipmentRefresherFactory;

            handler = new PropertyChangedHandler(this, () => PropertyChanged, () => PropertyChanging);

            //Origin = new AddressViewModel();
            //Destination = new AddressViewModel();

            //ShipmentViewModel = shipmentViewModelFactory();

            //CreateLabelCommand = new RelayCommand(async () => await ProcessShipment());
        }

        /// <summary>
        /// Wire up any Obseravable patterns
        /// </summary>
        private void WireUpObservables()
        {
            // Wire up the rate criteria obseravable throttling for the PropertyChanged event.
            Observable.FromEventPattern<PropertyChangedEventArgs>(this, "PropertyChanged")
                // We only listen if listenForRateCriteriaChanged is true.
                .Where(evt => listenForRateCriteriaChanged)
                // Only fire the event if we have a shipment and it is a rating field.
                .Where(evt =>
                {
                    bool hasShipment = loadedShipment?.Shipment != null;
                    bool isRatingField = IsRatingField(evt.EventArgs.PropertyName);

                    // forceRateCriteriaChanged is used for race conditions:
                    // For example, ShipmentType property changes, and then before the throttle time, SupportsMultipleShipments changes.
                    // Since SupportsMultipleShipments isn't a rating field, the event would not be fired, even though 
                    // ShipmentType changed and the event needs to be raised.
                    // So keep track that during the throttling a rate criteria was changed.
                    forceRateCriteriaChanged = forceRateCriteriaChanged || (hasShipment && isRatingField);

                    return forceRateCriteriaChanged;
                })
                .Throttle(TimeSpan.FromMilliseconds(250))
                .Subscribe(evt =>
                {
                    // Reset forceRateCriteriaChanged so that we don't force it on the next round.
                    forceRateCriteriaChanged = false;
                    OnRateCriteriaPropertyChanged(null, evt.EventArgs);
                });
        }

        /// <summary>
        /// Command to create a label
        /// </summary>
        public ICommand CreateLabelCommand { get; }

        /// <summary>
        /// Selected shipment type code for the current shipment
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ShipmentTypeCode ShipmentType
        {
            get { return selectedShipmentType; }
            set
            {
                if (handler.Set(nameof(ShipmentType), ref selectedShipmentType, value))
                {
                    if (loadedShipment?.Shipment != null)
                    {
                        shippingManager.EnsureShipmentLoaded(loadedShipment.Shipment);
                    }

                    SupportsMultiplePackages = shipmentTypeFactory.Get(selectedShipmentType)?.SupportsMultiplePackages ?? false;
                    //UpdateServices();
                    //UpdatePackages();
                }
            }
        }

        /// <summary>
        /// Initial shipment type code for the current shipment
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ShipmentTypeCode InitialShipmentTypeCode
        {
            get { return initialShipmentTypeCode; }
            set { handler.Set(nameof(InitialShipmentTypeCode), ref initialShipmentTypeCode, value); }
        }

        /// <summary>
        /// Method of shipping requested by the customer
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string RequestedShippingMethod
        {
            get { return requestedShippingMethod; }
            set { handler.Set(nameof(RequestedShippingMethod), ref requestedShippingMethod, value); }
        }

        /// <summary>
        /// The ShippingPanelLoadedShipmentResult for the selected order
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ShippingPanelLoadedShipmentResult LoadedShipmentResult
        {
            get { return loadedShipmentResult; }
            set { handler.Set(nameof(LoadedShipmentResult), ref loadedShipmentResult, value); }
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
        /// Is the loaded shipment processed?
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool IsProcessed
        {
            get { return isProcessed; }
            set { handler.Set(nameof(IsProcessed), ref isProcessed, value); }
        }

        /// <summary>
        /// Origin address type that should be used
        /// </summary>
        [Obfuscation(Exclude = true)]
        public long OriginAddressType
        {
            get { return originAddressType; }
            set { handler.Set(nameof(OriginAddressType), ref originAddressType, value); }
        }

        /// <summary>
        /// Original origin address type
        /// </summary>
        [Obfuscation(Exclude = true)]
        public long InitialOriginAddressType
        {
            get { return initialOriginAddressType; }
            set { handler.Set(nameof(InitialOriginAddressType), ref initialOriginAddressType, value); }
        }

        /// <summary>
        /// The origin address view model.
        /// </summary>
        public AddressViewModel Origin { get; }

        /// <summary>
        /// The destination address view model.
        /// </summary>
        public AddressViewModel Destination { get; }

        /// <summary>
        /// The Shipment view model.
        /// </summary>
        public ShipmentViewModel ShipmentViewModel { get; set; }

        /// <summary>
        /// Save the current shipment to the database
        /// </summary>
        public void SaveToDatabase()
        {
            if (loadedShipment?.Shipment?.Processed ?? true)
            {
                return;
            }

            Save();
            shippingManager.SaveShipmentToDatabase(loadedShipment.Shipment, ValidatedAddressScope.Current, false);
        }

        /// <summary>
        /// Load the shipment from the given order
        /// </summary>
        public async Task LoadOrder(long orderID)
        {
            loadedShipment = await shipmentLoader.LoadAsync(orderID);

            LoadedShipmentResult = loadedShipment.Result;

            if (loadedShipment.Shipment == null)
            {
                // No shipment was created.  Show a message and return.
                // TODO: Show a message

                return;
            }

            listenForRateCriteriaChanged = false;
            //DisableRateCriteriaChanged();
            //DisableNeedToUpdateServices();
            //DisableNeedToUpdatePackages();

            RequestedShippingMethod = loadedShipment.RequestedShippingMode;
            InitialShipmentTypeCode = loadedShipment.Shipment.ShipmentTypeCode;
            ShipmentType = loadedShipment.Shipment.ShipmentTypeCode;
            OriginAddressType = loadedShipment.Shipment.OriginOriginID;
            InitialOriginAddressType = loadedShipment.Shipment.OriginOriginID;

            //Origin.Load(loadedShipment.Shipment.OriginPerson);

            //Destination.Load(loadedShipment.Shipment.ShipPerson);

            //ShipmentViewModel.Load(loadedShipment.Shipment);

            IsProcessed = loadedShipment.Shipment.Processed;

            listenForRateCriteriaChanged = true;

            //EnableRateCriteriaChanged();
            //EnableNeedToUpdateServices();
            //EnableNeedToUpdatePackages();
        }

        /// <summary>
        /// Process the current shipment using the specified processor
        /// </summary>
        public async Task ProcessShipment()
        {
            Save();

            using (ICarrierConfigurationShipmentRefresher refresher = shipmentRefresherFactory().Value)
            {
                IEnumerable<ShipmentEntity> shipments = await shipmentProcessor.Process(new[] { loadedShipment.Shipment }, refresher, null, null);
                await LoadOrder(loadedShipment.Shipment.OrderID);
                IsProcessed = shipments?.FirstOrDefault()?.Processed ?? false;
            }
        }

        /// <summary>
        /// Save the UI values to the shipment
        /// </summary>
        public void Save()
        {
            if (!loadedShipment.Shipment.Processed)
            {
                ShipmentType shipmentType = shipmentTypeFactory.Get(loadedShipment.Shipment);
                shipmentType.UpdateDynamicShipmentData(loadedShipment.Shipment);
                shipmentType.UpdateTotalWeight(loadedShipment.Shipment);

                loadedShipment.Shipment.ShipmentTypeCode = ShipmentType;
                //Origin.SaveToEntity(loadedShipment.Shipment.OriginPerson);
                //Destination.SaveToEntity(loadedShipment.Shipment.ShipPerson);
                //ShipmentViewModel.Save(loadedShipment.Shipment);

                customsManager.EnsureCustomsLoaded(new[] { loadedShipment.Shipment }, ValidatedAddressScope.Current);
            }
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
        private void UpdateServices() => ShipmentViewModel.RefreshServiceTypes(loadedShipment.Shipment);

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
            if (loadedShipment?.Shipment == null)
            {
                return;
            }
            
            // Save UI values to the shipment so we can send the new values to the rates panel
            Save();

            messenger.Send(new ShipmentChangedMessage(this, loadedShipment.Shipment));
        }

        /// <summary>
        /// Event for updating the view model when a shipment has changed.
        /// </summary>
        private void OnShipmentChanged(ShipmentChangedMessage shipmentChangedMessage)
        {
            if (shipmentChangedMessage?.Shipment == null || loadedShipment?.Shipment == null)
            {
                return;
            }

            if (shipmentChangedMessage.Shipment.ShipmentID == loadedShipment.Shipment.ShipmentID)
            {
                ShipmentType = shipmentChangedMessage.Shipment.ShipmentTypeCode;
            }
        }

        /// <summary>
        /// Determines if a view model field is used for the shipment's rating criteria.
        /// </summary>
        private bool IsRatingField(string propertyname)
        {
            // Only send the ShipmentChangedMessage message if the field that changed is a rating field.
            ShipmentType shipmentType = shipmentTypeFactory.Get(ShipmentType);

            // Since we have a generic AddressViewModel whose properties do not match entity feild names,
            // we need to translate the Ship, Origin, and Street properties to know if the changed field
            // is one rating cares about.
            string name = propertyname;
            string shipName = string.Format("Ship{0}", name);
            string origName = string.Format("Origin{0}", name);

            return shipmentType.RatingFields.FieldsContainName(name) ||
                   shipmentType.RatingFields.FieldsContainName(shipName) ||
                   shipmentType.RatingFields.FieldsContainName(origName) ||
                   name.Equals("Street", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
