using Interapptive.Shared.ComponentRegistration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipEngine.ApiClient.Model;
using ShipWorks.Shipping.Editing.Rating;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Collections;

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
        public RateGroup Create(RateShipmentResponse rateResponse, ShipmentTypeCode shipmentType, IEnumerable<string> availableServiceTypeApiCodes)
        {
            List<RateResult> results = new List<RateResult>();

            foreach (Rate apiRate in rateResponse.RateResponse.Rates.Where(r => availableServiceTypeApiCodes.Contains(r.ServiceCode)))
            {
                RateResult rate = new RateResult(apiRate.ServiceType, apiRate.DeliveryDays.ToString(), (decimal) apiRate.ShippingAmount.Amount, apiRate.ServiceCode)
                {
                    CarrierDescription = apiRate.CarrierNickname,
                    ExpectedDeliveryDate = apiRate.EstimatedDeliveryDate,
                    ShipmentType = shipmentType,
                    ProviderLogo = EnumHelper.GetImage(ShipmentTypeCode.DhlExpress)
                };
                results.Add(rate);
            }

            RateGroup rateGroup = new RateGroup(results);

            AddInvalidRateErrors(rateGroup, rateResponse, shipmentType);

            return rateGroup;
        }

        /// <summary>
        /// Add any invalid rate errors
        /// </summary>
        private static void AddInvalidRateErrors(RateGroup rateGroup, RateShipmentResponse rateResponse, ShipmentTypeCode shipmentType)
        {
            StringBuilder errorBuilder = new StringBuilder();

            if (rateResponse.RateResponse.InvalidRates.Any())
            {
                foreach (Rate invalidRate in rateResponse.RateResponse.InvalidRates)
                {
                    invalidRate.ErrorMessages.ForEach(m => errorBuilder.AppendLine(m));
                }
            }

            if (rateResponse.RateResponse.Errors.Any())
            {
                foreach (ProviderError error in rateResponse.RateResponse.Errors)
                {
                    errorBuilder.AppendLine(error.Message);
                }
            }
            
            if (errorBuilder.Length > 0)
            {
                rateGroup.AddFootnoteFactory(new ExceptionsRateFootnoteFactory(shipmentType, errorBuilder.ToString()));
            }
        }
    }
}
