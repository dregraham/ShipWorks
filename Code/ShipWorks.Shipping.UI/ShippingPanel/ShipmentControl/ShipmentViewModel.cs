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

namespace ShipWorks.Shipping.UI.ShippingPanel.ShipmentControl
{
    /// <summary>
    /// View model for use by ShipmentControl
    /// </summary>
    public partial class ShipmentViewModel : INotifyPropertyChanged, INotifyPropertyChanging, IDisposable
    {
        private readonly IRateSelectionFactory rateSelectionFactory;
        private readonly IDisposable subscription;
        private readonly PropertyChangedHandler handler;
        private readonly IShipmentServicesBuilderFactory shipmentServicesBuilderFactory;
        private readonly IShipmentPackageTypesBuilderFactory shipmentPackageTypesBuilderFactory;

        [SuppressMessage("SonarQube", "S2290:Field-like events should not be virtual", Justification = "Event is virtual to allow tests to fire it")]
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
            IShipmentPackageTypesBuilderFactory shipmentPackageTypesBuilderFactory, IMessenger messenger,
            IRateSelectionFactory rateSelectionFactory) : this()
        {
            this.shipmentPackageTypesBuilderFactory = shipmentPackageTypesBuilderFactory;
            this.rateSelectionFactory = rateSelectionFactory;
            this.shipmentServicesBuilderFactory = shipmentServicesBuilderFactory;

            subscription = messenger.OfType<SelectedRateChangedMessage>().Subscribe(HandleSelectedRateChangedMessage);
        }

        /// <summary>
        /// Load the shipment
        /// </summary>
        public virtual void Load(ICarrierShipmentAdapter newShipmentAdapter)
        {
            shipmentAdapter = newShipmentAdapter;

            ShipDate = shipmentAdapter.ShipDate;
            TotalWeight = shipmentAdapter.TotalWeight;
            UsingInsurance = shipmentAdapter.UsingInsurance;
            SupportsPackageTypes = shipmentAdapter.SupportsPackageTypes;
            SupportsMultiplePackages = shipmentAdapter.SupportsMultiplePackages;

            RefreshServiceTypes();
            RefreshPackageTypes();

            PackageAdapters = shipmentAdapter.GetPackageAdapters();
            NumberOfPackages = PackageAdapters.Count();

            SelectedPackageAdapter = PackageAdapters.FirstOrDefault();
        }

        /// <summary>
        /// Refreshes the shipment types.
        /// </summary>
        public void RefreshServiceTypes()
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
        public void RefreshPackageTypes()
        {
            Dictionary<int, string> packageTypes = shipmentPackageTypesBuilderFactory.Get(shipmentAdapter.ShipmentTypeCode)
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
        public virtual void Save()
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