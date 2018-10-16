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
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Services.Builders;
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
            ShipmentModel = shipmentModel;
            ShipmentModel.PropertyChanged += ShipmentModelPropertyChanged;

            this.shipmentTypeManager = shipmentTypeManager;
            this.getDimensionsManagerDlg = getDimensionsManagerDlg;
            this.carrierShipmentAdapterOptionsProvider = carrierShipmentAdapterOptionsProvider;
            InsuranceViewModel = insuranceViewModel;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
            ManageDimensionalProfiles = new RelayCommand(ManageDimensionalProfilesAction);

            RefreshDimensionalProfiles();
            RefreshInsurance();
            RefreshPackageTypes();
            RefreshConfirmationTypes();
            RefreshServiceTypes();
            RefreshProviders();
        }

        /// <summary>
        /// Is the section expanded
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool Expanded { get; set; } = true;

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
                if (e.PropertyName == PostalShipmentFields.Service.Name ||
                    e.PropertyName == nameof(ShipmentModel.ShipmentAdapter.ShipmentTypeCode) ||
                    e.PropertyName == ShipmentFields.ShipCountryCode.Name)
                {
                    RefreshInsurance();
                }

                if (e.PropertyName == PostalShipmentFields.Service.Name)
                {
                    handler.RaisePropertyChanged(nameof(ShipmentModel));
                }

                if (e.PropertyName == "DimsProfileID")
                {
                    PostalShipmentEntity postal = ShipmentModel.ShipmentAdapter.Shipment.Postal;
                    if (postal.DimsProfileID != 0)
                    {
                        DimensionsProfileEntity profile =
                            DimensionProfiles.SingleOrDefault(p => p.DimensionsProfileID == postal.DimsProfileID);

                        if (profile != null)
                        {
                            postal.DimsLength = profile.Length;
                            postal.DimsWidth = profile.Width;
                            postal.DimsHeight = profile.Height;
                            postal.DimsWeight = profile.Weight;
                        }
                    }

                    handler.RaisePropertyChanged(nameof(IsProfileSelected));
                }

                if (e.PropertyName == PostalShipmentFields.Service.Name ||
                    e.PropertyName == "PackagingType")
                {
                    RefreshConfirmationTypes();
                }

                if (e.PropertyName == nameof(ShipmentFields.ShipCountryCode.Name))
                {
                    RefreshServiceTypes();
                }
            }
        }

        /// <summary>
        /// Refresh the providers
        /// </summary>
        private void RefreshProviders()
        {
            if (ShipmentModel.ShipmentAdapter?.Shipment == null)
            {
                Providers = new Dictionary<ShipmentTypeCode, string>();
            }
            else
            {
                Providers = carrierShipmentAdapterOptionsProvider
                    .GetProviders(ShipmentModel.ShipmentAdapter, ShipmentModel.OriginalShipmentTypeCode);
            }
        }

        /// <summary>
        /// Refreshes Insurance
        /// </summary>
        private void RefreshInsurance()
        {
            InsuranceViewModel.Load(ShipmentModel.PackageAdapters, ShipmentModel.PackageAdapters.FirstOrDefault(), ShipmentModel.ShipmentAdapter);
        }

        /// <summary>
        /// Refresh the dimension profiles
        /// </summary>
        private void RefreshDimensionalProfiles()
        {
            DimensionProfiles =
                carrierShipmentAdapterOptionsProvider.GetDimensionsProfiles(ShipmentModel.PackageAdapters.FirstOrDefault()).ToList();

            if (ShipmentModel.ShipmentAdapter.Shipment.Postal != null && DimensionProfiles.None(d => d.DimensionsProfileID ==
                                            ShipmentModel.ShipmentAdapter.Shipment.Postal.DimsProfileID))
            {
                ShipmentModel.ShipmentAdapter.Shipment.Postal.DimsProfileID = 0;
            }
        }

        /// <summary>
        /// Refresh the package types
        /// </summary>
        private void RefreshPackageTypes()
        {
            if (ShipmentModel.ShipmentAdapter?.Shipment == null)
            {
                PackageTypes = Enumerable.Empty<KeyValuePair<int, string>>();
            }
            else
            {
                PackageTypes = carrierShipmentAdapterOptionsProvider.GetPackageTypes(ShipmentModel.ShipmentAdapter);
            }
        }

        /// <summary>
        /// Refresh the confirmation types
        /// </summary>
        private void RefreshConfirmationTypes()
        {
            // Check to see if object is a postal shipment adapter
            if (ShipmentModel.ShipmentAdapter != null &&
                !PostalUtility.IsPostalShipmentType(ShipmentModel.ShipmentAdapter.ShipmentTypeCode))
            {
                ConfirmationTypes = Enumerable.Empty<KeyValuePair<int, string>>();
            }
            else
            {
                PostalShipmentType postalShipmentType = (PostalShipmentType) shipmentTypeManager.Get(ShipmentModel.ShipmentAdapter.ShipmentTypeCode);
                PostalServiceType postalServiceType = (PostalServiceType) ShipmentModel.ShipmentAdapter.ServiceType;

                // See if all have confirmation as an option or not
                PostalPackagingType packagingType =
                    (PostalPackagingType) ShipmentModel.ShipmentAdapter.Shipment.Postal.PackagingType;
                ConfirmationTypes = postalShipmentType
                    .GetAvailableConfirmationTypes(ShipmentModel.ShipmentAdapter.Shipment.ShipCountryCode,
                                                   postalServiceType, packagingType)
                    .ToDictionary(serviceType => (int) serviceType,
                                  serviceType => EnumHelper.GetDescription(serviceType));
            }
        }

        /// <summary>
        /// Refresh the ServiceTypes
        /// </summary>
        private void RefreshServiceTypes()
        {
            ServiceTypes = carrierShipmentAdapterOptionsProvider.GetServiceTypes(ShipmentModel.ShipmentAdapter);
        }

        /// <summary>
        /// Shows the manage dimensional profiles dialog and updates the local profile collection after it closes
        /// </summary>
        private void ManageDimensionalProfilesAction()
        {
            using (DimensionsManagerDlg dlg = getDimensionsManagerDlg())
            {
                dlg.ShowDialog();
                RefreshDimensionalProfiles();
            }
        }

        public void Dispose() =>
            ShipmentModel.PropertyChanged -= ShipmentModelPropertyChanged;
    }
}
