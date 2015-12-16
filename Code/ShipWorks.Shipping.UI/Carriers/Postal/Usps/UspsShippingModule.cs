using Autofac;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Ups;

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
                .AsSelf()
                .Keyed<ILabelService>(ShipmentTypeCode.Usps);

            builder.RegisterType<UspsRatingService>()
                .Keyed<IRatingService>(ShipmentTypeCode.Usps)
                .Keyed<ISupportExpress1Rates>(ShipmentTypeCode.Usps)
                .AsSelf();

            builder.RegisterType<UspsRateHashingService>()
                .Keyed<IRateHashingService>(ShipmentTypeCode.Usps);

            builder.RegisterType<UspsAccountRepository>()
                .As<ICarrierAccountRepository<UspsAccountEntity>>();
            
        }
    }
}
