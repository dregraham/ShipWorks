using ShipWorks.Shipping.Carriers.UPS.WebServices.Promo;

namespace ShipWorks.Shipping.Carriers.UPS.Promo.API
{
    /// <summary>
    /// Holds Promo Terms and Conditions info
    /// </summary>
    public class PromoAcceptanceTerms
    {
        // URL to the terms and conditons
        readonly string URL;

        // Description of the promo 
        readonly string Description;

        // Code used to activate discount 
        readonly string AcceptanceCode;

        // Has the user accepted the Terms and Conditions
        public bool IsAccepted;

        /// <summary>
        /// Holds Promo Terms and Conditions info 
        /// </summary>
        /// <param name="response"></param>
        public PromoAcceptanceTerms(PromoDiscountAgreementResponse response)
        {
            this.URL = response.PromoAgreement.AgreementURL;
            this.Description = response.PromoDescription;
            this.AcceptanceCode = response.PromoAgreement.AcceptanceCode;
            this.IsAccepted = false;
        }
    }
}