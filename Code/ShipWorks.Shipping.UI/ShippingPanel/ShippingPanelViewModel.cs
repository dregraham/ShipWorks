using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac.Features.OwnedInstances;
using ShipWorks.Core.Messaging;
using ShipWorks.AddressValidation;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Shipping.Loading;
using ShipWorks.Shipping.Services;
using System.Windows;
using ShipWorks.Messaging.Messages;
using ShipWorks.Core.Messaging.Messages.Shipping;
using ShipWorks.Shipping.UI.MessageHandlers;
using Interapptive.Shared.Collections;
using System.Reactive.Disposables;
using ShipWorks.Messaging.Messages.Shipping;

namespace ShipWorks.Shipping.UI.ShippingPanel
{
    /// <summary>
    /// Main view model for the shipment panel
    /// </summary>
    public class ShippingPanelViewModel : INotifyPropertyChanged, INotifyPropertyChanging, IDisposable
    {
        private ShippingPanelLoadedShipmentResult loadedShipmentResult;
        private bool supportsMultiplePackages;
        private ShipmentTypeCode selectedShipmentType;
        private ShipmentTypeCode initialShipmentTypeCode;
        private string requestedShippingMethod;
        private long originAddressType;
        private long initialOriginAddressType;
        private long accountId;
        private readonly PropertyChangedHandler handler;
        private OrderSelectionLoaded orderSelectionLoaded;
        private readonly IMessenger messenger;
        private bool allowEditing;
        private bool destinationAddressEditable;
        private readonly IShipmentTypeFactory shipmentTypeFactory;
        private readonly Func<ShipmentViewModel> shipmentViewModelFactory;
        private readonly IShippingManager shippingManager;
        private readonly ICustomsManager customsManager;
        private readonly IShipmentProcessor shipmentProcessor;
        private readonly Func<Owned<ICarrierConfigurationShipmentRefresher>> shipmentRefresherFactory;
        private ShipmentEntity shipment;
        private Visibility accountVisibility;
        private readonly IShipmentTypeManager shipmentTypeManager;
        private string domesticInternationalText;

        private bool listenForRateCriteriaChanged = false;
        private bool forceRateCriteriaChanged = false;
        private bool forceDomesticInternationalChanged = false;

        private readonly ICarrierShipmentAdapterFactory carrierShipmentAdapterFactory;
        private readonly OrderSelectionChangedHandler shipmentChangedHandler;
        private readonly IDisposable subscriptions;
        private readonly IMessageHelper messageHelper;

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
            IMessenger messenger,
            OrderSelectionChangedHandler shipmentChangedHandler,
            IShippingManager shippingManager,
            IShipmentTypeFactory shipmentTypeFactory,
            ICustomsManager customsManager,
            IShipmentProcessor shipmentProcessor,
            ICarrierShipmentAdapterFactory carrierShipmentAdapterFactory,
            IMessageHelper messageHelper,
            Func<Owned<ICarrierConfigurationShipmentRefresher>> shipmentRefresherFactory,
            Func<ShipmentViewModel> shipmentViewModelFactory,
            Func<AddressViewModel> addressViewModelFactory) : this()
        {
            this.shipmentProcessor = shipmentProcessor;
            this.customsManager = customsManager;
            this.shippingManager = shippingManager;
            this.shipmentTypeFactory = shipmentTypeFactory;
            this.messenger = messenger;
            this.shipmentChangedHandler = shipmentChangedHandler;
            this.messageHelper = messageHelper;

            shipmentTypeManager = new ShipmentTypeManagerWrapper();

            listenForRateCriteriaChanged = false;

            subscriptions = new CompositeDisposable(
                messenger.AsObservable<ShipmentChangedMessage>().Subscribe(OnShipmentChanged),
                messenger.AsObservable<StoreChangedMessage>().Subscribe(OnStoreChanged),
                messenger.AsObservable<ShipmentDeletedMessage>().Where(x => x.DeletedShipmentId == shipment?.ShipmentID).Subscribe(OnShipmentDeleted),
                shipmentChangedHandler.OrderChangingStream().Subscribe(_ => AllowEditing = false),
                shipmentChangedHandler.ShipmentLoadedStream().Do(_ => AllowEditing = true).Subscribe(LoadOrder)
            );

            this.carrierShipmentAdapterFactory = carrierShipmentAdapterFactory;
            this.shipmentViewModelFactory = shipmentViewModelFactory;
            this.shipmentRefresherFactory = shipmentRefresherFactory;

            handler = new PropertyChangedHandler(this, () => PropertyChanged, () => PropertyChanging);

            Origin = addressViewModelFactory();
            Destination = addressViewModelFactory();

            // Wiring up observables needs objects to not be null, so do this last.
            WireUpObservables();

            PropertyChanging += OnPropertyChanging;
        }

