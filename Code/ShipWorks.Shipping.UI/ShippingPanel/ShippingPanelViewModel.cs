using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Interapptive.Shared;
using Interapptive.Shared.Business;
using Interapptive.Shared.Collections;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Core.Messaging;
using ShipWorks.Core.Messaging.Messages.Shipping;
using ShipWorks.Core.UI;
using ShipWorks.Data.Connection;
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
        private LoadedOrderSelection loadedOrderSelection;

        private readonly HashSet<string> internalFields = new HashSet<string> { nameof(AllowEditing) };

        private readonly IShippingManager shippingManager;
        private readonly IMessenger messenger;
        private readonly IDisposable subscriptions;
        private readonly IMessageHelper messageHelper;
        private readonly IShippingViewModelFactory shippingViewModelFactory;
        private readonly ILog log;
        private readonly IShippingErrorManager errorManager;

        private bool isLoadingShipment;
        private IDisposable shipmentChangedSubscription;
        private long[] selectedOrderIds;

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
        /// <remarks>We need the logger, so the amount of dependencies is ok for now</remarks>
        [NDependIgnoreTooManyParams]
        public ShippingPanelViewModel(
            IEnumerable<IShippingPanelObservableRegistration> registrations,
            IMessenger messenger,
            IShippingManager shippingManager,
            IMessageHelper messageHelper,
            IShippingViewModelFactory shippingViewModelFactory,
            Func<Type, ILog> logFactory,
            IShippingErrorManager errorManager) : this()
        {
            this.shippingManager = shippingManager;
            this.messenger = messenger;
            this.messageHelper = messageHelper;
            this.shippingViewModelFactory = shippingViewModelFactory;
            log = logFactory(typeof(ShippingPanelViewModel));
            this.errorManager = errorManager;

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

            PropertyChanging += OnPropertyChanging;

            CreateLabelCommand = new RelayCommand(CreateLabel);
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
        public virtual IObservable<string> PropertyChangeStream => handler.Where(x => !internalFields.Contains(x));

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
            // Only call save if we were in an "editing" allowed mode.
            // This handles the case where we lost focus due to opening the shipping dialog.
            // The view model needs to save itself before opening the shipping dialog.
            // So just return if we are in a non editing state...no need to save.
            if (!AllowEditing || (ShipmentAdapter?.Shipment?.Processed ?? true))
            {
                return;
            }

            CommitBindings?.Invoke();

            Save();

            IDictionary<ShipmentEntity, Exception> errors = shippingManager.SaveShipmentToDatabase(ShipmentAdapter.Shipment, false);
            DisplayError(errors);
        }

        /// <summary>
        /// Commit any outstanding bindings
        /// </summary>
        /// <remarks>This is necessary for UI elements that are bound using LostFocus as
        /// the update trigger. There are some cases where SaveToDatabase gets called before
        /// these bindings are committed</remarks>
        public Action CommitBindings { get; set; }

        /// <summary>
        /// Load the shipment from the given order
        /// </summary>
        public virtual void LoadOrder(OrderSelectionChangedMessage orderMessage)
        {
            selectedOrderIds = orderMessage.LoadedOrderSelection.Select(x => x.OrderID).ToArray();
            int orders = orderMessage.LoadedOrderSelection.OfType<LoadedOrderSelection>().HasMoreOrLessThanCount(1);
            if (orders != 0)
            {
                UnloadShipment();

                if (selectedOrderIds.Length > 1)
                {
                    LoadedShipmentResult = ShippingPanelLoadedShipmentResult.Multiple;
                }

                return;
            }

            loadedOrderSelection = orderMessage.LoadedOrderSelection.OfType<LoadedOrderSelection>().Single();
            LoadedShipmentResult = GetLoadedShipmentResult(loadedOrderSelection);

            if (LoadedShipmentResult == ShippingPanelLoadedShipmentResult.Success)
            {
                Populate(loadedOrderSelection.ShipmentAdapters.Single());
            }
            else
            {
                ShipmentAdapter = null;
            }
        }

        /// <summary>
        /// Sets the LoadedShipmentResult based on orderSelectionLoaded
        /// </summary>
        private ShippingPanelLoadedShipmentResult GetLoadedShipmentResult(LoadedOrderSelection loadedSelection)
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
        public virtual void Populate(ICarrierShipmentAdapter fromShipmentAdapter) =>
            Populate(fromShipmentAdapter, null);

        /// <summary>
        /// Populate the view model with the current state of the shipment
        /// </summary>
        public virtual void Populate(ICarrierShipmentAdapter fromShipmentAdapter, string changedField)
        {
            shipmentChangedSubscription?.Dispose();
            isLoadingShipment = true;

            ShipmentAdapter = fromShipmentAdapter;
            InitialShipmentTypeCode = ShipmentAdapter.ShipmentTypeCode;

            // Set the shipment type without going back through the shipment changed machinery
            selectedShipmentType = ShipmentAdapter.ShipmentTypeCode;

            ShipmentViewModel?.Dispose();
            ShipmentViewModel = shippingViewModelFactory.GetShipmentViewModel(selectedShipmentType);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShipmentType)));

            RequestedShippingMethod = loadedOrderSelection.Order?.RequestedShipping;

            SupportsAccounts = ShipmentAdapter.SupportsAccounts;

            // If the shipment type does not support accounts, and the current origin id is account, default to store origin.
            OriginAddressType = !ShipmentAdapter.SupportsAccounts && ShipmentAdapter.Shipment.OriginOriginID == 2 ? 0 : ShipmentAdapter.Shipment.OriginOriginID;
            InitialOriginAddressType = OriginAddressType;

            AccountId = ShipmentAdapter.AccountId.GetValueOrDefault();

            ShipmentViewModel.Load(ShipmentAdapter);

            Origin.Load(ShipmentAdapter.Shipment.OriginPerson);
            Destination.Load(ShipmentAdapter.Shipment.ShipPerson);

            AllowEditing = !ShipmentAdapter.Shipment.Processed;

            DestinationAddressEditableState = loadedOrderSelection.DestinationAddressEditable;

            Origin.SetAddressFromOrigin(OriginAddressType, ShipmentAdapter.Shipment?.OrderID ?? 0, AccountId, ShipmentType);

            SupportsMultiplePackages = ShipmentAdapter.SupportsMultiplePackages;

            isLoadingShipment = false;

            shipmentChangedSubscription = PropertyChangeStream
                .Merge(ShipmentViewModel.PropertyChangeStream)
                .Merge(Origin.PropertyChangeStream.Select(x => $"Origin{x}"))
                .Merge(Destination.PropertyChangeStream.Select(x => $"Ship{x}"))
                .Do(_ => Save())
                .CatchAndContinue((NullReferenceException ex) => log.Error("Error occurred while handling property changed", ex))
                .Subscribe(x => messenger.Send(new ShipmentChangedMessage(this, ShipmentAdapter, x)));

            messenger.Send(new ShipmentChangedMessage(this, ShipmentAdapter, changedField));
        }

        /// <summary>
        /// Process the current shipment using the specified processor
        /// </summary>
        public virtual void CreateLabel()
        {
            if (loadedShipmentResult == ShippingPanelLoadedShipmentResult.Multiple)
            {
                messenger.Send(new OpenShippingDialogWithOrdersMessage(this, selectedOrderIds));
                return;
            }

            if (loadedShipmentResult == ShippingPanelLoadedShipmentResult.Success)
            {
                if (!AllowEditing || (ShipmentAdapter?.Shipment?.Processed ?? true))
                {
                    return;
                }

                SaveToDatabase();

                AllowEditing = false;

                messenger.Send(new ProcessShipmentsMessage(this, new[] { ShipmentAdapter.Shipment }));
            }
        }

        /// <summary>
        /// Void a shipment
        /// </summary>
        public virtual void VoidLabel(VoidLabelMessage voidLabelMessage)
        {
            ShipmentEntity shipment = ShipmentAdapter?.Shipment;

            if (shipment == null || !shipment.Processed || shipment.Voided)
            {
                return;
            }

            SaveToDatabase();

            AllowEditing = false;

            string errorMessage = string.Empty;
            ICarrierShipmentAdapter voidedShipmentAdapter = null;
            long shipmentID = shipment.ShipmentID;

            try
            {
                // Void it
                voidedShipmentAdapter = shippingManager.VoidShipment(shipmentID);
            }
            catch (Exception ex) when (ex is ORMConcurrencyException ||
                                        ex is ObjectDeletedException ||
                                        ex is SqlForeignKeyException)
            {
                errorMessage = errorManager.SetShipmentErrorMessage(shipmentID, ex, "voided");
            }
            catch (ShippingException ex)
            {
                errorMessage = errorManager.SetShipmentErrorMessage(shipmentID, ex);
            }

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                messageHelper.ShowError(errorMessage);
                return;
            }

            if (voidedShipmentAdapter != null)
            {
                VoidShipmentResult voidShipmentResult = new VoidShipmentResult(voidedShipmentAdapter.Shipment);
                messenger.Send(new ShipmentsVoidedMessage(this, new[] { voidShipmentResult }));
            }
        }

        /// <summary>
        /// Save the UI values to the shipment
        /// </summary>
        public void Save()
        {
            if (ShipmentAdapter?.Shipment == null || ShipmentAdapter.Shipment.Processed)
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
        /// Updates the service types.
        /// </summary>
        public void UpdateServices() => ShipmentViewModel?.RefreshServiceTypes();

        /// <summary>
        /// Updates the package types.
        /// </summary>
        public void UpdatePackages() => ShipmentViewModel?.RefreshPackageTypes();

        /// <summary>
        /// Updates the insurance view for the shipment.
        /// </summary>
        public void RefreshInsurance() => ShipmentViewModel?.RefreshInsurance();

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
            SaveToDatabase();

            AllowEditing = false;

            // If there's no shipment and multiple order ids, send the message to open the shipping
            // dialog with order ids.
            // Otherwise, send the single shipment message if the shipment isn't null.
            if (Shipment == null && selectedOrderIds.Length > 1)
            {
                messenger.Send(new OpenShippingDialogWithOrdersMessage(this, selectedOrderIds));
            }
            else if (Shipment != null)
            {
                messenger.Send(new OpenShippingDialogMessage(this, new[] { Shipment }));
            }
        }

        /// <summary>
        /// Show an error if one is associated with the current shipment
        /// </summary>
        private void DisplayError(IDictionary<ShipmentEntity, Exception> errors)
        {
            Exception error = null;

            if (errors.TryGetValue(ShipmentAdapter.Shipment, out error))
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
            shipmentChangedSubscription?.Dispose();
        }
    }
}
