﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using Shared.System.ComponentModel.DataAnnotations;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.UPS.Enums;
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
    [KeyedComponent(typeof(IDetailsViewModel), ShipmentTypeCode.UpsOnLineTools)]
    [WpfView(typeof(UpsShipmentDetailsControl))]
    public class UpsShipmentDetailsViewModel : IDetailsViewModel, INotifyPropertyChanged, IDataErrorInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly PropertyChangedHandler handler;
        private readonly IDimensionsManager dimensionsManager;
        private readonly IShipmentPackageTypesBuilderFactory shipmentPackageTypesBuilderFactory;
        private readonly IShipmentServicesBuilderFactory shipmentServicesBuilderFactory;
        private readonly Func<DimensionsManagerDlg> getDimensionsManagerDlg;
        private List<DimensionsProfileEntity> dimensionProfiles;
        private IEnumerable<KeyValuePair<int, string>> packageTypes;
        private IEnumerable<KeyValuePair<int, string>> confirmationTypes;
        private IEnumerable<KeyValuePair<int, string>> serviceTypes;
        System.Collections.ObjectModel.ObservableCollection<IPackageAdapter> packages;
        private IPackageAdapter selectedPackage;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsShipmentDetailsViewModel(
            IOrderLookupShipmentModel shipmentModel,
            IDimensionsManager dimensionsManager,
            IShipmentPackageTypesBuilderFactory shipmentPackageTypesBuilderFactory,
            IShipmentServicesBuilderFactory shipmentServicesBuilderFactory,
            IInsuranceViewModel insuranceViewModel,
            Func<DimensionsManagerDlg> getDimensionsManagerDlg)
        {
            ShipmentModel = shipmentModel;
            ShipmentModel.PropertyChanged += ShipmentModelPropertyChanged;

            this.dimensionsManager = dimensionsManager;
            this.shipmentPackageTypesBuilderFactory = shipmentPackageTypesBuilderFactory;
            this.shipmentServicesBuilderFactory = shipmentServicesBuilderFactory;
            this.getDimensionsManagerDlg = getDimensionsManagerDlg;
            InsuranceViewModel = insuranceViewModel;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
            ManageDimensionalProfiles = new RelayCommand(ManageDimensionalProfilesAction);
            AddPackageCommand = new RelayCommand(AddPackageAction);
            DeletePackageCommand = new RelayCommand(DeletePackageAction,
                () => SelectedPackage != null && Packages.Count > 1);

            Packages = new System.Collections.ObjectModel.ObservableCollection<IPackageAdapter>(ShipmentModel.PackageAdapters);
            SelectedPackage = Packages.FirstOrDefault();

            RefreshDimensionalProfiles();
            RefreshInsurance();
            RefreshPackageTypes();
            RefreshServiceTypes();

            ConfirmationTypes =
                EnumHelper.GetEnumList<UpsDeliveryConfirmationType>()
                .Select(e => new KeyValuePair<int, string>((int) e.Value, e.Description));
        }

        /// <summary>
        /// Delete the selected package
        /// </summary>
        private void DeletePackageAction()
        {
            if (Packages.Count < 2)
            {
                return;
            }

            IPackageAdapter packageAdapter = SelectedPackage;
            ShipmentModel.ShipmentAdapter.DeletePackage(packageAdapter);

            int location = Packages.IndexOf(packageAdapter);
            SelectedPackage = Packages.Last() == packageAdapter ?
                Packages.ElementAt(location - 1) :
                Packages.ElementAt(location + 1);

            Packages.Remove(packageAdapter);

            RefreshInsurance();

            for (int i = 0; i < Packages.Count; i++)
            {
                Packages[i].Index = i + 1;
            }
        }

        /// <summary>
        /// Add a package
        /// </summary>
        private void AddPackageAction()
        {
            IPackageAdapter newPackage = ShipmentModel.ShipmentAdapter.AddPackage();
            Packages.Add(newPackage);
            SelectedPackage = newPackage;

            RefreshInsurance();
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
        /// Delete a package
        /// </summary>
        [Obfuscation(Exclude = true)]
        public RelayCommand DeletePackageCommand { get; set; }

        /// <summary>
        /// Add a package
        /// </summary>
        [Obfuscation(Exclude = true)]
        public RelayCommand AddPackageCommand { get; set; }

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
        public bool IsProfileSelected => SelectedPackage.DimsProfileID > 0;

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
        /// Collection of packages
        /// </summary>
        [Obfuscation(Exclude = true)]
        public System.Collections.ObjectModel.ObservableCollection<IPackageAdapter> Packages
        {
            get => packages;
            set => handler.Set(nameof(Packages), ref packages, value);
        }

        /// <summary>
        /// Collection of packages
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IPackageAdapter SelectedPackage
        {
            get => selectedPackage;
            set
            {
                handler.Set(nameof(SelectedPackage), ref selectedPackage, value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedPackageWeight)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedPackageDimsProfileID)));
            }
        }

        /// <summary>
        /// The shipment content weight
        /// </summary>
        [Obfuscation(Exclude = true)]
        [Required(AllowEmptyStrings = false, ErrorMessage = @"Weight value is required.")]
        [Range(0.0001, 999999999, ErrorMessage = @"Please enter a valid weight.")]
        [DoubleCompare(0, ValueCompareOperatorType.GreaterThanOrEqualTo, ErrorMessage = @"Weight must be greater than or equal $0.00.")]
        public double SelectedPackageWeight
        {
            get { return SelectedPackage.Weight; }
            set
            {
                handler.Set(nameof(SelectedPackageWeight), (v) => SelectedPackage.Weight = v, SelectedPackage.Weight, value);
            }
        }

        /// <summary>
        /// The shipment content weight
        /// </summary>
        [Obfuscation(Exclude = true)]
        public long SelectedPackageDimsProfileID
        {
            get { return SelectedPackage.DimsProfileID; }
            set
            {
                handler.Set(nameof(SelectedPackageDimsProfileID), (v) => SelectedPackage.DimsProfileID = v, SelectedPackage.DimsProfileID, value);

                if (SelectedPackage.DimsProfileID != 0)
                {
                    DimensionsProfileEntity profile =
                        DimensionProfiles.SingleOrDefault(p => p.DimensionsProfileID == SelectedPackage.DimsProfileID);

                    if (profile != null)
                    {
                        SelectedPackage.DimsLength = profile.Length;
                        SelectedPackage.DimsWidth = profile.Width;
                        SelectedPackage.DimsHeight = profile.Height;
                        SelectedPackage.Weight = profile.Weight;

                    }
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsProfileSelected)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedPackage)));
            }
        }

        /// <summary>
        /// The shipment content weight
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool SelectedPackageApplyAdditionalWeight
        {
            get { return SelectedPackage.ApplyAdditionalWeight; }
            set
            {
                handler.Set(nameof(SelectedPackageDimsProfileID), (v) => SelectedPackage.ApplyAdditionalWeight = v, SelectedPackage.ApplyAdditionalWeight, value);
            }
        }

        /// <summary>
        /// Update when order changes
        /// </summary>
        private void ShipmentModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (ShipmentModel.SelectedOrder != null)
            {
                if (e.PropertyName == UpsShipmentFields.Service.Name ||
                    e.PropertyName == ShipmentFields.ShipCountryCode.Name)
                {
                    RefreshInsurance();
                }

                if (e.PropertyName == nameof(ShipmentFields.ShipCountryCode.Name))
                {
                    RefreshServiceTypes();
                }
            }
        }

        /// <summary>
        /// Refreshes Insurance
        /// </summary>
        private void RefreshInsurance()
        {
            InsuranceViewModel.Load(Packages, SelectedPackage, ShipmentModel.ShipmentAdapter);
        }

        /// <summary>
        /// Refresh the dimension profiles
        /// </summary>
        private void RefreshDimensionalProfiles()
        {
            DimensionProfiles =
                dimensionsManager.Profiles(ShipmentModel.PackageAdapters.FirstOrDefault()).ToList();

            if (SelectedPackage != null && DimensionProfiles.None(d => d.DimensionsProfileID == SelectedPackage.DimsProfileID))
            {
                SelectedPackage.DimsProfileID = 0;
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
                    .BuildPackageTypeDictionary(new[] { ShipmentModel.ShipmentAdapter.Shipment });
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
                    .BuildServiceTypeDictionary(new[] { ShipmentModel.ShipmentAdapter.Shipment });
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

        public void Dispose() =>
            ShipmentModel.PropertyChanged -= ShipmentModelPropertyChanged;

        #region IDataErrorInfo

        /// <summary>
        /// Do nothing
        /// </summary>
        public string Error => null;

        /// <summary>
        /// Validate the ColumnNames value
        /// </summary>
        public string this[string columnName]
        {
            get
            {
                // If the shipment is null or processed, don't validate anything.
                if (ShipmentModel.ShipmentAdapter?.Shipment == null || ShipmentModel.ShipmentAdapter.Shipment.Processed)
                {
                    return string.Empty;
                }

                return InputValidation<UpsShipmentDetailsViewModel>.Validate(this, columnName);
            }
        }

        #endregion
    }
}
