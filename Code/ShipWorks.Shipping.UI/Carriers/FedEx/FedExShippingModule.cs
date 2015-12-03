﻿using Autofac;
using Interapptive.Shared.Net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Shipping module for the FedEx carrier
    /// </summary>
    public class FedExShippingModule : Module
    {
        /// <summary>
        /// Load the module configuration
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FedExShipmentType>()
                .AsSelf()
                .Keyed<ShipmentType>(ShipmentTypeCode.FedEx);

            builder.RegisterType<FedExLabelService>()
                .Keyed<ILabelService>(ShipmentTypeCode.FedEx);

            builder.RegisterType<FedExRatingService>()
                .Keyed<IRatingService>(ShipmentTypeCode.FedEx);

            builder.RegisterType<FedExSettingsRepository>()
                .Keyed<ICarrierSettingsRepository>(ShipmentTypeCode.FedEx)
                .AsSelf();

            builder.RegisterType<FedExRateHashingService>()
                .AsSelf()
                .Keyed<IRateHashingService>(ShipmentTypeCode.FedEx);

            builder.RegisterType<FedExRateHashingService>()
                .Keyed<IRateHashingService>(ShipmentTypeCode.FedEx);

            builder.Register<IShippingClerk>((container, parameters) =>
            {
                return FedExShippingClerkFactory.CreateShippingClerk(
                    parameters.TypedAs<ShipmentEntity>(),
                    container.ResolveKeyed<ICarrierSettingsRepository>(ShipmentTypeCode.FedEx));
            });
        }
    }
}
