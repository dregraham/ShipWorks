using ShipWorks.Shipping.Carriers.UPS.WebServices.Promo;

namespace ShipWorks.Shipping.Carriers.UPS.Promo.API
{
    /// <summary>
    /// Holds Promo Terms and Conditions info
    /// </summary>
    public class PromoAcceptanceTerms
    {
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

        /// <summary>
        /// Accepts the terms.
        /// </summary>
        public void AcceptTerms() =>
                    IsAccepted = true;

        // URL to the terms and conditons
        public string URL { get; }

        // Description of the promo 
        public string Description { get; }

        // Code used to activate discount 
        public string AcceptanceCode { get; }

        // Has the user accepted the Terms and Conditions
        public bool IsAccepted { get; set; }
    }
}