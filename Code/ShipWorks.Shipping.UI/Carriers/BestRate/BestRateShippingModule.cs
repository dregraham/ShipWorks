using Autofac;

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
        }
    }
}
