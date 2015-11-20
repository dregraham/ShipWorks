using System;

namespace ShipWorks.Data.Connection
{
    public interface ISqlSession
    {
        /// <summary>
        /// Get the latest time information from the server. Uses a cache mechanism for efficiency, so
        /// we don't go to the server every invocation.
        /// 
        /// If the time has been retrieved from the server withing the past 30 minutes, then the current time
        /// is estimated by adding the last retrieved time plus the elapsed time.
        /// </summary>
        DateTime GetLocalDate();
    }
}