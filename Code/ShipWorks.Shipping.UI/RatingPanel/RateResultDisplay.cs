using System.Drawing;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.UI.RatingPanel
{
    public class RateResultDisplay
    {
        public RateResultDisplay()
        {

        }

        public RateResultDisplay(RateResult rate)
        {
            ProviderLogo = rate.ProviderLogo;
            Description = rate.Description;
            Days = rate.Days;
            Shipping = SetAuxiliaryAmount(rate, rate.Shipping);
            Taxes = SetAuxiliaryAmount(rate, rate.Taxes);
            Duties = SetAuxiliaryAmount(rate, rate.Duties);
            Amount = rate.Selectable ? rate.FormattedAmount : ""; //, rate.AmountFootnote
            Rate = rate;
        }

        /// <summary>
        /// Sets an auxiliary amount
        /// </summary>
        /// <param name="rate"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        private string SetAuxiliaryAmount(RateResult rate, decimal? amount) =>
            rate.Selectable && amount.HasValue ? amount.Value.FormatFriendlyCurrency() : string.Empty;

        public string Days { get; set; }
        public string Description { get; set; }
        public string Duties { get; set; }
        public string Amount { get; set; }
        public string Shipping { get; set; }
        public string Taxes { get; set; }
        public Image ProviderLogo { get; set; }

        /// <summary>
        /// Rate that is displayed
        /// </summary>
        public RateResult Rate { get; private set; }

        /// <summary>
        /// Get whether the rate applies to a given service
        /// </summary>
        public bool AppliesToService(IRatingService ratingService, ICarrierShipmentAdapter shipmentAdapter)
        {
            return ratingService.IsRateSelectedByShipment(Rate, shipmentAdapter);
        }
    }
}