using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.LinkNewAccount.Response;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.WebServices.Registration;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.UPS.LinkNewAccount.Response.Manipulators
{

    public class UpsLinkNewAccountToProfileResponseTest
    {
        [Fact]
        public void Process_UpsApiExceptionIsThrown_WhenResponseStatusNotSuccess()
        {
            LinkNewAccountResponse response = new LinkNewAccountResponse
            {
                Response = new ResponseType()
                {
                    ResponseStatus = new CodeDescriptionType()
                    {
                        Code = EnumHelper.GetApiValue(UpsInvoiceRegistrationResponseStatusCode.Failed)
                    }
                },
                ShipperAccountStatus = new[]
                {
                    new RegCodeDescriptionType()
                    {
                        Code = UpsLinkNewAccountToProfileResponse.GoodShipperAccountStatus
                    }
                }
            };

            var testObject = new UpsLinkNewAccountToProfileResponse(response, null);

            Assert.Throws<UpsApiException>(() => testObject.Process());
        }

        [Fact]
        public void Process_UpsApiExceptionIsThrown_WhenShipperAccountStatusNot010()
        {
            LinkNewAccountResponse response = new LinkNewAccountResponse
            {
                Response = new ResponseType()
                {
                    ResponseStatus = new CodeDescriptionType()
                    {
                        Code = EnumHelper.GetApiValue(UpsInvoiceRegistrationResponseStatusCode.Success)
                    }
                },
                ShipperAccountStatus = new[]
                {
                    new RegCodeDescriptionType()
                    {
                        Code = "42"
                    }
                }
            };

            var testObject = new UpsLinkNewAccountToProfileResponse(response, null);

            Assert.Throws<UpsApiException>(() => testObject.Process());
        }

        [Fact]
        public void Process_NoExceptionIsThrown_WhenShipperAccountStatus010_AndResponseCodeIsSuccess()
        {
            LinkNewAccountResponse response = new LinkNewAccountResponse
            {
                Response = new ResponseType()
                {
                    ResponseStatus = new CodeDescriptionType()
                    {
                        Code = EnumHelper.GetApiValue(UpsInvoiceRegistrationResponseStatusCode.Success)
                    }
                },
                ShipperAccountStatus = new[]
                {
                    new RegCodeDescriptionType()
                    {
                        Code = UpsLinkNewAccountToProfileResponse.GoodShipperAccountStatus
                    }
                }
            };

            var testObject = new UpsLinkNewAccountToProfileResponse(response, null);

            testObject.Process();
        }
    }
}
