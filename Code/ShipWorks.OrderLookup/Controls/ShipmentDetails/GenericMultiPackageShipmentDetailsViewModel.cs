using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.Collections;
using Shared.System.ComponentModel.DataAnnotations;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.OrderLookup.FieldManager;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.UI.ShippingPanel;
using ShipWorks.Shipping.UI.ShippingPanel.ShipmentControl;

namespace ShipWorks.OrderLookup.Controls.ShipmentDetails
{
    /// <summary>
    /// View model for shipment details
    /// </summary>
    public abstract class GenericMultiPackageShipmentDetailsViewModel : OrderLookupViewModelBase, IDetailsViewModel, IDataErrorInfo
    {
        private const int MaxPackageCount = 25;

        private readonly Func<DimensionsManagerDlg> getDimensionsManagerDlg;
        private readonly ICarrierShipmentAdapterOptionsProvider carrierShipmentAdapterOptionsProvider;
        private List<DimensionsProfileEntity> dimensionProfiles;
        private Dictionary<ShipmentTypeCode, string> providers;
        private IDictionary<int, string> packageTypes;
        private IDictionary<int, string> serviceTypes;
        private System.Collections.ObjectModel.ObservableCollection<PackageAdapterWrapper> packages;
        private PackageAdapterWrapper selectedPackage;

        /// <summary>
        /// Constructor
        /// </summary>
        protected GenericMultiPackageShipmentDetailsViewModel(IOrderLookupShipmentModel shipmentModel,
                                                              IInsuranceViewModel insuranceViewModel,
                                                              Func<DimensionsManagerDlg> getDimensionsManagerDlg,
                                                              ICarrierShipmentAdapterOptionsProvider carrierShipmentAdapterOptionsProvider) :
            base(shipmentModel)
        {
            this.getDimensionsManagerDlg = getDimensionsManagerDlg;
            this.carrierShipmentAdapterOptionsProvider = carrierShipmentAdapterOptionsProvider;
            InsuranceViewModel = insuranceViewModel;

            ManageDimensionalProfiles = new RelayCommand(ManageDimensionalProfilesAction);
            AddPackageCommand = new RelayCommand(AddPackageAction, () => Packages.IsCountLessThan(MaxPackageCount));
            DeletePackageCommand = new RelayCommand(DeletePackageAction,
                () => SelectedPackage != null && Packages.Count > 1);

            Packages = new System.Collections.ObjectModel.ObservableCollection<PackageAdapterWrapper>(ShipmentModel.PackageAdapters.Select(x => new PackageAdapterWrapper(x)));
            SelectedPackage = Packages.FirstOrDefault();

            RefreshDimensionalProfiles();
            RefreshInsurance();
            RefreshPackageTypes();
            RefreshServiceTypes();
            RefreshProviders();
        }

        /// <summary>
        /// Panel ID
        /// </summary>
        public override SectionLayoutIDs PanelID => SectionLayoutIDs.ShipmentDetails;

        /// <summary>
        /// Delete the selected package
        /// </summary>
        private void DeletePackageAction()
        {
            if (Packages.Count < 2)
            {
                return;
            }

            PackageAdapterWrapper packageAdapter = SelectedPackage;
            ShipmentModel.ShipmentAdapter.DeletePackage(packageAdapter.WrappedAdapter, p => ShipmentModel.UnwirePropertyChangedEvent(p));

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

            // Force rates to refresh after adding a package
            ShipmentModel.RaisePropertyChanged(null);
            Handler.RaisePropertyChanged(nameof(ShipmentModel));

            AddPackageCommand.RaiseCanExecuteChanged();
            DeletePackageCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Add a package
        /// </summary>
        private void AddPackageAction()
        {
            PackageAdapterWrapper newPackage = new PackageAdapterWrapper(ShipmentModel.ShipmentAdapter.AddPackage(p => ShipmentModel.WirePropertyChangedEvent(p)));

            Packages.Add(newPackage);
            SelectedPackage = newPackage;

            RefreshInsurance();

            // Force rates to refresh after adding a package
            ShipmentModel.RaisePropertyChanged(null);
            AddPackageCommand.RaiseCanExecuteChanged();
            DeletePackageCommand.RaiseCanExecuteChanged();
        }

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
            set => Handler.Set(nameof(DimensionProfiles), ref dimensionProfiles, value);
        }

        /// <summary>
        /// True if a profile is selected
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool IsProfileSelected => SelectedPackage.DimsProfileID > 0;

