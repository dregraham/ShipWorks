using Autofac;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.OnTrac
{
    /// <summary>
    /// Shipping module for the OnTrac carrier
    /// </summary>
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

            builder.RegisterType<OnTracLabelService>()
                .Keyed<ILabelService>(ShipmentTypeCode.OnTrac);

            builder.RegisterType<OnTracAccountRepository>()
                .As<ICarrierAccountRepository<OnTracAccountEntity>>();

            builder.RegisterType<OnTracRateHashingService>()
                .Keyed<IRateHashingService>(ShipmentTypeCode.OnTrac)
                .AsSelf();

            builder.RegisterType<OnTracRatingService>()
                .Keyed<IRatingService>(ShipmentTypeCode.OnTrac)
                .AsImplementedInterfaces();

        }
    }
}
