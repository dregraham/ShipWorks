using Autofac;
using ShipWorks.Data.Model.Custom;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    public class BestRateShippingModule : Module
    {
        /// <summary>
        /// Load the module configuration
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BestRateShipmentType>()
                .AsSelf()
                .Keyed<ShipmentType>(ShipmentTypeCode.BestRate);

            builder.RegisterType<NullAccountRepository>()
                .Keyed<ICarrierAccountRepository<ICarrierAccount>>(ShipmentTypeCode.BestRate)
                .SingleInstance();
        }
    }
}
