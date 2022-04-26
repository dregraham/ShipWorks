using System;
using System.Threading.Tasks;
using ShipWorks.Shipping.ShipEngine.DTOs;

namespace ShipWorks.Shipping.ShipEngine.API
{
    public class RatesApi : IRatesApi
    {
        private readonly string endpoint = string.Empty;

        public RatesApi()
        {

        }

        public RatesApi(string endpoint)
        {
            this.endpoint = endpoint;
        }

        public Task<RateShipmentResponse> RatesRateShipmentAsync(RateShipmentRequest request, string apiKey)
        {
            throw new NotImplementedException();
        }

        public Task<RateShipmentResponse> RatesRateShipmentAsync(RateShipmentRequest request, string apiKey, string onBehalfOf = null)
        {
            throw new NotImplementedException();
        }
    }
}
