using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Shipping.Loading;
using ShipWorks.Shipping.Services;
using ShipWorks.Messaging.Messages;
using ShipWorks.Core.Messaging.Messages.Shipping;
using ShipWorks.Shipping.UI.MessageHandlers;
using Interapptive.Shared.Collections;
using System.Reactive.Disposables;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping.UI.ShippingPanel.AddressControl;

namespace ShipWorks.Shipping.UI.ShippingPanel
{
    /// <summary>
    /// Main view model for the shipment panel
    /// </summary>
    public partial class ShippingPanelViewModel : INotifyPropertyChanged, INotifyPropertyChanging, IDisposable
    {
        private readonly PropertyChangedHandler handler;
        private ICarrierShipmentAdapter shipmentAdapter;
        private OrderSelectionLoaded orderSelectionLoaded;
        
        private bool forceDomesticInternationalChanged = false;

        private readonly IShippingManager shippingManager;
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
            OrderSelectionChangedHandler shipmentChangedHandler,
            IShippingManager shippingManager,
            IMessageHelper messageHelper,
            IShippingViewModelFactory shippingViewModelFactory) : this()
        {
            this.shippingManager = shippingManager;
            this.shipmentChangedHandler = shipmentChangedHandler;
            this.messageHelper = messageHelper;
            
            handler = new PropertyChangedHandler(this, () => PropertyChanged, () => PropertyChanging);

            subscriptions = new CompositeDisposable(
                shipmentChangedHandler.OfType<ShipmentChangedMessage>().Subscribe(OnShipmentChanged),
                shipmentChangedHandler.OfType<StoreChangedMessage>().Subscribe(OnStoreChanged),
                shipmentChangedHandler.OfType<ShipmentDeletedMessage>().Where(x => x.DeletedShipmentId == shipmentAdapter?.Shipment?.ShipmentID).Subscribe(OnShipmentDeleted),
                shipmentChangedHandler.OrderChangingStream().Subscribe(_ => AllowEditing = false),
                shipmentChangedHandler.ShipmentLoadedStream().Do(_ => AllowEditing = true).Subscribe(LoadOrder),
                handler.Where(x => x == nameof(ShipmentType))
                    .Where(x => !(shipmentAdapter.Shipment?.Processed ?? true))
                    .Subscribe(_ => {
                        shipmentAdapter = shippingManager.ChangeShipmentType(ShipmentType, shipmentAdapter.Shipment);
                        Populate();
                    }),
                handler.Where(x => x == nameof(OriginAddressType))
                    .Subscribe(_ => Origin.SetAddressFromOrigin(OriginAddressType, shipmentAdapter.Shipment?.OrderID ?? 0, AccountId, ShipmentType))
            );
            
            Origin = shippingViewModelFactory.GetAddressViewModel();
            Destination = shippingViewModelFactory.GetAddressViewModel();
            Destination.IsAddressValidationEnabled = true;

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
                (new List<AddressViewModel> { Origin, Destination }).Select(
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

                    Destination.SaveToEntity(shipmentAdapter.Shipment.ShipPerson);
                    Origin.SaveToEntity(shipmentAdapter.Shipment.OriginPerson);

                    DomesticInternationalText = shipmentAdapter.IsDomestic ? "Domestic" : "International";
                });

#pragma warning disable S125 // Sections of code should not be "commented out"
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
#pragma warning restore S125 // Sections of code should not be "commented out"

        /// <summary>
        /// Save the current shipment to the database
        /// </summary>
        public void SaveToDatabase()
        {
            if (shipmentAdapter?.Shipment?.Processed ?? true)
            {
                return;
            }

            Save();
            IDictionary<ShipmentEntity, Exception> errors = shippingManager.SaveShipmentToDatabase(shipmentAdapter.Shipment, false);
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
                shipmentAdapter = orderSelectionLoaded.ShipmentAdapters.Single();
                Populate();
            }
            else
            {
                shipmentAdapter = null;
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

            int moreOrLessThanOne = loadedSelection.ShipmentAdapters.HasMoreOrLessThanCount(1);

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
            InitialShipmentTypeCode = shipmentAdapter.ShipmentTypeCode;

            // Set the shipment type without going back through the shipment changed machinery
            selectedShipmentType = shipmentAdapter.ShipmentTypeCode;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShipmentType)));

            RequestedShippingMethod = orderSelectionLoaded.Order.RequestedShipping;

            SupportsAccounts = shipmentAdapter.SupportsAccounts;

            // If the shipment type does not support accounts, and the current origin id is account, default to store origin.
            OriginAddressType = !shipmentAdapter.SupportsAccounts && shipmentAdapter.Shipment.OriginOriginID == 2 ? 0 : shipmentAdapter.Shipment.OriginOriginID;
            InitialOriginAddressType = OriginAddressType;

            AccountId = shipmentAdapter.AccountId.GetValueOrDefault();

            Origin.Load(shipmentAdapter.Shipment.OriginPerson);
            Destination.Load(shipmentAdapter.Shipment.ShipPerson);

            AllowEditing = !shipmentAdapter.Shipment.Processed;

            DestinationAddressEditableState = orderSelectionLoaded.DestinationAddressEditable;
            
            Origin.SetAddressFromOrigin(OriginAddressType, shipmentAdapter.Shipment?.OrderID ?? 0, AccountId, ShipmentType);

            DomesticInternationalText = "Domestic";

            SupportsMultiplePackages = shipmentAdapter.SupportsMultiplePackages;
        }

