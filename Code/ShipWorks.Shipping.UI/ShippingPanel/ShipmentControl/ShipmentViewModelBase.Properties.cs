using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Shared.System.ComponentModel.DataAnnotations;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.UI.ShippingPanel.ShipmentControl
{
    /// <summary>
    /// View model for use by ShipmentControl
    /// </summary>
    public abstract partial class ShipmentViewModelBase
    {
        private DateTime shipDate;
        private int serviceType;
        private ObservableCollection<PackageAdapterWrapper> packageAdapters;
        private PackageAdapterWrapper selectedPackageAdapter;
        private bool supportsMultiplePackages;
        private bool supportsPackageTypes;
        private bool supportsDimensions;
        protected ICarrierShipmentAdapter shipmentAdapter;
        private ObservableCollection<DimensionsProfileEntity> dimensionsProfiles;
        private DimensionsProfileEntity selectedDimensionsProfile;
        private IInsuranceViewModel insuranceViewModel;
        private double dimsLength;
        private double dimsWidth;
        private double dimsHeight;
        private long dimsProfileID;
        private double dimsWeight;
        private bool dimsAddWeight;
        private int packagingType;
        private string weightErrorMessage;
        private IEnumerable<KeyValuePair<int, string>> packageTypes;
        private IEnumerable<KeyValuePair<int, string>> services;

        /// <summary>
        /// The insurance view model to use.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public virtual IInsuranceViewModel InsuranceViewModel
        {
            get { return insuranceViewModel; }
            set { handler.Set(nameof(InsuranceViewModel), ref insuranceViewModel, value); }
        }

        /// <summary>
        /// Carrier service types
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IEnumerable<KeyValuePair<int, string>> Services
        {
            get { return services; }
            set { handler.Set(nameof(Services), ref services, value); }
        }

        /// <summary>
        /// Carrier package types
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IEnumerable<KeyValuePair<int, string>> PackageTypes
        {
            get { return packageTypes; }
            set { handler.Set(nameof(PackageTypes), ref packageTypes, value); }
        }

        /// <summary>
        /// Shipment ship date
        /// </summary>
        [Obfuscation(Exclude = true)]
        [Required(ErrorMessage = @"Ship date is required")]
        [DateCompare(DateCompareType.Today, ValueCompareOperatorType.GreaterThanOrEqualTo, ErrorMessage = @"Ship date must be today or in the future.")]
        public DateTime ShipDate
        {
            get { return shipDate; }
            set { handler.Set(nameof(ShipDate), ref shipDate, value); }
        }

        /// <summary>
        /// Current error message
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string WeightErrorMessage
        {
            get { return weightErrorMessage; }
            set { handler.Set(nameof(WeightErrorMessage), ref weightErrorMessage, value, true); }
        }

        /// <summary>
        /// Shipment selected service type
        /// </summary>
        [Obfuscation(Exclude = true)]
        public int ServiceType
        {
            get { return serviceType; }
            set
            {
                // Verify that the currently selected service type is in the list allowed
                // If not, just select the first one in the list.
                int updatedServiceType = value;
                if (Services.All(x => x.Key != value))
                {
                    updatedServiceType = Services.FirstOrDefault().Key;
                }

                handler.Set(nameof(ServiceType), ref serviceType, updatedServiceType, false);
                RefreshPackageTypes();
            }
        }

        /// <summary>
        /// Bindable list of numbers for packages count drop down
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IEnumerable<int> PackageCountList
        {
            get { return Enumerable.Range(1, 25); }
        }

        /// <summary>
        /// List of package adapters for the shipment
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ObservableCollection<PackageAdapterWrapper> PackageAdapters
        {
            get { return packageAdapters; }
            set
            {
                handler.Set(nameof(PackageAdapters), ref packageAdapters, value, true);
                InsuranceViewModel.PackageAdapters = PackageAdapters;
            }
        }

        /// <summary>
        /// Currently selected package adapter
        /// </summary>
        [Obfuscation(Exclude = true)]
        public PackageAdapterWrapper SelectedPackageAdapter
        {
            get { return selectedPackageAdapter; }
            set { handler.Set(nameof(SelectedPackageAdapter), ref selectedPackageAdapter, value, true); }
        }

        /// <summary>
        /// Does the shipment support multiple packages?
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool SupportsMultiplePackages
        {
            get { return supportsMultiplePackages; }
            set { handler.Set(nameof(SupportsMultiplePackages), ref supportsMultiplePackages, value); }
        }

        /// <summary>
        /// Does the shipment support package types?
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool SupportsPackageTypes
        {
            get { return supportsPackageTypes; }
            set { handler.Set(nameof(SupportsPackageTypes), ref supportsPackageTypes, value); }
        }

        /// <summary>
        /// Does the shipment support dimensions?
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool SupportsDimensions
        {
            get { return supportsDimensions; }
            set { handler.Set(nameof(SupportsDimensions), ref supportsDimensions, value); }
        }

        /// <summary>
        /// Gets or sets the packaging type.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public DimensionsProfileEntity SelectedDimensionsProfile
        {
            get
            {
                return selectedDimensionsProfile;
            }
            set
            {
                if (handler.Set(nameof(SelectedDimensionsProfile), ref selectedDimensionsProfile, value, true))
                {
                    if (SelectedDimensionsProfile == null)
                    {
                        return;
                    }

                    if (SelectedDimensionsProfile.DimensionsProfileID == 0)
                    {
                        DimsProfileID = 0;
                    }
                    else
                    {
                        DimsProfileID = SelectedDimensionsProfile.DimensionsProfileID;
                        DimsLength = SelectedDimensionsProfile.Length;
                        DimsWidth = SelectedDimensionsProfile.Width;
                        DimsHeight = SelectedDimensionsProfile.Height;
                        DimsWeight = SelectedDimensionsProfile.Weight;
                    }
                }
            }
        }

        /// <summary>
        /// Does the shipment support package types?
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ObservableCollection<DimensionsProfileEntity> DimensionsProfiles
        {
            get { return dimensionsProfiles; }
            set { handler.Set(nameof(DimensionsProfiles), ref dimensionsProfiles, value, true); }
        }

        /// <summary>
        /// Length of the current package
        /// </summary>
        [Obfuscation(Exclude = true)]
        public double DimsLength
        {
            get { return dimsLength; }
            set { handler.Set(nameof(DimsLength), ref dimsLength, value, true); }
        }

        /// <summary>
        /// Width of the current package
        /// </summary>
        [Obfuscation(Exclude = true)]
        public double DimsWidth
        {
            get { return dimsWidth; }
            set { handler.Set(nameof(DimsWidth), ref dimsWidth, value, true); }
        }

        /// <summary>
        /// Height of the current package
        /// </summary>
        [Obfuscation(Exclude = true)]
        public double DimsHeight
        {
            get { return dimsHeight; }
            set { handler.Set(nameof(DimsHeight), ref dimsHeight, value, true); }
        }

        /// <summary>
        /// ProfileID of the current package
        /// </summary>
        [Obfuscation(Exclude = true)]
        public long DimsProfileID
        {
            get { return dimsProfileID; }
            set { handler.Set(nameof(DimsProfileID), ref dimsProfileID, value, true); }
        }

        /// <summary>
        /// Additional weight of the package
        /// </summary>
        [Obfuscation(Exclude = true)]
        public double DimsWeight
        {
            get { return dimsWeight; }
            set { handler.Set(nameof(DimsWeight), ref dimsWeight, value, true); }
        }

        /// <summary>
        /// Apply additional weight to the package
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool DimsAddWeight
        {
            get { return dimsAddWeight; }
            set { handler.Set(nameof(DimsAddWeight), ref dimsAddWeight, value, true); }
        }

        /// <summary>
        /// Type of packaging
        /// </summary>
        [Obfuscation(Exclude = true)]
        public int PackagingType
        {
            get { return packagingType; }
            set
            {
                // Verify that the currently selected package type is in the list allowed
                // If not, just select the first one in the list.
                int updatedPackagingType = value;
                if (PackageTypes.All(x => x.Key != value))
                {
                    updatedPackagingType = PackageTypes.FirstOrDefault().Key;
                }

                handler.Set(nameof(PackagingType), ref packagingType, updatedPackagingType, false);
            }
        }

        /// <summary>
        /// Gets the currently selected rate
        /// </summary>
        /// <remarks>We don't care about the selected rate for Other</remarks>
        public virtual RateResult SelectedRate { get; set; } = null;
    }
}
