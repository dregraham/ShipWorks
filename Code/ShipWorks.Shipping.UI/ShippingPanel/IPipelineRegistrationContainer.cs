using System;

namespace ShipWorks.Shipping.UI.ShippingPanel
{
    /// <summary>
    /// Manage shipping panel pipeline registrations
    /// </summary>
    public interface IPipelineRegistrationContainer : IDisposable
    {
        /// <summary>
        /// Register the global pipelines
        /// </summary>
        void RegisterGlobal(ShippingPanelViewModel viewModel);

        /// <summary>
        /// Register transient pipelines
        /// </summary>
        void RegisterTransient(ShippingPanelViewModel viewModel);

        /// <summary>
        /// Discard any transient pipelines
        /// </summary>
        void DiscardTransient();
    }
}