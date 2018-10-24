using System.Collections.Generic;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Represents a factory for creating OrderLookupPanels
    /// </summary>
    public interface IOrderLookupPanelFactory
    {
        /// <summary>
        /// Get a collection of IOrderLookupPanelViewModels
        /// </summary>
        IEnumerable<IOrderLookupPanelViewModel<T>> GetPanels<T>() where T : IOrderLookupViewModel;
    }
}
