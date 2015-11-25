using Autofac;
using ShipWorks.Shipping.Carriers.Other;

namespace ShipWorks.Shipping.UI.Carriers.Other
{
    public class OtherShippingModule : Module
    {
        /// <summary>
        /// Load the module configuration
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<OtherLabelService>()
                .Keyed<ILabelService>(ShipmentTypeCode.Other);
        }
    }
}