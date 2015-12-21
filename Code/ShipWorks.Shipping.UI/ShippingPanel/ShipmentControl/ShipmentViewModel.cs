using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using log4net;
using ShipWorks.Core.Messaging;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Rating;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Services.Builders;

namespace ShipWorks.Shipping.UI.ShippingPanel.ShipmentControl
{
    /// <summary>
    /// View model for use by ShipmentControl
    /// </summary>
    public partial class ShipmentViewModel : IShipmentViewModel, INotifyPropertyChanged, INotifyPropertyChanging
    {
        private readonly IRateSelectionFactory rateSelectionFactory;
        private readonly IDisposable subscriptions;
        private readonly PropertyChangedHandler handler;
        private readonly IShipmentServicesBuilderFactory shipmentServicesBuilderFactory;
        private readonly IShipmentPackageTypesBuilderFactory shipmentPackageTypesBuilderFactory;
        private readonly IDimensionsManager dimensionsManager;
        private readonly IShippingViewModelFactory shippingViewModelFactory;

        [SuppressMessage("SonarQube", "S2290:Field-like events should not be virtual", Justification = "Event is virtual to allow tests to fire it")]
        public virtual event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        static readonly ILog log = LogManager.GetLogger(typeof(ShipmentViewModel));

        /// <summary>
        /// Constructor for use by tests and WPF designer
        /// </summary>
        protected ShipmentViewModel()
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged, () => PropertyChanging);
            Services = new ObservableCollection<KeyValuePair<int, string>>();
            PackageTypes = new ObservableCollection<PackageTypeBinding>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentViewModel(IShipmentServicesBuilderFactory shipmentServicesBuilderFactory,
            IShipmentPackageTypesBuilderFactory shipmentPackageTypesBuilderFactory, IMessenger messenger,
            IRateSelectionFactory rateSelectionFactory,
            IDimensionsManager dimensionsManager,
            IShippingViewModelFactory shippingViewModelFactory,
            ICustomsManager customsManager) : this()
        {
            this.shipmentPackageTypesBuilderFactory = shipmentPackageTypesBuilderFactory;
            this.rateSelectionFactory = rateSelectionFactory;
            this.shipmentServicesBuilderFactory = shipmentServicesBuilderFactory;
            this.dimensionsManager = dimensionsManager;
            this.shippingViewModelFactory = shippingViewModelFactory;
            this.customsManager = customsManager;

            insuranceViewModel = shippingViewModelFactory.GetInsuranceViewModel();

            subscriptions = new CompositeDisposable(
                messenger.OfType<DimensionsProfilesChangedMessage>().Subscribe(ManageDimensionsProfiles),
                messenger.OfType<SelectedRateChangedMessage>().Subscribe(HandleSelectedRateChangedMessage));
        }

        /// <summary>
        /// Load the shipment
        /// </summary>
        public virtual void Load(ICarrierShipmentAdapter newShipmentAdapter)
        {
            shipmentAdapter = newShipmentAdapter;

            ShipDate = shipmentAdapter.ShipDate;
            TotalWeight = shipmentAdapter.TotalWeight;
            UsingInsurance = shipmentAdapter.UsingInsurance;
            SupportsPackageTypes = shipmentAdapter.SupportsPackageTypes;
            SupportsMultiplePackages = shipmentAdapter.SupportsMultiplePackages;
            SupportsDimensions = shipmentAdapter.ShipmentTypeCode != ShipmentTypeCode.Other;

            RefreshServiceTypes();
            RefreshPackageTypes();

            PackageAdapters = shipmentAdapter.GetPackageAdapters();
            NumberOfPackages = PackageAdapters.Count();

            RefreshDimensionsProfiles();

            SelectedPackageAdapter = PackageAdapters.FirstOrDefault();

            InsuranceViewModel.Load(PackageAdapters, SelectedPackageAdapter);

            UpdateSelectedDimensionsProfile();

            LoadCustoms();
        }

