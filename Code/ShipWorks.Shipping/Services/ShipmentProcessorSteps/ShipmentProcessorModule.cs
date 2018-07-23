using Autofac;

namespace ShipWorks.Shipping.Services.ShipmentProcessorSteps
{
    public class ShipmentProcessorModule : Module
    {
        /// <summary>
        /// Override to add registrations to the container.
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterDecorator<ILabelRetrievalStep>(
                (c, inner) => new TelemetricLabelRetrievalStep(inner, c.Resolve<ICarrierShipmentAdapterFactory>()),
                "LabelRetrievalStep");
        }
    }
}