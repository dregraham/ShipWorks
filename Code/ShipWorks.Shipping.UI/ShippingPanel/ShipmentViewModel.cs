using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using ShipWorks.Core.Messaging;
using ShipWorks.Core.UI;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Rating;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Services.Builders;

namespace ShipWorks.Shipping.UI.ShippingPanel
{
    /// <summary>
    /// View model for use by ShipmentControl
    /// </summary>
    public class ShipmentViewModel : INotifyPropertyChanged, INotifyPropertyChanging, IDisposable
    {
        private DateTime shipDate;
        private double totalWeight;
        private bool usingInsurance;
        private int serviceType;
        private int numberOfPackages;
        private IEnumerable<IPackageAdapter> packageAdapters;
        private readonly IRateSelectionFactory rateSelectionFactory;
        private readonly IDisposable subscription;
        private IPackageAdapter selectedPackageAdapter;
        private bool supportsMultiplePackages;
        private bool supportsPackageTypes;

        private readonly PropertyChangedHandler handler;
        private readonly IShipmentServicesBuilderFactory shipmentServicesBuilderFactory;
        private readonly IShipmentPackageBuilderFactory shipmentPackageBuilderFactory;

        [SuppressMessage("SonarQube", "S2290:Field-like events should not be virtual",
            Justification = "Event is virtual to allow tests to fire it")]
        public virtual event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        /// <summary>
        /// Constructor for use by tests and WPF designer
        /// </summary>
        protected ShipmentViewModel()
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged, () => PropertyChanging);
            Services = new ObservableCollection<KeyValuePair<int, string>>();
            PackageTypes = new ObservableCollection<PackageTypeBinding>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentViewModel(IShipmentServicesBuilderFactory shipmentServicesBuilderFactory,
            IShipmentPackageBuilderFactory shipmentPackageBuilderFactory, IMessenger messenger,
            IRateSelectionFactory rateSelectionFactory) : this()
        {
            this.shipmentPackageBuilderFactory = shipmentPackageBuilderFactory;
            this.rateSelectionFactory = rateSelectionFactory;
            this.shipmentServicesBuilderFactory = shipmentServicesBuilderFactory;

            subscription = messenger.OfType<SelectedRateChangedMessage>().Subscribe(HandleSelectedRateChangedMessage);
        }

        [Obfuscation(Exclude = true)]
        public ObservableCollection<KeyValuePair<int, string>> Services { get; }

        [Obfuscation(Exclude = true)]
        public ObservableCollection<PackageTypeBinding> PackageTypes { get; }

        [Obfuscation(Exclude = true)]
        public DateTime ShipDate
        {
            get { return shipDate; }
            set { handler.Set(nameof(ShipDate), ref shipDate, value); }
        }

        [Obfuscation(Exclude = true)]
        public double TotalWeight
        {
            get { return totalWeight; }
            set { handler.Set(nameof(TotalWeight), ref totalWeight, value); }
        }

        [Obfuscation(Exclude = true)]
        public bool UsingInsurance
        {
            get { return usingInsurance; }
            set { handler.Set(nameof(UsingInsurance), ref usingInsurance, value); }
        }

        [Obfuscation(Exclude = true)]
        public int ServiceType
        {
            get { return serviceType; }
            set { handler.Set(nameof(ServiceType), ref serviceType, value, true); }
        }

        [Obfuscation(Exclude = true)]
        public IEnumerable<int> PackageCountList
        {
            get { return Enumerable.Range(1, 25); }
        }

        [Obfuscation(Exclude = true)]
        public int NumberOfPackages
        {
            get { return numberOfPackages; }
            set { handler.Set(nameof(NumberOfPackages), ref numberOfPackages, value, true); }
        }

        [Obfuscation(Exclude = true)]
        public IEnumerable<IPackageAdapter> PackageAdapters
        {
            get { return packageAdapters; }
            set { handler.Set(nameof(PackageAdapters), ref packageAdapters, value, true); }
        }

