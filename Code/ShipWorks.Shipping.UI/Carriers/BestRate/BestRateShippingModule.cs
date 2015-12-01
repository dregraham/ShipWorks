using Autofac;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    /// <summary>
    /// Shipping module for the BestRate carrier
    /// </summary>
    public class BestRateShippingModule : Module
    {
        /// <summary>
        /// Load the module configuration
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BestRateLabelService>()
                .Keyed<ILabelService>(ShipmentTypeCode.BestRate);
        }
    }
}
