using System;
using System.Collections.ObjectModel;
using System.Reflection;
using GalaSoft.MvvmLight.Command;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.UI.ShippingPanel.ShipmentControl
{
    /// <summary>
    /// View model for use by ShipmentControl
    /// </summary>
    public partial class OtherShipmentViewModel
    {
        private DateTime shipDate;
        private double totalWeight;
        private bool usingInsurance;

        private string carrierName;
        private string service;
        private decimal cost;
        private string trackingNumber;

        private ICarrierShipmentAdapter shipmentAdapter;

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
        /// Name of the carrier used
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string CarrierName
        {
            get { return carrierName; }
            set { handler.Set(nameof(CarrierName), ref carrierName, value); }
        }

        /// <summary>
        /// Name of the service used
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Service
        {
            get { return service; }
            set { handler.Set(nameof(Service), ref service, value); }
        }

        /// <summary>
        /// Cost of the shipment
        /// </summary>
        [Obfuscation(Exclude = true)]
        public decimal Cost
        {
            get { return cost; }
            set { handler.Set(nameof(Cost), ref cost, value); }
        }

        /// <summary>
        /// Tracking number
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string TrackingNumber
        {
            get { return trackingNumber; }
            set { handler.Set(nameof(TrackingNumber), ref trackingNumber, value); }
        }
    }
}
