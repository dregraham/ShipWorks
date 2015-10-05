using ShipWorks.Shipping.Carriers.UPS.WebServices.Promo;

namespace ShipWorks.Shipping.Carriers.UPS.Promo.API
{
    public class PromoAcceptanceTerms
    {
        readonly string URL;
        readonly string Description;
        readonly string AcceptanceCode;
        public bool IsAccepted;

        public PromoAcceptanceTerms(PromoDiscountAgreementResponse response)
        {
            this.URL = response.PromoAgreement.AgreementURL;
            this.Description = response.PromoDescription;
            this.AcceptanceCode = response.PromoAgreement.AcceptanceCode;
        }
    }
}