using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;
using System.Windows.Input;
using Interapptive.Shared.Utility;
using GalaSoft.MvvmLight.Command;
using log4net;
using ShipWorks.Core.Messaging;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Services.Builders;

namespace ShipWorks.Shipping.UI.ShippingPanel.ShipmentControl
{
    /// <summary>
    /// Base view model for use by ShipmentControls
    /// </summary>
    public abstract partial class ShipmentViewModelBase : IShipmentViewModel, INotifyPropertyChanged, INotifyPropertyChanging, IDataErrorInfo
    {
        protected int maxPackages = 25;
        protected readonly IMessenger messenger;
        private readonly IDisposable baseSubscriptions;
        protected readonly PropertyChangedHandler handler;
        private readonly IShipmentServicesBuilderFactory shipmentServicesBuilderFactory;
        private readonly IShipmentPackageTypesBuilderFactory shipmentPackageTypesBuilderFactory;
        private readonly IDimensionsManager dimensionsManager;

        [SuppressMessage("SonarQube", "S2290:Field-like events should not be virtual", Justification = "Event is virtual to allow tests to fire it")]
        public virtual event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        static readonly ILog log = LogManager.GetLogger(typeof(ShipmentViewModelBase));
        private bool suppressExternalChangeNotifications;

        /// <summary>
        /// Constructor for use by tests and WPF designer
        /// </summary>
        protected ShipmentViewModelBase()
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged, () => PropertyChanging);
            Services = new ObservableCollection<KeyValuePair<int, string>>();
            PackageTypes = new ObservableCollection<KeyValuePair<int, string>>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentViewModelBase(IShipmentServicesBuilderFactory shipmentServicesBuilderFactory,
            IShipmentPackageTypesBuilderFactory shipmentPackageTypesBuilderFactory, IMessenger messenger,
            IDimensionsManager dimensionsManager,
            IShippingViewModelFactory shippingViewModelFactory,
            ICustomsManager customsManager) : this()
        {
            this.shipmentPackageTypesBuilderFactory = shipmentPackageTypesBuilderFactory;
            this.shipmentServicesBuilderFactory = shipmentServicesBuilderFactory;
            this.dimensionsManager = dimensionsManager;
            this.customsManager = customsManager;
            this.messenger = messenger;

            InsuranceViewModel = shippingViewModelFactory.GetInsuranceViewModel();

            AddPackageCommand = new RelayCommand(AddPackageAction, () => PackageAdapters?.Count < 25);
            DeletePackageCommand = new RelayCommand(DeletePackageAction,
                () => SelectedPackageAdapter != null && PackageAdapters.Count > 1);

            baseSubscriptions = new CompositeDisposable(
                messenger.OfType<DimensionsProfilesChangedMessage>().Subscribe(ManageDimensionsProfiles),
                messenger.OfType<ShippingSettingsChangedMessage>().Subscribe(HandleShippingSettingsChangedMessage),
                handler.PropertyChangingStream
                    .Where(x => nameof(SelectedPackageAdapter).Equals(x, StringComparison.Ordinal))
                    .Subscribe(_ => SaveDimensionsToSelectedPackageAdapter()),
                handler.Where(x => nameof(SelectedPackageAdapter).Equals(x, StringComparison.Ordinal))
                    .Subscribe(_ => LoadDimensionsFromSelectedPackageAdapter()));
        }

        /// <summary>
        /// Load dimensions into the UI from the selected package adapter
        /// </summary>
        private void LoadDimensionsFromSelectedPackageAdapter()
        {
            if (SelectedPackageAdapter == null)
            {
                return;
            }

            using (Disposable.Create(() => suppressExternalChangeNotifications = false))
            {
                suppressExternalChangeNotifications = true;

                PackagingType = SelectedPackageAdapter.PackagingType;
                DimsAddWeight = SelectedPackageAdapter.ApplyAdditionalWeight;
                DimsWeight = SelectedPackageAdapter.AdditionalWeight;
                DimsLength = SelectedPackageAdapter.DimsLength;
                DimsWidth = SelectedPackageAdapter.DimsWidth;
                DimsHeight = SelectedPackageAdapter.DimsHeight;
                DimsProfileID = SelectedPackageAdapter.DimsProfileID;
                TotalWeight = SelectedPackageAdapter.Weight;

                UpdateSelectedDimensionsProfile();

                InsuranceViewModel.SelectedPackageAdapter = SelectedPackageAdapter;
            }
        }

