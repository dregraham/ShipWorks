using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Tests.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Postal.Usps
{
    public class UspsFirstClassInternationalShipmentValidatorTest
    {
        private readonly AutoMock mock;
        private readonly Mock<IMessageHelper> messageHelper;
        private readonly Mock<IDialog> dialog;
        private readonly Mock<ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>> accountRepository;
        private readonly PostalFirstClassInternationalShipmentValidator testObject;
        private readonly UspsAccountEntity accountOne;
        private readonly UspsAccountEntity accountTwo;

        public UspsFirstClassInternationalShipmentValidatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            messageHelper = mock.Mock<IMessageHelper>();
            messageHelper.Setup(m => m.ShowDialog(It.IsAny<Func<IDialog>>())).Returns(true);
            dialog = mock.Mock<IDialog>();
            accountRepository = mock.Mock<ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>>();
            accountOne = new UspsAccountEntity() { AcceptedFCMILetterWarning = false, UspsAccountID = 1 };
            accountTwo = new UspsAccountEntity() { AcceptedFCMILetterWarning = false, UspsAccountID = 2 };
            accountRepository.SetupGet(a => a.AccountsReadOnly).Returns(new[] { accountOne, accountTwo });
            accountRepository.SetupGet(a => a.Accounts).Returns(new[] { accountOne, accountTwo });
            accountRepository.Setup(a => a.GetAccount(It.IsAny<ShipmentEntity>())).Returns(accountOne);

            testObject = mock.Create<PostalFirstClassInternationalShipmentValidator>();
        }

        [Fact]
        public void ValidateShipment_DelegatesToMessageHelperShowDialog_WhenShipmentMeetsConditionsForShowingWarning()
        {
            var shipment = new ShipmentEntity()
            {
                Postal = new PostalShipmentEntity()
                {
                    Service = (int)PostalServiceType.InternationalFirst,
                    CustomsContentType = (int) PostalCustomsContentType.Documents,
                    PackagingType = (int) PostalPackagingType.Envelope,
                    Usps = new UspsShipmentEntity()
                    {
                        RateShop = true
                    }
                },
            };

            testObject.ValidateShipment(shipment);
            messageHelper.Verify(m => m.ShowDialog(It.IsAny<Func<IDialog>>()));
        }

        [Fact]
        public void ValidateShipment_DelegatesToAccountRepoForAllAccounts_WhenShipmentIsRateShop()
        {
            var shipment = new ShipmentEntity()
            {
                Postal = new PostalShipmentEntity()
                {
                    Service = (int)PostalServiceType.InternationalFirst,
                    CustomsContentType = (int)PostalCustomsContentType.Documents,
                    PackagingType = (int)PostalPackagingType.Envelope,
                    Usps = new UspsShipmentEntity()
                    {
                        RateShop = true
                    }
                },
            };

            testObject.ValidateShipment(shipment);
            accountRepository.Verify(a => a.AccountsReadOnly);
        }

        [Fact]
        public void ValidateShipment_DelegatesToAccountRepoForShipmentsAccount_WhenShipmentIsRateShopIsDisabled()
        {
            var shipment = new ShipmentEntity()
            {
                Postal = new PostalShipmentEntity()
                {
                    Service = (int)PostalServiceType.InternationalFirst,
                    CustomsContentType = (int)PostalCustomsContentType.Documents,
                    PackagingType = (int)PostalPackagingType.Envelope,
                    Usps = new UspsShipmentEntity()
                    {
                        RateShop = false
                    }
                },
            };

            testObject.ValidateShipment(shipment);
            accountRepository.Verify(a => a.GetAccountReadOnly(shipment));
        }

        [Fact]
        public void ValidateShipment_ThrowsShippingException_WhenShowDialogReturnsFalse()
        {
            messageHelper.Setup(m => m.ShowDialog(It.IsAny<Func<IDialog>>())).Returns(false);

            var shipment = new ShipmentEntity()
            {
                Postal = new PostalShipmentEntity()
                {
                    Service = (int)PostalServiceType.InternationalFirst,
                    CustomsContentType = (int)PostalCustomsContentType.Documents,
                    PackagingType = (int)PostalPackagingType.Envelope,
                    Usps = new UspsShipmentEntity()
                    {
                        RateShop = false
                    }
                },
            };

            ShippingException ex = Assert.Throws<ShippingException>(() => testObject.ValidateShipment(shipment));
            Assert.Equal("Please update the customs general Content: type, the shipment Packaging: type, and/or the Service: type prior to processing the shipment.", ex.Message);
        }

        [Fact]
        public void ValidateShipment_SetsAcceptedFCMILetterWarningToTrueForAllAccounts_WhenUserAcceptsWarningAndRateShoppingIsEnabled()
        {
            accountOne.AcceptedFCMILetterWarning = false;
            messageHelper.Setup(m => m.ShowDialog(It.IsAny<Func<IDialog>>())).Returns(true);

            var shipment = new ShipmentEntity()
            {
                Postal = new PostalShipmentEntity()
                {
                    Service = (int)PostalServiceType.InternationalFirst,
                    CustomsContentType = (int)PostalCustomsContentType.Documents,
                    PackagingType = (int)PostalPackagingType.Envelope,
                    Usps = new UspsShipmentEntity()
                    {
                        RateShop = true
                    }
                },
            };

            testObject.ValidateShipment(shipment);
            accountRepository.VerifyGet(r => r.Accounts);
            Assert.True(accountOne.AcceptedFCMILetterWarning);
        }

        [Fact]
        public void ValidateShipment_SetsAcceptedFCMILetterWarningToTrueForShipmentsAccount_WhenUserAcceptsWarningAndRateShoppingIsDisabled()
        {
            accountOne.AcceptedFCMILetterWarning = false;
            messageHelper.Setup(m => m.ShowDialog(It.IsAny<Func<IDialog>>())).Returns(true);

            var shipment = new ShipmentEntity()
            {
                Postal = new PostalShipmentEntity()
                {
                    Service = (int)PostalServiceType.InternationalFirst,
                    CustomsContentType = (int)PostalCustomsContentType.Documents,
                    PackagingType = (int)PostalPackagingType.Envelope,
                    Usps = new UspsShipmentEntity()
                    {
                        RateShop = false
                    }
                },
            };

            testObject.ValidateShipment(shipment);
            accountRepository.Verify(a => a.GetAccount(shipment));
            Assert.True(accountOne.AcceptedFCMILetterWarning);
        }
    }
}
