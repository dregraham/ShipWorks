using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Shipping.ShipEngine.DTOs;

namespace ShipWorks.Shipping.ShipEngine.API
{
    public class TrackingApi : ITrackingApi
    {
        private readonly string endpoint = string.Empty;

        public TrackingApi()
        {

        }

        public TrackingApi(string endpoint)
        {
            this.endpoint = endpoint;
        }

        public Task<TrackingInformation> TrackingTrackAsync(string apiKey, string carrierCode = null, string trackingNumber = null)
        {
            throw new NotImplementedException();
        }
    }
}
