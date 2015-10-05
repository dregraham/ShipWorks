using ShipWorks.Shipping.Carriers.UPS.Promo.API;
using ShipWorks.Shipping.Carriers.UPS.WebServices.Promo;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.UPS.Promo.Api
{
    public class PromoAcceptanceTermsTest
    {
        [Fact]
        public void PromoAcceptanceTermsURL_EqualsURL_FromPromoDiscountAgreementResponse()
        {
            string Url = "www.example.com";

            PromoDiscountAgreementResponse response = new PromoDiscountAgreementResponse()
            {
                PromoAgreement = new PromoAgreementType() { AgreementURL = Url}
            };

            PromoAcceptanceTerms testObject = new PromoAcceptanceTerms(response);

            Assert.Equal(Url, testObject.URL);
        }

    }
}
