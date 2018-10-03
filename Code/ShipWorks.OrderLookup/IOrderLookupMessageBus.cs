using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Services;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Represents the Order Lookup Data Service
    /// </summary>
    public interface IViewModelOrchestrator
    {
        /// <summary>
        /// The order that's in context
        /// </summary>
        OrderEntity SelectedOrder { get; }

        /// <summary>
        /// Does the Shipment allow editing
        /// </summary>
        bool ShipmentAllowEditing { get; }

        /// <summary>
        /// The orders shipment adapter
        /// </summary>
        ICarrierShipmentAdapter ShipmentAdapter { get; }

        /// <summary>
        /// ShipmentType
        /// </summary>
        ShipmentTypeCode ShipmentTypeCode { get; set; }

        /// <summary>
        /// The pacakge adpaters for the order in context
        /// </summary>
        IEnumerable<IPackageAdapter> PackageAdapters { get; }

        /// <summary>
        /// Event raised when an order property changes
        /// </summary>
        event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raise property changed event when an order property changes
        /// </summary>
        void RaisePropertyChanged(string propertyName);

        /// <summary>
        /// Saves changes to the database
        /// </summary>
        void SaveToDatabase();
    }
}