        /// <summary>
        /// Save dimensions from the UI into the selected package adapter
        /// </summary>
        private void SaveDimensionsToSelectedPackageAdapter()
        {
            if (SelectedPackageAdapter == null)
            {
                return;
            }

            SelectedPackageAdapter.PackagingType = PackagingType;
            SelectedPackageAdapter.ApplyAdditionalWeight = DimsAddWeight;
            SelectedPackageAdapter.AdditionalWeight = DimsWeight;
            SelectedPackageAdapter.DimsLength = DimsLength;
            SelectedPackageAdapter.DimsWidth = DimsWidth;
            SelectedPackageAdapter.DimsHeight = DimsHeight;
            SelectedPackageAdapter.DimsProfileID = DimsProfileID;
            SelectedPackageAdapter.Weight = TotalWeight;
        }

        /// <summary>
        /// Delete a package
        /// </summary>
        private void DeletePackageAction()
        {
            if (!shipmentAdapter.SupportsMultiplePackages || PackageAdapters.Count < 2)
            {
                return;
            }

            PackageAdapterWrapper packageAdapter = SelectedPackageAdapter;
            SelectedPackageAdapter = null;
            shipmentAdapter.DeletePackage(packageAdapter.WrappedAdapter);

            int location = PackageAdapters.IndexOf(packageAdapter);
            SelectedPackageAdapter = PackageAdapters.Last() == packageAdapter ?
                PackageAdapters.ElementAt(location - 1) :
                PackageAdapters.ElementAt(location + 1);

            PackageAdapters.Remove(packageAdapter);

            for (int i = 0; i < PackageAdapters.Count; i++)
            {
                PackageAdapters[i].Index = i + 1;
            }

            messenger.Send(new ShipmentChangedMessage(this, shipmentAdapter));
        }

        /// <summary>
        /// Add a package
        /// </summary>
        private void AddPackageAction()
        {
            if (!shipmentAdapter.SupportsMultiplePackages || PackageAdapters.Count >= maxPackages)
            {
                return;
            }

            PackageAdapterWrapper packageAdapter = new PackageAdapterWrapper(shipmentAdapter.AddPackage());
            PackageAdapters.Add(packageAdapter);
            SelectedPackageAdapter = packageAdapter;

            messenger.Send(new ShipmentChangedMessage(this, shipmentAdapter));
        }

        /// <summary>
        /// Stream of property change events
        /// </summary>
        public IObservable<string> PropertyChangeStream
        {
            get
            {
                return handler.Merge(InsuranceViewModel.PropertyChangeStream)
                    .Where(_ => !suppressExternalChangeNotifications);
            }
        }

        /// <summary>
        /// Load the shipment
        /// </summary>
        public virtual void Load(ICarrierShipmentAdapter newShipmentAdapter)
        {
            shipmentAdapter = newShipmentAdapter;

            ShipDate = shipmentAdapter.ShipDate;
            TotalWeight = shipmentAdapter.TotalWeight;
            SupportsPackageTypes = shipmentAdapter.SupportsPackageTypes;
            SupportsMultiplePackages = shipmentAdapter.SupportsMultiplePackages;
            SupportsDimensions = shipmentAdapter.ShipmentTypeCode != ShipmentTypeCode.Other;

            RefreshServiceTypes();
            RefreshPackageTypes();

            PackageAdapters = new ObservableCollection<PackageAdapterWrapper>(shipmentAdapter.GetPackageAdapters().Select(x => new PackageAdapterWrapper(x)));

            RefreshDimensionsProfiles();

            PackageAdapterWrapper packageAdapter = PackageAdapters.FirstOrDefault();
            InsuranceViewModel.Load(PackageAdapters, packageAdapter, shipmentAdapter);
            SelectedPackageAdapter = packageAdapter;

            LoadCustoms();
        }

