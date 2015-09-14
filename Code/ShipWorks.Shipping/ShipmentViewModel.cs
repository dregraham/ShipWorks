using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        }

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
        public void Load(ShipmentEntity shipment)
        {
            ShipDate = shipment.ShipDate;
            TotalWeight = shipment.TotalWeight;
            Insurance = shipment.Insurance;
        }
    }
}