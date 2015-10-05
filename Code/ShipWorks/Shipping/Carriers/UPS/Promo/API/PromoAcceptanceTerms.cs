using ShipWorks.Shipping.Carriers.UPS.WebServices.Promo;

namespace ShipWorks.Shipping.Carriers.UPS.Promo.API
{
    /// <summary>
    /// Holds Promo Terms and Conditions info
    /// </summary>
    public class PromoAcceptanceTerms
    {
        // URL to the terms and conditons
        public readonly string URL;

        // Description of the promo 
        public readonly string Description;

        // Code used to activate discount 
        public readonly string AcceptanceCode;

        // Has the user accepted the Terms and Conditions
        public bool IsAccepted;

        /// <summary>
        /// Holds Promo Terms and Conditions info 
        /// </summary>
        /// <param name="response"></param>
        public PromoAcceptanceTerms(PromoDiscountAgreementResponse response)
        {
            URL = response.PromoAgreement.AgreementURL;
            Description = response.PromoDescription;
            AcceptanceCode = response.PromoAgreement.AcceptanceCode;
            IsAccepted = false;
        }
    }
}