        /// <summary>
        /// Refreshes the shipment types.
        /// </summary>
        public void RefreshServiceTypes()
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
        /// Refreshes the package types.
        /// </summary>
        public void RefreshPackageTypes()
        {
            Dictionary<int, string> packageTypes = shipmentPackageTypesBuilderFactory.Get(shipmentAdapter.ShipmentTypeCode)
                   .BuildPackageTypeDictionary(new[] { shipmentAdapter.Shipment });

            PackageTypes.Clear();
            foreach (KeyValuePair<int, string> entry in packageTypes)
            {
                PackageTypes.Add(new PackageTypeBinding()
                {
                    PackageTypeID = entry.Key,
                    Name = entry.Value
                });
            }
        }

        /// <summary>
        /// Save UI values to the shipment
        /// </summary>
        public virtual void Save()
        {
            shipmentAdapter.ShipDate = ShipDate;
            shipmentAdapter.UsingInsurance = UsingInsurance;
            shipmentAdapter.ServiceType = ServiceType;
            
            shipmentAdapter.UpdateDynamicData();
        }

        /// <summary>
        /// Called when the shipment service type has changed.
        /// </summary>
        private void HandleSelectedRateChangedMessage(SelectedRateChangedMessage message)
        {
            IRateSelection rateSelection = rateSelectionFactory.CreateRateSelection(message.RateResult);

            // Set the newly selected service type
            ServiceType = rateSelection.ServiceType;
        }

        #region Dimensions
        
        /// <summary>
        /// Updates the selected dimensions profile based on SelectedPackageAdapter
        /// </summary>
        private void UpdateSelectedDimensionsProfile()
        {
            SelectedDimensionsProfile = DimensionsProfiles.Any(dp => dp.DimensionsProfileID == SelectedPackageAdapter?.DimsProfileID)
                    ? DimensionsProfiles.FirstOrDefault(dp => dp.DimensionsProfileID == SelectedPackageAdapter?.DimsProfileID)
                    : DimensionsProfiles.FirstOrDefault(dp => dp.DimensionsProfileID == 0);
        }

