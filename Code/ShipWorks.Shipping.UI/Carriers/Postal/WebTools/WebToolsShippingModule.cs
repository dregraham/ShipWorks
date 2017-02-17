﻿using Autofac;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.WebTools;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Services.Builders;

namespace ShipWorks.Shipping.Carriers.WebTools
{
    /// <summary>
    /// Service registrations for the WebTools shipping carrier
    /// </summary>
    public class WebToolsShippingModule : Module
    {
        /// <summary>
        /// Load the module configuration
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PostalWebShipmentType>()
                .AsSelf()
                .Keyed<ShipmentType>(ShipmentTypeCode.PostalWebTools);

            builder.RegisterType<PostalWebToolsShipmentServicesBuilder>()
                .Keyed<IShipmentServicesBuilder>(ShipmentTypeCode.PostalWebTools)
                .SingleInstance();

            builder.RegisterType<PostalWebToolsShipmentAdapter>()
                .Keyed<ICarrierShipmentAdapter>(ShipmentTypeCode.PostalWebTools)
                .ExternallyOwned();

            builder.RegisterType<PostalWebToolsShipmentPackageTypesBuilder>()
                .Keyed<IShipmentPackageTypesBuilder>(ShipmentTypeCode.PostalWebTools)
                .SingleInstance();

            builder.RegisterType<WebToolsLabelService>()
                .Keyed<ILabelService>(ShipmentTypeCode.PostalWebTools);

            builder.RegisterType<WebToolsRatingService>()
                .Keyed<IRatingService>(ShipmentTypeCode.PostalWebTools);

            builder.RegisterType<PostalRateHashingService>()
                .Keyed<IRateHashingService>(ShipmentTypeCode.PostalWebTools);
        }
    }
}
