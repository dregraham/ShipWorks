using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.ShipEngine.DTOs;
using ShipWorks.Shipping.Carriers.UPS.ShipEngine;
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
        public virtual RateGroup Create(RateResponse rateResponse, ShipmentTypeCode shipmentType, IEnumerable<string> availableServiceTypeApiCodes)
        {
            List<RateResult> results = new List<RateResult>();

            var ratesToAdd = rateResponse.Rates.Where(r => availableServiceTypeApiCodes.Contains(r.ServiceCode));

            foreach (Rate apiRate in ratesToAdd)
            {
                results.Add(GetRateResult(apiRate, shipmentType));
            }

            RateGroup rateGroup = new RateGroup(results);

            AddInvalidRateErrors(rateGroup, rateResponse, shipmentType);

            return rateGroup;
        }

        /// <summary>
        /// Build the rate tag from the given shipment and service code
        /// </summary>
        protected virtual object GetRateTag(ShipmentTypeCode shipmentType, string serviceCode)
        {
            if (shipmentType == ShipmentTypeCode.UpsOnLineTools)
            {
                return UpsShipEngineServiceTypeUtility.GetServiceType(serviceCode);
            }
            return serviceCode;
        }

        /// <summary>
        /// Build the rate result with the given rate
        /// </summary>
        protected virtual RateResult GetRateResult(Rate apiRate, ShipmentTypeCode shipmentType)
        {
            double amount = (apiRate.ShippingAmount?.Amount ?? 0) + (apiRate.OtherAmount?.Amount ?? 0) + (apiRate.InsuranceAmount?.Amount ?? 0) + (apiRate.ConfirmationAmount?.Amount ?? 0);
            string days = apiRate.DeliveryDays.ToString();

            if (!string.IsNullOrWhiteSpace(apiRate.CarrierDeliveryDays))
            {
                days = $"{days} ({apiRate.CarrierDeliveryDays})";
            }

            return new RateResult(apiRate.ServiceType, days, (decimal) (amount), GetRateTag(shipmentType, apiRate.ServiceCode))
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
                rateResponse.InvalidRates.SelectMany(x => x.ErrorMessages).Distinct().ForEach(e => errorBuilder.AppendLine(e));
            }

            if (rateResponse.Errors.Any())
            {
                rateResponse.Errors.Select(x => x.Message).Distinct().ForEach(e => errorBuilder.AppendLine(e));
            }

            if (errorBuilder.Length > 0)
            {
                rateGroup.AddFootnoteFactory(new ExceptionsRateFootnoteFactory(shipmentType, errorBuilder.ToString()));
            }
        }
    }
}
