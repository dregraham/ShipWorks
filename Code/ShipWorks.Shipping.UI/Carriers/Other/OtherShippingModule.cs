﻿using Autofac;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Other;
using ShipWorks.Shipping.Carriers.Postal.Other;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Services.Builders;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.UI.Carriers.Other
{
    /// <summary>
    /// IoC registration module for the Other shipment type
    /// </summary>
    public class OtherShippingModule : Module
    {
        /// <summary>
        /// Load the module configuration
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<OtherShipmentType>()
                .AsSelf()
                .Keyed<ShipmentType>(ShipmentTypeCode.Other);

            builder.RegisterType<OtherServiceControl>()
                .Keyed<ServiceControlBase>(ShipmentTypeCode.Other);

            builder.RegisterType<OtherProfileControl>()
                .Keyed<ShippingProfileControlBase>(ShipmentTypeCode.Other);

            builder.RegisterType<OtherShipmentAdapter>()
                .Keyed<ICarrierShipmentAdapter>(ShipmentTypeCode.Other)
                .ExternallyOwned();

            builder.RegisterType<OtherLabelService>()
                .Keyed<ILabelService>(ShipmentTypeCode.Other);

            builder.RegisterType<EmptyRatingService>()
                .Keyed<IRatingService>(ShipmentTypeCode.Other);

            builder.RegisterType<EmptyRateHashingService>()
                .Keyed<IRateHashingService>(ShipmentTypeCode.Other);
        }
    }
}
