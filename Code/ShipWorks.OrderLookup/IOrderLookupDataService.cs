using System.ComponentModel;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Represents the Order Lookup Data Service
    /// </summary>
    [Service]
    public interface IOrderLookupDataService
    {
        /// <summary>
        /// The order thats in context
        /// </summary>
        OrderEntity Order { get; }

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