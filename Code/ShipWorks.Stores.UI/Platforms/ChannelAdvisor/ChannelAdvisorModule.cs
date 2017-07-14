using Autofac;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Platforms.ChannelAdvisor;

namespace ShipWorks.Stores.UI.Platforms.ChannelAdvisor
{
    /// <summary>
    /// Make necessary registrations for ChannelAdvisor
    /// </summary>
    public class ChannelAdvisorModule : Module
    {
        /// <summary>
        /// Loads the specified builder.
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ChannelAdvisorRestDownloader>()
                .Keyed<StoreDownloader>(StoreTypeCode.ChannelAdvisor)
                .ExternallyOwned();
        }
    }
}