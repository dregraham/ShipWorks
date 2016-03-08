using System;
using Autofac;
using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Ups;
using ShipWorks.ApplicationCore.Licensing;

namespace ShipWorks.Shipping.Carriers.Usps
{
    public class UspsShippingModule : Module
    {
        /// <summary>
        /// Load the module configuration
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UspsShipmentType>()
                .AsSelf()
                .Keyed<ShipmentType>(ShipmentTypeCode.Usps);

            builder.RegisterType<UspsLabelService>()
                .Keyed<ILabelService>(ShipmentTypeCode.Usps);

            builder.RegisterType<UspsRatingService>()
                .Keyed<IRatingService>(ShipmentTypeCode.Usps)
                .Keyed<ISupportExpress1Rates>(ShipmentTypeCode.Usps)
                .AsSelf();

            builder.RegisterType<UspsRateHashingService>()
                .Keyed<IRateHashingService>(ShipmentTypeCode.Usps);

            builder.RegisterType<UspsAccountRepository>()
                .As<ICarrierAccountRepository<UspsAccountEntity>>();

            builder.RegisterType<UspsWebClient>()
                .AsImplementedInterfaces()
                .UsingConstructor(typeof (ICarrierAccountRepository<UspsAccountEntity>),
                    typeof (ILogEntryFactory),
                    typeof (Func<string, ICertificateInspector>),
                    typeof (UspsResellerType));

            builder.RegisterType<UspsResellerType>()
                .AsImplementedInterfaces();

            builder.RegisterType<UspsAccountManagerWrapper>()
                .AsImplementedInterfaces();

            builder.RegisterType<AssociateShipworksWithItselfRequest>();    
        }
    }
}