        /// <summary>
        /// Refreshes the shipment types.
        /// </summary>
        public virtual void RefreshServiceTypes()
        {
            Dictionary<int, string> services;
            try
            {
                services = shipmentServicesBuilderFactory.Get(shipmentAdapter.ShipmentTypeCode)
                    .BuildServiceTypeDictionary(new[] { shipmentAdapter.Shipment });
            }
            catch (InvalidRateGroupShippingException)
            {
                Services.Add(new KeyValuePair<int, string>(shipmentAdapter.ServiceType, "Error getting service types."));
                return;
            }

            Services.Clear();

            foreach (KeyValuePair<int, string> entry in services)
            {
                Services.Add(entry);
            }

            ServiceType = shipmentAdapter.ServiceType;
        }

        /// <summary>
        /// Updates the insurance view for the shipment.
        /// </summary>
        public void RefreshInsurance()
        {
            InsuranceViewModel.Load(PackageAdapters, SelectedPackageAdapter, shipmentAdapter);
        }

        /// <summary>
        /// Refreshes the package types.
        /// </summary>
        public virtual void RefreshPackageTypes()
        {
            Dictionary<int, string> packageTypes = shipmentPackageTypesBuilderFactory.Get(shipmentAdapter.ShipmentTypeCode)
                .BuildPackageTypeDictionary(new[] { shipmentAdapter.Shipment });

            PackageTypes.Clear();

            // If no package types are returned, the carrier doesn't support package types,
            // so just return.
            if (!packageTypes.Any())
            {
                return;
            }

            foreach (KeyValuePair<int, string> entry in packageTypes)
            {
                PackageTypes.Add(entry);
            }

            PackagingType = PackageTypes.Any(pt => pt.Key == SelectedPackageAdapter?.PackagingType) ? 
                SelectedPackageAdapter.PackagingType : 
                PackageTypes.First().Key;
        }

        /// <summary>
        /// Save UI values to the shipment
        /// </summary>
        public virtual void Save()
        {
            SaveDimensionsToSelectedPackageAdapter();
            SaveInsuranceToSelectedPackageAdapter();

            shipmentAdapter.ShipDate = ShipDate;
            shipmentAdapter.ServiceType = ServiceType;
            shipmentAdapter.ContentWeight = PackageAdapters.Sum(pa => pa.Weight);
        }

        /// <summary>
        /// Saves insurance values to selected package adapter
        /// </summary>
        private void SaveInsuranceToSelectedPackageAdapter()
        {
            if (SelectedPackageAdapter?.InsuranceChoice == null)
            {
                return;
            }

            SelectedPackageAdapter.InsuranceChoice.Insured = InsuranceViewModel.Insurance;
            SelectedPackageAdapter.InsuranceChoice.InsuranceValue = InsuranceViewModel.DeclaredValue;
        }

        /// <summary>
        /// Select the given rate
        /// </summary>
        public virtual void SelectRate(RateResult rateResult)
        {
            shipmentAdapter.SelectServiceFromRate(rateResult);

            // If the currently selected service type matches the specified rateResult service,
            // don't set it again.  ServiceType uses the force set value so that combo boxes
            // update correctly.  But in this case we don't want to force an update because it 
            // fires off a rate update which isn't needed.
            if (ServiceType != shipmentAdapter.ServiceType)
            {
                ServiceType = shipmentAdapter.ServiceType;
            }
        }

        /// <summary>
        /// Called when the shipment service type has changed.
        /// </summary>
        private void HandleShippingSettingsChangedMessage(ShippingSettingsChangedMessage message)
        {
            if (shipmentAdapter.Shipment.Processed)
            {
                return;
            }

            shipmentAdapter.UpdateInsuranceFields(message.ShippingSettings);
            foreach (var packageAdapter in PackageAdapters)
            {
                packageAdapter.UpdateInsuranceFields(message.ShippingSettings);
            }

            InsuranceViewModel.Load(PackageAdapters, SelectedPackageAdapter, shipmentAdapter);

            RefreshServiceTypes();
            RefreshPackageTypes();
        }