        /// <summary>
        /// Handles the dimensions profiles changed message so that we can update the list of dimensions profiles.
        /// </summary>
        private void ManageDimensionsProfiles(DimensionsProfilesChangedMessage message)
        {
            long originalDimensionsProfileID = SelectedDimensionsProfile?.DimensionsProfileID ?? 0;

            // Refresh the dimensions profiles.
            RefreshDimensionsProfiles();

            // Update the Dimensions combo box selected text.
            UpdateDimensionsSelectedText(originalDimensionsProfileID);
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

        /// <summary>
        /// Updates the Dimensions selected item.
        /// There's an issue with the combo box when updating it's data source where the items in the drop down will be updated
        /// but the text displayed as the selected item, when the drop down items are not show, is still the old value.
        ///
        /// The hack to get it updated is to switch the selected item, SelectedDimensionsProfile, and then switch it back to the
        /// real selected item.
        /// </summary>
        /// <param name="originalDimensionsProfileID"></param>
        private void UpdateDimensionsSelectedText(long originalDimensionsProfileID)
        {
            // First change to a different selected profile
            SelectedDimensionsProfile = originalDimensionsProfileID != 0 ?
                DimensionsProfiles.FirstOrDefault(dp => dp.DimensionsProfileID == 0) :
                DimensionsProfiles.FirstOrDefault(dp => dp.DimensionsProfileID != 0);

            // Now change back to the real selected one so that the selected text updates correctly.
            SelectedDimensionsProfile = DimensionsProfiles.Any(dp => dp.DimensionsProfileID == originalDimensionsProfileID) ?
                DimensionsProfiles.FirstOrDefault(dp => dp.DimensionsProfileID == originalDimensionsProfileID) :
                DimensionsProfiles.FirstOrDefault(dp => dp.DimensionsProfileID == 0);
        }

        #endregion Dimensions

        #region Customs

        /// <summary>
        /// Load customs
        /// </summary>
        private void LoadCustoms()
        {
            CustomsAllowed = !shipmentAdapter.IsDomestic;

            if (!CustomsAllowed)
            {
                return;
            }

            CustomsItems = new ObservableCollection<ShipmentCustomsItemEntity>(shipmentAdapter.CustomsItems);

            TotalCustomsValue = shipmentAdapter.Shipment.CustomsValue;

            SelectedCustomsItem = CustomsItems.FirstOrDefault();
        }

        /// <summary>
        /// Add a customs item
        /// </summary>
        private void AddCustomsItem()
        {
            try
            {
                ShipmentCustomsItemEntity shipmentCustomsItemEntity = customsManager.CreateCustomsItem(shipmentAdapter.Shipment);
                CustomsItems.Add(shipmentCustomsItemEntity);
                SelectedCustomsItem = shipmentCustomsItemEntity;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        /// <summary>
        /// Delete a customs item
        /// </summary>
        private void DeleteCustomsItem()
        {
            long shipmentCustomsItemID = SelectedCustomsItem.ShipmentCustomsItemID;
            if (!CustomsItems.Contains(SelectedCustomsItem))
            {
                ShipmentCustomsItemEntity shipmentCustomsItem = CustomsItems.FirstOrDefault(sci => sci.ShipmentCustomsItemID == shipmentCustomsItemID);

                if (shipmentCustomsItem != null)
                {
                    shipmentAdapter.Shipment.CustomsItems.Remove(shipmentCustomsItem);
                }
            }
            else
            {
                shipmentAdapter.Shipment.CustomsItems.Remove(SelectedCustomsItem);
            }

            CustomsItems = new ObservableCollection<ShipmentCustomsItemEntity>(CustomsItems.Where(ci => ci.ShipmentCustomsItemID != shipmentCustomsItemID));
            SelectedCustomsItem = CustomsItems.FirstOrDefault();
        }

        /// <summary>
        /// Handle the ShipmentCustomsItemEntity property changed event.
        /// </summary>
        private void OnSelectedCustomsItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(SelectedCustomsItem.Fields[ShipmentCustomsItemFields.UnitValue.FieldIndex].Name, StringComparison.OrdinalIgnoreCase) ||
                e.PropertyName.Equals(SelectedCustomsItem.Fields[ShipmentCustomsItemFields.Quantity.FieldIndex].Name, StringComparison.OrdinalIgnoreCase))
            {
                TotalCustomsValue = CustomsItems.Sum(ci => ci.UnitValue * (decimal)ci.Quantity);
            }

            if (e.PropertyName.Equals(SelectedCustomsItem.Fields[ShipmentCustomsItemFields.Weight.FieldIndex].Name, StringComparison.OrdinalIgnoreCase) ||
                e.PropertyName.Equals(SelectedCustomsItem.Fields[ShipmentCustomsItemFields.Quantity.FieldIndex].Name, StringComparison.OrdinalIgnoreCase))
            {
                shipmentAdapter.Shipment.ContentWeight = CustomsItems.Sum(ci => ci.Weight * ci.Quantity);
                shipmentAdapter.UpdateDynamicData();

                int selectedPackageAdapterIndex = SelectedPackageAdapter.Index;
                PackageAdapters = shipmentAdapter.GetPackageAdapters();

                if (PackageAdapters?.Any(pa => pa.Index == selectedPackageAdapterIndex) == true)
                {
                    SelectedPackageAdapter = PackageAdapters.First(pa => pa.Index == selectedPackageAdapterIndex);
                }
                else
                {
                    SelectedPackageAdapter = PackageAdapters.FirstOrDefault();
                }
            }
        }
        #endregion Customs

        /// <summary>
        /// Dispose resources
        /// </summary>
        public void Dispose()
        {
            subscriptions.Dispose();
        }
    }
}