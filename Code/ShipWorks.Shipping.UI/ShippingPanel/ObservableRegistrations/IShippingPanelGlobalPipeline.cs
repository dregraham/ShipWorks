using System;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations
{
    /// <summary>
    /// Register an observable pipeline for the shipping panel view model
    /// </summary>
    [Service]
    public interface IShippingPanelGlobalPipeline
    {
        /// <summary>
        /// Register the pipeline on the view model
        /// </summary>
        IDisposable Register(ShippingPanelViewModel viewModel);
    }
}
