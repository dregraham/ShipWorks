﻿using Autofac;
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

namespace ShipWorks.Tests.Shipping.Carriers.UPS.Promo.Api
{
    public class UpsPromoTest
    {
        private const string ContinentalUSPromoCode = "P090029838";
        private const string AlaskaPromoCode = "P950029472";
        private const string HawaiiPromoCode = "P780029996";

        public static AutoMock GetLooseThatReturnsMocks() =>
            AutoMock.GetFromRepository(new MockRepository(MockBehavior.Loose) {DefaultValue = DefaultValue.Mock});

        [Fact]
        public void GetStatus_Returns_UpsAccountPromoStatus()
        {
            using (var mock = GetLooseThatReturnsMocks())
            {
                UpsAccountEntity upsAccount = new UpsAccountEntity
                {
                    PromoStatus = (int)UpsPromoStatus.Applied
                };

                mock.Mock<ICarrierSettingsRepository>().Setup(s => s.GetShippingSettings().UpsAccessKey).Returns("blahh");
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

                var promoActivation = new PromoActivation(discountResponse);

                Mock<IUpsApiPromoClient> client = mock.Mock<IUpsApiPromoClient>();
                client.Setup(c => c.Activate(It.IsAny<string>())).Returns(promoActivation);
                var testObject = CreateUpsPromo(mock, client);
                testObject.Terms.AcceptTerms();

                testObject.Apply();

                client.Verify(c => c.Activate(It.IsAny<string>()), Times.Once);
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

                var promoActivation = new PromoActivation(discountResponse);

                Mock<IUpsApiPromoClient> client = mock.Mock<IUpsApiPromoClient>();
                client.Setup(c => c.Activate(It.IsAny<string>())).Returns(promoActivation);
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

                var promoActivation = new PromoActivation(discountResponse);
                var accountRepo = mock.Mock<ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity>>();
                Mock<IUpsApiPromoClient> client = mock.Mock<IUpsApiPromoClient>();
                client.Setup(c => c.Activate(It.IsAny<string>())).Returns(promoActivation);
                var testObject = CreateUpsPromo(mock, client, upsAccount);
                testObject.Terms.AcceptTerms();

                testObject.Apply();

                accountRepo.Verify(r => r.Save(It.IsAny<UpsAccountEntity>()), Times.Once);
            }
        }

        [Fact]
        public void Apply_ThrowsUpsPromoException_WhenPromoActivationIsNotSuccessful()
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

                var promoActivation = new PromoActivation(discountResponse);

                Mock<IUpsApiPromoClient> client = mock.Mock<IUpsApiPromoClient>();
                client.Setup(c => c.Activate(It.IsAny<string>())).Returns(promoActivation);
                var testObject = CreateUpsPromo(mock, client, upsAccount);
                testObject.Terms.AcceptTerms();

                Assert.Throws<UpsPromoException>(() => testObject.Apply());
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
                client.Verify(c => c.GetAgreement(It.IsAny<string>()), Times.Once);
            }
        }

        [Fact]
        public void GetTerms_UsesAlaskaPromoCode_WhenAccountStateIsAlaska()
        {
            using (var mock = GetLooseThatReturnsMocks())
            {
                Mock<IUpsApiPromoClient> client = mock.Mock<IUpsApiPromoClient>();
                UpsAccountEntity upsAccount = new UpsAccountEntity
                {
                    PromoStatus = (int)UpsPromoStatus.None,
                    CountryCode = "US",
                    StateProvCode = "AK"
                };
                var testObject = CreateUpsPromo(mock, client, upsAccount);
                var terms = testObject.Terms;
                client.Verify(c => c.GetAgreement(AlaskaPromoCode), Times.Once);
            }
        }

        [Fact]
        public void GetTerms_UsesHawaiiPromoCode_WhenAccountStateIsHawaii()
        {
            using (var mock = GetLooseThatReturnsMocks())
            {
                Mock<IUpsApiPromoClient> client = mock.Mock<IUpsApiPromoClient>();
                UpsAccountEntity upsAccount = new UpsAccountEntity
                {
                    PromoStatus = (int)UpsPromoStatus.None,
                    CountryCode = "US",
                    StateProvCode = "HI"
                };
                var testObject = CreateUpsPromo(mock, client, upsAccount);
                var terms = testObject.Terms;
                client.Verify(c => c.GetAgreement(HawaiiPromoCode), Times.Once);
            }
        }

        [Fact]
        public void GetTerms_UsesContinentalUSPromoCode_WhenAccountStateIsContinentalUSState()
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
                var terms = testObject.Terms;
                client.Verify(c => c.GetAgreement(ContinentalUSPromoCode), Times.Once);
            }
        }

        [Fact]
        public void GetTerms_UsesEmptyPromoCode_WhenAccountCountryIsNotUnitedStates()
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
                var terms = testObject.Terms;
                client.Verify(c => c.GetAgreement(string.Empty), Times.Once);
            }
        }

        [Fact]
        public void GetFootnoteFactory_ReturnsNull_WhenPromoPolicyIsNotEligible()
        {
            using (var mock = GetLooseThatReturnsMocks())
            {
                Mock<IUpsApiPromoClient> client = mock.Mock<IUpsApiPromoClient>();
                var promoPolicy = mock.Mock<IUpsPromoPolicy>();
                promoPolicy.Setup(p => p.IsEligible(It.IsAny<UpsPromo>())).Returns(false);
                var testObject = CreateUpsPromo(mock, client);

                Assert.Null(testObject.GetFootnoteFactory());
            }
        }

        [Fact]
        public void GetFootnoteFactory_ReturnsUpsPromoFootnoteFactory_WhenPromoPolicyIsEligible()
        {
            using (var mock = GetLooseThatReturnsMocks())
            {
                Mock<IUpsApiPromoClient> client = mock.Mock<IUpsApiPromoClient>();
                var promoPolicy = mock.Mock<IUpsPromoPolicy>();
                promoPolicy.Setup(p => p.IsEligible(It.IsAny<UpsPromo>())).Returns(true);
                var testObject = CreateUpsPromo(mock, client);

                Assert.IsAssignableFrom<UpsPromoFootnoteFactory>(testObject.GetFootnoteFactory());
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
            mock.Mock<ICarrierSettingsRepository>().Setup(s => s.GetShippingSettings().UpsAccessKey).Returns("blahh");

            PromoDiscountAgreementResponse response = new PromoDiscountAgreementResponse()
            {
                PromoAgreement = new PromoAgreementType() {AgreementURL = "www.example.com", AcceptanceCode = "accept"},
                Response = new ResponseType {ResponseStatus = new CodeDescriptionType() {Code = "1"}}
            };

            PromoAcceptanceTerms acceptanceTerms = new PromoAcceptanceTerms(response);

            client.Setup(c => c.GetAgreement(It.IsAny<string>())).Returns(acceptanceTerms);

            mock.Mock<IPromoClientFactory>()
                .Setup(x => x.CreatePromoClient(It.IsAny<UpsPromo>()))
                .Returns(client.Object);

            var testObject = mock.Create<UpsPromo>(new TypedParameter(typeof(UpsAccountEntity), upsAccount));
            return testObject;
        }
    }
}
