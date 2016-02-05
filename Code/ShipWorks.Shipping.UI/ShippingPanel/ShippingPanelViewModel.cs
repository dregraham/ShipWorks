using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Interapptive.Shared.Business;
using Interapptive.Shared.Collections;
using Interapptive.Shared.UI;
using ShipWorks.Core.Messaging;
using ShipWorks.Core.Messaging.Messages.Shipping;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping.Loading;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations;
using ShipWorks.UI.Controls.AddressControl;

namespace ShipWorks.Shipping.UI.ShippingPanel
{
    /// <summary>
    /// Main view model for the shipment panel
    /// </summary>
    public partial class ShippingPanelViewModel : INotifyPropertyChanged, INotifyPropertyChanging, IDisposable, IDataErrorInfo
    {
        private readonly PropertyChangedHandler handler;
        private OrderSelectionLoaded orderSelectionLoaded;

        private readonly IShippingManager shippingManager;
        private readonly IMessenger messenger;
        private readonly IDisposable subscriptions;
        private readonly IMessageHelper messageHelper;
        private readonly IShippingViewModelFactory shippingViewModelFactory;

        private bool isLoadingShipment;

        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShippingPanelViewModel"/> class.
        /// </summary>
        public ShippingPanelViewModel()
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged, () => PropertyChanging);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingPanelViewModel(
            IEnumerable<IShippingPanelObservableRegistration> registrations,
            IMessenger messenger,
            IShippingManager shippingManager,
            IMessageHelper messageHelper,
            IShippingViewModelFactory shippingViewModelFactory) : this()
        {
            this.shippingManager = shippingManager;
            this.messenger = messenger;
            this.messageHelper = messageHelper;
            this.shippingViewModelFactory = shippingViewModelFactory;

            OpenShippingDialogCommand = new RelayCommand(SendShowShippingDlgMessage);

            Origin = shippingViewModelFactory.GetAddressViewModel();
            Destination = shippingViewModelFactory.GetAddressViewModel();
            Destination.IsAddressValidationEnabled = true;

            // Wiring up observables needs objects to not be null, so do this last.
            subscriptions = new CompositeDisposable(registrations.Select(x => x.Register(this))
                .Concat(new[] {
                    LoadCustomsWhenAddressChanges(Origin, x => ShipmentAdapter?.Shipment?.OriginPerson),
                    LoadCustomsWhenAddressChanges(Destination, x => ShipmentAdapter?.Shipment?.ShipPerson)
                }));

            WireUpObservables();

            PropertyChanging += OnPropertyChanging;

            CreateLabelCommand = new RelayCommand(ProcessShipment);
        }

        /// <summary>
        /// Command that triggers processing of the current shipment
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand CreateLabelCommand { get; }

        /// <summary>
        /// Is the current shipment domestic
        /// </summary>
        public virtual bool IsDomestic => ShipmentAdapter?.IsDomestic ?? true;

        /// <summary>
        /// Current shipment adapter
        /// </summary>
        public virtual ICarrierShipmentAdapter ShipmentAdapter { get; private set; }

        /// <summary>
        /// Expose a stream of property changes
        /// </summary>
        public IObservable<string> PropertyChangeStream => handler;

        /// <summary>
        /// Gets the shipment from the current adapter
        /// </summary>
        public virtual ShipmentEntity Shipment => ShipmentAdapter?.Shipment;

        /// <summary>
        /// Gets the id of the order associated with the current shipment
        /// </summary>
        public virtual long? OrderID => ShipmentAdapter?.Shipment?.OrderID;

        /// <summary>
        /// Is the current shipment processed
        /// </summary>
        public virtual bool? IsProcessed => ShipmentAdapter?.Shipment?.Processed;

