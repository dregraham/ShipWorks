﻿using Autofac;
using Autofac.Features.Indexed;
using ShipWorks.Shipping.Services.Telemetry;

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
                (c, inner) => new TelemetricLabelRetrievalStep(inner, c.Resolve<ICarrierShipmentAdapterFactory>(), 
                    c.Resolve<IIndex<ShipmentTypeCode, ICarrierTelemetryMutator>>()), "LabelRetrievalStep");
        }
    }
}