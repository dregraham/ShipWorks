using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;

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
        
        private readonly PropertyChangedHandler handler;
        private readonly IShipmentServicesBuilderFactory shipmentServicesBuilderFactory;

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
        public ShipmentViewModel(IShipmentServicesBuilderFactory shipmentServicesBuilderFactory) : this()
        {
            this.shipmentServicesBuilderFactory = shipmentServicesBuilderFactory;
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

        /// <summary>
        /// Load the shipment
        /// </summary>
        public virtual void Load(ShipmentEntity shipment)
        {
            ShipDate = shipment.ShipDate;
            TotalWeight = shipment.TotalWeight;
            Insurance = shipment.Insurance;
            RefreshShipmentTypes(shipment);
            RefreshPackageTypes(shipment);
        }

        /// <summary>
        /// Refreshes the shipment types.
        /// </summary>
        public virtual void RefreshShipmentTypes(ShipmentEntity shipment)
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
            //TODO: Replace with the package factory that should be coming soon
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
    }
}