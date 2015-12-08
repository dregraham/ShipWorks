using Autofac;
using ShipWorks.Shipping.Carriers.None;

namespace ShipWorks.Shipping.UI.Carriers.None
{
    /// <summary>
    /// Shipping module for the None carrier
    /// </summary>
    public class NoneShippingModule : Module
    {
        /// <summary>
        /// Load the module configuration
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<NoneLabelService>()
                .Keyed<ILabelService>(ShipmentTypeCode.None);

            builder.RegisterType<EmptyRatingService>()
                .Keyed<IRatingService>(ShipmentTypeCode.None);

            builder.RegisterType<EmptyRateHashingService>()
                .Keyed<IRateHashingService>(ShipmentTypeCode.None);
        }
    }
}