        /// <summary>
        /// Wire up any Observable patterns
        /// </summary>
        private void WireUpObservables()
        {


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
        /// Load customs when address properties change
        /// </summary>
        private IDisposable LoadCustomsWhenAddressChanges(AddressViewModel model, Func<string, PersonAdapter> getAdapter)
        {
            return model.PropertyChangeStream
                .Where(_ => !isLoadingShipment)
                .Select(getAdapter)
                .Where(person => person != null)
                .Subscribe(person =>
                {
                    model.SaveToEntity(person);
                    shipmentViewModel.LoadCustoms();
                });
        }

        /// <summary>
        /// Save the current shipment to the database
        /// </summary>
        public void SaveToDatabase()
        {
            if (!AllowEditing || (ShipmentAdapter?.Shipment?.Processed ?? true))
            {
                return;
            }

            // Only call save if we were in an "editing" allowed mode.
            // This handles the case where we lost focus due to opening the shipping dialog.
            // The view model needs to save itself before opening the shipping dialog.
            // So just return if we are in a non editing state...no need to save.
            if (!AllowEditing)
            {
                return;
            }

            Save();

            IDictionary<ShipmentEntity, Exception> errors = shippingManager.SaveShipmentToDatabase(ShipmentAdapter.Shipment, false);
            DisplayError(errors);
        }

        /// <summary>
        /// Load the shipment from the given order
        /// </summary>
        public virtual void LoadOrder(OrderSelectionChangedMessage orderMessage)
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
                Populate(orderSelectionLoaded.ShipmentAdapters.Single());
            }
            else
            {
                ShipmentAdapter = null;
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
        public virtual void Populate(ICarrierShipmentAdapter fromShipmentAdapter)
        {
            isLoadingShipment = true;

            ShipmentAdapter = fromShipmentAdapter;
            InitialShipmentTypeCode = ShipmentAdapter.ShipmentTypeCode;

            // Set the shipment type without going back through the shipment changed machinery
            selectedShipmentType = ShipmentAdapter.ShipmentTypeCode;

            ShipmentViewModel?.Dispose();
            ShipmentViewModel = shippingViewModelFactory.GetShipmentViewModel(selectedShipmentType);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShipmentType)));

            RequestedShippingMethod = orderSelectionLoaded.Order?.RequestedShipping;

            SupportsAccounts = ShipmentAdapter.SupportsAccounts;

            // If the shipment type does not support accounts, and the current origin id is account, default to store origin.
            OriginAddressType = !ShipmentAdapter.SupportsAccounts && ShipmentAdapter.Shipment.OriginOriginID == 2 ? 0 : ShipmentAdapter.Shipment.OriginOriginID;
            InitialOriginAddressType = OriginAddressType;

            AccountId = ShipmentAdapter.AccountId.GetValueOrDefault();

            Origin.Load(ShipmentAdapter.Shipment.OriginPerson);
            Destination.Load(ShipmentAdapter.Shipment.ShipPerson);

            AllowEditing = !ShipmentAdapter.Shipment.Processed;

            DestinationAddressEditableState = orderSelectionLoaded.DestinationAddressEditable;

            Origin.SetAddressFromOrigin(OriginAddressType, ShipmentAdapter.Shipment?.OrderID ?? 0, AccountId, ShipmentType);

            SupportsMultiplePackages = ShipmentAdapter.SupportsMultiplePackages;

            ShipmentViewModel.Load(ShipmentAdapter);

            isLoadingShipment = false;
        }

        /// <summary>
        /// Process the current shipment using the specified processor
        /// </summary>
        public void ProcessShipment()
        {
            if (!AllowEditing || (ShipmentAdapter?.Shipment?.Processed ?? true))
            {
                return;
            }

            SaveToDatabase();

            AllowEditing = false;

            messenger.Send(new ProcessShipmentsMessage(this, new[] { ShipmentAdapter.Shipment }));
        }

        /// <summary>
        /// Save the UI values to the shipment
        /// </summary>
        public void Save()
        {
            if (ShipmentAdapter?.Shipment?.Processed == true)
            {
                return;
            }

            // Only call save if we were in an "editing" allowed mode.
            // This handles the case where we lost focus due to opening the shipping dialog.
            // The view model needs to save itself before opening the shipping dialog.
            // So just return if we are in a non editing state...no need to save.
            if (!AllowEditing)
            {
                return;
            }

            ShipmentAdapter.AccountId = AccountId;
            ShipmentAdapter.Shipment.OriginOriginID = OriginAddressType;

            Origin.SaveToEntity(ShipmentAdapter.Shipment.OriginPerson);
            Destination.SaveToEntity(ShipmentAdapter.Shipment.ShipPerson);

            ShipmentViewModel.Save();

            IDictionary<ShipmentEntity, Exception> errors = ShipmentAdapter.UpdateDynamicData();
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
        /// Updates the service types.
        /// </summary>
        private void UpdateServices() => ShipmentViewModel?.RefreshServiceTypes();

        /// <summary>
        /// Updates the package types.
        /// </summary>
        private void UpdatePackages() => ShipmentViewModel?.RefreshPackageTypes();

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
            if (ShipmentAdapter.Shipment == null)
            {
                return;
            }

            // Save UI values to the shipment so we can send the new values to the rates panel
            Save();

            messenger.Send(new ShipmentChangedMessage(this, ShipmentAdapter));
        }


        /// <summary>
        /// Determines if a view model field is used for the shipment's rating criteria.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("SonarQube", "S125:Sections of code should not be \"commented out\"", Justification = "<Pending>")]
        private bool IsRatingField(string propertyname)
        {
            return false;

            //// Only send the ShipmentChangedMessage message if the field that changed is a rating field.
            //ShipmentType shipmentType = shipmentTypeManager.Get(ShipmentType);

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
        public virtual void UnloadShipment()
        {
            ShipmentAdapter = null;
        }

        /// <summary>
        /// Send a message to open the shipping dialog for the selected shipment
        /// </summary>
        private void SendShowShippingDlgMessage()
        {
            // Call save before asking the shipping dialog to open, that way the shipment is in the db
            // prior to the shipping dialog getting the shipment.
            Save();

            AllowEditing = false;

            messenger.Send(new OpenShippingDialogMessage(this, new[] { Shipment }));
        }

        /// <summary>
        /// Show an error if one is associated with the current shipment
        /// </summary>
        private void DisplayError(IDictionary<ShipmentEntity, Exception> errors)
        {
            if (errors.ContainsKey(ShipmentAdapter.Shipment))
            {
                messageHelper.ShowError("The selected shipments were edited or deleted by another ShipWorks user and your changes could not be saved.\n\n" +
                                        "The shipments will be refreshed to reflect the recent changes.");

                messenger.Send(new OrderSelectionChangingMessage(this, new[] { ShipmentAdapter.Shipment.OrderID }));
            }
        }

        #region IDataErrorInfo

        /// <summary>
        /// Accessor for property validation
        /// </summary>
        public string this[string columnName]
        {
            get
            {
                return InputValidation<ShippingPanelViewModel>.Validate(this, columnName);
            }
        }

        /// <summary>
        /// IDataErrorInfo Error implementation
        /// </summary>
        public string Error => null;

        /// <summary>
        /// List of all validation errors
        /// </summary>
        /// <returns></returns>
        public ICollection<string> AllErrors()
        {
            return InputValidation<ShippingPanelViewModel>.Validate(this);
        }

        #endregion

        /// <summary>
        /// Dispose resources
        /// </summary>
        public void Dispose()
        {
            subscriptions.Dispose();
        }
    }
}