        /// <summary>
        /// Wire up any Obseravable patterns
        /// </summary>
        private void WireUpObservables()
        {
            // Merge the two address view models together so that we can respond to their property changed events
            Observable.Merge(
                (new List<AddressViewModel>() { Origin, Destination }).Select(
                    o => Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                        h => o.PropertyChanged += h,
                        h => o.PropertyChanged -= h
                        )
                    ))
                    .Where(evt =>
                    {
                        string propertyName = evt.EventArgs.PropertyName;

                        bool handleField = propertyName.Equals(nameof(AddressViewModel.CountryCode), StringComparison.InvariantCultureIgnoreCase) ||
                                           propertyName.Equals(nameof(AddressViewModel.PostalCode), StringComparison.InvariantCultureIgnoreCase) ||
                                           propertyName.Equals(nameof(AddressViewModel.StateProvCode), StringComparison.InvariantCultureIgnoreCase); 

                        // forceDomesticInternationalChanged is used for race conditions:
                        // For example (from rate criteria changing), ShipmentType property changes, and then before the throttle time, SupportsMultipleShipments changes.
                        // Since SupportsMultipleShipments isn't a rating field, the event would not be fired, even though 
                        // ShipmentType changed and the event needs to be raised.
                        // So keep track that during the throttling a rate criteria was changed.
                        forceDomesticInternationalChanged = forceDomesticInternationalChanged || handleField;

                        return forceDomesticInternationalChanged;
                    })
                .Throttle(TimeSpan.FromMilliseconds(250))
                .Subscribe(evt =>
                {
                    // Reset forceDomesticInternationalChanged so that we don't force it on the next round.
                    forceDomesticInternationalChanged = false;
                    Save();
                    DomesticInternationalText = shipmentTypeFactory.Get(shipment).IsDomestic(shipment) ? "Domestic" : "International";
                });

            //// Wire up the rate criteria obseravable throttling for the PropertyChanged event.
            //Observable.FromEventPattern<PropertyChangedEventArgs>(this, "PropertyChanged")
            //    // We only listen if listenForRateCriteriaChanged is true.
            //    .Where(evt => listenForRateCriteriaChanged)
            //    // Only fire the event if we have a shipment and it is a rating field.
            //    .Where(evt =>
            //    {
            //        bool hasShipment = orderSelectionLoaded?.Shipment != null;
            //        bool isRatingField = IsRatingField(evt.EventArgs.PropertyName);

            //        // forceRateCriteriaChanged is used for race conditions:
            //        // For example, ShipmentType property changes, and then before the throttle time, SupportsMultipleShipments changes.
            //        // Since SupportsMultipleShipments isn't a rating field, the event would not be fired, even though 
            //        // ShipmentType changed and the event needs to be raised.
            //        // So keep track that during the throttling a rate criteria was changed.
            //        forceRateCriteriaChanged = forceRateCriteriaChanged || (hasShipment && isRatingField);

