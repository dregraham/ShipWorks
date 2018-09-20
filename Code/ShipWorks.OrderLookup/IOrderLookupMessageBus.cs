using System.ComponentModel;
using System.Reflection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Represents the Order Lookup Data Service
    /// </summary>
    public interface IOrderLookupMessageBus
    {
        /// <summary>
        /// The order that's in context
        /// </summary>
        [Obfuscation(Exclude = true)]
        OrderEntity Order { get; }

        /// <summary>
        /// Doss the Shipment allow editing
        /// </summary>
        bool ShipmentAllowEditing { get; }

        /// <summary>
        /// The orders shipment adapter
        /// </summary>
        ICarrierShipmentAdapter ShipmentAdapter { get; }

        /// <summary>
        /// Event raised when an order property changes
        /// </summary>
        event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raise property changed event when an order property changes
        /// </summary>
        void RaisePropertyChanged(string propertyName);
    }
}