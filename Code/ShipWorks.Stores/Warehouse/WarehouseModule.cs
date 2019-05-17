using Autofac;
using ShipWorks.Warehouse;

namespace ShipWorks.Stores.Warehouse
{
    public class WarehouseModule : Module
    {
        /// <summary>
        /// Loads the specified builder.
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register((container, parameters) =>
                                 container.ResolveKeyed<IWarehouseOrderFactory>(parameters.TypedAs<StoreTypeCode>()));
        }
    }
}