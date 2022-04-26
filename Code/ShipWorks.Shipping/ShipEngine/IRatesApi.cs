using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Shipping.ShipEngine.DTOs;

namespace ShipWorks.Shipping.ShipEngine
{
    public interface IRatesApi
    {
        Task<RateShipmentResponse> RatesRateShipmentAsync(RateShipmentRequest request, string apiKey);

        Task<RateShipmentResponse> RatesRateShipmentAsync(RateShipmentRequest request, string apiKey, string onBehalfOf = null);
    }
}
