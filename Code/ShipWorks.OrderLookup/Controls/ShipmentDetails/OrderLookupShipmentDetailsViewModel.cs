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
using ShipWorks.Templates.Saving;

namespace ShipWorks.OrderLookup.Controls.ShipmentDetails
{
    /// <summary>
    /// Viewmodel for orderlookup
    /// </summary>
    [KeyedComponent(typeof(INotifyPropertyChanged), OrderLookupPanels.ShipmentDetails)]
    public class OrderLookupShipmentDetailsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly PropertyChangedHandler handler;
        private readonly IDimensionsManager dimensionsManager;
        private readonly IShipmentPackageTypesBuilderFactory shipmentPackageTypesBuilderFactory;
        private readonly IShipmentTypeManager shipmentTypeManager;
        private readonly IShipmentServicesBuilderFactory shipmentServicesBuilderFactory;
        private readonly ShipmentTypeProvider shipmentTypeProvider;
        private readonly Func<DimensionsManagerDlg> getDimensionsManagerDlg;
        private List<DimensionsProfileEntity> dimensionProfiles;
        private Dictionary<ShipmentTypeCode, string> providers;
        private IEnumerable<KeyValuePair<int, string>> packageTypes;
        private IEnumerable<KeyValuePair<int, string>> confirmationTypes;
        private IEnumerable<KeyValuePair<int, string>> serviceTypes;
        private IPackageAdapter selectedPackage;
        private double length;
        private double width;
        private double height;
        private double weight;
        private bool addToWeight;
        private double addToWeightValue;
        private int packagingType;
        private long dimsProfileID;
        private List<IPackageAdapter> packages;
        private int selectedPackageCount;
        private List<int> packageCountRange;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupShipmentDetailsViewModel(
            IOrderLookupShipmentModel shipmentModel,
            IDimensionsManager dimensionsManager,
            IShipmentPackageTypesBuilderFactory shipmentPackageTypesBuilderFactory,
            IShipmentTypeManager shipmentTypeManager,
            IShipmentServicesBuilderFactory shipmentServicesBuilderFactory,
            IInsuranceViewModel insuranceViewModel,
            ShipmentTypeProvider shipmentTypeProvider,
            Func<DimensionsManagerDlg> getDimensionsManagerDlg)
        {
            ShipmentModel = shipmentModel;
            ShipmentModel.PropertyChanged += ShipmentModelPropertyChanged;

            this.dimensionsManager = dimensionsManager;
            this.shipmentPackageTypesBuilderFactory = shipmentPackageTypesBuilderFactory;
            this.shipmentTypeManager = shipmentTypeManager;
            this.shipmentServicesBuilderFactory = shipmentServicesBuilderFactory;
            this.shipmentTypeProvider = shipmentTypeProvider;
            this.getDimensionsManagerDlg = getDimensionsManagerDlg;
            InsuranceViewModel = insuranceViewModel;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
            ManageDimensionalProfiles = new RelayCommand(ManageDimensionalProfilesAction);
            PackageCountRange = Enumerable.Range(1, 25).ToList();
        }

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
            set => handler.Set(nameof(DimensionProfiles), ref dimensionProfiles, value);
        }
        
        /// <summary>
        /// True if a profile is selected
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool IsProfileSelected => ShipmentModel.ShipmentAdapter.Shipment.Postal.DimsProfileID > 0;
      
        /// <summary>
        /// Packages for the shipment
        /// </summary>
        [Obfuscation(Exclude = true)]
        public List<IPackageAdapter> Packages
        {
            get => packages;
            set => handler.Set(nameof(Packages), ref packages, value);
        }

        /// <summary>
        /// List of numbers representing the number of packages the user can select
        /// </summary>
        [Obfuscation(Exclude = true)]
        public List<int> PackageCountRange
        {
            get => packageCountRange;
            set => handler.Set(nameof(PackageCountRange), ref packageCountRange, value);
        }
        
        /// <summary>
        /// The currently selected package
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IPackageAdapter SelectedPackage
        {
            get => selectedPackage;
            set
            {
                handler.Set(nameof(SelectedPackage), ref selectedPackage, value);
                
                // When adding or removing packages, the selected package is lost, so set it to the first one.
                if (SelectedPackage == null)
                {
                    // Using lowercase version so we don't come back recursively
                    selectedPackage = Packages.FirstOrDefault();
                }

                // Load properties from selected package
                if (SelectedPackage != null)
                {                   
                    DimsProfileID = SelectedPackage.DimsProfileID;
                    Length = SelectedPackage.DimsLength;
                    Width = SelectedPackage.DimsWidth;
                    Height = SelectedPackage.DimsHeight;
                    Weight = SelectedPackage.Weight;
                    AddToWeight = SelectedPackage.ApplyAdditionalWeight;
                    AddToWeightValue = SelectedPackage.AdditionalWeight;
                    PackagingType = SelectedPackage.PackagingType;
                }
            }
        }
        
        /// <summary>
        /// Dimesion profile ID for the selected package
        /// </summary>
        [Obfuscation(Exclude = true)]
        public long DimsProfileID
        {
            get => dimsProfileID;
            set
            {
                handler.Set(nameof(DimsProfileID), ref dimsProfileID, value);
                SelectedPackage.DimsProfileID = dimsProfileID;
            }
        }
        
        /// <summary>
        /// Length of the selected package
        /// </summary>
        [Obfuscation(Exclude = true)]
        public double Length
        {
            get => length;
            set
            {
                handler.Set(nameof(Length), ref length, value);
                SelectedPackage.DimsLength = length;
            }
        }

        /// <summary>
        /// Width of the selected package
        /// </summary>
        [Obfuscation(Exclude = true)]
        public double Width
        {
            get => width;
            set
            {
                handler.Set(nameof(Width), ref width, value);
                SelectedPackage.DimsWidth = width;
            }
        }

        /// <summary>
        /// Height of the selected package
        /// </summary>
        [Obfuscation(Exclude = true)]
        public double Height
        {
            get => height;
            set
            {
                handler.Set(nameof(Height), ref height, value);
                SelectedPackage.DimsHeight = height;
            }
        }

        /// <summary>
        /// Weight of the selected package
        /// </summary>
        [Obfuscation(Exclude = true)]
        public double Weight
        {
            get => weight;
            set
            {
                handler.Set(nameof(Weight), ref weight, value);
                SelectedPackage.Weight = weight;
            }
        }

        /// <summary>
        /// Whether or not to add additional weight to the selected package
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool AddToWeight
        {
            get => addToWeight;
            set
            {
                handler.Set(nameof(AddToWeight), ref addToWeight, value);
                SelectedPackage.ApplyAdditionalWeight = addToWeight;
            }
        }

        /// <summary>
        /// The amount of additional weight to add to the selected package
        /// </summary>
        [Obfuscation(Exclude = true)]
        public double AddToWeightValue
        {
            get => addToWeightValue;
            set
            {
                handler.Set(nameof(AddToWeightValue), ref addToWeightValue, value);
                SelectedPackage.AdditionalWeight = addToWeightValue;
            }
        }
        
        /// <summary>
        /// Packaging type for the selected package
        /// </summary>
        [Obfuscation(Exclude = true)]
        public int PackagingType
        {
            get => packagingType;
            set
            {
                handler.Set(nameof(PackagingType), ref packagingType, value);
                SelectedPackage.PackagingType = packagingType;
            }
        }
        
        /// <summary>
        /// The selected number of packages
        /// </summary>
        [Obfuscation(Exclude = true)]
        public int SelectedPackageCount
        {
            get => selectedPackageCount;
            set
            {
                handler.Set(nameof(SelectedPackageCount), ref selectedPackageCount, value);
                
                int packageCountDifference = SelectedPackageCount - Packages.Count; 
                if (packageCountDifference > 0)
                {
                    AddPackages(packageCountDifference);
                }
                else if (packageCountDifference < 0)
                {
                    DeletePackage(packageCountDifference);
                }
                
                RefreshPackages();
                RefreshInsurance();
            }
        }
        
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
                if (e.PropertyName == nameof(ShipmentModel.SelectedOrder))
                {
                    Providers = shipmentTypeProvider.GetAvailableShipmentTypes(ShipmentModel.ShipmentAdapter).ToDictionary(s => s, s => EnumHelper.GetDescription(s));
                    RefreshDimensionalProfiles();

                    if (Packages == null)
                    {
                        LoadPackages();
                    }
                }

                if (e.PropertyName == nameof(ShipmentModel.SelectedOrder) ||
                    e.PropertyName == PostalShipmentFields.Service.Name ||
                    e.PropertyName == nameof(ShipmentModel.ShipmentAdapter.ShipmentTypeCode) ||
                    e.PropertyName == ShipmentFields.ShipCountryCode.Name)
                {
                    RefreshInsurance();
                }

                if (e.PropertyName == nameof(ShipmentModel.SelectedOrder) || e.PropertyName == PostalShipmentFields.Service.Name)
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

                if (e.PropertyName == nameof(ShipmentModel.ShipmentAdapter.ShipmentTypeCode) || e.PropertyName == nameof(ShipmentModel.SelectedOrder))
                {
                    RefreshPackageTypes();
                }

                if (e.PropertyName == nameof(ShipmentModel.SelectedOrder) || e.PropertyName == nameof(ShipmentModel.ShipmentAdapter.ShipmentTypeCode) || e.PropertyName == PostalShipmentFields.Service.Name ||
                    e.PropertyName == "PackagingType")
                {
                    RefreshConfirmationTypes();
                }

                if (e.PropertyName == nameof(ShipmentModel.SelectedOrder) || e.PropertyName == nameof(ShipmentModel.ShipmentAdapter.ShipmentTypeCode) ||
                    e.PropertyName == nameof(ShipmentFields.ShipCountryCode.Name))
                {
                    RefreshServiceTypes();
                }
            }
        }

        /// <summary>
        /// Load the packages for the shipment
        /// </summary>
        private void LoadPackages()
        {
            RefreshPackages();
            SelectedPackageCount = Packages.Count;
            if (SelectedPackage == null)
            {
                SelectedPackage = Packages.FirstOrDefault();
            }
        }

        /// <summary>
        /// Refresh the packages for the shipment
        /// </summary>
        private void RefreshPackages() => Packages = ShipmentModel.ShipmentAdapter.GetPackageAdapters().ToList();

        /// <summary>
        /// Add the given amount of packages to the shipment
        /// </summary>
        private void AddPackages(int packageCountDifference)
        {
            for (int i = 0; i < packageCountDifference; i++)
            {
                ShipmentModel.ShipmentAdapter.AddPackage();
            }
        }

        /// <summary>
        /// Delete the given amount of packages from the shipment
        /// </summary>
        private void DeletePackage(int packageCountDifference)
        {
            int packagesToDelete = Math.Abs(packageCountDifference);

            for (int i = 0; i < packagesToDelete; i++)
            {
                RefreshPackages();
                ShipmentModel.ShipmentAdapter.DeletePackage(Packages.LastOrDefault());
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
                dimensionsManager.Profiles(ShipmentModel.PackageAdapters.FirstOrDefault()).ToList();

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
                PackageTypes = shipmentPackageTypesBuilderFactory.Get(ShipmentModel.ShipmentAdapter.ShipmentTypeCode)
                    .BuildPackageTypeDictionary(new[] {ShipmentModel.ShipmentAdapter.Shipment});
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
            Dictionary<int, string> updatedServices = new Dictionary<int, string>();

            try
            {
                updatedServices = shipmentServicesBuilderFactory.Get(ShipmentModel.ShipmentAdapter.ShipmentTypeCode)
                    .BuildServiceTypeDictionary(new[] {ShipmentModel.ShipmentAdapter.Shipment});
            }
            catch (InvalidRateGroupShippingException)
            {
                updatedServices.Add(ShipmentModel.ShipmentAdapter.ServiceType, "Error getting service types.");
            }

            // If no service types are returned, the carrier doesn't support service types,
            // so just return.
            if (!updatedServices.Any())
            {
                ServiceTypes = new List<KeyValuePair<int, string>>();
            }
            else
            {
                ServiceTypes = updatedServices.ToList();
            }
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
    }
}
