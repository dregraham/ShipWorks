using System;
using ShipWorks.ApplicationCore.ComponentRegistration;

namespace ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations
{
    /// <summary>
    /// Register an observable pipeline for the shipping panel view model
    /// </summary>
    [Service]
    public interface IShippingPanelTransientPipeline : IDisposable
    {
        /// <summary>
        /// Register the pipeline on the view model
        /// </summary>
        void Register(ShippingPanelViewModel viewModel);
    }
}
