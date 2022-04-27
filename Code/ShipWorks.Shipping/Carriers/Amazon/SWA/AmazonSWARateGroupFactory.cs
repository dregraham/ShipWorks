using Interapptive.Shared.ComponentRegistration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.ShipEngine.DTOs;
using ShipWorks.Shipping.Editing.Rating;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.Amazon.SWA
{
    /// <summary>
    /// Factory for creating a RateGroup from a ShipEngine rate response
    /// </summary>
    [Component]
    public class AmazonSWARateGroupFactory : IAmazonSWARateGroupFactory
    {
        /// <summary>
        /// Creates a RateGroup from the given RateShipmentResponse
        /// </summary>
        public RateGroup Create(RateResponse rateResponse, ShipmentTypeCode shipmentType, IEnumerable<string> availableServiceTypeApiCodes)
        {
            List<RateResult> results = new List<RateResult>();

            foreach (Rate apiRate in rateResponse.Rates.Where(r => availableServiceTypeApiCodes.Contains(r.ServiceCode)))
            {
                RateResult rate = new RateResult("Amazon Shipping Ground", apiRate.DeliveryDays.ToString(), (decimal) (apiRate.ShippingAmount?.Amount ?? 0), apiRate.RateId)
                {
                    CarrierDescription = string.Empty,
                    ExpectedDeliveryDate = apiRate.EstimatedDeliveryDate,
                    ShipmentType = shipmentType,
                    ProviderLogo = EnumHelper.GetImage(shipmentType)
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
        private static void AddInvalidRateErrors(RateGroup rateGroup, RateResponse rateResponse, ShipmentTypeCode shipmentType)
        {
            StringBuilder errorBuilder = new StringBuilder();

            if (rateResponse.InvalidRates.Any())
            {
                foreach (Rate invalidRate in rateResponse.InvalidRates)
                {
                    invalidRate.ErrorMessages.ForEach(m => errorBuilder.AppendLine(m));
                }
            }

            if (rateResponse.Errors.Any())
            {
                foreach (ProviderError error in rateResponse.Errors)
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
