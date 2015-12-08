using Autofac;
using ShipWorks.Shipping.Carriers.Other;

namespace ShipWorks.Shipping.UI.Carriers.Other
{
    /// <summary>
    /// Shipping module for the other carrier
    /// </summary>
    public class OtherShippingModule : Module
    {
        /// <summary>
        /// Load the module configuration
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<OtherLabelService>()
                .Keyed<ILabelService>(ShipmentTypeCode.Other);

            builder.RegisterType<EmptyRatingService>()
                .Keyed<IRatingService>(ShipmentTypeCode.Other);

            builder.RegisterType<EmptyRateHashingService>()
                .Keyed<IRateHashingService>(ShipmentTypeCode.Other);
        }
    }
}