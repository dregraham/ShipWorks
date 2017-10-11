using Interapptive.Shared.ComponentRegistration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipEngine.ApiClient.Model;
using ShipWorks.Shipping.Editing.Rating;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.ShipEngine
{
    /// <summary>
    /// Factory for creating a RateGroup from a ShipEngine rate response
    /// </summary>
    [Component]
    public class ShipEngineRateGroupFactory : IShipEngineRateGroupFactory
    {
        /// <summary>
        /// Creates a RateGroup from the given RateShipmentResponse
        /// </summary>
        public RateGroup Create(RateShipmentResponse rateResponse, ShipmentTypeCode shipmentType)
        {
            List<RateResult> results = new List<RateResult>();

            foreach (var apiRate in rateResponse.RateResponse.Rates)
            {
                RateResult rate = new RateResult(apiRate.ServiceType, apiRate.DeliveryDays.ToString())
                {
                    CarrierDescription = apiRate.CarrierNickname,
                    ExpectedDeliveryDate = apiRate.EstimatedDeliveryDate,
                    ShipmentType = shipmentType,
                    ProviderLogo = EnumHelper.GetImage(ShipmentTypeCode.DhlExpress)
                };
                results.Add(rate);
            }

            return new RateGroup(results);
        }
    }
}