        [Obfuscation(Exclude = true)]
        public IPackageAdapter SelectedPackageAdapter
        {
            get { return selectedPackageAdapter; }
            set { handler.Set(nameof(SelectedPackageAdapter), ref selectedPackageAdapter, value, true); }
        }

        [Obfuscation(Exclude = true)]
        public bool SupportsMultiplePackages
        {
            get { return supportsMultiplePackages; }
            set { handler.Set(nameof(SupportsMultiplePackages), ref supportsMultiplePackages, value); }
        }

        [Obfuscation(Exclude = true)]
        public bool SupportsPackageTypes
        {
            get { return supportsPackageTypes; }
            set { handler.Set(nameof(SupportsPackageTypes), ref supportsPackageTypes, value); }
        }

        /// <summary>
        /// Load the shipment
        /// </summary>
        public virtual void Load(ICarrierShipmentAdapter shipmentAdapter)
        {
            ShipDate = shipmentAdapter.ShipDate;
            TotalWeight = shipmentAdapter.TotalWeight;
            UsingInsurance = shipmentAdapter.UsingInsurance;
            SupportsPackageTypes = shipmentAdapter.SupportsPackageTypes;
            SupportsMultiplePackages = shipmentAdapter.SupportsMultiplePackages;

            RefreshServiceTypes(shipmentAdapter);
            RefreshPackageTypes(shipmentAdapter);

            PackageAdapters = shipmentAdapter.GetPackageAdapters();
            NumberOfPackages = PackageAdapters.Count();

            SelectedPackageAdapter = PackageAdapters.FirstOrDefault();

        }

        /// <summary>
        /// Refreshes the shipment types.
        /// </summary>
        public virtual void RefreshServiceTypes(ICarrierShipmentAdapter shipmentAdapter)
        {
            Dictionary<int, string> services;
            try
            {
                services = shipmentServicesBuilderFactory.Get(shipmentAdapter.ShipmentTypeCode)
                    .BuildServiceTypeDictionary(new[] { shipmentAdapter.Shipment });
            }
            catch (InvalidRateGroupShippingException)
            {
                Services.Add(new KeyValuePair<int, string>(shipmentAdapter.ServiceType, "Error getting service types."));
                return;
            }

            Services.Clear();

            foreach (KeyValuePair<int, string> entry in services)
            {
                Services.Add(entry);
            }

            ServiceType = shipmentAdapter.ServiceType;
        }


        /// <summary>
        /// Refreshes the package types.
        /// </summary>
        public virtual void RefreshPackageTypes(ICarrierShipmentAdapter shipmentAdapter)
        {
            Dictionary<int, string> packageTypes = shipmentPackageBuilderFactory.Get(shipmentAdapter.ShipmentTypeCode)
                   .BuildPackageTypeDictionary(new[] { shipmentAdapter.Shipment });

            PackageTypes.Clear();
            foreach (KeyValuePair<int, string> entry in packageTypes)
            {
                PackageTypes.Add(new PackageTypeBinding()
                {
                    PackageTypeID = entry.Key,
                    Name = entry.Value
                });
            }
        }

        /// <summary>
        /// Save UI values to the shipment
        /// </summary>
        public virtual void Save(ICarrierShipmentAdapter shipmentAdapter)
        {
            shipmentAdapter.ShipDate = ShipDate;
            // shipmentAdapter.TotalWeight = TotalWeight
            shipmentAdapter.UsingInsurance = UsingInsurance;
            shipmentAdapter.ServiceType = ServiceType;
        }

        /// <summary>
        /// Called when the shipment service type has changed.
        /// </summary>
        private void HandleSelectedRateChangedMessage(SelectedRateChangedMessage message)
        {
            IRateSelection rateSelection = rateSelectionFactory.CreateRateSelection(message.RateResult);

            // Set the newly selected service type
            ServiceType = rateSelection.ServiceType;
        }

        /// <summary>
        /// Dispose resources
        /// </summary>
        public void Dispose()
        {
            subscription.Dispose();
        }
    }
}