using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.OrderLookup.FieldManager;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.UI.ShippingPanel;
using ShipWorks.UI;

namespace ShipWorks.OrderLookup.Controls.ShipmentDetails
{
    /// <summary>
    /// View model for shipment details
    /// </summary>
    [KeyedComponent(typeof(IDetailsViewModel), ShipmentTypeCode.Usps)]
    [KeyedComponent(typeof(IDetailsViewModel), ShipmentTypeCode.Endicia)]
    [WpfView(typeof(PostalShipmentDetailsControl))]
    public class PostalShipmentDetailsViewModel : OrderLookupViewModelBase, IDetailsViewModel, INotifyPropertyChanged
    {
        private readonly IShipmentTypeManager shipmentTypeManager;
        private readonly Func<DimensionsManagerDlg> getDimensionsManagerDlg;
        private readonly ICarrierShipmentAdapterOptionsProvider carrierShipmentAdapterOptionsProvider;
        private List<DimensionsProfileEntity> dimensionProfiles;
        private Dictionary<ShipmentTypeCode, string> providers;
        private IDictionary<int, string> packageTypes;
        private IDictionary<int, string> confirmationTypes;
        private IDictionary<int, string> serviceTypes;
        private double contentWeight;

        /// <summary>
        /// Constructor
        /// </summary>
        public PostalShipmentDetailsViewModel(
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
            RefreshConfirmationTypes(shipmentModel.ShipmentAdapter);
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
        public List<DimensionsProfileEntity> DimensionProfiles
        {
            get => dimensionProfiles;
            set { Handler.Set(nameof(DimensionProfiles), ref dimensionProfiles, value); }
        }

        /// <summary>
        /// True if a profile is selected
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool IsProfileSelected => ShipmentModel.ShipmentAdapter.Shipment.Postal.DimsProfileID > 0;

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

                if (e.PropertyName == PostalShipmentFields.Service.Name ||
                    e.PropertyName == nameof(ShipmentModel.ShipmentAdapter.ShipmentTypeCode) ||
                    e.PropertyName == ShipmentFields.ShipCountryCode.Name)
                {
                    RefreshInsurance(ShipmentModel);
                }

                if (e.PropertyName == PostalShipmentFields.Service.Name)
                {
                    Handler.RaisePropertyChanged(nameof(ShipmentModel));
                }

                if (e.PropertyName == nameof(PostalShipmentFields.DimsProfileID))
                {
                    ApplyDimensionalProfile();
                }

                if (e.PropertyName == PostalShipmentFields.Service.Name ||
                    e.PropertyName == PostalShipmentFields.PackagingType.Name)
                {
                    RefreshConfirmationTypes(ShipmentModel.ShipmentAdapter);
                }

                if (e.PropertyName == ShipmentFields.ShipCountryCode.Name)
                {
                    RefreshServiceTypes(ShipmentModel.ShipmentAdapter);
                }

                if (e.PropertyName == PostalShipmentFields.Service.Name ||
                    e.PropertyName == PostalShipmentFields.PackagingType.Name ||
                    e.PropertyName == PostalShipmentFields.Confirmation.Name ||
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

            if (!ConfirmationTypes.ContainsKey(ShipmentModel.ShipmentAdapter.Shipment.Postal.Confirmation))
            {
                RefreshConfirmationTypes(ShipmentModel.ShipmentAdapter);
            }

            if (!ServiceTypes.ContainsKey(ShipmentModel.ShipmentAdapter.Shipment.Postal.Service))
            {
                RefreshServiceTypes(ShipmentModel.ShipmentAdapter);
            }

            if (!PackageTypes.ContainsKey(ShipmentModel.ShipmentAdapter.Shipment.Postal.PackagingType))
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
                DimensionsProfileEntity profile =
                    DimensionProfiles.SingleOrDefault(p => p.DimensionsProfileID == packageAdapter.DimsProfileID);

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
            DimensionProfiles =
                carrierShipmentAdapterOptionsProvider.GetDimensionsProfiles(model.PackageAdapters.FirstOrDefault()).ToList();

            if (model.ShipmentAdapter.Shipment.Postal != null &&
                DimensionProfiles.None(d => d.DimensionsProfileID == model.ShipmentAdapter.Shipment.Postal.DimsProfileID))
            {
                model.ShipmentAdapter.Shipment.Postal.DimsProfileID = 0;
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
        /// Refresh the confirmation types
        /// </summary>
        private void RefreshConfirmationTypes(ICarrierShipmentAdapter adapter)
        {
            // Check to see if object is a postal shipment adapter
            if (adapter != null &&
                !PostalUtility.IsPostalShipmentType(adapter.ShipmentTypeCode))
            {
                ConfirmationTypes = new Dictionary<int, string>();
            }
            else
            {
                PostalShipmentType postalShipmentType = (PostalShipmentType) shipmentTypeManager.Get(adapter.ShipmentTypeCode);
                PostalServiceType postalServiceType = (PostalServiceType) adapter.ServiceType;

                // See if all have confirmation as an option or not
                PostalPackagingType packagingType = (PostalPackagingType) adapter.Shipment.Postal.PackagingType;
                ConfirmationTypes = postalShipmentType
                    .GetAvailableConfirmationTypes(adapter.Shipment.ShipCountryCode, postalServiceType, packagingType)
                    .ToDictionary(serviceType => (int) serviceType,
                                  serviceType => EnumHelper.GetDescription(serviceType));

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
