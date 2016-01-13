﻿using Autofac;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.WebTools;

namespace ShipWorks.Shipping.UI.Carriers.Postal.WebTools
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

            builder.RegisterType<WebToolsLabelService>()
                .Keyed<ILabelService>(ShipmentTypeCode.PostalWebTools);

            builder.RegisterType<WebToolsRatingService>()
                .Keyed<IRatingService>(ShipmentTypeCode.PostalWebTools);

            builder.RegisterType<PostalRateHashingService>()
                .Keyed<IRateHashingService>(ShipmentTypeCode.PostalWebTools);
        }
    }
}
