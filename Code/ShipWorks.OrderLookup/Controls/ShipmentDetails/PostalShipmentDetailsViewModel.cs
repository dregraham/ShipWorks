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
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
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
    public class PostalShipmentDetailsViewModel : IDetailsViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly PropertyChangedHandler handler;
        private readonly IShipmentTypeManager shipmentTypeManager;
        private readonly Func<DimensionsManagerDlg> getDimensionsManagerDlg;
        private readonly ICarrierShipmentAdapterOptionsProvider carrierShipmentAdapterOptionsProvider;
        private List<DimensionsProfileEntity> dimensionProfiles;
        private Dictionary<ShipmentTypeCode, string> providers;
        private IEnumerable<KeyValuePair<int, string>> packageTypes;
        private IEnumerable<KeyValuePair<int, string>> confirmationTypes;
        private IEnumerable<KeyValuePair<int, string>> serviceTypes;
        private double contentWeight;

        /// <summary>
        /// Constructor
        /// </summary>
        public PostalShipmentDetailsViewModel(
            IOrderLookupShipmentModel shipmentModel,
            IShipmentTypeManager shipmentTypeManager,
            IInsuranceViewModel insuranceViewModel,
            Func<DimensionsManagerDlg> getDimensionsManagerDlg,
            ICarrierShipmentAdapterOptionsProvider carrierShipmentAdapterOptionsProvider)
        {
            this.shipmentTypeManager = shipmentTypeManager;
            this.getDimensionsManagerDlg = getDimensionsManagerDlg;
            this.carrierShipmentAdapterOptionsProvider = carrierShipmentAdapterOptionsProvider;
            InsuranceViewModel = insuranceViewModel;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
            ManageDimensionalProfiles = new RelayCommand(ManageDimensionalProfilesAction);

            RefreshDimensionalProfiles(shipmentModel);
            RefreshInsurance(shipmentModel);
            RefreshPackageTypes(shipmentModel.ShipmentAdapter);
            RefreshConfirmationTypes(shipmentModel.ShipmentAdapter);
            RefreshServiceTypes(shipmentModel.ShipmentAdapter);
            RefreshProviders(shipmentModel);

            ShipmentModel = shipmentModel;
            ShipmentModel.PropertyChanged += ShipmentModelPropertyChanged;

            ContentWeight = ShipmentModel.ShipmentAdapter.ContentWeight;
        }

        /// <summary>
        /// Title of the section
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Title => "Shipment Details";

        /// <summary>
        /// Is the section visible
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool Visible => true;

        /// <summary>
        /// Manages Dimensional Profiles
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand ManageDimensionalProfiles { get; set; }

        /// <summary>
        /// The ViewModel ShipmentModel
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IOrderLookupShipmentModel ShipmentModel { get; }

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
            set { handler.Set(nameof(DimensionProfiles), ref dimensionProfiles, value); }
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
            set => handler.Set(nameof(Providers), ref providers, value);
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
        public IEnumerable<KeyValuePair<int, string>> PackageTypes
        {
            get => packageTypes;
            set => handler.Set(nameof(PackageTypes), ref packageTypes, value);
        }

        /// <summary>
        /// Collection of ConfirmationTypes
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IEnumerable<KeyValuePair<int, string>> ConfirmationTypes
        {
            get => confirmationTypes;
            set => handler.Set(nameof(ConfirmationTypes), ref confirmationTypes, value);
        }

        /// <summary>
        /// Collection of ServiceTypes
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IEnumerable<KeyValuePair<int, string>> ServiceTypes
        {
            get => serviceTypes;
            set => handler.Set(nameof(ServiceTypes), ref serviceTypes, value);
        }

        /// <summary>
        /// Update when order changes
        /// </summary>
        private void ShipmentModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
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
                    handler.RaisePropertyChanged(nameof(ShipmentModel));
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
                    var shipmentType = (PostalShipmentType) shipmentTypeManager.Get(ShipmentModel.ShipmentAdapter.ShipmentTypeCode);
                    shipmentType.UpdatePostalDetails(ShipmentModel.ShipmentAdapter.Shipment);
                }
            }
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

            handler.RaisePropertyChanged(nameof(IsProfileSelected));
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
        }

        /// <summary>
        /// Refresh the package types
        /// </summary>
        private void RefreshPackageTypes(ICarrierShipmentAdapter adapter)
        {
            if (adapter?.Shipment == null)
            {
                PackageTypes = Enumerable.Empty<KeyValuePair<int, string>>();
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
                ConfirmationTypes = Enumerable.Empty<KeyValuePair<int, string>>();
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

        public void Dispose() =>
            ShipmentModel.PropertyChanged -= ShipmentModelPropertyChanged;
    }
}
