using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace ShipWorks.Shipping.UI
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
        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentViewModel()
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged, () => PropertyChanging);
            Services = new ObservableCollection<KeyValuePair<int, string>>();
            Packages = new ObservableCollection<KeyValuePair<int, string>>();
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
        public void Load(ShipmentType shipmentType, ShipmentEntity shipment)
        {
            ShipDate = shipment.ShipDate;
            TotalWeight = shipment.TotalWeight;
            Insurance = shipment.Insurance;
            RefreshShipmentTypes(shipmentType, shipment);
            RefreshPackageTypes(shipmentType, shipment);
        }

        /// <summary>
        /// Refreshes the shipment types.
        /// </summary>
        public void RefreshShipmentTypes(ShipmentType shipmentType, ShipmentEntity shipment)
        {
            Dictionary<int, string> services = shipmentType.BuildServiceTypeDictionary(new List<ShipmentEntity> { shipment });
            Services.Clear();
            
            foreach (KeyValuePair<int, string> entry in services)
            {
                Services.Add(entry);
            }
        }

        /// <summary>
        /// Refreshes the package types.
        /// </summary>
        public void RefreshPackageTypes(ShipmentType shipmentType, ShipmentEntity shipment)
        {
            Dictionary<int, string> packages = shipmentType.BuildPackageTypeDictionary(new List<ShipmentEntity> { shipment });
            Packages.Clear();

            foreach (KeyValuePair<int, string> entry in packages)
            {
                Packages.Add(entry);
            }
        }

        /// <summary>
        /// Save UI values to the shipment
        /// </summary>
        public void Save(ShipmentEntity shipment)
        {
            shipment.ShipDate = ShipDate;
            shipment.TotalWeight = TotalWeight;
            shipment.Insurance = Insurance;
        }
    }
}