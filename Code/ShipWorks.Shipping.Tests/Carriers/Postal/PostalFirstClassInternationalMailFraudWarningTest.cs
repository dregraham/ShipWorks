using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Tests.Shared;
using System;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Postal
{
    public class PostalFirstClassInternationalMailFraudWarningTest
    {
        private readonly AutoMock mock;
        private readonly Mock<IMessageHelper> messageHelper;
        private readonly Mock<IDialog> dialog;
        private readonly Mock<ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>> uspsAccountRepository;
        private readonly UspsAccountEntity uspsAccountOne;
        private readonly UspsAccountEntity uspsAccountTwo;
        private readonly Mock<ICarrierAccountRepository<EndiciaAccountEntity, IEndiciaAccountEntity>> endiciaAccountRepository;
        private readonly EndiciaAccountEntity endiciaAccountOne;
        private readonly EndiciaAccountEntity endiciaAccountTwo;
        private readonly PostalFirstClassInternationalMailFraudWarning testObject;
        
        public PostalFirstClassInternationalMailFraudWarningTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            messageHelper = mock.Mock<IMessageHelper>();
            messageHelper.Setup(m => m.ShowDialog(It.IsAny<Func<IDialog>>())).Returns(true);
            dialog = mock.Mock<IDialog>();
            uspsAccountRepository = mock.Mock<ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>>();
            uspsAccountOne = new UspsAccountEntity() { AcceptedFCMILetterWarning = false, UspsAccountID = 1 };
            uspsAccountTwo = new UspsAccountEntity() { AcceptedFCMILetterWarning = false, UspsAccountID = 2 };
            uspsAccountRepository.SetupGet(a => a.AccountsReadOnly).Returns(new[] { uspsAccountOne, uspsAccountTwo });
            uspsAccountRepository.SetupGet(a => a.Accounts).Returns(new[] { uspsAccountOne, uspsAccountTwo });
            uspsAccountRepository.Setup(a => a.GetAccount(It.IsAny<ShipmentEntity>())).Returns(uspsAccountOne);

            endiciaAccountRepository = mock.Mock<ICarrierAccountRepository<EndiciaAccountEntity, IEndiciaAccountEntity>>();
            endiciaAccountOne = new EndiciaAccountEntity() { AcceptedFCMILetterWarning = false, EndiciaAccountID = 1 };
            endiciaAccountTwo = new EndiciaAccountEntity() { AcceptedFCMILetterWarning = false, EndiciaAccountID = 2 };
            endiciaAccountRepository.SetupGet(a => a.AccountsReadOnly).Returns(new[] { endiciaAccountOne, endiciaAccountTwo });
            endiciaAccountRepository.SetupGet(a => a.Accounts).Returns(new[] { endiciaAccountOne, endiciaAccountTwo });
            endiciaAccountRepository.Setup(a => a.GetAccount(It.IsAny<ShipmentEntity>())).Returns(endiciaAccountOne);
            endiciaAccountRepository.Setup(a => a.GetAccountReadOnly(It.IsAny<ShipmentEntity>())).Returns(endiciaAccountOne);

            testObject = mock.Create<PostalFirstClassInternationalMailFraudWarning>();
        }

        [Fact]
        public void ShowWarningIfApplicable_DelegatesToMessageHelperShowDialog_WhenShipmentIsUSpsAndMeetsConditionsForShowingWarning()
        {
            var shipment = new ShipmentEntity()
            {
                ShipmentTypeCode = ShipmentTypeCode.Usps,
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

            testObject.ShowWarningIfApplicable(shipment);
            messageHelper.Verify(m => m.ShowDialog(It.IsAny<Func<IDialog>>()));
        }

        [Fact]
        public void ShowWarningIfApplicable_DelegatesToAccountRepoForAllAccounts_WhenShipmentIsUSpsAndIsRateShop()
        {
            var shipment = new ShipmentEntity()
            {
                ShipmentTypeCode = ShipmentTypeCode.Usps,
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

            testObject.ShowWarningIfApplicable(shipment);
            uspsAccountRepository.Verify(a => a.AccountsReadOnly);
        }

        [Fact]
        public void ShowWarningIfApplicable_DelegatesToAccountRepoForShipmentsAccount_WhenShipmentIsUSpsAndIsRateShopIsDisabled()
        {
            var shipment = new ShipmentEntity()
            {
                ShipmentTypeCode = ShipmentTypeCode.Usps,
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

            testObject.ShowWarningIfApplicable(shipment);
            uspsAccountRepository.Verify(a => a.GetAccountReadOnly(shipment));
        }

        [Fact]
        public void ShowWarningIfApplicable_ThrowsShippingException_WhenShipmentIsUSpsAndShowDialogReturnsFalse()
        {
            messageHelper.Setup(m => m.ShowDialog(It.IsAny<Func<IDialog>>())).Returns(false);

            var shipment = new ShipmentEntity()
            {
                ShipmentTypeCode = ShipmentTypeCode.Usps,
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

            ShippingException ex = Assert.Throws<ShippingException>(() => testObject.ShowWarningIfApplicable(shipment));
            Assert.Equal("Please update the customs general Content: type, the shipment Packaging: type, and/or the Service: type prior to processing the shipment.", ex.Message);
        }

        [Fact]
        public void ShowWarningIfApplicable_SetsAcceptedFCMILetterWarningToTrueForAllAccounts_WhenShipmentIsUSpsAndUserAcceptsWarningAndRateShoppingIsEnabled()
        {
            uspsAccountOne.AcceptedFCMILetterWarning = false;
            messageHelper.Setup(m => m.ShowDialog(It.IsAny<Func<IDialog>>())).Returns(true);

            var shipment = new ShipmentEntity()
            {
                ShipmentTypeCode = ShipmentTypeCode.Usps,
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

            testObject.ShowWarningIfApplicable(shipment);
            uspsAccountRepository.VerifyGet(r => r.Accounts);
            Assert.True(uspsAccountOne.AcceptedFCMILetterWarning);
        }

        [Fact]
        public void ShowWarningIfApplicable_SetsAcceptedFCMILetterWarningToTrueForShipmentsAccount_WhenShipmentIsUspsAndUserAcceptsWarningAndRateShoppingIsDisabled()
        {
            uspsAccountOne.AcceptedFCMILetterWarning = false;
            messageHelper.Setup(m => m.ShowDialog(It.IsAny<Func<IDialog>>())).Returns(true);

            var shipment = new ShipmentEntity()
            {
                ShipmentTypeCode = ShipmentTypeCode.Usps,
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

            testObject.ShowWarningIfApplicable(shipment);
            uspsAccountRepository.Verify(a => a.GetAccount(shipment));
            Assert.True(uspsAccountOne.AcceptedFCMILetterWarning);
        }
        [Fact]
        public void ShowWarningIfApplicable_DelegatesToMessageHelperShowDialog_WhenShipmentIsEndiciaAndMeetsConditionsForShowingWarning()
        {
            var shipment = new ShipmentEntity()
            {
                ShipmentTypeCode = ShipmentTypeCode.Endicia,
                Postal = new PostalShipmentEntity()
                {
                    Service = (int)PostalServiceType.InternationalFirst,
                    CustomsContentType = (int)PostalCustomsContentType.Documents,
                    PackagingType = (int)PostalPackagingType.Envelope
                },
            };

            testObject.ShowWarningIfApplicable(shipment);
            messageHelper.Verify(m => m.ShowDialog(It.IsAny<Func<IDialog>>()));
        }

        [Fact]
        public void ShowWarningIfApplicable_DelegatesToAccountRepoForAllAccounts_WhenShipmentIsEndiciaAndIsRateShop()
        {
            var shipment = new ShipmentEntity()
            {
                ShipmentTypeCode = ShipmentTypeCode.Endicia,
                Postal = new PostalShipmentEntity()
                {
                    Service = (int)PostalServiceType.InternationalFirst,
                    CustomsContentType = (int)PostalCustomsContentType.Documents,
                    PackagingType = (int)PostalPackagingType.Envelope
                },
            };

            testObject.ShowWarningIfApplicable(shipment);
            endiciaAccountRepository.Verify(a => a.GetAccountReadOnly(shipment));
        }

        [Fact]
        public void ShowWarningIfApplicable_DelegatesToAccountRepoForShipmentsAccount_WhenShipmentIsEndiciaAndIsRateShopIsDisabled()
        {
            var shipment = new ShipmentEntity()
            {
                ShipmentTypeCode = ShipmentTypeCode.Endicia,
                Postal = new PostalShipmentEntity()
                {
                    Service = (int)PostalServiceType.InternationalFirst,
                    CustomsContentType = (int)PostalCustomsContentType.Documents,
                    PackagingType = (int)PostalPackagingType.Envelope
                },
            };

            testObject.ShowWarningIfApplicable(shipment);
            endiciaAccountRepository.Verify(a => a.GetAccountReadOnly(shipment));
        }

        [Fact]
        public void ShowWarningIfApplicable_ThrowsShippingException_WhenShipmentIsEndiciaAndShowDialogReturnsFalse()
        {
            messageHelper.Setup(m => m.ShowDialog(It.IsAny<Func<IDialog>>())).Returns(false);

            var shipment = new ShipmentEntity()
            {
                ShipmentTypeCode = ShipmentTypeCode.Endicia,
                Postal = new PostalShipmentEntity()
                {
                    Service = (int)PostalServiceType.InternationalFirst,
                    CustomsContentType = (int)PostalCustomsContentType.Documents,
                    PackagingType = (int)PostalPackagingType.Envelope
                },
            };

            ShippingException ex = Assert.Throws<ShippingException>(() => testObject.ShowWarningIfApplicable(shipment));
            Assert.Equal("Please update the customs general Content: type, the shipment Packaging: type, and/or the Service: type prior to processing the shipment.", ex.Message);
        }

        [Fact]
        public void ShowWarningIfApplicable_SetsAcceptedFCMILetterWarningToTrueForShipmentsAccount_WhenShipmentIsEndiciaAndUserAcceptsWarningAndRateShoppingIsDisabled()
        {
            endiciaAccountOne.AcceptedFCMILetterWarning = false;
            messageHelper.Setup(m => m.ShowDialog(It.IsAny<Func<IDialog>>())).Returns(true);

            var shipment = new ShipmentEntity()
            {
                ShipmentTypeCode = ShipmentTypeCode.Endicia,
                Postal = new PostalShipmentEntity()
                {
                    Service = (int)PostalServiceType.InternationalFirst,
                    CustomsContentType = (int)PostalCustomsContentType.Documents,
                    PackagingType = (int)PostalPackagingType.Envelope
                },
            };

            testObject.ShowWarningIfApplicable(shipment);
            endiciaAccountRepository.Verify(a => a.GetAccount(shipment));
            Assert.True(endiciaAccountOne.AcceptedFCMILetterWarning);
        }

        [Fact]
        public void ShowWarningIfApplicable_DoesNotShowWarning_WhenShipmentIsEndiciaAndConsolidator()
        {
            endiciaAccountOne.AcceptedFCMILetterWarning = false;
            endiciaAccountOne.EndiciaReseller = (int) EndiciaReseller.Express1;
            messageHelper.Setup(m => m.ShowDialog(It.IsAny<Func<IDialog>>())).Returns(true);

            var shipment = new ShipmentEntity()
            {
                ShipmentTypeCode = ShipmentTypeCode.Endicia,
                Postal = new PostalShipmentEntity()
                {
                    Service = (int)PostalServiceType.InternationalFirst,
                    CustomsContentType = (int)PostalCustomsContentType.Documents,
                    PackagingType = (int)PostalPackagingType.Envelope
                },
            };

            testObject.ShowWarningIfApplicable(shipment);

            messageHelper.Verify(m => m.ShowDialog(It.IsAny<Func<IDialog>>()), Times.Never);
        }
    }
}
