using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// The states of pace of the application wide heartbeat
    /// </summary>
    public enum HeartbeatPace
    {
        /// <summary>
        /// Not beating
        /// </summary>
        Stopped,

        /// <summary>
        /// Normal pace
        /// </summary>
        Normal,

        /// <summary>
        /// Increased pace, used to wait for data changes
        /// </summary>
        Fast,

        /// <summary>
        /// A single fast beat, that then reverts to normal
        /// </summary>
        SingleFast
    }
}
