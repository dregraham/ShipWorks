using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using ShipWorks.Core.Messaging;
using Moq;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping.Services;
using ShipWorks.Tests.Shared;
using Xunit;
using System.Reactive.Subjects;

namespace ShipWorks.Shipping.Tests.Services
{
    public class ShipmentTypeProviderTest
    {
        [Fact]
        public void Constructor_PopulatesAvailableListOfShipments()
        {
            using (AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                mock.Mock<IShipmentTypeManager>().SetupGet(x => x.EnabledShipmentTypeCodes)
                    .Returns(new List<ShipmentTypeCode> { ShipmentTypeCode.Other, ShipmentTypeCode.Usps });

                ShipmentTypeProvider testObject = mock.Create<ShipmentTypeProvider>();

                Assert.Equal(2, testObject.Available.Count());
                Assert.Contains(ShipmentTypeCode.Other, testObject.Available);
                Assert.Contains(ShipmentTypeCode.Usps, testObject.Available);
            }
        }

        [Fact]
        public void Constructor_UpdatesAvailableShipmentTypesFromManager_WhenEnabledCarriersChangedMessageReceived()
        {
            using (AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                Subject<EnabledCarriersChangedMessage> subject = new Subject<EnabledCarriersChangedMessage>();

                mock.Mock<IShipmentTypeManager>().SetupSequence(x => x.EnabledShipmentTypeCodes)
                    .Returns(new List<ShipmentTypeCode> { ShipmentTypeCode.Other, ShipmentTypeCode.Usps })
                    .Returns(new List<ShipmentTypeCode> { ShipmentTypeCode.FedEx, ShipmentTypeCode.Usps });
                mock.Mock<IMessenger>()
                    .Setup(x => x.AsObservable<EnabledCarriersChangedMessage>())
                    .Returns(subject);

                ShipmentTypeProvider testObject = mock.Create<ShipmentTypeProvider>();

                subject.OnNext(new EnabledCarriersChangedMessage(null, new List<int>(), new List<int>()));

                Assert.Equal(2, testObject.Available.Count());
                Assert.Contains(ShipmentTypeCode.FedEx, testObject.Available);
                Assert.Contains(ShipmentTypeCode.Usps, testObject.Available);
            }
        }

        [Fact]
        public void Constructor_RemovesExcludedShipmentTypes_WhenMessageContainsCarriersToExclude()
        {
            using (AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                Subject<EnabledCarriersChangedMessage> subject = new Subject<EnabledCarriersChangedMessage>();

                mock.Mock<IShipmentTypeManager>().SetupGet(x => x.EnabledShipmentTypeCodes)
                    .Returns(new List<ShipmentTypeCode> { ShipmentTypeCode.Other, ShipmentTypeCode.Usps });
                mock.Mock<IMessenger>()
                    .Setup(x => x.AsObservable<EnabledCarriersChangedMessage>())
                    .Returns(subject);

                ShipmentTypeProvider testObject = mock.Create<ShipmentTypeProvider>();

                subject.OnNext(new EnabledCarriersChangedMessage(null, new List<int>(), new List<int> { (int)ShipmentTypeCode.Other }));

                Assert.DoesNotContain(ShipmentTypeCode.Other, testObject.Available);
            }
        }

        [Fact]
        public void Constructor_AddsShipmentTypes_WhenMessageContainsCarriersToAdd()
        {
            using (AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                using (Subject<EnabledCarriersChangedMessage> subject = new Subject<EnabledCarriersChangedMessage>())
                {
                    mock.Mock<IShipmentTypeManager>().SetupGet(x => x.EnabledShipmentTypeCodes)
                        .Returns(new List<ShipmentTypeCode> { ShipmentTypeCode.Other, ShipmentTypeCode.Usps });
                    mock.Mock<IMessenger>()
                        .Setup(x => x.AsObservable<EnabledCarriersChangedMessage>())
                        .Returns(subject);

                    ShipmentTypeProvider testObject = mock.Create<ShipmentTypeProvider>();

                    subject.OnNext(new EnabledCarriersChangedMessage(null, new List<int> { (int)ShipmentTypeCode.FedEx }, new List<int>()));

                    Assert.Contains(ShipmentTypeCode.FedEx, testObject.Available);
                }
            }
        }

        [Fact]
        public void Constructor_RemovesDuplicates_WhenAddedShipmentTypeAlreadyExistsInList()
        {
            using (AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                Subject<EnabledCarriersChangedMessage> subject = new Subject<EnabledCarriersChangedMessage>();

                mock.Mock<IShipmentTypeManager>().SetupGet(x => x.EnabledShipmentTypeCodes)
                    .Returns(new List<ShipmentTypeCode> { ShipmentTypeCode.Other, ShipmentTypeCode.Usps });
                mock.Mock<IMessenger>()
                    .Setup(x => x.AsObservable<EnabledCarriersChangedMessage>())
                    .Returns(subject);

                ShipmentTypeProvider testObject = mock.Create<ShipmentTypeProvider>();

                subject.OnNext(new EnabledCarriersChangedMessage(null, new List<int> { (int)ShipmentTypeCode.Other }, new List<int>()));

                Assert.Equal(2, testObject.Available.Count());
            }
        }
    }
}
