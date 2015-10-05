using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Shipping.Carriers.UPS.Promo.API;
using ShipWorks.Shipping.Carriers.UPS.WebServices.Promo;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.UPS.Promo.Api
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

            PromoActivation testObject = new PromoActivation(response);

            Assert.Equal(desc, testObject.Info);
        }

        [Fact]
        public void PromoActivation_Info_ContainsEmptyStringWhenAlertIsNull()
        {
            PromoDiscountResponse response = new PromoDiscountResponse()
            {
                Response = new ResponseType()
            };

            PromoActivation testObject = new PromoActivation(response);

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

            PromoActivation testObject = new PromoActivation(response);

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

            PromoActivation testObject = new PromoActivation(response);

            Assert.False(testObject.IsSuccessful);
        }

        [Fact]
        public void PromoActivation_IsSuccessful_IsFalseWhenResponseStatusIsNull()
        {
            PromoDiscountResponse response = new PromoDiscountResponse()
            {
                Response = new ResponseType()
            };

            PromoActivation testObject = new PromoActivation(response);

            Assert.False(testObject.IsSuccessful);
        }
    }
}
