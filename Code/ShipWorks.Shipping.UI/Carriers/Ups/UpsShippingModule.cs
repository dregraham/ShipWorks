using Autofac;
using Autofac.Core;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using ShipWorks.Shipping.Carriers.UPS.ServiceManager;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Services.Builders;

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

            builder.RegisterType<UpsOltShipmentType>()
                .AsSelf()
                .Keyed<ShipmentType>(ShipmentTypeCode.UpsOnLineTools);

            builder.RegisterType<WorldShipShipmentType>()
                .AsSelf()
                .Keyed<ShipmentType>(ShipmentTypeCode.UpsWorldShip);

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

            builder.RegisterType<UpsOltShipmentPackageTypesBuilder>()
                .Keyed<IShipmentPackageTypesBuilder>(ShipmentTypeCode.UpsOnLineTools)
                .SingleInstance();

            builder.RegisterType<WorldShipShipmentPackageTypesBuilder>()
                .Keyed<IShipmentPackageTypesBuilder>(ShipmentTypeCode.UpsWorldShip)
                .SingleInstance();

            builder.RegisterType<UpsAccountRepository>()
                .Keyed<ICarrierAccountRepository<UpsAccountEntity>>(ShipmentTypeCode.UpsOnLineTools);

            builder.RegisterType<UpsOltLabelService>()
                .Keyed<ILabelService>(ShipmentTypeCode.UpsOnLineTools);

            builder.RegisterType<WorldShipLabelService>()
                .Keyed<ILabelService>(ShipmentTypeCode.UpsWorldShip);

            builder.RegisterType<UpsOltShipmentValidator>()
                .AsImplementedInterfaces();

            builder.RegisterType<UpsRateHashingService>()
                .Keyed<IRateHashingService>(ShipmentTypeCode.UpsOnLineTools)
                .Keyed<IRateHashingService>(ShipmentTypeCode.UpsWorldShip);

            builder.RegisterType<UpsApiTransitTimeClient>();

            builder.RegisterType<UpsApiRateClient>();

            RegisterRatingServiceFor(ShipmentTypeCode.UpsOnLineTools, builder);
            RegisterRatingServiceFor(ShipmentTypeCode.UpsWorldShip, builder);
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
