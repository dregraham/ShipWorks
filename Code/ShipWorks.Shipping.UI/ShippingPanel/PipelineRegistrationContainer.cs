using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using Autofac;
using ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations;

namespace ShipWorks.Shipping.UI.ShippingPanel
{
    /// <summary>
    /// Manage shipping panel pipeline registrations
    /// </summary>
    public class PipelineRegistrationContainer : IPipelineRegistrationContainer
    {
        private readonly ILifetimeScope lifetimeScope;
        private ILifetimeScope currentLifetimeScope;
        private IDisposable globalSubscriptions;

        /// <summary>
        /// Constructor
        /// </summary>
        public PipelineRegistrationContainer(ILifetimeScope lifetimeScope)
        {
            this.lifetimeScope = lifetimeScope;
        }

        /// <summary>
        /// Discard any transient pipelines
        /// </summary>
        public void DiscardTransient()
        {
            currentLifetimeScope?.Dispose();
            currentLifetimeScope = null;
        }

        /// <summary>
        /// Register the global pipelines
        /// </summary>
        public void RegisterGlobal(ShippingPanelViewModel viewModel)
        {
            globalSubscriptions = new CompositeDisposable(lifetimeScope
                .Resolve<IEnumerable<IShippingPanelGlobalPipeline>>()
                .Select(x => x.Register(viewModel)));
        }

        /// <summary>
        /// Register transient pipelines
        /// </summary>
        public void RegisterTransient(ShippingPanelViewModel viewModel)
        {
            DiscardTransient();

            currentLifetimeScope = lifetimeScope.BeginLifetimeScope();

            foreach (IShippingPanelTransientPipeline pipeline in currentLifetimeScope.Resolve<IEnumerable<IShippingPanelTransientPipeline>>())
            {
                pipeline.Register(viewModel);
            }
        }

        /// <summary>
        /// Dispose the container
        /// </summary>
        public void Dispose()
        {
            DiscardTransient();

            globalSubscriptions?.Dispose();
        }
    }
}
