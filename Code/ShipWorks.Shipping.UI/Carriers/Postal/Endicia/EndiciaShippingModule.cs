﻿using Autofac;
using ShipWorks.Shipping.Carriers.Postal.Endicia;

namespace ShipWorks.Shipping.Carriers.Endicia
{
    public class EndiciaShippingModule : Module
    {
        /// <summary>
        /// Load the module configuration
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EndiciaShipmentType>()
                .Keyed<ShipmentType>(ShipmentTypeCode.Endicia);
        }
    }
}
