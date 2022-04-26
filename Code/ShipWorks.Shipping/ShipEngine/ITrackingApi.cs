using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Shipping.ShipEngine.DTOs;

namespace ShipWorks.Shipping.ShipEngine
{
    public interface ITrackingApi
    {
        Task<TrackingInformation> TrackingTrackAsync(string apiKey, string carrierCode = null, string trackingNumber = null);
    }
}
