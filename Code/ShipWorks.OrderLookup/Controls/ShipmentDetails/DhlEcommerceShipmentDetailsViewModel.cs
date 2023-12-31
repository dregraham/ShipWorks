﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.OrderLookup.FieldManager;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.DhlEcommerce;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.UI.ShippingPanel;
using ShipWorks.UI;

namespace ShipWorks.OrderLookup.Controls.ShipmentDetails
{
    /// <summary>
    /// View model for shipment details
    /// </summary>
    [KeyedComponent(typeof(IDetailsViewModel), ShipmentTypeCode.DhlEcommerce)]
    [WpfView(typeof(DhlEcommerceShipmentDetailsControl))]
    public class DhlEcommerceShipmentDetailsViewModel : GenericSinglePackageShipmentDetailsViewModel, IDetailsViewModel
    {
        private readonly IShipmentTypeManager shipmentTypeManager;
        private readonly Func<DimensionsManagerDlg> getDimensionsManagerDlg;
        private readonly ICarrierShipmentAdapterOptionsProvider carrierShipmentAdapterOptionsProvider;
        private IDictionary<long, string> dimensionProfiles;
        private Dictionary<ShipmentTypeCode, string> providers;
        private IDictionary<int, string> packageTypes;
        private IDictionary<int, string> confirmationTypes;
        private IDictionary<int, string> serviceTypes;
        private double contentWeight;

        /// <summary>
        /// Constructor
        /// </summary>
        public DhlEcommerceShipmentDetailsViewModel(
            IOrderLookupShipmentModel shipmentModel,
            IShipmentTypeManager shipmentTypeManager,
            IInsuranceViewModel insuranceViewModel,
            Func<DimensionsManagerDlg> getDimensionsManagerDlg,
            ICarrierShipmentAdapterOptionsProvider carrierShipmentAdapterOptionsProvider,
            OrderLookupFieldLayoutProvider fieldLayoutProvider) : base(shipmentModel, fieldLayoutProvider)
        {
            this.shipmentTypeManager = shipmentTypeManager;
            this.getDimensionsManagerDlg = getDimensionsManagerDlg;
            this.carrierShipmentAdapterOptionsProvider = carrierShipmentAdapterOptionsProvider;
            InsuranceViewModel = insuranceViewModel;
            ManageDimensionalProfiles = new RelayCommand(ManageDimensionalProfilesAction);

            RefreshDimensionalProfiles(shipmentModel);
            RefreshInsurance(shipmentModel);
            RefreshPackageTypes(shipmentModel.ShipmentAdapter);
            RefreshServiceTypes(shipmentModel.ShipmentAdapter);
            RefreshProviders(shipmentModel);

            ContentWeight = ShipmentModel.ShipmentAdapter.ContentWeight;
        }

        /// <summary>
        /// Field layout repository
        /// </summary>
        public override IOrderLookupFieldLayoutProvider FieldLayoutProvider => ShipmentModel.FieldLayoutProvider;

        /// <summary>
        /// Panel ID
        /// </summary>
        public override SectionLayoutIDs PanelID => SectionLayoutIDs.ShipmentDetails;

        /// <summary>
        /// Title of the section
        /// </summary>
        [Obfuscation(Exclude = true)]
        public override string Title { get; protected set; } = "Shipment Details";

        /// <summary>
        /// Manages Dimensional Profiles
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand ManageDimensionalProfiles { get; set; }

        /// <summary>
        /// Insurance information
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IInsuranceViewModel InsuranceViewModel { get; }

        /// <summary>
        /// The dimension profiles
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IDictionary<long, string> DimensionProfiles
        {
            get => dimensionProfiles;
            set { Handler.Set(nameof(DimensionProfiles), ref dimensionProfiles, value); }
        }

        /// <summary>
        /// True if a profile is selected
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool IsProfileSelected => ShipmentModel.ShipmentAdapter?.Shipment.DhlEcommerce.DimsProfileID > 0;

