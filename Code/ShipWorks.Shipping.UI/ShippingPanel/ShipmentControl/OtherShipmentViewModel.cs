using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.UI.ShippingPanel.ShipmentControl
{
    /// <summary>
    /// View model for use by ShipmentControl
    /// </summary>
    public partial class OtherShipmentViewModel : IShipmentViewModel, INotifyPropertyChanged, INotifyPropertyChanging
    {
        private readonly PropertyChangedHandler handler;

        [SuppressMessage("SonarQube", "S2290:Field-like events should not be virtual",
            Justification = "Event is virtual to allow tests to fire it")]
        public virtual event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        /// <summary>
        /// Constructor for use by tests and WPF designer
        /// </summary>
        public OtherShipmentViewModel()
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged, () => PropertyChanging);
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

            OtherShipmentEntity otherShipment = shipmentAdapter.Shipment.Other;

            Debug.Assert(otherShipment != null);

            CarrierName = otherShipment.Carrier;
            Service = otherShipment.Service;
            Cost = shipmentAdapter.Shipment.ShipmentCost;
            TrackingNumber = shipmentAdapter.Shipment.TrackingNumber;
        }

        /// <summary>
        /// Save UI values to the shipment
        /// </summary>
        public virtual void Save()
        {
            shipmentAdapter.ShipDate = ShipDate;
            shipmentAdapter.Shipment.TotalWeight = TotalWeight;
            shipmentAdapter.UsingInsurance = UsingInsurance;

            shipmentAdapter.Shipment.ShipmentCost = Cost;
            shipmentAdapter.Shipment.TrackingNumber = TrackingNumber;

            OtherShipmentEntity otherShipment = shipmentAdapter.Shipment.Other;
            Debug.Assert(otherShipment != null);

            otherShipment.Carrier = CarrierName;
            otherShipment.Service = Service;
        }

        /// <summary>
        /// Dispose of any held resources
        /// </summary>
        public void Dispose()
        {
            // No resources to dispose
        }

        /// <summary>
        /// Updates the services
        /// </summary>
        public void RefreshServiceTypes()
        {
            // Other shipment type does not support services
        }

        /// <summary>
        /// Updates the packages
        /// </summary>
        public void RefreshPackageTypes()
        {
            // Other shipment type does not support packages
        }
    }
}