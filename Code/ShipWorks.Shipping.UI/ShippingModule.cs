﻿using Autofac;

namespace ShipWorks.Shipping.UI
{
    /// <summary>
    /// IoC registration module for this assembly
    /// </summary>
    public class ShippingModule : Module
    {
        /// <summary>
        /// Load the module configuration
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ShipmentPanelRegistration>()
                .AsImplementedInterfaces()
                .PreserveExistingDefaults();

            builder.RegisterType<RatingPanelRegistration>()
                .AsImplementedInterfaces()
                .PreserveExistingDefaults();

            builder.RegisterType<ShippingPanelViewModel>();

            builder.RegisterType<RatingPanelViewModel>();

            builder.RegisterType<ShippingPanelConfigurator>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<ShipmentTypeProvider>();

            builder.RegisterType<ShipmentLoader>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<ShippingPanelShipmentLoader>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<ShipmentAddressValidator>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<ShipmentProcessor>()
                .AsImplementedInterfaces();
        }
    }
}
