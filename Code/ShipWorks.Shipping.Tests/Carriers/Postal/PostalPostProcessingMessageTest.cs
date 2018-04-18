using System;
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
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

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
        private readonly Mock<IReadOnlyCarrierAccountRetriever<IEndiciaAccountEntity>> endiciaAccountRepository;
        private readonly EndiciaAccountEntity endiciaAccount;

        public PostalPostProcessingMessageTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            endiciaAccount = new EndiciaAccountEntity();
            endiciaAccountRepository = mock.Mock<IReadOnlyCarrierAccountRetriever<IEndiciaAccountEntity>>();
            endiciaAccountRepository.Setup(e => e.GetAccountReadOnly(AnyIShipment)).Returns(endiciaAccount);

            globalPostNotification = mock.Mock<IGlobalPostLabelNotification>();
            globalPostNotification.Setup(g => g.AppliesToCurrentUser()).Returns(true);

            dateTimeProvider = mock.Mock<IDateTimeProvider>();
            dateTimeProvider.SetupGet(d => d.Now).Returns(new DateTime(2019, 01, 01));

            globalPostShipment = new ShipmentEntity()
            {
                Processed = true,
                Postal = new PostalShipmentEntity()
                {
                    Service = (int) PostalServiceType.GlobalPostEconomyIntl
                }
            };

            gapShipment = new ShipmentEntity()
            {
                Processed = true,
                Postal = new PostalShipmentEntity()
                {
                    Service = (int) PostalServiceType.InternationalFirst,
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

            globalPostNotification.Verify(g => g.Show(gapShipment), Times.Never);
        }

        [Fact]
        public void Show_DelegatesToEndiciaAccountRepository_WhenShipmentIsEndiciaAndGap()
        {
            gapShipment.ShipmentTypeCode = ShipmentTypeCode.Endicia;

            testObject.Show(new[] { gapShipment });

            endiciaAccountRepository.Verify(r => r.GetAccountReadOnly(gapShipment));
        }

        [Fact]
        public void Show_DelegatesToGlobalPostNotificationShow_WhenShipmentIsGap()
        {
            testObject.Show(new[] { gapShipment });
            globalPostNotification.Verify(g => g.Show(gapShipment));
        }

        [Fact]
        public void Show_DelegatesToGlobalPostNotificationShow_WhenShipmentIsGlobalPost()
        {
            testObject.Show(new[] { globalPostShipment });
            globalPostNotification.Verify(g => g.Show(globalPostShipment));
        }

        [Fact]
        public void Show_DelegatesToGlobalPostNotificationAppliesToCurrentUser()
        {
            testObject.Show(new[] { globalPostShipment });
            globalPostNotification.Verify(g => g.AppliesToCurrentUser());
        }

        [Fact]
        public void Show_DoesNotDelegateToGlobalPostNotificationShow_WhenAppliesToCurrentUserIsFalse()
        {
            globalPostNotification.Setup(g => g.AppliesToCurrentUser()).Returns(false);
            testObject.Show(new[] { globalPostShipment });

            globalPostNotification.Verify(g => g.Show(globalPostShipment), Times.Never);
        }

        [Theory]
        [InlineData(PostalServiceType.InternationalFirst, GlobalPostServiceAvailability.InternationalFirst)]
        [InlineData(PostalServiceType.InternationalPriority, GlobalPostServiceAvailability.InternationalPriority)]
        [InlineData(PostalServiceType.InternationalExpress, GlobalPostServiceAvailability.InternationalExpress)]
        public void Show_DelegatesToGlobalPostNotificationShow_BasedOnShipmentAndSettings(PostalServiceType service, GlobalPostServiceAvailability availability)
        {
            var shipment = Create.Shipment()
                .AsPostal(p => p.AsUsps().Set(x => x.Service = (int) service))
                .Set(x => x.Processed = true)
                .Build();

            mock.Mock<IReadOnlyCarrierAccountRetriever<IUspsAccountEntity>>()
                .Setup(x => x.GetAccountReadOnly(shipment))
                .Returns(new UspsAccountEntity { GlobalPostAvailability = (int) availability });

            testObject.Show(new[] { shipment });

            globalPostNotification.Verify(g => g.Show(shipment));
        }

        [Fact]
        public void Show_DoesNotDelegateToGlobalPostNotificationShow_WhenShipmentIsNotGapOrGlobalPost()
        {
            var shipment = Create.Shipment()
                .AsPostal(p => p.Set(x => x.Service = (int) PostalServiceType.PriorityMail))
                .Set(x => x.Processed = true).Build();

            testObject.Show(new[] { shipment });

            globalPostNotification.Verify(g => g.Show(AnyIShipment), Times.Never);
        }

        [Fact]
        public void Show_DoesNotShowNotification_WhenGapLabelAndDateIsBeforeJan212018()
        {
            dateTimeProvider.SetupGet(d => d.Now).Returns(new DateTime(2017, 1, 1));

            testObject.Show(new[] { gapShipment });

            globalPostNotification.Verify(g => g.Show(gapShipment), Times.Never);
        }

        [Fact]
        public void Show_DoesNotShowsNotificationForGapLabel_WhenLabelIsNotInternationalFirst()
        {
            gapShipment.Postal.Service = (int) PostalServiceType.FirstClass;

            testObject.Show(new[] { gapShipment });

            globalPostNotification.Verify(g => g.Show(gapShipment), Times.Never);
        }

        [Fact]
        public void Show_DoesNotShowsNotificationForGapLabel_WhenLabelCustomsIsDocuments()
        {
            gapShipment.Postal.CustomsContentType = (int) PostalCustomsContentType.Documents;

            testObject.Show(new[] { gapShipment });

            globalPostNotification.Verify(g => g.Show(gapShipment), Times.Never);
        }

        [Fact]
        public void Show_DoesNotShowsNotificationForGapLabel_WhenLabelPackageIsNotEnvelope()
        {
            gapShipment.Postal.PackagingType = (int) PostalPackagingType.Package;

            testObject.Show(new[] { gapShipment });

            globalPostNotification.Verify(g => g.Show(gapShipment), Times.Never);
        }
    }
}