        /// <summary>
        /// Collection of ServiceTypes
        /// </summary>
        [Obfuscation(Exclude = true)]
        public Dictionary<ShipmentTypeCode, string> Providers
        {
            get => providers;
            set => Handler.Set(nameof(Providers), ref providers, value);
        }

        /// <summary>
        /// Shipment type code
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ShipmentTypeCode ShipmentTypeCode
        {
            get => ShipmentModel.ShipmentAdapter?.ShipmentTypeCode ?? ShipmentTypeCode.None;
            set
            {
                if (value != ShipmentModel.ShipmentAdapter.ShipmentTypeCode)
                {
                    ShipmentModel.ChangeShipmentType(value);
                }
            }
        }

        /// <summary>
        /// ContentWeight
        /// </summary>
        [Obfuscation(Exclude = true)]
        public double ContentWeight
        {
            get => contentWeight;
            set
            {
                contentWeight = value;
                if (!contentWeight.IsEquivalentTo(ShipmentModel.ShipmentAdapter.ContentWeight))
                {
                    ShipmentEntity shipment = ShipmentModel.ShipmentAdapter.Shipment;
                    shipment.ContentWeight = contentWeight;
                    shipmentTypeManager.Get(shipment)
                        .UpdateTotalWeight(shipment);
                }
            }
        }

        /// <summary>
        /// Collection of valid PackageTypes
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IDictionary<int, string> PackageTypes
        {
            get => packageTypes;
            set => Handler.Set(nameof(PackageTypes), ref packageTypes, value);
        }

        /// <summary>
        /// Collection of ConfirmationTypes
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IDictionary<int, string> ConfirmationTypes
        {
            get => confirmationTypes;
            set => Handler.Set(nameof(ConfirmationTypes), ref confirmationTypes, value);
        }

        /// <summary>
        /// Collection of ServiceTypes
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IDictionary<int, string> ServiceTypes
        {
            get => serviceTypes;
            set => Handler.Set(nameof(ServiceTypes), ref serviceTypes, value);
        }

        /// <summary>
        /// Update when order changes
        /// </summary>
        protected override void ShipmentModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.ShipmentModelPropertyChanged(sender, e);

            if (ShipmentModel.SelectedOrder != null)
            {
                if ((e.PropertyName == nameof(ShipmentModel.ShipmentAdapter) ||
                    e.PropertyName == nameof(ShipmentModel)) &&
                    (ShipmentModel?.ShipmentAdapter?.Shipment != null))
                {
                    ContentWeight = ShipmentModel.ShipmentAdapter.Shipment.ContentWeight;
                }

                if (e.PropertyName == DhlEcommerceShipmentFields.Service.Name ||
                    e.PropertyName == nameof(ShipmentModel.ShipmentAdapter.ShipmentTypeCode) ||
                    e.PropertyName == ShipmentFields.ShipCountryCode.Name)
                {
                    RefreshInsurance(ShipmentModel);
                }

                if (e.PropertyName == DhlEcommerceShipmentFields.Service.Name)
                {
                    Handler.RaisePropertyChanged(nameof(ShipmentModel));
                }

                if (e.PropertyName == nameof(DhlEcommerceShipmentFields.DimsProfileID))
                {
                    ApplyDimensionalProfile();
                }

                if (e.PropertyName == ShipmentFields.ShipCountryCode.Name)
                {
                    RefreshServiceTypes(ShipmentModel.ShipmentAdapter);
                }

                if (e.PropertyName == DhlEcommerceShipmentFields.Service.Name ||
                    e.PropertyName == DhlEcommerceShipmentFields.PackagingType.Name ||
                    e.PropertyName == ShipmentFields.ShipCountryCode.Name)
                {
                    UpdateDynamicData();
                }
            }
        }

