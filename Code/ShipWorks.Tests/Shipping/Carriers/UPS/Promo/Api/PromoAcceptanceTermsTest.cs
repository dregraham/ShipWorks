using log4net;
using Moq;
using ShipWorks.Shipping.Carriers.UPS.Promo.API;
using ShipWorks.Shipping.Carriers.UPS.WebServices.Promo;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.UPS.Promo.Api
{
    public class PromoAcceptanceTermsTest
    {
        private PromoAcceptanceTerms testObject;
        
        private readonly Mock<ILog> log;

        public PromoAcceptanceTermsTest()
        {
            log = new Mock<ILog>();
            log.Setup(l => l.InfoFormat(It.IsAny<string>(), It.IsAny<object[]>()));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(10)]
        [InlineData(20)]
        public void PromoAcceptanceTerms_ThrowsUpsPromoException_WhenResponseCodeIsNotOne(int responseCode)
        {
            PromoDiscountAgreementResponse response = new PromoDiscountAgreementResponse()
            {
                PromoAgreement = new PromoAgreementType() { AgreementURL = "www.example.com" },
                Response = new ResponseType { ResponseStatus = new CodeDescriptionType() { Code = responseCode.ToString() } }
            };

            Assert.Throws<UpsPromoException>(() => new PromoAcceptanceTerms(response, log.Object));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(10)]
        [InlineData(20)]
        public void PromoAcceptanceTerms_LogsMessage_WhenResponseCodeIsNotOne(int responseCode)
        {
            PromoDiscountAgreementResponse response = new PromoDiscountAgreementResponse()
            {
                PromoAgreement = new PromoAgreementType() { AgreementURL = "www.example.com" },
                Response = new ResponseType { ResponseStatus = new CodeDescriptionType() { Code = responseCode.ToString() } }
            };

            Assert.Throws<UpsPromoException>(() => new PromoAcceptanceTerms(response, log.Object));

            log.Verify(l => l.InfoFormat(It.IsAny<string>(), response.Response.ResponseStatus.Code, response.Response.ResponseStatus.Description), Times.Once());
        }

        [Fact]
        public void Constructor_SetsIsAcceptedToFalse_WhenResponseCodeIsOne()
        {
            PromoDiscountAgreementResponse response = new PromoDiscountAgreementResponse()
            {
                PromoAgreement = new PromoAgreementType() { AgreementURL = "www.example.com" },
                Response = new ResponseType { ResponseStatus = new CodeDescriptionType() { Code = "1" } }
            };

            testObject = new PromoAcceptanceTerms(response, log.Object);

            Assert.Equal(false, testObject.IsAccepted);
        }

        [Fact]
        public void Constructor_SetsAgreementUrl_WhenResponseCodeIsOne()
        {
            PromoDiscountAgreementResponse response = new PromoDiscountAgreementResponse()
            {
                PromoAgreement = new PromoAgreementType() { AgreementURL = "www.example.com" },
                Response = new ResponseType { ResponseStatus = new CodeDescriptionType() { Code = "1" } }
            };
            
            testObject = new PromoAcceptanceTerms(response, log.Object);

            Assert.Equal(response.PromoAgreement.AgreementURL, testObject.URL);
        }

        [Fact]
        public void Constructor_SetsAcceptanceCode_WhenResponseCodeIsOne()
        {
            PromoDiscountAgreementResponse response = new PromoDiscountAgreementResponse()
            {
                PromoAgreement = new PromoAgreementType() { AgreementURL = "www.example.com", AcceptanceCode = "someCode" },
                Response = new ResponseType { ResponseStatus = new CodeDescriptionType() { Code = "1" } }
            };

            testObject = new PromoAcceptanceTerms(response, log.Object);

            Assert.Equal(response.PromoAgreement.AcceptanceCode, testObject.AcceptanceCode);
        }

        [Fact]
        public void Constructor_SetsDescription_WhenResponseCodeIsOne()
        {
            PromoDiscountAgreementResponse response = new PromoDiscountAgreementResponse()
            {
                PromoAgreement = new PromoAgreementType() { AgreementURL = "www.example.com", AcceptanceCode = "someCode" },
                Response = new ResponseType { ResponseStatus = new CodeDescriptionType() { Code = "1" } },
                PromoDescription = "theDescription"
            };

            testObject = new PromoAcceptanceTerms(response, log.Object);

            Assert.Equal(response.PromoDescription, testObject.Description);
        }

        [Fact]
        public void AcceptTerms_LogsAgreementWasAccepted()
        {
            PromoDiscountAgreementResponse response = new PromoDiscountAgreementResponse()
            {
                PromoAgreement = new PromoAgreementType() { AgreementURL = "www.example.com" },
                Response = new ResponseType { ResponseStatus = new CodeDescriptionType() { Code = "1" } }
            };

            testObject = new PromoAcceptanceTerms(response, log.Object);
            testObject.AcceptTerms();

            log.Verify(l => l.Info("UPS promo terms and conditions have been accepted."), Times.Once());
        }

        [Fact]
        public void AcceptTerms_SetsIsAcceptedToTrue()
        {
            PromoDiscountAgreementResponse response = new PromoDiscountAgreementResponse()
            {
                PromoAgreement = new PromoAgreementType() { AgreementURL = "www.example.com" },
                Response = new ResponseType { ResponseStatus = new CodeDescriptionType() { Code = "1" } }
            };

            testObject = new PromoAcceptanceTerms(response, log.Object);
            testObject.AcceptTerms();

            Assert.Equal(true, testObject.IsAccepted);
        }

    }
}
