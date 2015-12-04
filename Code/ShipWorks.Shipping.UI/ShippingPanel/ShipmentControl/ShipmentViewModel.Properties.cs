using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using ShipWorks.Shipping.Rating;
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
        private bool usingInsurance;
        private int serviceType;
        private int numberOfPackages;
        private IEnumerable<IPackageAdapter> packageAdapters;
        private IPackageAdapter selectedPackageAdapter;
        private bool supportsMultiplePackages;
        private bool supportsPackageTypes;
        private ICarrierShipmentAdapter shipmentAdapter;

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
        /// Is the shipment using insurance? 
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool UsingInsurance
        {
            get { return usingInsurance; }
            set { handler.Set(nameof(UsingInsurance), ref usingInsurance, value); }
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
        /// Number of packages the shipment has
        /// </summary>
        [Obfuscation(Exclude = true)]
        public int NumberOfPackages
        {
            get { return numberOfPackages; }
            set
            {
                if (numberOfPackages != value && numberOfPackages != 0 && value != 0)
                {
                    PackageAdapters = shipmentAdapter.GetPackageAdapters(value);
                    SelectedPackageAdapter = PackageAdapters.First();
                }

                handler.Set(nameof(NumberOfPackages), ref numberOfPackages, value, true);
            }
        }

        /// <summary>
        /// List of package adapters for the shipment
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IEnumerable<IPackageAdapter> PackageAdapters
        {
            get { return packageAdapters; }
            set { handler.Set(nameof(PackageAdapters), ref packageAdapters, value, true); }
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
    }
}
