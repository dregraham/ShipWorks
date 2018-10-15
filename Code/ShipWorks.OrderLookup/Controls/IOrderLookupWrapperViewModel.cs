using System;
using System.ComponentModel;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Basic Order Lookup view model
    /// </summary>
    public interface IOrderLookupWrapperViewModel<T> : INotifyPropertyChanged, IDisposable where T : IOrderLookupViewModel
    {
        /// <summary>
        /// Carrier specific context
        /// </summary>
        T Context { get; set; }
    }
}