#pragma warning disable S125 // Sections of code should not be "commented out"
        ///// <summary>
        ///// Process the current shipment using the specified processor
        ///// </summary>
        //public async Task ProcessShipment()
        //{
        //    Save();

        //    using (ICarrierConfigurationShipmentRefresher refresher = shipmentRefresherFactory().Value)
        //    {
        //        IEnumerable<ShipmentEntity> shipments = await shipmentProcessor.Process(new[] { shipmentAdapter.Shipment }, refresher, null, null);
        //        //await LoadOrder(null);
        //        AllowEditing = (shipments?.FirstOrDefault()?.Processed ?? false) == false;
        //    }
        //}
#pragma warning restore S125 // Sections of code should not be "commented out"

        /// <summary>
        /// Save the UI values to the shipment
        /// </summary>
        public void Save()
        {
            if (shipmentAdapter.Shipment?.Processed == true)
            {
                return;
            }

            shipmentAdapter.AccountId = AccountId;
            shipmentAdapter.Shipment.OriginOriginID = OriginAddressType;

            Origin.SaveToEntity(shipmentAdapter.Shipment.OriginPerson);
            Destination.SaveToEntity(shipmentAdapter.Shipment.ShipPerson);

            IDictionary<ShipmentEntity, Exception> errors = shipmentAdapter.UpdateDynamicData();
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
        private void UpdateServices() => ShipmentViewModel.RefreshServiceTypes(shipmentAdapter.Shipment);

        /// <summary>
        /// Updates the packages.
        /// </summary>
        private void UpdatePackages() => ShipmentViewModel.RefreshPackageTypes(shipmentAdapter.Shipment);

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
            if (shipmentAdapter.Shipment == null)
            {
                return;
            }

            // Save UI values to the shipment so we can send the new values to the rates panel
            Save();

            shipmentChangedHandler.Send(new ShipmentChangedMessage(this, shipmentAdapter));
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

            if (shipmentChangedMessage?.ShipmentAdapter?.Shipment == null || shipmentAdapter?.Shipment == null)
            {
                return;
            }

            if (shipmentChangedMessage.ShipmentAdapter.Shipment.ShipmentID == shipmentAdapter.Shipment.ShipmentID)
            {
                shipmentAdapter = shipmentChangedMessage.ShipmentAdapter;
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

            Origin.SetAddressFromOrigin(OriginAddressType, shipmentAdapter.Shipment?.OrderID ?? 0, AccountId, ShipmentType);
        }


        /// <summary>
        /// Determines if a view model field is used for the shipment's rating criteria.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("SonarQube", "S125:Sections of code should not be \"commented out\"", Justification = "<Pending>")]
        private bool IsRatingField(string propertyname)
        {
            return false;

            //// Only send the ShipmentChangedMessage message if the field that changed is a rating field.
            //ShipmentType shipmentType = shipmentTypeFactory.Get(ShipmentType);

            //// Since we have a generic AddressViewModel whose properties do not match entity feild names,
            //// we need to translate the Ship, Origin, and Street properties to know if the changed field
            //// is one rating cares about.
            //string name = propertyname;
            //string shipName = string.Format("Ship{0}", name);
            //string origName = string.Format("Origin{0}", name);

            //return shipmentType.RatingFields.FieldsContainName(name) ||
            //       shipmentType.RatingFields.FieldsContainName(shipName) ||
            //       shipmentType.RatingFields.FieldsContainName(origName) ||
            //       name.Equals("Street", StringComparison.InvariantCultureIgnoreCase);
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
            // Show as deleted.
            LoadedShipmentResult = ShippingPanelLoadedShipmentResult.Deleted;
            shipmentAdapter = null;
        }

        /// <summary>
        /// Show an error if one is associated with the current shipment
        /// </summary>
        private void DisplayError(IDictionary<ShipmentEntity, Exception> errors)
        {
            if (errors.ContainsKey(shipmentAdapter.Shipment))
            {
                messageHelper.ShowError("The selected shipments were edited or deleted by another ShipWorks user and your changes could not be saved.\n\n" +
                                        "The shipments will be refreshed to reflect the recent changes.");

                shipmentChangedHandler.Send(new OrderSelectionChangingMessage(this, new[] { shipmentAdapter.Shipment.OrderID }));
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
