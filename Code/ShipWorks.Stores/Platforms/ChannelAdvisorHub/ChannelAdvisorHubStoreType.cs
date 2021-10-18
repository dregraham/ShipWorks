using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Platform;

namespace ShipWorks.Stores.Platforms
{
    /// <summary>
    /// ChannelAdvisorHub Store type
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.ChannelAdvisorHub)]
    [Component(RegistrationType.Self)]
    public class ChannelAdvisorHubStoreType : PlatformStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelAdvisorHubStoreType() : base()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelAdvisorHubStoreType(StoreEntity store)
            : base(store)
        {
        }

        /// <summary>
        /// ChannelAdvisorHub StoreTypeCode
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.ChannelAdvisorHub;
    }
}
