using Autofac;
using ShipWorks.Stores.Services;

namespace ShipWorks.Stores.UI
{
    public class StoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<StoreTypeManagerWrapper>().AsImplementedInterfaces().SingleInstance();
        }
    }
}
