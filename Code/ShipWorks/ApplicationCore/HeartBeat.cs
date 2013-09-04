using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.ApplicationCore.Enums;
using ShipWorks.Data.Utility;

namespace ShipWorks.ApplicationCore
{
    public class HeartBeat
    {
        protected DateTime lastHeartbeatTime;

        protected TimeSpan heartbeatMinimumWait = TimeSpan.FromSeconds(.5);

        // If we are in a forced heartbeat that caused the heart rate to change, this is how many fast beats are left
        public int heartbeatForcedFastRatesStart = 10;
        public int HeartbeatForcedFastRatesLeft = 0;

        // Heartbeat standards
        public static readonly int HeartbeatFastRate = (int)TimeSpan.FromSeconds(1).TotalMilliseconds;
        protected int heartbeatNormalRate = (int)TimeSpan.FromSeconds(15).TotalMilliseconds;

        protected TimestampTracker heartbeatTimestampTracker = new TimestampTracker();

        protected bool heartbeatChangeProcessingPending = false;

        protected int storesChangeVersion = -1;


        protected virtual void Initialize()
        {
            lastHeartbeatTime = DateTime.MinValue;

            // Reset timestamp tracking for the heartbeat
            heartbeatTimestampTracker.Reset();
        }

    }
}