        #region Dimensions

        /// <summary>
        /// Updates the selected dimensions profile based on SelectedPackageAdapter
        /// </summary>
        private void UpdateSelectedDimensionsProfile()
        {
            SelectedDimensionsProfile = DimensionsProfiles.Any(dp => dp.DimensionsProfileID == SelectedPackageAdapter?.DimsProfileID) ?
                DimensionsProfiles.FirstOrDefault(dp => dp.DimensionsProfileID == SelectedPackageAdapter?.DimsProfileID) :
                DimensionsProfiles.FirstOrDefault(dp => dp.DimensionsProfileID == 0);
        }

        /// <summary>
        /// Handles the dimensions profiles changed message so that we can update the list of dimensions profiles.
        /// </summary>
        private void ManageDimensionsProfiles(DimensionsProfilesChangedMessage message)
        {
            // For "Enter Dimensions" we need to capture entered values so that we can re-add them later.
            long originalDimensionsProfileID = SelectedDimensionsProfile?.DimensionsProfileID ?? 0;
            double originalLength = DimsLength;
            double originalWidth = DimsWidth;
            double originalHeight = DimsHeight;
            double originalWeight = DimsWeight;

            // Refresh the dimensions profiles.
            RefreshDimensionsProfiles();

            // Update the Dimensions combo box selected text.
            UpdateDimensionsSelectedText(originalDimensionsProfileID, originalLength, originalWidth, originalHeight, originalWeight);
        }

        /// <summary>
        /// Update the list of dimensions profiles
        /// </summary>
        private void RefreshDimensionsProfiles()
        {
            DimensionsProfiles = new ObservableCollection<DimensionsProfileEntity>();
            foreach (var dimensionsProfileEntity in dimensionsManager.Profiles(SelectedPackageAdapter))
            {
                DimensionsProfiles.Add(dimensionsProfileEntity);
            }
        }

        ///  <summary>
        ///  Updates the Dimensions selected item.
        ///  There's an issue with the combo box when updating it's data source where the items in the drop down will be updated
        ///  but the text displayed as the selected item, when the drop down items are not show, is still the old value.
        ///
        ///  The hack to get it updated is to switch the selected item, SelectedDimensionsProfile, and then switch it back to the
        ///  real selected item.
        ///  </summary>
        private void UpdateDimensionsSelectedText(long originalDimensionsProfileID, double originalLength, double originalWidth, double originalHeight, double originalWeight)
        {
            // First change to a different selected profile
            SelectedDimensionsProfile = originalDimensionsProfileID != 0 ?
                DimensionsProfiles.FirstOrDefault(dp => dp.DimensionsProfileID == 0) :
                DimensionsProfiles.FirstOrDefault(dp => dp.DimensionsProfileID != 0);

            // Now change back to the real selected one so that the selected text updates correctly.
            SelectedDimensionsProfile = DimensionsProfiles.Any(dp => dp.DimensionsProfileID == originalDimensionsProfileID) ?
                DimensionsProfiles.FirstOrDefault(dp => dp.DimensionsProfileID == originalDimensionsProfileID) :
                DimensionsProfiles.FirstOrDefault(dp => dp.DimensionsProfileID == 0);

            // For "Enter Dimensions" we need to reset to the original entered values.
            if (originalDimensionsProfileID == 0)
            {
                DimsLength = originalLength;
                DimsWidth = originalWidth;
                DimsHeight = originalHeight;
                DimsWeight = originalWeight;
            }
        }

        #endregion Dimensions

        #region Customs

