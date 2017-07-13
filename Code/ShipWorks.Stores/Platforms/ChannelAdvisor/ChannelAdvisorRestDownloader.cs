using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor
{
    /// <summary>
    /// Downloader for downloading orders from ChannelAdvisor via their REST api
    /// </summary>
    [Component]
    public class ChannelAdvisorRestDownloader : StoreDownloader, IChannelAdvisorRestDownloader
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelAdvisorRestDownloader(StoreEntity store) : base(store)
        {
        }
        
        /// <summary>
        /// Download orders from ChannelAdvisor REST
        /// </summary>
        protected override void Download(TrackedDurationEvent trackedDurationEvent)
        {
            throw new System.NotImplementedException();
        }
    }
}