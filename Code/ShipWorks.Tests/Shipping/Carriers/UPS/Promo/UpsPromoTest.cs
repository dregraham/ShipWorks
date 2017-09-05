using Autofac;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.Promo;
using ShipWorks.Shipping.Carriers.UPS.Promo.Api;
using ShipWorks.Shipping.Carriers.UPS.Promo.RateFootnotes;
using ShipWorks.Shipping.Carriers.UPS.WebServices.Promo;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.UPS.Promo
{
    public class UpsPromoTest
    {
        private const string LivePromoCode = "BXA4S3YB9";
        private const string TestPromoCode = "BVOGIGNA7";

        public static AutoMock GetLooseThatReturnsMocks() =>
            AutoMock.GetFromRepository(new MockRepository(MockBehavior.Loose) {DefaultValue = DefaultValue.Mock});

        [Fact]
        public void Constructor_SetsAccountNumber()
        {
            using (var mock = GetLooseThatReturnsMocks())
            {
                Mock<IUpsApiPromoClient> client = mock.Mock<IUpsApiPromoClient>();
                UpsAccountEntity account = new UpsAccountEntity()
                {
                    AccountNumber = "12345"
                };

                var testObject = CreateUpsPromo(mock, client, account);

                Assert.Equal("12345", testObject.AccountNumber);
            }
        }

        [Fact]
        public void Constructor_SetsUsername()
        {
            using (var mock = GetLooseThatReturnsMocks())
            {
                Mock<IUpsApiPromoClient> client = mock.Mock<IUpsApiPromoClient>();
                UpsAccountEntity account = new UpsAccountEntity()
                {
                    UserID = "username"
                };

                var testObject = CreateUpsPromo(mock, client, account);

                Assert.Equal("username", testObject.Username);
            }
        }

        [Fact]
        public void Constructor_SetsPassword()
        {
            using (var mock = GetLooseThatReturnsMocks())
            {
                Mock<IUpsApiPromoClient> client = mock.Mock<IUpsApiPromoClient>();
                UpsAccountEntity account = new UpsAccountEntity()
                {
                    Password = "password"
                };

                var testObject = CreateUpsPromo(mock, client, account);

                Assert.Equal("password", testObject.Password);
            }
        }

        [Fact]
        public void Constructor_SetsAccessLicenseNumber()
        {
            using (var mock = GetLooseThatReturnsMocks())
            {
                Mock<IUpsApiPromoClient> client = mock.Mock<IUpsApiPromoClient>();

                var testObject = CreateUpsPromo(mock, client);

                Assert.Equal("blah", testObject.AccessLicenseNumber);
            }
        }

        [Fact]
        public void Constructor_SetsCountryCode()
        {
            using (var mock = GetLooseThatReturnsMocks())
            {
                Mock<IUpsApiPromoClient> client = mock.Mock<IUpsApiPromoClient>();
                UpsAccountEntity account = new UpsAccountEntity()
                {
                    CountryCode = "US"
                };

                var testObject = CreateUpsPromo(mock, client, account);

                Assert.Equal("US", testObject.CountryCode);
            }
        }

        [Fact]
        public void Constructor_SetsAccountId()
        {
            using (var mock = GetLooseThatReturnsMocks())
            {
                Mock<IUpsApiPromoClient> client = mock.Mock<IUpsApiPromoClient>();
                UpsAccountEntity account = new UpsAccountEntity()
                {
                    UpsAccountID = 12345
                };

                var testObject = CreateUpsPromo(mock, client, account);

                Assert.Equal(12345, testObject.AccountId);
            }
        }

        [Fact]
        public void GetStatus_Returns_UpsAccountPromoStatus()
        {
            using (var mock = GetLooseThatReturnsMocks())
            {
                UpsAccountEntity upsAccount = new UpsAccountEntity
                {
                    PromoStatus = (int)UpsPromoStatus.Applied
                };
                mock.Mock<ICarrierSettingsRepository>()
                    .Setup(s => s.GetShippingSettings())
                    .Returns(new ShippingSettingsEntity() {UpsAccessKey = "blahh"});

                var upsPromo = mock.Create<UpsPromo>(new TypedParameter(typeof (UpsAccountEntity), upsAccount));

                Assert.Equal(UpsPromoStatus.Applied, upsPromo.GetStatus());
            }
        }

        [Fact]
        public void Apply_ThrowsUpsPromoException_WhenTermsHaveNotBeenAccepted()
        {
            using (var mock = GetLooseThatReturnsMocks())
            {
                Mock<IUpsApiPromoClient> client = mock.Mock<IUpsApiPromoClient>();
                var testObject = CreateUpsPromo(mock, client);

                Assert.Throws<UpsPromoException>(() => testObject.Apply());
            }
        }

        [Fact]
        public void Apply_DelegatesToUpsApiPromoClient_WhenTermsHaveBeenAccepted()
        {
            using (var mock = GetLooseThatReturnsMocks())
            {
                var discountResponse = new PromoDiscountResponse()
                {
                    Response = new ResponseType()
                    {
                        ResponseStatus = new CodeDescriptionType() {Code = "1"}
                    }
                };

                var promoActivation = PromoActivation.FromPromoDiscountResponse(discountResponse);

                Mock<IUpsApiPromoClient> client = mock.Mock<IUpsApiPromoClient>();
                client.Setup(c => c.Activate(It.IsAny<string>(), It.IsAny<string>())).Returns(promoActivation);
                var testObject = CreateUpsPromo(mock, client);
                testObject.Terms.AcceptTerms();

                testObject.Apply();

                client.Verify(c => c.Activate(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            }
        }

        [Fact]
        public void Apply_SetsPromoStatusToApplied_WhenPromoActivationIsSuccessful()
        {
            using (var mock = GetLooseThatReturnsMocks())
            {
                var discountResponse = new PromoDiscountResponse()
                {
                    Response = new ResponseType()
                    {
                        ResponseStatus = new CodeDescriptionType() { Code = "1" }
                    }
                };

                UpsAccountEntity upsAccount = new UpsAccountEntity
                {
                    PromoStatus = (int)UpsPromoStatus.None,
                    StateProvCode = "MO"
                };

                var promoActivation = PromoActivation.FromPromoDiscountResponse(discountResponse);

                Mock<IUpsApiPromoClient> client = mock.Mock<IUpsApiPromoClient>();
                client.Setup(c => c.Activate(It.IsAny<string>(), It.IsAny<string>())).Returns(promoActivation);
                var testObject = CreateUpsPromo(mock, client, upsAccount);
                testObject.Terms.AcceptTerms();

                testObject.Apply();

                Assert.Equal(UpsPromoStatus.Applied, (UpsPromoStatus) upsAccount.PromoStatus);
            }
        }

        [Fact]
        public void Apply_SavesUpsAccount_WhenPromoActivationIsSuccessful()
        {
            using (var mock = GetLooseThatReturnsMocks())
            {
                var discountResponse = new PromoDiscountResponse()
                {
                    Response = new ResponseType()
                    {
                        ResponseStatus = new CodeDescriptionType() { Code = "1" }
                    }
                };

                UpsAccountEntity upsAccount = new UpsAccountEntity
                {
                    PromoStatus = (int)UpsPromoStatus.None,
                    StateProvCode = "MO"
                };

                var promoActivation = PromoActivation.FromPromoDiscountResponse(discountResponse);
                var accountRepo = mock.Mock<ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity>>();
                Mock<IUpsApiPromoClient> client = mock.Mock<IUpsApiPromoClient>();
                client.Setup(c => c.Activate(It.IsAny<string>(), It.IsAny<string>())).Returns(promoActivation);
                var testObject = CreateUpsPromo(mock, client, upsAccount);
                testObject.Terms.AcceptTerms();

                testObject.Apply();

                accountRepo.Verify(r => r.Save(It.IsAny<UpsAccountEntity>()), Times.Once);
            }
        }

        [Fact]
        public void Apply_ReturnsPromoActivation_WhenPromoActivationIsNotSuccessful()
        {
            using (var mock = GetLooseThatReturnsMocks())
            {
                var discountResponse = new PromoDiscountResponse()
                {
                    Response = new ResponseType()
                    {
                        ResponseStatus = new CodeDescriptionType() { Code = "0" }
                    }
                };

                UpsAccountEntity upsAccount = new UpsAccountEntity
                {
                    PromoStatus = (int)UpsPromoStatus.None,
                    StateProvCode = "MO"
                };

                var promoActivation = PromoActivation.FromPromoDiscountResponse(discountResponse);

                Mock<IUpsApiPromoClient> client = mock.Mock<IUpsApiPromoClient>();
                client.Setup(c => c.Activate(It.IsAny<string>(), It.IsAny<string>())).Returns(promoActivation);
                var testObject = CreateUpsPromo(mock, client, upsAccount);
                testObject.Terms.AcceptTerms();

                Assert.False(testObject.Apply().IsSuccessful);
            }
        }

        [Fact]
        public void GetTerms_DelegatesToUpsApiPromoClient()
        {
            using (var mock = GetLooseThatReturnsMocks())
            {
                Mock<IUpsApiPromoClient> client = mock.Mock<IUpsApiPromoClient>();

                var testObject = CreateUpsPromo(mock, client);
                var terms = testObject.Terms;
                client.Verify(c => c.GetAgreement(), Times.Once);
            }
        }

        [Fact]
        public void FactMethodName()
        {
            using (var mock = GetLooseThatReturnsMocks())
            {
                Mock<IUpsApiPromoClient> client = mock.Mock<IUpsApiPromoClient>();
                UpsAccountEntity upsAccount = new UpsAccountEntity
                {
                    PromoStatus = (int) UpsPromoStatus.None,
                    CountryCode = "US",
                    StateProvCode = "MO"
                };

                mock.Mock<ICarrierSettingsRepository>().Setup(s=>s.UseTestServer).Returns(true);

                var testObject = CreateUpsPromo(mock, client, upsAccount);
                Assert.Equal(TestPromoCode, testObject.PromoCode);
            }
        }

        [Fact]
        public void PromoCode_ReturnsContinentalUSPromoCode_WhenAccountStateIsContinentalUSState()
        {
            using (var mock = GetLooseThatReturnsMocks())
            {
                Mock<IUpsApiPromoClient> client = mock.Mock<IUpsApiPromoClient>();
                UpsAccountEntity upsAccount = new UpsAccountEntity
                {
                    PromoStatus = (int)UpsPromoStatus.None,
                    CountryCode = "US",
                    StateProvCode = "MO"
                };
                var testObject = CreateUpsPromo(mock, client, upsAccount);
                Assert.Equal(LivePromoCode, testObject.PromoCode);
            }
        }

        [Fact]
        public void PromoCode_ReturnsEmptyPromoCode_WhenAccountCountryIsNotUnitedStates()
        {
            using (var mock = GetLooseThatReturnsMocks())
            {
                Mock<IUpsApiPromoClient> client = mock.Mock<IUpsApiPromoClient>();
                UpsAccountEntity upsAccount = new UpsAccountEntity
                {
                    PromoStatus = (int)UpsPromoStatus.None,
                    CountryCode = "CA"
                };
                var testObject = CreateUpsPromo(mock, client, upsAccount);
                Assert.Empty(testObject.PromoCode);
            }
        }

        [Fact]
        public void Decline_SetsPromoStatusToDeclined()
        {
            using (var mock = GetLooseThatReturnsMocks())
            {
                UpsAccountEntity upsAccount = new UpsAccountEntity
                {
                    PromoStatus = (int) UpsPromoStatus.None
                };

                Mock<IUpsApiPromoClient> client = mock.Mock<IUpsApiPromoClient>();
                var testObject = CreateUpsPromo(mock, client, upsAccount);

                testObject.Decline();

                Assert.Equal(UpsPromoStatus.Declined, (UpsPromoStatus)upsAccount.PromoStatus);
            }
        }

        [Fact]
        public void Decline_SavesUpsAccount()
        {
            using (var mock = GetLooseThatReturnsMocks())
            {
                UpsAccountEntity upsAccount = new UpsAccountEntity
                {
                    PromoStatus = (int)UpsPromoStatus.None,
                };

                var accountRepo = mock.Mock<ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity>>();
                Mock<IUpsApiPromoClient> client = mock.Mock<IUpsApiPromoClient>();
                var testObject = CreateUpsPromo(mock, client, upsAccount);

                testObject.Decline();

                accountRepo.Verify(r => r.Save(It.IsAny<UpsAccountEntity>()), Times.Once);
            }
        }

        [Fact]
        public void RemindMe_DelegatesToPromoPolicy()
        {
            using (var mock = GetLooseThatReturnsMocks())
            {
                Mock<IUpsApiPromoClient> client = mock.Mock<IUpsApiPromoClient>();
                var promoPolicy = mock.Mock<IUpsPromoPolicy>();
                var testObject = CreateUpsPromo(mock, client);

                testObject.RemindMe();

                promoPolicy.Verify(p => p.RemindLater(It.IsAny<UpsPromo>()));
            }
        }

        private static UpsPromo CreateUpsPromo(AutoMock mock, Mock<IUpsApiPromoClient> client)
        {
            UpsAccountEntity upsAccount = new UpsAccountEntity
            {
                PromoStatus = (int) UpsPromoStatus.None,
                StateProvCode = "MO"
            };

            return CreateUpsPromo(mock, client, upsAccount);
        }

        private static UpsPromo CreateUpsPromo(AutoMock mock, Mock<IUpsApiPromoClient> client, UpsAccountEntity upsAccount)
        {
            mock.Mock<ICarrierSettingsRepository>()
                .Setup(s => s.GetShippingSettings())
                .Returns(new ShippingSettingsEntity() {UpsAccessKey = "itWof6javyE="});

            PromoDiscountAgreementResponse response = new PromoDiscountAgreementResponse()
            {
                PromoAgreement = new PromoAgreementType() {AgreementURL = "www.example.com", AcceptanceCode = "accept"},
                Response = new ResponseType {ResponseStatus = new CodeDescriptionType() {Code = "1"}}
            };

            PromoAcceptanceTerms acceptanceTerms = new PromoAcceptanceTerms(response);

            client.Setup(c => c.GetAgreement()).Returns(acceptanceTerms);

            mock.Mock<IPromoClientFactory>()
                .Setup(x => x.CreatePromoClient(It.IsAny<UpsPromo>()))
                .Returns(client.Object);

            var testObject = mock.Create<UpsPromo>(new TypedParameter(typeof(UpsAccountEntity), upsAccount));
            return testObject;
        }
    }
}
