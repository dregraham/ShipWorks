﻿using Autofac;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Shipping.Carriers.Postal.WebTools;

namespace ShipWorks.Shipping.Carriers.WebTools
{
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

            builder.RegisterType<PostalWebShipmentServicesBuilder>()
                .Keyed<IShipmentServicesBuilder>(ShipmentTypeCode.PostalWebTools)
                .SingleInstance();

            builder.RegisterType<NullAccountRepository>()
                .Keyed<ICarrierAccountRepository<ICarrierAccount>>(ShipmentTypeCode.PostalWebTools)
                .SingleInstance();
        }
    }
}
