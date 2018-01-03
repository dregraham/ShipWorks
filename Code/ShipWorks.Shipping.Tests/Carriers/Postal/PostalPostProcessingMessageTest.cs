using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Tests.Shared;
using System;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Postal
{
    public class PostalPostProcessingMessageTest
    {
        private readonly AutoMock mock;
        private readonly Mock<IGlobalPostLabelNotification> globalPostNotification;
        private readonly ShipmentEntity globalPostShipment;
        private readonly ShipmentEntity gapShipment;
        private readonly PostalPostProcessingMessage testObject;
        private readonly Mock<IDateTimeProvider> dateTimeProvider;
        private readonly Mock<ICarrierAccountRepository<EndiciaAccountEntity, IEndiciaAccountEntity>> endiciaAccountRepository;
        private readonly EndiciaAccountEntity endiciaAccount;

        public PostalPostProcessingMessageTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            
            endiciaAccount = new EndiciaAccountEntity();
            endiciaAccountRepository = mock.Mock<ICarrierAccountRepository<EndiciaAccountEntity, IEndiciaAccountEntity>>();
            endiciaAccountRepository.Setup(e => e.GetAccountReadOnly(It.IsAny<ShipmentEntity>())).Returns(endiciaAccount);

            globalPostNotification = mock.Mock<IGlobalPostLabelNotification>();
            globalPostNotification.Setup(g => g.AppliesToCurrentUser()).Returns(true);
            
            dateTimeProvider = mock.Mock<IDateTimeProvider>();
            dateTimeProvider.SetupGet(d => d.Now).Returns(new DateTime(2019, 01, 01));

            globalPostShipment = new ShipmentEntity()
            {
                Postal = new PostalShipmentEntity()
                {
                    Service = (int)PostalServiceType.GlobalPostEconomyIntl
                }
            };

            gapShipment = new ShipmentEntity()
            {
                Postal = new PostalShipmentEntity()
                {
                    Service = (int)PostalServiceType.InternationalFirst,
                    CustomsContentType = (int) PostalCustomsContentType.Gift,
                    PackagingType = (int) PostalPackagingType.Envelope
                }
            };
            
            testObject = mock.Create<PostalPostProcessingMessage>();
        }

        [Fact]
        public void Show_DoesNotShowGapNotification_WhenShipmentIsEndiciaAndConsolidator()
        {
            gapShipment.ShipmentTypeCode = ShipmentTypeCode.Endicia;
            endiciaAccount.EndiciaReseller = (int) EndiciaReseller.Express1;

            testObject.Show(new[] { gapShipment });

            globalPostNotification.Verify(g => g.Show(), Times.Never);
        }

        [Fact]
        public void Show_DeligatesToEndiciaAccountRepository_WhenShipmentIsEndiciaAndGap()
        {
            gapShipment.ShipmentTypeCode = ShipmentTypeCode.Endicia;

            testObject.Show(new[] { gapShipment });

            endiciaAccountRepository.Verify(r => r.GetAccountReadOnly(gapShipment));
        }

        [Fact]
        public void Show_DeligatesToGlobalPostNotificationShow_WhenShipmentIsGap()
        {
            testObject.Show(new[] { gapShipment });
            globalPostNotification.Verify(g => g.Show());
        }

        [Fact]
        public void Show_DeligatesToGlobalPostNotificationShow_WhenShipmentIsGlobalPost()
        {
            testObject.Show(new[] { globalPostShipment });
            globalPostNotification.Verify(g => g.Show());
        }

        [Fact]
        public void Show_DeligatesToGlobalPostNotificationAppliesToCurrentUser()
        {
            testObject.Show(new[] { globalPostShipment });
            globalPostNotification.Verify(g => g.AppliesToCurrentUser());
        }

        [Fact]
        public void Show_DoesNotDeligateToGlobalPostNotificationShow_WhenAppliesToCurrentUserIsFalse()
        {
            globalPostNotification.Setup(g => g.AppliesToCurrentUser()).Returns(false);
            testObject.Show(new[] { globalPostShipment });

            globalPostNotification.Verify(g => g.Show(), Times.Never);
        }

        [Fact]
        public void Show_DoesNotDeligateToGlobalPostNotificationShow_WhenShipmentIsNotGapOrGlobalPost()
        {
            testObject.Show(
                new[] 
                {
                    new ShipmentEntity()
                    {
                        Postal = new PostalShipmentEntity()
                        {
                            Service = (int) PostalServiceType.PriorityMail
                        }
                    }
                });

            globalPostNotification.Verify(g => g.Show(), Times.Never);
        }

        [Fact]
        public void Show_DoesNotShowNotification_WhenGapLabelAndDateIsBeforeJan212018()
        {
            dateTimeProvider.SetupGet(d => d.Now).Returns(new DateTime(2017, 1, 1));

            testObject.Show(new[] { gapShipment });
            
            globalPostNotification.Verify(g => g.Show(), Times.Never);
        }

        [Fact]
        public void Show_DoesNotShowsNotificationForGapLabel_WhenLabelIsNotInternationalFirst()
        {
            gapShipment.Postal.Service = (int)PostalServiceType.FirstClass;

            testObject.Show(new[] { gapShipment });
            
            globalPostNotification.Verify(g => g.Show(), Times.Never);
        }

        [Fact]
        public void Show_DoesNotShowsNotificationForGapLabel_WhenLabelCustomsIsDocuments()
        {
            gapShipment.Postal.CustomsContentType = (int) PostalCustomsContentType.Documents;

            testObject.Show(new[] { gapShipment });

            globalPostNotification.Verify(g => g.Show(), Times.Never);
        }

        [Fact]
        public void Show_DoesNotShowsNotificationForGapLabel_WhenLabelPackageIsNotEnvelope()
        {
            gapShipment.Postal.PackagingType = (int)PostalPackagingType.Package;

            testObject.Show(new[] { gapShipment });

            globalPostNotification.Verify(g => g.Show(), Times.Never);
        }
    }
}
