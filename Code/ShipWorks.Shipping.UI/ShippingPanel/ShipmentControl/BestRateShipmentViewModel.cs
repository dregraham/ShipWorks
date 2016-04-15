using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Core.Messaging;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.UI.ShippingPanel.ShipmentControl
{
    /// <summary>
    /// View model for use by BestRateShipmentControl
    /// </summary>
    public partial class BestRateShipmentViewModel : IShipmentViewModel, INotifyPropertyChanged, INotifyPropertyChanging, IDataErrorInfo
    {
        private readonly IMessenger messenger;
        private readonly IDisposable subscriptions;
        private readonly PropertyChangedHandler handler;
        private readonly IDimensionsManager dimensionsManager;

        [SuppressMessage("SonarQube", "S2290:Field-like events should not be virtual", Justification = "Event is virtual to allow tests to fire it")]
        public virtual event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        static readonly ILog log = LogManager.GetLogger(typeof(BestRateShipmentViewModel));
        private bool suppressExternalChangeNotifications;

        /// <summary>
        /// Constructor for use by tests and WPF designer
        /// </summary>
        protected BestRateShipmentViewModel()
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged, () => PropertyChanging);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public BestRateShipmentViewModel(IMessenger messenger,
            IDimensionsManager dimensionsManager,
            IShippingViewModelFactory shippingViewModelFactory,
            ICustomsManager customsManager) : this()
        {
            this.dimensionsManager = dimensionsManager;

            serviceLevels.Clear();
            EnumHelper.GetEnumList<ServiceLevelType>().Select(x => x.Value).ToList().ForEach(slt => serviceLevels.Add((int)slt, EnumHelper.GetDescription(slt)));

            this.customsManager = customsManager;
            this.messenger = messenger;

            InsuranceViewModel = shippingViewModelFactory.GetInsuranceViewModel();

            subscriptions = new CompositeDisposable(
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

            SelectedPackageAdapter.ApplyAdditionalWeight = DimsAddWeight;
            SelectedPackageAdapter.AdditionalWeight = DimsWeight;
            SelectedPackageAdapter.DimsLength = DimsLength;
            SelectedPackageAdapter.DimsWidth = DimsWidth;
            SelectedPackageAdapter.DimsHeight = DimsHeight;
            SelectedPackageAdapter.DimsProfileID = DimsProfileID;
            SelectedPackageAdapter.Weight = TotalWeight;
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
            ServiceLevel = shipmentAdapter.Shipment.BestRate.ServiceLevel;
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
        public void RefreshServiceTypes()
        {
            // BestRate does not have service types
        }

        /// <summary>
        /// Refreshes the package types.
        /// </summary>
        public void RefreshPackageTypes()
        {
            // BestRate does not have package types
        }

        /// <summary>
        /// Updates the insurance view for the shipment.
        /// </summary>
        public void RefreshInsurance()
        {
            InsuranceViewModel.Load(PackageAdapters, SelectedPackageAdapter, shipmentAdapter);
        }

        /// <summary>
        /// Save UI values to the shipment
        /// </summary>
        public virtual void Save()
        {
            SaveDimensionsToSelectedPackageAdapter();
            SaveInsuranceToSelectedPackageAdapter();

            shipmentAdapter.ShipDate = ShipDate;
            shipmentAdapter.Shipment.BestRate.ServiceLevel = ServiceLevel;
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
        public void SelectRate(RateResult rateResult)
        {
            shipmentAdapter.SelectServiceFromRate(rateResult);
//            ServiceType = shipmentAdapter.ServiceType;
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
        private void AddCustomsItem()
        {
            IShipmentCustomsItemAdapter newItem = shipmentAdapter.AddCustomsItem();
            CustomsItems.Add(newItem);
            SelectedCustomsItem = newItem;
        }

        /// <summary>
        /// Delete a customs item
        /// </summary>
        private void DeleteCustomsItem()
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
        private void OnSelectedCustomsItemPropertyChanged(object sender, PropertyChangedEventArgs e)
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
        public string this[string columnName]
        {
            get
            {
                // If the shipment is processed, don't validate anything.
                if (shipmentAdapter?.Shipment?.Processed == true)
                {
                    return string.Empty;
                }

                return InputValidation<BestRateShipmentViewModel>.Validate(this, columnName);
            }
        }

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
        public ICollection<string> AllErrors()
        {
            return InputValidation<BestRateShipmentViewModel>.Validate(this);
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