using Autofac;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Services.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Shipping.UI.Carriers.Dhl
{
    /// <summary>
    /// Shipping module for the DHL Express carrier
    /// </summary>
    public class DhlExpressShippingModule : Module
    {
        /// <summary>
        /// Load the module configuration
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<NullShipmentPackageTypesBuilder>()
                .Keyed<IShipmentPackageTypesBuilder>(ShipmentTypeCode.DhlExpress)
                .SingleInstance();
        }
    }
}