        /// <summary>
        /// Load customs
        /// </summary>
        public void LoadCustoms()
        {
            CustomsAllowed = shipmentAdapter?.CustomsAllowed == true;

            if (!CustomsAllowed)
            {
                return;
            }

            CustomsItems = new ObservableCollection<IShipmentCustomsItemAdapter>(shipmentAdapter.GetCustomsItemAdapters());

            TotalCustomsValue = shipmentAdapter.Shipment.CustomsValue;

            SelectedCustomsItem = CustomsItems.FirstOrDefault();
            DeleteCustomsItemCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Add a customs item
        /// </summary>
        protected void AddCustomsItem()
        {
            IShipmentCustomsItemAdapter newItem = shipmentAdapter.AddCustomsItem();
            CustomsItems.Add(newItem);
            SelectedCustomsItem = newItem;
        }

        /// <summary>
        /// Delete a customs item
        /// </summary>
        protected void DeleteCustomsItem()
        {
            double originalShipmentcontentWeight = ShipmentContentWeight;
            ShipmentContentWeight = CustomsItems.Sum(ci => ci.Weight * ci.Quantity);
            RedistributeContentWeight(originalShipmentcontentWeight);
            TotalCustomsValue = CustomsItems.Sum(ci => ci.UnitValue * (decimal) ci.Quantity);

            IShipmentCustomsItemAdapter selectedItem = SelectedCustomsItem;
            int location = CustomsItems.IndexOf(selectedItem);

            if (CustomsItems.Count > 1)
            {
                SelectedCustomsItem = CustomsItems.Last() == selectedItem ?
                    CustomsItems.ElementAt(location - 1) :
                    CustomsItems.ElementAt(location + 1);
            }

            CustomsItems.Remove(selectedItem);
            shipmentAdapter.DeleteCustomsItem(selectedItem);
        }

        /// <summary>
        /// Handle the ShipmentCustomsItemEntity property changed event.
        /// </summary>
        protected void OnSelectedCustomsItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(IShipmentCustomsItemAdapter.UnitValue), StringComparison.OrdinalIgnoreCase) ||
                e.PropertyName.Equals(nameof(IShipmentCustomsItemAdapter.Quantity), StringComparison.OrdinalIgnoreCase))
            {
                TotalCustomsValue = CustomsItems.Sum(ci => ci.UnitValue * (decimal) ci.Quantity);
            }

            if (e.PropertyName.Equals(nameof(IShipmentCustomsItemAdapter.Weight), StringComparison.OrdinalIgnoreCase) ||
                e.PropertyName.Equals(nameof(IShipmentCustomsItemAdapter.Quantity), StringComparison.OrdinalIgnoreCase))
            {
                double originalShipmentcontentWeight = ShipmentContentWeight;
                ShipmentContentWeight = CustomsItems.Sum(ci => ci.Weight * ci.Quantity);
                RedistributeContentWeight(originalShipmentcontentWeight);
            }
        }

        /// <summary>
        /// Redistribute the ContentWeight from the shipment to each package in the shipment.  This only does something
        /// if the ContentWeight is different from the total Content.
        /// </summary>
        public void RedistributeContentWeight(double originalShipmentcontentWeight)
        {
            // If the content weight changed outside of us, redistribute what the new weight among the packages
            if (Math.Abs(originalShipmentcontentWeight - ShipmentContentWeight) > 0.001)
            {
                foreach (IPackageAdapter packageAdapter in PackageAdapters)
                {
                    packageAdapter.Weight = ShipmentContentWeight / PackageAdapters.Count();
                }

                LoadDimensionsFromSelectedPackageAdapter();
            }
        }
        #endregion Customs

        #region IDataErrorInfo

        /// <summary>
        /// Accessor for property validation
        /// </summary>
        public abstract string this[string columnName] { get; }

        /// <summary>
        /// IDataErrorInfo Error implementation
        /// </summary>
        public string Error => null;

        /// <summary>
        /// Command to add a new package
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand AddPackageCommand { get; }

        /// <summary>
        /// Command to delete a package
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand DeletePackageCommand { get; }

        /// <summary>
        /// List of all validation errors
        /// </summary>
        /// <returns></returns>
        public abstract ICollection<string> AllErrors();

        #endregion

        /// <summary>
        /// Dispose resources
        /// </summary>
        public virtual void Dispose()
        {
            baseSubscriptions?.Dispose();
        }
    }
}