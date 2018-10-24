using System.ComponentModel;
using Autofac;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Basic Order Lookup view model
    /// </summary>
    public interface IOrderLookupPanelViewModel<out T> : INotifyPropertyChanged where T : IOrderLookupViewModel
    {
        /// <summary>
        /// Carrier specific context
        /// </summary>
        T Context { get; }

        /// <summary>
        /// Update the view model with new inner view models if necessary
        /// </summary>
        void UpdateViewModel(IOrderLookupShipmentModel shipmentModel, ILifetimeScope innerScope);
    }
}
