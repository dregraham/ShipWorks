using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Tracking;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// Tracks a UPS Shipment
    /// </summary>
    public interface IUpsTrackClient
    {
        /// <summary>
        /// Tracks shipment
        /// </summary>
        Task<TrackingResult> TrackShipment(ShipmentEntity shipment);
    }
}
