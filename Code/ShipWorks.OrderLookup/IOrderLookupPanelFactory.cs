using System.Collections.Generic;
using Autofac;

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
        IEnumerable<IOrderLookupPanelViewModel<IOrderLookupViewModel>> GetPanels(ILifetimeScope scope);
    }
}
