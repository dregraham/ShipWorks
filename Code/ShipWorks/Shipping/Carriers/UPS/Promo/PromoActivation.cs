using System.Linq;
using ShipWorks.Shipping.Carriers.UPS.WebServices.Promo;

namespace ShipWorks.Shipping.Carriers.UPS.Promo
{
    /// <summary>
    /// Holds information about PromoActivation
    /// </summary>
    public class PromoActivation
    {
        /// <summary>
        /// Create a PromoActivation from a PromoDiscountResponse
        /// </summary>
        public static PromoActivation FromPromoDiscountResponse(PromoDiscountResponse upsResponse)
        {
            return new PromoActivation(upsResponse);
        }

        /// <summary>
        /// Create a PromoActivation from an error message
        /// </summary>
        public static PromoActivation FromError(string errorMessage)
        {
            return new PromoActivation(errorMessage);   
        }

        /// <summary>
        /// Constructor
        /// </summary>
        private PromoActivation(PromoDiscountResponse upsResponse)
        {
            IsSuccessful = (upsResponse?.Response?.ResponseStatus?.Code ?? "0") == "1";
            Info = upsResponse?.Response?.Alert?.FirstOrDefault()?.Description ?? string.Empty;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        private PromoActivation(string errorMessage)
        {
            IsSuccessful = false;
            Info = errorMessage;
        }

        /// <summary>
        /// Gets any information if there were any alerts
        /// </summary>
        public string Info { get; }

        /// <summary>
        /// Returns true if activation was sucessful.
        /// </summary>
        public bool IsSuccessful { get; }
    }
}