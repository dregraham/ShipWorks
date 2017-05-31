using System;
using Autofac;
using Autofac.Core;
using Autofac.Features.Indexed;
using Interapptive.Shared.Metrics;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Shipping.Configuration;

namespace ShipWorks.Shipping.Settings
{
    /// <summary>
    /// Factory to create shipment type setup wizards
    /// </summary>
    [Component]
    public class ShipmentTypeSetupWizardFactory : IShipmentTypeSetupWizardFactory
    {
        private readonly Func<IShipmentTypeSetupWizard, ShipmentTypeCode, ConfigurationShipmentTypeSetupWizard> wrapWithConfiguration;
        private readonly Func<IShipmentTypeSetupWizard, ShipmentTypeCode, OpenedFromSource, TelemetricShipmentTypeSetupWizardForm> wrapWithTelemetry;
        private readonly ILifetimeScope lifetimeScope;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentTypeSetupWizardFactory(ILifetimeScope lifetimeScope,
            Func<IShipmentTypeSetupWizard, ShipmentTypeCode, ConfigurationShipmentTypeSetupWizard> wrapWithConfiguration,
            Func<IShipmentTypeSetupWizard, ShipmentTypeCode, OpenedFromSource, TelemetricShipmentTypeSetupWizardForm> wrapWithTelemetry)
        {
            this.wrapWithConfiguration = wrapWithConfiguration;
            this.lifetimeScope = lifetimeScope;
            this.wrapWithTelemetry = wrapWithTelemetry;
        }

        /// <summary>
        /// Create a wizard for the given shipment type
        /// </summary>
        public IShipmentTypeSetupWizard Create(ShipmentTypeCode shipmentType, OpenedFromSource openedFrom, params Parameter[] parameters)
        {
            IShipmentTypeSetupWizard wizard = lifetimeScope.ResolveKeyed<IShipmentTypeSetupWizard>(shipmentType, parameters);
            IShipmentTypeSetupWizard wizardWithConfiguration = wrapWithConfiguration(wizard, shipmentType);
            return wrapWithTelemetry(wizardWithConfiguration, shipmentType, openedFrom);
        }
    }
}
