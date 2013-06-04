using System;
using System.Collections.Generic;
using System.Text;

namespace ShipWorks.Data.Connection
{
    /// <summary>
    /// The various states the connection monitor can be in
    /// </summary>
    public enum ConnectionMonitorStatus
    {
        Normal,
        AttemptReconnect,
        ConnectionLost
    }
}
