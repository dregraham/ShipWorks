﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Shared.System.ComponentModel.DataAnnotations;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.UI.ShippingPanel.ShipmentControl
{
    /// <summary>
    /// View model for use by ShipmentControl
    /// </summary>
    public partial class ShipmentViewModel
    {
        private DateTime shipDate;
        private double totalWeight;
        private int serviceType;
        private ObservableCollection<IPackageAdapter> packageAdapters;
        private IPackageAdapter selectedPackageAdapter;
        private bool supportsMultiplePackages;
        private bool supportsPackageTypes;
        private bool supportsDimensions;
        private ICarrierShipmentAdapter shipmentAdapter;
        private ObservableCollection<DimensionsProfileEntity> dimensionsProfiles;
        private DimensionsProfileEntity selectedDimensionsProfile;
        private IInsuranceViewModel insuranceViewModel;
        private double dimsLength;
        private double dimsWidth;
        private double dimsHeight;
        private long dimsProfileID;
        private double additionalWeight;
        private bool applyAdditionalWeight;
        private int packagingType;

        /// <summary>
        /// The insurance view model to use.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IInsuranceViewModel InsuranceViewModel
        {
            get { return insuranceViewModel; }
            set { handler.Set(nameof(InsuranceViewModel), ref insuranceViewModel, value); }
        }

        /// <summary>
        /// Observable collection of carrier service types
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ObservableCollection<KeyValuePair<int, string>> Services { get; }

        /// <summary>
        /// Observable collection of carrier package types
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ObservableCollection<PackageTypeBinding> PackageTypes { get; }

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
        /// Shipment total weight
        /// </summary>
        [Obfuscation(Exclude = true)]
        public double TotalWeight
        {
            get { return totalWeight; }
            set { handler.Set(nameof(TotalWeight), ref totalWeight, value); }
        }

        /// <summary>
        /// Shipment selected service type
        /// </summary>
        [Obfuscation(Exclude = true)]
        public int ServiceType
        {
            get { return serviceType; }
            set { handler.Set(nameof(ServiceType), ref serviceType, value, true); }
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
        public ObservableCollection<IPackageAdapter> PackageAdapters
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
        public IPackageAdapter SelectedPackageAdapter
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
                        AdditionalWeight = SelectedDimensionsProfile.Weight;
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
        public double AdditionalWeight
        {
            get { return additionalWeight; }
            set { handler.Set(nameof(AdditionalWeight), ref additionalWeight, value, true); }
        }

        /// <summary>
        /// Apply additional weight to the package
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ApplyAdditionalWeight
        {
            get { return applyAdditionalWeight; }
            set { handler.Set(nameof(ApplyAdditionalWeight), ref applyAdditionalWeight, value, true); }
        }

        /// <summary>
        /// Type of packaging
        /// </summary>
        [Obfuscation(Exclude = true)]
        public int PackagingType
        {
            get { return packagingType; }
            set { handler.Set(nameof(PackagingType), ref packagingType, value, true); }
        }
    }
}
