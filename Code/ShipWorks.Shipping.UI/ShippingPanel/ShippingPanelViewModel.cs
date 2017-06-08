﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Input;
using Autofac;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared;
using Interapptive.Shared.Collections;
using Interapptive.Shared.UI;
using Interapptive.Shared.Win32;
using log4net;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.Messaging;
using ShipWorks.Core.Messaging.Messages.Shipping;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Messaging.Messages.Dialogs;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping.Loading;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.UI.ShippingPanel.ShipmentControl;
using ShipWorks.Users.Security;

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
        private readonly IPipelineRegistrationContainer pipelines;
        private readonly IMessageHelper messageHelper;
        private readonly IShippingViewModelFactory shippingViewModelFactory;
        private readonly ILog log;
        private readonly Func<ISecurityContext> securityContextRetriever;
        private IDisposable shipmentChangedSubscription;
        private long[] selectedOrderIds;
        private long? lastSelectedShipmentID;
        private bool isSaving;

        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        // List of property names to exclude when checking PropertyChangeStream
        private readonly HashSet<string> PropertyChangedStreamFieldsToIgnore = new HashSet<string>
        {
            nameof(AllowEditing),
            nameof(ShipmentType),
            nameof(IsLoading),
            nameof(BestRateShipmentViewModel.RatesLoaded),
            nameof(DomesticInternationalText)
        };

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
            IPipelineRegistrationContainer pipelines,
            IMessenger messenger,
            IShippingManager shippingManager,
            IMessageHelper messageHelper,
            IShippingViewModelFactory shippingViewModelFactory,
            Func<Type, ILog> logFactory,
            Func<ISecurityContext> securityContextRetriever) : this()
        {
            this.pipelines = pipelines;
            this.shippingManager = shippingManager;
            this.messenger = messenger;
            this.messageHelper = messageHelper;
            this.shippingViewModelFactory = shippingViewModelFactory;
            log = logFactory(typeof(ShippingPanelViewModel));
            this.securityContextRetriever = securityContextRetriever;

            OpenShippingDialogCommand = new RelayCommand<OpenShippingDialogType>(SendShowShippingDlgMessage);

            Origin = shippingViewModelFactory.GetAddressViewModel();
            Destination = shippingViewModelFactory.GetAddressViewModel();
            Destination.IsAddressValidationEnabled = true;
            LoadedShipmentResult = ShippingPanelLoadedShipmentResult.NotLoaded;

            // Wiring up observables needs objects to not be null, so do this last.
            pipelines.RegisterGlobal(this);

            PropertyChanging += OnPropertyChanging;

            CreateLabelCommand = new RelayCommand(CreateLabel);
            TrackShipmentCommand = new RelayCommand(TrackShipment);
            CopyTrackingNumberToClipboardCommand = new RelayCommand(CopyTrackingNumberToClipboard);
        }

        /// <summary>
        /// List of currently selected shipments
        /// </summary>
        public IEnumerable<long> SelectedShipments { get; set; }

        /// <summary>
        /// Is the shipment currently being loaded
        /// </summary>
        public bool IsLoadingShipment { get; set; }

        /// <summary>
        /// Command that triggers processing of the current shipment
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand CreateLabelCommand { get; }

        /// <summary>
        /// Command that opens the shipping dialog to the tracking tab
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand TrackShipmentCommand { get; }

        /// <summary>
        /// Command that opens the shipping dialog to the tracking tab
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand CopyTrackingNumberToClipboardCommand { get; }

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
        /// Save the current shipment to the database
        /// </summary>
        public virtual void SaveToDatabase()
        {
            // Only call save if we were in an "editing" allowed mode.
            // This handles the case where we lost focus due to opening the shipping dialog.
            // The view model needs to save itself before opening the shipping dialog.
            // So just return if we are in a non editing state...no need to save.
            if (!AllowEditing || (ShipmentAdapter?.Shipment?.Processed ?? true))
            {
                return;
            }

            isSaving = true;

            CommitBindings?.Invoke();

            Save();

            IDictionary<ShipmentEntity, Exception> errors = shippingManager.SaveShipmentToDatabase(ShipmentAdapter?.Shipment, false);
            DisplayError(errors);

            isSaving = false;
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
            ErrorMessage = string.Empty;
            ShipmentStatus = ShipmentStatus.None;
            selectedOrderIds = orderMessage.LoadedOrderSelection.Select(x => x.OrderID).ToArray();
            bool hasOneOrder = orderMessage.LoadedOrderSelection.OfType<LoadedOrderSelection>().IsCountEqualTo(1);

            if (!hasOneOrder)
            {
                UnloadShipment();

                if (selectedOrderIds.Length > 1)
                {
                    LoadedShipmentResult = ShippingPanelLoadedShipmentResult.Multiple;
                }

                IsLoading = false;
                return;
            }

            loadedOrderSelection = orderMessage.LoadedOrderSelection.OfType<LoadedOrderSelection>().Single();

            if (LoadedShipmentResult == ShippingPanelLoadedShipmentResult.NotLoaded)
            {
                LoadedShipmentResult = GetLoadedShipmentResult(loadedOrderSelection);
            }

            ShipmentCount = loadedOrderSelection.ShipmentAdapters.Count();

            if (LoadedShipmentResult == ShippingPanelLoadedShipmentResult.Success)
            {
                LoadShipment(GetShipmentToLoad());
            }
            else if (LoadedShipmentResult == ShippingPanelLoadedShipmentResult.UnsupportedShipmentType)
            {
                // We need to set this so that we can open the dialog from the "Ship Orders" button,
                // since it expects the current shipment to be non-null
                ShipmentAdapter = GetShipmentToLoad();
            }
            else
            {
                if (LoadedShipmentResult == ShippingPanelLoadedShipmentResult.NotCreated)
                {
                    messenger.Send(new RatesNotSupportedMessage(this, "Unable to get rates for orders with no shipments."));
                }

                ErrorMessage = loadedOrderSelection.Exception?.Message ?? "An error occurred while loading the shipment.";

                ShipmentAdapter = null;
            }

            IsLoading = false;
        }

        /// <summary>
        /// Get the shipment that should be loaded
        /// </summary>
        private ICarrierShipmentAdapter GetShipmentToLoad()
        {
            long loadShipmentID = lastSelectedShipmentID.GetValueOrDefault();
            lastSelectedShipmentID = null;

            return GetShipmentAdapterWithID(loadShipmentID) ??
                loadedOrderSelection.ShipmentAdapters.OrderByDescending(x => x.Shipment.ShipmentID).First();
        }

        /// <summary>
        /// Get a shipment adapter with the given ID, if any
        /// </summary>
        public virtual ICarrierShipmentAdapter GetShipmentAdapterWithID(long shipmentID)
        {
            return loadedOrderSelection.ShipmentAdapters?.FirstOrDefault(s => s.Shipment.ShipmentID == shipmentID);
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

            if (loadedSelection.ShipmentAdapters.None())
            {
                return ShippingPanelLoadedShipmentResult.NotCreated;
            }

            ICarrierShipmentAdapter shipmetAdapter = loadedSelection.ShipmentAdapters.FirstOrDefault();
            if (shipmetAdapter?.ShipmentTypeCode == ShipmentTypeCode.Amazon)
            {
                return ShippingPanelLoadedShipmentResult.UnsupportedShipmentType;
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
        public virtual void LoadShipment(ICarrierShipmentAdapter fromShipmentAdapter) =>
            LoadShipment(fromShipmentAdapter, null);

        /// <summary>
        /// Load the shipment into the view model
        /// </summary>
        public virtual void LoadShipment(ICarrierShipmentAdapter fromShipmentAdapter, string changedField)
        {
            shipmentChangedSubscription?.Dispose();
            IsLoadingShipment = true;

            fromShipmentAdapter.UpdateDynamicData();
            ShipmentAdapter = fromShipmentAdapter;

            UpdateStoredShipment(fromShipmentAdapter);

            LoadedShipmentResult = GetLoadedShipmentResult(loadedOrderSelection);

            // If we are an unsupported shipment type, stop and show the appropriate message.
            if (ShipmentAdapter.Shipment.ShipmentTypeCode == ShipmentTypeCode.Amazon)
            {
                LoadedShipmentResult = ShippingPanelLoadedShipmentResult.UnsupportedShipmentType;
                IsLoadingShipment = false;
                return;
            }

            pipelines.RegisterTransient(this);

            Populate(ShipmentAdapter);
            ShipmentViewModel.WeightErrorMessage = string.Empty;

            AllowEditing = AllowEditing && securityContextRetriever().HasPermission(PermissionType.ShipmentsCreateEditProcess, OrderID);

            IsLoadingShipment = false;

            shipmentChangedSubscription = PropertyChangeStream
                .Where(x => !PropertyChangedStreamFieldsToIgnore.Contains(x))
                .Merge(ShipmentViewModel.PropertyChangeStream)
                .Merge(Origin.PropertyChangeStream.Select(x => $"Origin{x}"))
                .Merge(Destination.PropertyChangeStream.Select(x => $"Ship{x}"))
                .Do(x =>
                {
                    // Since the content weight can be set from a keyboard shortcut,
                    // it may get changed without getting focus. So we'll force a save to ensure the
                    // change makes it to the DB
                    if (x == "ContentWeight" && !isSaving)
                    {
                        SaveToDatabase();
                    }
                    else
                    {
                        Save();
                    }
                })
                .CatchAndContinue((NullReferenceException ex) => log.Error("Error occurred while handling property changed", ex))
                .Subscribe(x => messenger.Send(new ShipmentChangedMessage(this, ShipmentAdapter, x)));

            messenger.Send(new ShipmentChangedMessage(this, ShipmentAdapter, changedField));
        }

        /// <summary>
        /// Update a stored shipment
        /// </summary>
        /// <remarks>
        /// If LoadShipment is called directly without going through LoadOrder, the LoadedShipmentResult could
        /// be out of sync.  So we find the requested shipment in the list of order selection shipment adapters
        /// and replace it with the requested shipment adapter.  Then update the LoadedShipmentResult so that
        /// panels update correctly.
        /// </remarks>
        public virtual void UpdateStoredShipment(ICarrierShipmentAdapter shipmentAdapter) =>
            loadedOrderSelection = loadedOrderSelection.CreateSelectionWithUpdatedShipment(shipmentAdapter);

        /// <summary>
        /// Load the shipment with the given ID when it becomes available
        /// </summary>
        /// <param name="shipmentID"></param>
        public virtual void LoadShipmentWhenAvailable(long shipmentID) => lastSelectedShipmentID = shipmentID;

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

                messenger.Send(new ProcessShipmentsMessage(this, new[] { ShipmentAdapter.Shipment },
                    loadedOrderSelection.ShipmentAdapters?.Select(x => x.Shipment),
                    ShipmentViewModel?.SelectedRate));
            }
        }

        /// <summary>
        /// Track the current shipment
        /// </summary>
        private void TrackShipment()
        {
            messenger.Send(new OpenShippingDialogMessage(this, new[] { ShipmentAdapter.Shipment }, InitialShippingTabDisplay.Tracking));
        }

        /// <summary>
        /// Copy the tracking number to the clipboard
        /// </summary>
        private void CopyTrackingNumberToClipboard()
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                lifetimeScope.Resolve<ClipboardHelper>().SetText(TrackingNumber, TextDataFormat.Text, null);
            }
        }

        /// <summary>
        /// Save the UI values to the shipment
        /// </summary>
        public void Save()
        {
            if (ShipmentAdapter?.Shipment == null || ShipmentAdapter.Shipment.Processed || LoadedShipmentResult != ShippingPanelLoadedShipmentResult.Success ||
                ShipmentAdapter.Shipment.ShipmentTypeCode == ShipmentTypeCode.None)
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
            IsDomestic = ShipmentAdapter.IsDomestic;

            DisplayError(errors);
        }

        /// <summary>
        /// Updates the service types.
        /// </summary>
        public virtual void UpdateServices() => ShipmentViewModel?.RefreshServiceTypes();

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
        /// Unload the entire order
        /// </summary>
        public virtual void UnloadOrder()
        {
            UnloadShipment();

            loadedOrderSelection = default(LoadedOrderSelection);
            ShipmentCount = 0;
        }

        /// <summary>
        /// Unload the current shipment
        /// </summary>
        public virtual void UnloadShipment()
        {
            pipelines.DiscardTransient();

            shipmentChangedSubscription?.Dispose();
            ShipmentAdapter = null;
            ShipmentStatus = ShipmentStatus.None;
            StatusDate = DateTime.MinValue;
            LoadedShipmentResult = ShippingPanelLoadedShipmentResult.NotLoaded;

            lastSelectedShipmentID = null;
            SelectedShipments = null;

            // long.MinValue is not special, it's just meant to clear the field since it is not a valid account id
            AccountId = long.MinValue;
            OriginAddressType = long.MinValue;
        }

        /// <summary>
        /// Send a message to open the shipping dialog for the selected shipment
        /// </summary>
        private void SendShowShippingDlgMessage(OpenShippingDialogType type)
        {
            // Call save before asking the shipping dialog to open, that way the shipment is in the db
            // prior to the shipping dialog getting the shipment.
            SaveToDatabase();

            AllowEditing = false;

            if (type == OpenShippingDialogType.SelectedOrders && SelectedShipments != null)
            {
                messenger.Send(new OpenShippingDialogMessage(this,
                    SelectedShipments.Select(x => loadedOrderSelection.ShipmentAdapters.FirstOrDefault(s => s.Shipment.ShipmentID == x))
                        .Where(x => x != null)
                        .Select(x => x.Shipment)));
            }
            else if (type == OpenShippingDialogType.SelectedOrders && Shipment == null && selectedOrderIds.Length > 0)
            {
                messenger.Send(new OpenShippingDialogWithOrdersMessage(this, selectedOrderIds));
            }
            else if (type == OpenShippingDialogType.SelectedShipment && Shipment != null)
            {
                messenger.Send(new OpenShippingDialogMessage(this, new[] { Shipment }));
            }
            else if (type == OpenShippingDialogType.AllShipments)
            {
                messenger.Send(new OpenShippingDialogMessage(this, loadedOrderSelection.ShipmentAdapters.Select(x => x.Shipment)));
            }
        }

        /// <summary>
        /// Show an error if one is associated with the current shipment
        /// </summary>
        private void DisplayError(IDictionary<ShipmentEntity, Exception> errors)
        {
            Exception error = null;

            if (ShipmentAdapter?.Shipment != null && errors.TryGetValue(ShipmentAdapter.Shipment, out error))
            {
                messageHelper.ShowError("The selected shipments were edited or deleted by another ShipWorks user and your changes could not be saved.\n\n" +
                                        "The shipments will be refreshed to reflect the recent changes.");

                messenger.Send(new OrderSelectionChangingMessage(this, new[] { ShipmentAdapter.Shipment.OrderID }));
            }
        }

        /// <summary>
        /// Populate the view model with the current state of the shipment
        /// </summary>
        private void Populate(ICarrierShipmentAdapter fromShipmentAdapter)
        {
            InitialShipmentTypeCode = fromShipmentAdapter.ShipmentTypeCode;

            // Set the shipment type without going back through the shipment changed machinery
            selectedShipmentType = fromShipmentAdapter.ShipmentTypeCode;

            ShipmentViewModel?.Dispose();
            ShipmentViewModel = shippingViewModelFactory.GetShipmentViewModel(selectedShipmentType);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShipmentType)));

            RequestedShippingMethod = loadedOrderSelection.Order?.RequestedShipping;
            if (string.IsNullOrWhiteSpace(RequestedShippingMethod))
            {
                RequestedShippingMethod = "N/A";
            }

            SupportsAccounts = fromShipmentAdapter.SupportsAccounts;
            SupportsRateShopping = fromShipmentAdapter.SupportsRateShopping;

            // If the shipment type does not support accounts, and the current origin id is account, default to store origin.
            OriginAddressType = !fromShipmentAdapter.SupportsAccounts && fromShipmentAdapter.Shipment.OriginOriginID == 2 ?
                0 :
                fromShipmentAdapter.Shipment.OriginOriginID;
            InitialOriginAddressType = OriginAddressType;

            AccountId = fromShipmentAdapter.AccountId.GetValueOrDefault();

            ShipmentViewModel.Load(fromShipmentAdapter);

            Origin.Load(fromShipmentAdapter.Shipment.OriginPerson);
            Destination.Load(fromShipmentAdapter.Shipment.ShipPerson);
            Destination.IsAddressValidationEnabled = fromShipmentAdapter.Store.AddressValidationSetting != (int) AddressValidationStoreSettingType.ValidationDisabled;

            AllowEditing = !fromShipmentAdapter.Shipment.Processed;

            DestinationAddressEditableState = loadedOrderSelection.DestinationAddressEditable;

            Origin.SetAddressFromOrigin(OriginAddressType, fromShipmentAdapter.Shipment?.OrderID ?? 0, AccountId, ShipmentType);

            SupportsMultiplePackages = fromShipmentAdapter.SupportsMultiplePackages;
            TrackingNumber = fromShipmentAdapter.Shipment.TrackingNumber;
            ShipmentStatus = fromShipmentAdapter.Shipment.Status;

            if (ShipmentStatus == ShipmentStatus.Voided)
            {
                StatusDate = fromShipmentAdapter.Shipment.VoidedDate.GetValueOrDefault();
            }

            if (ShipmentStatus == ShipmentStatus.Processed)
            {
                StatusDate = fromShipmentAdapter.Shipment.ProcessedDate.GetValueOrDefault();
            }

            IsDomestic = fromShipmentAdapter.IsDomestic;
        }

        #region IDataErrorInfo

        /// <summary>
        /// Accessor for property validation
        /// </summary>
        public string this[string columnName]
        {
            get
            {
                // If the shipment is null or processed, don't validate anything.
                if (ShipmentAdapter?.Shipment == null || ShipmentAdapter.Shipment.Processed)
                {
                    return string.Empty;
                }

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
            pipelines?.Dispose();
            shipmentChangedSubscription?.Dispose();
        }
    }
}
