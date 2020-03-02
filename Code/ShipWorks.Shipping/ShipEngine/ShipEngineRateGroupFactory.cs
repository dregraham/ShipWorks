using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipEngine.ApiClient.Model;
using ShipWorks.Shipping.Editing.Rating;

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
        public RateGroup Create(RateResponse rateResponse, ShipmentTypeCode shipmentType, IEnumerable<string> availableServiceTypeApiCodes)
        {
            List<RateResult> results = new List<RateResult>();

            foreach (Rate apiRate in rateResponse.Rates.Where(r => availableServiceTypeApiCodes.Contains(r.ServiceCode)))
            {
                results.Add(GetRateResult(apiRate, shipmentType));
            }

            RateGroup rateGroup = new RateGroup(results);

            AddInvalidRateErrors(rateGroup, rateResponse, shipmentType);

            return rateGroup;
        }

        /// <summary>
        /// Build the rate result with the given rate
        /// </summary>
        private RateResult GetRateResult(Rate apiRate, ShipmentTypeCode shipmentType)
        {
            double amount = (apiRate.ShippingAmount?.Amount ?? 0) + (apiRate.OtherAmount?.Amount ?? 0) + (apiRate.InsuranceAmount?.Amount ?? 0);

            return new RateResult(apiRate.ServiceType, apiRate.DeliveryDays.ToString(), (decimal) (amount), apiRate.ServiceCode)
            {
                CarrierDescription = apiRate.CarrierNickname,
                ExpectedDeliveryDate = apiRate.EstimatedDeliveryDate,
                ShipmentType = shipmentType,
                ProviderLogo = EnumHelper.GetImage(shipmentType)
            };
        }

        /// <summary>
        /// Add any invalid rate errors
        /// </summary>
        private static void AddInvalidRateErrors(RateGroup rateGroup, RateResponse rateResponse, ShipmentTypeCode shipmentType)
        {
            StringBuilder errorBuilder = new StringBuilder();

            if (rateResponse.InvalidRates.Any())
            {
                foreach (Rate invalidRate in rateResponse.InvalidRates)
                {
                    invalidRate.ErrorMessages.Distinct().ForEach(m => errorBuilder.AppendLine(m));
                }
            }

            if (rateResponse.Errors.Any())
            {
                foreach (string error in rateResponse.Errors.Select(x => x.Message).Distinct())
                {
                    errorBuilder.AppendLine(error);
                }
            }

            if (errorBuilder.Length > 0)
            {
                rateGroup.AddFootnoteFactory(new ExceptionsRateFootnoteFactory(shipmentType, errorBuilder.ToString()));
            }
        }
    }
}
