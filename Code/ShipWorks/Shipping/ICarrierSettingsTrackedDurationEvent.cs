using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Metrics;
using ShipWorks.Shipping;

namespace ShipWorks.Core.Shipping
{
    /// <summary>
    /// Tracked duration event for carrier
    /// </summary>
    public interface ICarrierSettingsTrackedDurationEvent : ITrackedDurationEvent
    {
        /// <summary>
        /// Record configuration for a carrier
        /// </summary>
        void RecordConfiguration(ShipmentTypeCode carrier);
    }
}