            //        return forceRateCriteriaChanged;
            //    })
            //    .Throttle(TimeSpan.FromMilliseconds(250))
            //    .Subscribe(evt =>
            //    {
            //        // Reset forceRateCriteriaChanged so that we don't force it on the next round.
            //        forceRateCriteriaChanged = false;
            //        OnRateCriteriaPropertyChanged(null, evt.EventArgs);
            //    });
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
                if (handler.Set(nameof(ShipmentType), ref selectedShipmentType, value) && (shipment?.Processed ?? true) == false)
                {
                    shipment.ShipmentTypeCode = ShipmentType;
                    shippingManager.EnsureShipmentLoaded(shipment);

                    Populate();
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
        /// Returns "Domestic" or "International" depending on the shipments to/from fields.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string DomesticInternationalText
        {
            get { return domesticInternationalText; }
            set { handler.Set(nameof(DomesticInternationalText), ref domesticInternationalText, value); }
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
        public bool AllowEditing
        {
            get { return allowEditing; }
            set { handler.Set(nameof(AllowEditing), ref allowEditing, value); }
        }


        /// <summary>
        /// Is the loaded shipment destination address editable?
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool DestinationAddressEditable
        {
            get { return destinationAddressEditable; }
            set { handler.Set(nameof(DestinationAddressEditable), ref destinationAddressEditable, value); }
        }
        
        /// <summary>
        /// Origin address type that should be used
        /// </summary>
        [Obfuscation(Exclude = true)]
        public long OriginAddressType
        {
            get { return originAddressType; }
            set
            {
                if (handler.Set(nameof(OriginAddressType), ref originAddressType, value))
                {
                    Origin.SetAddressFromOrigin(OriginAddressType, shipment?.OrderID ?? 0, AccountId, ShipmentType);
                }
            }
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
        /// Id of the account for the shipment
        /// </summary>
        [Obfuscation(Exclude = true)]
        public long AccountId
        {
            get { return accountId; }
            set { handler.Set(nameof(AccountId), ref accountId, value); }
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
        /// True if the carrier supports accounts, false otherwise.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public Visibility AccountVisibility
        {
            get { return accountVisibility; }
            set { handler.Set(nameof(AccountVisibility), ref accountVisibility, value); }
        }

        /// <summary>
        /// Save the current shipment to the database
        /// </summary>
        public void SaveToDatabase()
        {
            if (shipment?.Processed ?? true)
            {
                return;
            }

            Save();
            IDictionary<ShipmentEntity, Exception> errors = shippingManager.SaveShipmentToDatabase(shipment, ValidatedAddressScope.Current, false);
            DisplayError(errors);
        }

        /// <summary>
        /// Load the shipment from the given order
        /// </summary>
        public void LoadOrder(OrderSelectionChangedMessage orderMessage)
        {
            int orders = orderMessage.LoadedOrderSelection.HasMoreOrLessThanCount(1);
            if (orders != 0)
            {
                return;
            }

            orderSelectionLoaded = orderMessage.LoadedOrderSelection.Single();
            LoadedShipmentResult = GetLoadedShipmentResult(orderSelectionLoaded);

            if (LoadedShipmentResult == ShippingPanelLoadedShipmentResult.Success)
            {
                shipment = orderSelectionLoaded.Shipments.Single();
                Populate();
            }
            else
            {
                shipment = null;
            }
        }

        /// <summary>
        /// Sets the LoadedShipmentResult based on orderSelectionLoaded
        /// </summary>
        private ShippingPanelLoadedShipmentResult GetLoadedShipmentResult(OrderSelectionLoaded loadedSelection)
        {
            if (loadedSelection.Exception != null)
            {
                return ShippingPanelLoadedShipmentResult.Error;
            }

            int moreOrLessThanOne = loadedSelection.Shipments.HasMoreOrLessThanCount(1);

            if (moreOrLessThanOne > 0)
            {
                return ShippingPanelLoadedShipmentResult.Multiple;
            }

            if (moreOrLessThanOne < 0)
            {
                return ShippingPanelLoadedShipmentResult.NotCreated;
            }

            return ShippingPanelLoadedShipmentResult.Success;
        }

        /// <summary>
        /// Order selection has changed
        /// </summary>
        internal void SelectionChanged() => AllowEditing = false;

        /// <summary>
        /// Populate the view model with the current state of the shipment
        /// </summary>
        private void Populate()
        {
            listenForRateCriteriaChanged = false;
            InitialShipmentTypeCode = shipment.ShipmentTypeCode;

            // Set the shipment type without going back through the shipment changed machinery
            selectedShipmentType = shipment.ShipmentTypeCode;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShipmentType)));

            RequestedShippingMethod = orderSelectionLoaded.Order.RequestedShipping;

            bool supportsAccounts = shipmentTypeManager.ShipmentTypesSupportingAccounts.Contains(shipment.ShipmentTypeCode);
            AccountVisibility = supportsAccounts ? Visibility.Visible : Visibility.Collapsed;

            // If the shipment type does not support accounts, and the current origin id is account, default to store origin.
            OriginAddressType = !supportsAccounts && shipment.OriginOriginID == 2 ? 0 : shipment.OriginOriginID;
            InitialOriginAddressType = OriginAddressType;

            ICarrierShipmentAdapter adapter = carrierShipmentAdapterFactory.Get(shipment);
            AccountId = adapter.AccountId.GetValueOrDefault();

            Origin.Load(shipment.OriginPerson);
            Destination.Load(shipment.ShipPerson);

            AllowEditing = !shipment.Processed;
            
            DestinationAddressEditable = AllowEditing && orderSelectionLoaded.DestinationAddressEditable; 

            listenForRateCriteriaChanged = true;

            Origin.SetAddressFromOrigin(OriginAddressType, shipment?.OrderID ?? 0, AccountId, ShipmentType);

            DomesticInternationalText = "Domestic";

            SupportsMultiplePackages = shipmentTypeFactory.Get(selectedShipmentType)?.SupportsMultiplePackages ?? false;
        }

        /// <summary>
        /// Process the current shipment using the specified processor
        /// </summary>
        public async Task ProcessShipment()
        {
            Save();

            using (ICarrierConfigurationShipmentRefresher refresher = shipmentRefresherFactory().Value)
            {
                IEnumerable<ShipmentEntity> shipments = await shipmentProcessor.Process(new[] { shipment }, refresher, null, null);
                //await LoadOrder(null);
                AllowEditing = (shipments?.FirstOrDefault()?.Processed ?? false) == false;
            }
        }

        /// <summary>
        /// Save the UI values to the shipment
        /// </summary>
        public void Save()
        {
            if (shipment?.Processed == true)
            {
                return;
            }

            ICarrierShipmentAdapter adapter = carrierShipmentAdapterFactory.Get(shipment);
            adapter.AccountId = AccountId;

            ShipmentType shipmentType = shipmentTypeFactory.Get(shipment);
            shipmentType.UpdateDynamicShipmentData(shipment);
            shipmentType.UpdateTotalWeight(shipment);

            shipment.OriginOriginID = OriginAddressType;

            Origin.SaveToEntity(shipment.OriginPerson);
            Destination.SaveToEntity(shipment.ShipPerson);

            IDictionary<ShipmentEntity, Exception> errors = customsManager.EnsureCustomsLoaded(new[] { shipment }, ValidatedAddressScope.Current);
            DisplayError(errors);
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
        private void UpdateServices() => ShipmentViewModel.RefreshServiceTypes(shipment);

        /// <summary>
        /// Updates the packages.
        /// </summary>
        private void UpdatePackages() => ShipmentViewModel.RefreshPackageTypes(shipment);

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
            if (shipment == null)
            {
                return;
            }

            // Save UI values to the shipment so we can send the new values to the rates panel
            Save();

            messenger.Send(new ShipmentChangedMessage(this, shipment));
        }

        /// <summary>
        /// Event for updating the view model when a shipment has changed.
        /// </summary>
        private void OnShipmentChanged(ShipmentChangedMessage shipmentChangedMessage)
        {
            // Don't handle shipment changed messages from ourselves
            if (Equals(shipmentChangedMessage.Sender))
            {
                return;
            }

            if (shipmentChangedMessage?.Shipment == null || shipment == null)
            {
                return;
            }

            if (shipmentChangedMessage.Shipment.ShipmentID == shipment.ShipmentID)
            {
                shipment = shipmentChangedMessage.Shipment;
                Populate();
            }
        }

        /// <summary>
        /// Handles StoreChangedMessages.
        /// </summary>
        private void OnStoreChanged(StoreChangedMessage storeChangedMessage)
        {
            if (OriginAddressType != (int)ShipmentOriginSource.Store)
            {
                return;
            }

            Origin.SetAddressFromOrigin(OriginAddressType, shipment?.OrderID ?? 0, AccountId, ShipmentType);
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

        /// <summary>
        /// Handle a property change before it actually happens
        /// </summary>
        private void OnPropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            if (e.PropertyName != nameof(ShipmentType))
            {
                return;
            }

            Save();
        }

        /// <summary>
        /// A shipment has been deleted
        /// </summary>
        private void OnShipmentDeleted(ShipmentDeletedMessage message)
        {
            // Check to see if there are any other un processed shipments we could display.
            List<ShipmentEntity> shipments = shippingManager.GetShipments(shipment.OrderID, false).Where(s => !s.Processed).ToList();

            if (shipments.Count == 1)
            {
                messenger.Send(new OrderSelectionChangingMessage(this, new List<long>() { shipment.OrderID }));
            }
            else
            {
                // There weren't any unprocessed shipments, so show as deleted.
                LoadedShipmentResult = ShippingPanelLoadedShipmentResult.Deleted;
                shipment = null;
            }
        }

        /// <summary>
        /// Show an error if one is associated with the current shipment
        /// </summary>
        private void DisplayError(IDictionary<ShipmentEntity, Exception> errors)
        {
            if (errors.ContainsKey(shipment))
            {
                messageHelper.ShowError("The selected shipments were edited or deleted by another ShipWorks user and your changes could not be saved.\n\n" +
                                        "The shipments will be refreshed to reflect the recent changes.");

                messenger.Send(new OrderSelectionChangingMessage(this, new[] { shipment.OrderID }));
            }
        }

        /// <summary>
        /// Dispose resources
        /// </summary>
        public void Dispose()
        {
            subscriptions.Dispose();
        }
    }
}
