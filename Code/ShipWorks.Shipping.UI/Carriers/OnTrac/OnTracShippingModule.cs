﻿using Autofac;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Services.Builders;

namespace ShipWorks.Shipping.Carriers.OnTrac
{
    public class OnTracShippingModule : Module
    {
        /// <summary>
        /// Load the module configuration
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<OnTracShipmentType>()
                .AsSelf()
                .Keyed<ShipmentType>(ShipmentTypeCode.OnTrac);

            builder.RegisterType<OnTracShipmentServicesBuilder>()
                .Keyed<IShipmentServicesBuilder>(ShipmentTypeCode.OnTrac)
                .SingleInstance();

            builder.RegisterType<OnTracAccountRepository>()
                .Keyed<ICarrierAccountRetriever<ICarrierAccount>>(ShipmentTypeCode.OnTrac)
                .SingleInstance();

            builder.RegisterType<OnTracShipmentAdapter>()
                .Keyed<ICarrierShipmentAdapter>(ShipmentTypeCode.OnTrac)
                .ExternallyOwned();

            builder.RegisterType<OnTracShipmentPackageTypesBuilder>()
                .Keyed<IShipmentPackageTypesBuilder>(ShipmentTypeCode.OnTrac)
                .SingleInstance();
        }
    }
}
