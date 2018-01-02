﻿using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
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
    public class UspsPostProcessingMessageTest
    {
        private readonly AutoMock mock;
        private readonly Mock<IGlobalPostLabelNotification> globalPostNotification;
        private readonly ShipmentEntity globalPostShipment;
        private readonly ShipmentEntity gapShipment;
        private readonly UspsPostProcessingMessage testObject;

        public UspsPostProcessingMessageTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            globalPostNotification = mock.Mock<IGlobalPostLabelNotification>();

            globalPostNotification.Setup(g => g.AppliesToCurrentUser()).Returns(true);

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
            
            testObject = mock.Create<UspsPostProcessingMessage>();
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
    }
}
