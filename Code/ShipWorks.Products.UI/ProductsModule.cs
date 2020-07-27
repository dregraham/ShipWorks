using Autofac;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Products.Warehouse;

namespace ShipWorks.Products.UI
{
    /// <summary>
    /// IoC registration module for this assembly
    /// </summary>
    public class ProductsModule : Module
    {
        /// <summary>
        /// Load the module configuration
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register((c, _) =>
                c.Resolve<ILicenseService>().IsHub ?
                    (IWarehouseProductClient) c.Resolve<WarehouseProductClient>() :
                    c.Resolve<EcommerceWarehouseProductClient>())
            .As<IWarehouseProductClient>();
        }
    }
}
