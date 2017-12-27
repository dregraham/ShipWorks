using Autofac.Extras.Moq;
using Autofac.Features.Indexed;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Postal.Usps
{
    public class UspsTermsAndConditionsTest
    {
        private readonly AutoMock mock;
        private readonly UspsTermsAndConditions testObject;
        private readonly Mock<IIndex<ShipmentTypeCode, IUspsShipmentType>> shipmentTypeRepo;
        private readonly AccountInfoV25 accountInfo;
        private readonly Mock<IUspsWebClient> webClient;
        private readonly Mock<IUspsShipmentType> uspsShipmentType;
        private readonly Mock<ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>> accountRepo;
        private readonly UspsAccountEntity uspsAccount;

        public UspsTermsAndConditionsTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            uspsAccount = new UspsAccountEntity() { UspsAccountID = 123 };

            accountRepo = mock.Mock<ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>>();
            accountRepo.Setup(r => r.GetAccount(123)).Returns(uspsAccount);

            accountInfo = new AccountInfoV25()
            {
                Terms = new Terms()
                {
                    TermsAR = true,
                    TermsSL = true,
                    TermsGP = true
                }
            };

            webClient = mock.Mock<IUspsWebClient>();
            webClient.Setup(w => w.GetAccountInfo(It.IsAny<UspsAccountEntity>()))
                .Returns(accountInfo);

            uspsShipmentType = mock.Mock<IUspsShipmentType>();
            uspsShipmentType.Setup(s => s.CreateWebClient()).Returns(webClient);

            shipmentTypeRepo = mock.MockRepository.Create<IIndex<ShipmentTypeCode, IUspsShipmentType>>();
            shipmentTypeRepo.Setup(x => x[ShipmentTypeCode.Usps])
                .Returns(uspsShipmentType.Object);
            mock.Provide(shipmentTypeRepo.Object);

            testObject = mock.Create<UspsTermsAndConditions>();
        }

        [Fact]
        public void Validate_GetsUspsAccountFromRepoUsingAccountId()
        {
            var shipment = new ShipmentEntity()
            {
                Postal = new PostalShipmentEntity()
                {
                    Usps = new UspsShipmentEntity()
                    {
                        UspsAccountID = 123
                    }
                }
            };

            testObject.Validate(shipment);
            accountRepo.Verify(r => r.GetAccount(123));
        }

        [Fact]
        public void Validate_GetsUpspAccountInfoFromWebClient()
        {
            var shipment = new ShipmentEntity()
            {
                Postal = new PostalShipmentEntity()
                {
                    Usps = new UspsShipmentEntity()
                    {
                        UspsAccountID = 123
                    }
                }
            };

            testObject.Validate(shipment);
            webClient.Verify(w => w.GetAccountInfo(uspsAccount));
        }

        [Fact]
        public void Validate_ThrowsUspsTermsAndConditionsException_WhenTermsAreNotAccepted()
        {
            var shipment = new ShipmentEntity()
            {
                Postal = new PostalShipmentEntity()
                {
                    Usps = new UspsShipmentEntity()
                    {
                        UspsAccountID = 123
                    }
                }
            };

            accountInfo.Terms.TermsAR = false;
            Assert.Throws<UspsTermsAndConditionsException>(() => testObject.Validate(shipment));
        }

        [Fact]
        public void Show_DoesNothing_NoTermsAndConditionsRequiredAccountsExist()
        {
            testObject.Show();
        }
    }
}