        /// <summary>
        /// Ensure that the properties of the shipment are correct
        /// and that our lists contain the selected values in the shipment
        /// </summary>
        private void UpdateDynamicData()
        {
            ShipmentType shipmentType = shipmentTypeManager.Get(ShipmentModel.ShipmentAdapter.ShipmentTypeCode);
            shipmentType.RectifyCarrierSpecificData(ShipmentModel.ShipmentAdapter.Shipment);

            if (!ServiceTypes.ContainsKey(ShipmentModel.ShipmentAdapter.Shipment.DhlEcommerce.Service))
            {
                RefreshServiceTypes(ShipmentModel.ShipmentAdapter);
            }

            if (!PackageTypes.ContainsKey(ShipmentModel.ShipmentAdapter.Shipment.DhlEcommerce.PackagingType))
            {
                RefreshPackageTypes(ShipmentModel.ShipmentAdapter);
            }

            Handler.RaisePropertyChanged(null);
        }

        /// <summary>
        /// Apply a dimensional profile
        /// </summary>
        private void ApplyDimensionalProfile()
        {
            IPackageAdapter packageAdapter = ShipmentModel.PackageAdapters.First();
            if (packageAdapter?.DimsProfileID > 0)
            {
                var profile = carrierShipmentAdapterOptionsProvider.GetDimensionsProfile(packageAdapter.DimsProfileID);

                if (profile != null)
                {
                    packageAdapter.DimsLength = profile.Length;
                    packageAdapter.DimsWidth = profile.Width;
                    packageAdapter.DimsHeight = profile.Height;
                    packageAdapter.AdditionalWeight = profile.Weight;
                }
            }
            else
            {
                packageAdapter.DimsLength = 0;
                packageAdapter.DimsWidth = 0;
                packageAdapter.DimsHeight = 0;
                packageAdapter.AdditionalWeight = 0;
            }

            Handler.RaisePropertyChanged(nameof(IsProfileSelected));
        }

        /// <summary>
        /// Refresh the providers
        /// </summary>
        private void RefreshProviders(IOrderLookupShipmentModel model)
        {
            if (model.ShipmentAdapter?.Shipment == null)
            {
                Providers = new Dictionary<ShipmentTypeCode, string>();
            }
            else
            {
                Providers = carrierShipmentAdapterOptionsProvider
                    .GetProviders(model.ShipmentAdapter, model.OriginalShipmentTypeCode);
            }
        }

        /// <summary>
        /// Refreshes Insurance
        /// </summary>
        private void RefreshInsurance(IOrderLookupShipmentModel model)
        {
            InsuranceViewModel.Load(model.PackageAdapters, model.PackageAdapters.FirstOrDefault(), model.ShipmentAdapter);
        }

        /// <summary>
        /// Refresh the dimension profiles
        /// </summary>
        private void RefreshDimensionalProfiles(IOrderLookupShipmentModel model)
        {
            DimensionProfiles = carrierShipmentAdapterOptionsProvider.GetDimensionsProfiles(null);

            if (model.ShipmentAdapter.Shipment.DhlEcommerce != null &&
                !DimensionProfiles.ContainsKey(model.ShipmentAdapter.Shipment.DhlEcommerce.DimsProfileID))
            {
                model.ShipmentAdapter.Shipment.DhlEcommerce.DimsProfileID = 0;
            }

            handler.RaisePropertyChanged(null);
        }

        /// <summary>
        /// Refresh the package types
        /// </summary>
        private void RefreshPackageTypes(ICarrierShipmentAdapter adapter)
        {
            if (adapter?.Shipment == null)
            {
                PackageTypes = new Dictionary<int, string>();
            }
            else
            {
                PackageTypes = carrierShipmentAdapterOptionsProvider.GetPackageTypes(adapter);
            }
        }

        /// <summary>
        /// Refresh the ServiceTypes
        /// </summary>
        private void RefreshServiceTypes(ICarrierShipmentAdapter adapter)
        {
            ServiceTypes = carrierShipmentAdapterOptionsProvider.GetServiceTypes(adapter);
        }

        /// <summary>
        /// Shows the manage dimensional profiles dialog and updates the local profile collection after it closes
        /// </summary>
        private void ManageDimensionalProfilesAction()
        {
            using (DimensionsManagerDlg dlg = getDimensionsManagerDlg())
            {
                dlg.ShowDialog();
                RefreshDimensionalProfiles(ShipmentModel);
            }
        }
    }
}
