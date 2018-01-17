﻿using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.ApplicationCode;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Shipping.Carriers.Amazon.Api;
using ShipWorks.Shipping.Carriers.Amazon.RateGroupFilters;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Services.Builders;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.UI.Carriers.Amazon
{
    /// <summary>
    /// Service registrations for the Amazon shipping carrier
    /// </summary>
    public class AmazonShippingModule : Module
    {
        /// <summary>
        /// Load the registrations
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<AmazonCarrierTermsAndConditionsNotAcceptedFootnoteViewModel>()
                .AsImplementedInterfaces()
                .ExternallyOwned();

            builder.RegisterType<AmazonSettingsControl>()
                .Keyed<SettingsControlBase>(ShipmentTypeCode.Amazon)
                .InstancePerLifetimeScope();

            builder.RegisterType<AmazonShipmentType>()
                .AsSelf()
                .Keyed<ShipmentType>(ShipmentTypeCode.Amazon)
                .SingleInstance();

            builder.RegisterType<AmazonServiceControl>()
                .Keyed<ServiceControlBase>(ShipmentTypeCode.Amazon)
                .ExternallyOwned();

            builder.RegisterType<AmazonRatingService>()
                .Keyed<IRatingService>(ShipmentTypeCode.Amazon);

            builder.RegisterType<AmazonMwsWebClientSettingsFactory>()
                .As<IAmazonMwsWebClientSettingsFactory>();

            builder.RegisterType<AmazonProfileControl>()
                .Keyed<ShippingProfileControlBase>(ShipmentTypeCode.Amazon);

            builder.RegisterType<AmazonShipmentRequestDetailsFactory>()
                .As<IAmazonShipmentRequestDetailsFactory>();

            builder.RegisterType<AmazonAccountValidator>()
                .AsImplementedInterfaces();

            builder.RegisterType<AmazonShipmentAdapter>()
                .Keyed<ICarrierShipmentAdapter>(ShipmentTypeCode.Amazon)
                .ExternallyOwned();

            builder.RegisterType<AmazonShipmentServicesBuilder>()
                .Keyed<IShipmentServicesBuilder>(ShipmentTypeCode.Amazon)
                .FindConstructorsWith(new NonDefaultConstructorFinder())
                .SingleInstance();

            builder.RegisterType<AmazonRateHashingService>()
                .Keyed<IRateHashingService>(ShipmentTypeCode.Amazon)
                .AsSelf();

            builder.RegisterType<AmazonRateGroupFactory>()
                .AsImplementedInterfaces();
        }
    }
}
