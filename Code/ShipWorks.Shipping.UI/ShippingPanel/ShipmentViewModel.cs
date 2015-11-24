using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using ShipWorks.Core.Messaging;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping.Rating;
using ShipWorks.Shipping.Services.Builders;
using System.Reactive.Linq;
using ShipWorks.Shipping.Services;

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
        private readonly IRateSelectionFactory rateSelectionFactory;
        private readonly IDisposable subscription;

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
            Packages = new ObservableCollection<KeyValuePair<int, string>>();
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

        public ObservableCollection<KeyValuePair<int, string>> Services { get; }

        public ObservableCollection<KeyValuePair<int,string>> Packages { get; }

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

        /// <summary>
        /// Load the shipment
        /// </summary>
        public virtual void Load(ICarrierShipmentAdapter shipmentAdapter)
        {
            ShipDate = shipmentAdapter.ShipDate;
            TotalWeight = shipmentAdapter.TotalWeight;
            UsingInsurance = shipmentAdapter.UsingInsurance;
            RefreshServiceTypes(shipmentAdapter);
            RefreshPackageTypes(shipmentAdapter);
        }

        /// <summary>
        /// Refreshes the shipment types.
        /// </summary>
        public virtual void RefreshServiceTypes(ICarrierShipmentAdapter shipmentAdapter)
        {
            Dictionary<int, string> services = shipmentServicesBuilderFactory.Get(shipmentAdapter.ShipmentTypeCode)
                .BuildServiceTypeDictionary(new [] { shipmentAdapter.Shipment });

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
            //Dictionary<int, string> packageTypes = shipmentPackageBuilderFactory.Get(shipmentAdapter.ShipmentTypeCode)
            //       .BuildPackageTypeDictionary(new[] { shipmentAdapter.Shipment });

            //Packages.Clear();
            //foreach (KeyValuePair<int, string> entry in packageTypes)
            //{
            //    Packages.Add(entry);
            //}
        }

        /// <summary>
        /// Save UI values to the shipment
        /// </summary>
        public virtual void Save(ICarrierShipmentAdapter shipmentAdapter)
        {
            shipmentAdapter.ShipDate = ShipDate;
            //shipmentAdapter.TotalWeight = TotalWeight;
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