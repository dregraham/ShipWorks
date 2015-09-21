using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Interapptive.Shared.Messaging;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Rating;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// View model for use by ShipmentControl
    /// </summary>
    public class ShipmentViewModel : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private DateTime shipDate;
        private double totalWeight;
        private bool insurance;
        private int serviceType;
        private readonly IRateSelectionFactory rateSelectionFactory;

        private readonly PropertyChangedHandler handler;
        private readonly IShipmentServicesBuilderFactory shipmentServicesBuilderFactory;
        private readonly IShipmentPackageBuilderFactory shipmentPackageBuilderFactory;

        private readonly IMessenger messenger;

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
        public ShipmentViewModel(IShipmentServicesBuilderFactory shipmentServicesBuilder, IShipmentPackageBuilderFactory shipmentPackageBuilderFactory)
        {
            this.shipmentPackageBuilderFactory = shipmentPackageBuilderFactory;
            this.rateSelectionFactory = rateSelectionFactory;
            this.shipmentServicesBuilderFactory = shipmentServicesBuilderFactory;
            this.messenger = messenger;

            messenger.Handle<SelectedRateChangedMessage>(this, HandleSelectedRateChangedMessage);
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
        public bool Insurance
        {
            get { return insurance; }
            set { handler.Set(nameof(Insurance), ref insurance, value); }
        }

        [Obfuscation(Exclude = true)]
        public int ServiceType
        {
            get { return serviceType; }
            set { handler.Set(nameof(ServiceType), ref serviceType, value); }
        }

        /// <summary>
        /// Load the shipment
        /// </summary>
        public virtual void Load(ShipmentEntity shipment)
        {
            ShipDate = shipment.ShipDate;
            TotalWeight = shipment.TotalWeight;
            Insurance = shipment.Insurance;
            RefreshServiceTypes(shipment);
            RefreshPackageTypes(shipment);
        }

        /// <summary>
        /// Refreshes the shipment types.
        /// </summary>
        public virtual void RefreshServiceTypes(ShipmentEntity shipment)
        {
            Dictionary<int, string> services = shipmentServicesBuilderFactory.Get(shipment.ShipmentTypeCode)
                .BuildServiceTypeDictionary(new [] { shipment });

            Services.Clear();
            
            foreach (KeyValuePair<int, string> entry in services)
            {
                Services.Add(entry);
            }
        }

        /// <summary>
        /// Refreshes the package types.
        /// </summary>
        public virtual void RefreshPackageTypes(ShipmentEntity shipment)
        {
            Dictionary<int, string> packageTypes = shipmentPackageBuilderFactory.Get(shipment.ShipmentTypeCode)
                   .BuildPackageTypeDictionary(new[] { shipment });

            Packages.Clear();
            foreach (KeyValuePair<int, string> entry in packageTypes)
            {
                Packages.Add(entry);
            }
        }

        /// <summary>
        /// Save UI values to the shipment
        /// </summary>
        public virtual void Save(ShipmentEntity shipment)
        {
            shipment.ShipDate = ShipDate;
            shipment.TotalWeight = TotalWeight;
            shipment.Insurance = Insurance;
        }

        /// <summary>
        /// Called when the shipment service type has changed.
        /// </summary>
        private void HandleSelectedRateChangedMessage(SelectedRateChangedMessage message)
        {
            IRateSelection rateSelection = rateSelectionFactory.CreateRateSelection(message.RateResult);

            // Set the newly selected service type
            this.ServiceType = rateSelection.ServiceType;
        }
    }
}