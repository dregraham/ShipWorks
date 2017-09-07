using ShipWorks.Shipping.Carriers.UPS.Promo;
using ShipWorks.Shipping.Carriers.UPS.WebServices.Promo;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.UPS.Promo
{
    public class PromoActiviationTest
    {
        [Fact]
        public void PromoActivation_Info_ContainsFirstAlertValue()
        {
            string desc = "stupid description";

            PromoDiscountResponse response = new PromoDiscountResponse()
            {
                Response = new ResponseType()
                {
                    Alert = new[]
                    {
                        new CodeDescriptionType()
                        {
                            Description = desc
                        }
                    }
                }
            };

            PromoActivation testObject = PromoActivation.FromPromoDiscountResponse(response);

            Assert.Equal(desc, testObject.Info);
        }

        [Fact]
        public void PromoActivation_Info_ContainsEmptyStringWhenAlertIsNull()
        {
            PromoDiscountResponse response = new PromoDiscountResponse()
            {
                Response = new ResponseType()
            };

            PromoActivation testObject = PromoActivation.FromPromoDiscountResponse(response);

            Assert.Equal(string.Empty, testObject.Info);
        }

        [Fact]
        public void PromoActivation_IsSuccessful_IsTrueWhenStatusCodeIs1()
        {
            PromoDiscountResponse response = new PromoDiscountResponse()
            {
                Response = new ResponseType()
                {
                    ResponseStatus = new CodeDescriptionType()
                    {
                        Code = "1"
                    }
                }
            };

            PromoActivation testObject = PromoActivation.FromPromoDiscountResponse(response);

            Assert.True(testObject.IsSuccessful);
        }

        [Fact]
        public void PromoActivation_IsSuccessful_IsFalseWhenStatusCodeIsNot1()
        {
            PromoDiscountResponse response = new PromoDiscountResponse()
            {
                Response = new ResponseType()
                {
                    ResponseStatus = new CodeDescriptionType()
                    {
                        Code = "0"
                    }
                }
            };

            PromoActivation testObject = PromoActivation.FromPromoDiscountResponse(response);

            Assert.False(testObject.IsSuccessful);
        }

        [Fact]
        public void PromoActivation_IsSuccessful_IsFalseWhenResponseStatusIsNull()
        {
            PromoDiscountResponse response = new PromoDiscountResponse()
            {
                Response = new ResponseType()
            };

            PromoActivation testObject = PromoActivation.FromPromoDiscountResponse(response);

            Assert.False(testObject.IsSuccessful);
        }
    }
}