        /// <summary>
        /// Collection of Providers
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
        /// Collection of valid PackageTypes
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IDictionary<int, string> PackageTypes
        {
            get => packageTypes;
            set => Handler.Set(nameof(PackageTypes), ref packageTypes, value);
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
        /// Collection of packages
        /// </summary>
        [Obfuscation(Exclude = true)]
        public System.Collections.ObjectModel.ObservableCollection<PackageAdapterWrapper> Packages
        {
            get => packages;
            set => Handler.Set(nameof(Packages), ref packages, value);
        }

        /// <summary>
        /// The currently selected package
        /// </summary>
        [Obfuscation(Exclude = true)]
        public PackageAdapterWrapper SelectedPackage
        {
            get => selectedPackage;
            set
            {
                if (value == null)
                {
                    value = Packages.First();
                }

                Handler.Set(nameof(SelectedPackage), ref selectedPackage, value);

                RefreshInsurance();

                RaisePropertyChanged(nameof(SelectedPackageWeight));
                RaisePropertyChanged(nameof(SelectedPackageDimsProfileID));
                RaisePropertyChanged(nameof(IsProfileSelected));
            }
        }

        /// <summary>
        /// The selected packages content weight
        /// </summary>
        [Obfuscation(Exclude = true)]
        [Required(AllowEmptyStrings = false, ErrorMessage = @"Weight value is required.")]
        [Range(0.0001, 999999999, ErrorMessage = @"Please enter a valid weight.")]
        [DoubleCompare(0, ValueCompareOperatorType.GreaterThanOrEqualTo, ErrorMessage = @"Weight must be greater than or equal $0.00.")]
        public double SelectedPackageWeight
        {
            get => SelectedPackage.Weight;
            set
            {
                Handler.Set(nameof(SelectedPackageWeight), (v) => SelectedPackage.Weight = v, SelectedPackage.Weight, value);
            }
        }

        /// <summary>
        /// The selected packages DimsProfileID
        /// </summary>
        [Obfuscation(Exclude = true)]
        public long SelectedPackageDimsProfileID
        {
            get => SelectedPackage.DimsProfileID;
            set
            {
                Handler.Set(nameof(SelectedPackageDimsProfileID), (v) => SelectedPackage.DimsProfileID = v, SelectedPackage.DimsProfileID, value);

                if (SelectedPackage.DimsProfileID != 0)
                {
                    DimensionsProfileEntity profile =
                        DimensionProfiles.SingleOrDefault(p => p.DimensionsProfileID == SelectedPackage.DimsProfileID);

                    if (profile != null)
                    {
                        SelectedPackage.DimsLength = profile.Length;
                        SelectedPackage.DimsWidth = profile.Width;
                        SelectedPackage.DimsHeight = profile.Height;
                        SelectedPackage.AdditionalWeight = profile.Weight;
                    }
                }
                else
                {
                    SelectedPackage.DimsLength = 0;
                    SelectedPackage.DimsWidth = 0;
                    SelectedPackage.DimsHeight = 0;
                    SelectedPackage.AdditionalWeight = 0;
                }

                RaisePropertyChanged(nameof(IsProfileSelected));
                RaisePropertyChanged(nameof(SelectedPackage));
            }
        }

        /// <summary>
        /// Whether or not the selected package should apply additional weight
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool SelectedPackageApplyAdditionalWeight
        {
            get => SelectedPackage.ApplyAdditionalWeight;
            set
            {
                Handler.Set(nameof(SelectedPackageApplyAdditionalWeight), (v) => SelectedPackage.ApplyAdditionalWeight = v, SelectedPackage.ApplyAdditionalWeight, value);
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
                Providers = carrierShipmentAdapterOptionsProvider.GetProviders(ShipmentModel.ShipmentAdapter, ShipmentModel.OriginalShipmentTypeCode);
            }
        }

        /// <summary>
        /// Refreshes Insurance
        /// </summary>
        protected void RefreshInsurance()
        {
            InsuranceViewModel.Load(Packages, SelectedPackage, ShipmentModel.ShipmentAdapter);
        }

        /// <summary>
        /// Refresh the dimension profiles
        /// </summary>
        private void RefreshDimensionalProfiles()
        {
            DimensionProfiles = carrierShipmentAdapterOptionsProvider.GetDimensionsProfiles(SelectedPackage).ToList();

            if (SelectedPackage != null && DimensionProfiles.None(d => d.DimensionsProfileID == SelectedPackage.DimsProfileID))
            {
                SelectedPackage.DimsProfileID = 0;
            }
        }

        /// <summary>
        /// Refresh the package types
        /// </summary>
        protected void RefreshPackageTypes()
        {
            if (ShipmentModel.ShipmentAdapter?.Shipment == null)
            {
                PackageTypes = new Dictionary<int, string>();
            }
            else
            {
                PackageTypes = carrierShipmentAdapterOptionsProvider.GetPackageTypes(ShipmentModel.ShipmentAdapter);
            }
        }

        /// <summary>
        /// Refresh the ServiceTypes
        /// </summary>
        protected void RefreshServiceTypes()
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

        #region IDataErrorInfo

        /// <summary>
        /// Do nothing
        /// </summary>
        public string Error => null;

        /// <summary>
        /// Validate the ColumnNames value
        /// </summary>
        public virtual string this[string columnName]
        {
            get
            {
                Debug.Assert(true, "This method should be overriden in the implementing class");

                // If the shipment is null or processed, don't validate anything.
                if (ShipmentModel.ShipmentAdapter?.Shipment == null || ShipmentModel.ShipmentAdapter.Shipment.Processed)
                {
                    return string.Empty;
                }

                return InputValidation<GenericMultiPackageShipmentDetailsViewModel>.Validate(this, columnName);
            }
        }

        #endregion
    }
}
