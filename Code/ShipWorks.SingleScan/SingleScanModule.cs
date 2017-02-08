using Autofac;
using ShipWorks.Shipping.Services;
using Module = Autofac.Module;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Registrations for the SingleScan assembly
    /// </summary>
    /// <seealso cref="Autofac.Module" />
    public class SingleScanModule : Module
    {
        /// <summary>
        /// Override to add registrations to the container.
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterDecorator<ISingleScanOrderConfirmationService>(
                (c, inner) => new TelemetricSingleScanOrderConfirmationService(inner),
                nameof(SingleScanOrderConfirmationService));
        }
    }
}