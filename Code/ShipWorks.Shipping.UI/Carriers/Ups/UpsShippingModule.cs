using Autofac;
using Autofac.Core;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.InvoiceRegistration;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using ShipWorks.Shipping.Carriers.UPS.OpenAccount.Api;
using ShipWorks.Shipping.Carriers.UPS.Promo;
using ShipWorks.Shipping.Carriers.UPS.Promo.Api;
using ShipWorks.Shipping.Carriers.UPS.ServiceManager;
using ShipWorks.Shipping.Carriers.UPS.UpsEnvironment;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Services.Builders;
using System;
using System.Collections.Concurrent;


namespace ShipWorks.Shipping.UI.Carriers.Ups
{
    /// <summary>
    /// Service registrations for the Ups shipping carrier
    /// </summary>
    public class UpsShippingModule : Module
    {
        /// <summary>
        /// Load the module configuration
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

           builder.RegisterType<UpsShipmentServicesBuilder>()
                .Keyed<IShipmentServicesBuilder>(ShipmentTypeCode.UpsOnLineTools)
                .Keyed<IShipmentServicesBuilder>(ShipmentTypeCode.UpsWorldShip)
                .SingleInstance();

            builder.RegisterType<UpsServiceManagerFactory>()
                .AsImplementedInterfaces();

            builder.RegisterType<UpsAccountRepository>()
                .AsImplementedInterfaces()
                .Keyed<ICarrierAccountRetriever<ICarrierAccount>>(ShipmentTypeCode.UpsOnLineTools)
                .Keyed<ICarrierAccountRetriever<ICarrierAccount>>(ShipmentTypeCode.UpsWorldShip)
                .SingleInstance();

            builder.RegisterType<UpsShipmentAdapter>()
                .Keyed<ICarrierShipmentAdapter>(ShipmentTypeCode.UpsOnLineTools)
                .Keyed<ICarrierShipmentAdapter>(ShipmentTypeCode.UpsWorldShip)
                .ExternallyOwned();

            builder.RegisterType<UpsAccountRepository>()
                .Keyed<ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity>>(ShipmentTypeCode.UpsOnLineTools);

            builder.RegisterType<UpsRateHashingService>()
                .Keyed<IRateHashingService>(ShipmentTypeCode.UpsOnLineTools)
                .Keyed<IRateHashingService>(ShipmentTypeCode.UpsWorldShip);

            builder.RegisterType<UpsApiTransitTimeClient>();

            builder.RegisterType<UpsApiRateClient>();

            RegisterRatingServiceFor(ShipmentTypeCode.UpsOnLineTools, builder);
            RegisterRatingServiceFor(ShipmentTypeCode.UpsWorldShip, builder);

            builder.RegisterType<UpsClerk>()
                .AsImplementedInterfaces();

            builder.RegisterType<UpsResponseFactory>()
                .Keyed<ICarrierResponseFactory>(ShipmentTypeCode.UpsOnLineTools)
                .Keyed<ICarrierResponseFactory>(ShipmentTypeCode.UpsWorldShip);

            builder.RegisterType<UpsSettingsRepository>()
                .AsSelf()
                .Keyed<ICarrierSettingsRepository>(ShipmentTypeCode.UpsOnLineTools)
                .Keyed<ICarrierSettingsRepository>(ShipmentTypeCode.UpsWorldShip);

            builder.RegisterType<ConcurrentDictionary<long, DateTime>>()
                .UsingConstructor()
                .AsSelf();
			
            builder.RegisterType<UpsServiceGateway>()
                .AsImplementedInterfaces();

            builder.RegisterType<UpsOpenAccountRequestFactory>()
                .AsImplementedInterfaces();

            builder.RegisterType<UpsInvoiceRegistrationRequestFactory>()
                .AsImplementedInterfaces();

            builder.RegisterType<UpsShipmentTypePreProcessor>().AsSelf();

            RegisterOltSpecificTypes(builder);
            RegisterWorldShipSpecificTypes(builder);
            RegisterPromoTypes(builder);

        }

        /// <summary>
        /// Registers the olt specific classes.
        /// </summary>
        private void RegisterOltSpecificTypes(ContainerBuilder builder)
        {
            builder.RegisterType<UpsOltShipmentType>()
                .AsSelf()
                .Keyed<ShipmentType>(ShipmentTypeCode.UpsOnLineTools);

            builder.RegisterType<UpsOltShipmentPackageTypesBuilder>()
                .Keyed<IShipmentPackageTypesBuilder>(ShipmentTypeCode.UpsOnLineTools)
                .SingleInstance();

            builder.RegisterType<UpsOltLabelService>()
                .Keyed<ILabelService>(ShipmentTypeCode.UpsOnLineTools);

            builder.RegisterType<UpsOltShipmentValidator>()
                .AsImplementedInterfaces();
        }

        /// <summary>
        /// Registers the world ship specific classes.
        /// </summary>
        private void RegisterWorldShipSpecificTypes(ContainerBuilder builder)
        {
            builder.RegisterType<WorldShipShipmentType>()
                .AsSelf()
                .Keyed<ShipmentType>(ShipmentTypeCode.UpsWorldShip);

            builder.RegisterType<WorldShipShipmentPackageTypesBuilder>()
                .Keyed<IShipmentPackageTypesBuilder>(ShipmentTypeCode.UpsWorldShip)
                .SingleInstance();

            builder.RegisterType<WorldShipLabelService>()
                .Keyed<ILabelService>(ShipmentTypeCode.UpsWorldShip);

            builder.RegisterType<WorldShipPackageImporter>().AsSelf();
        }

        /// <summary>
        /// Registers the promo types.
        /// </summary>
        private void RegisterPromoTypes(ContainerBuilder builder)
        {
            builder.RegisterType<UpsPromoWebClientFactory>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<UpsPromoPolicy>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<UpsPromoFactory>()
                .AsImplementedInterfaces();

            builder.RegisterType<UpsPromoFootnoteViewModel>()
                .AsImplementedInterfaces();
        }

        /// <summary>
        /// Register the rating service for the given shipment type
        /// </summary>
        private void RegisterRatingServiceFor(ShipmentTypeCode shipmentType, ContainerBuilder builder)
        {
            builder.RegisterType<UpsRatingService>()
                .Keyed<IRatingService>(shipmentType)
                .WithParameter(new ResolvedParameter(
                    (parameters, _) => parameters.ParameterType == typeof(UpsShipmentType),
                    (_, context) => context.ResolveKeyed<ShipmentType>(shipmentType)));
        }

    }
}