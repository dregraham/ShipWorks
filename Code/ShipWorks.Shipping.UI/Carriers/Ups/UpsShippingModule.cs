using Autofac;
using Autofac.Core;
using ShipWorks.Shipping.Api;
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
using Interapptive.Shared.Metrics;
using ShipWorks.Shipping.Carriers.Ups.LocalRating;
using ShipWorks.Shipping.Carriers.Ups.LocalRating.Validation;
using ShipWorks.Shipping.Carriers.UPS.BestRate;


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

            builder.RegisterType<UpsShipmentAdapter>()
                .Keyed<ICarrierShipmentAdapter>(ShipmentTypeCode.UpsOnLineTools)
                .Keyed<ICarrierShipmentAdapter>(ShipmentTypeCode.UpsWorldShip)
                .ExternallyOwned();

            builder.RegisterType<UpsRateHashingService>()
                .Keyed<IRateHashingService>(ShipmentTypeCode.UpsOnLineTools)
                .Keyed<IRateHashingService>(ShipmentTypeCode.UpsWorldShip);

            builder.RegisterType<UpsApiTransitTimeClient>();

            builder.RegisterType<UpsClerk>()
                .AsImplementedInterfaces();

            builder.RegisterType<UpsResponseFactory>()
                .Keyed<ICarrierResponseFactory>(ShipmentTypeCode.UpsOnLineTools)
                .Keyed<ICarrierResponseFactory>(ShipmentTypeCode.UpsWorldShip);

            RegisterRatingServiceFor(ShipmentTypeCode.UpsOnLineTools, builder);
            RegisterRatingServiceFor(ShipmentTypeCode.UpsWorldShip, builder);

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

            RegisterOltSpecificTypes(builder);
            RegisterWorldShipSpecificTypes(builder);
            RegisterPromoTypes(builder);
            RegisterLocalRatingTypes(builder);
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
            
            builder.RegisterType<UpsBestRateRatingService>()
                .Keyed<IUpsBestRateRatingService>(shipmentType)
                .WithParameter(new ResolvedParameter(
                    (parameters, _) => parameters.ParameterType == typeof(UpsShipmentType),
                    (_, context) => context.ResolveKeyed<ShipmentType>(shipmentType)));
        }

        /// <summary>
        /// Registers the local rating types.
        /// </summary>
        /// <param name="builder">The builder.</param>
        private void RegisterLocalRatingTypes(ContainerBuilder builder)
        {
            builder.RegisterDecorator<IUpsLocalRateValidator>(
                (c, inner) => new TelemetricUpsLocalRateValidator(inner, c.Resolve<Func<string, ITrackedEvent>>()),
                nameof(UpsLocalRateValidator));
        }
    }
}