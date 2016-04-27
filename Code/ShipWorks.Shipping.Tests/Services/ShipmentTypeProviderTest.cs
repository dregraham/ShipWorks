using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Autofac.Extras.Moq;
using Interapptive.Shared.Messaging;
using Moq;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping.Services;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Services
{
    public class ShipmentTypeProviderTest : IDisposable
    {
        readonly AutoMock mock;
        readonly Subject<EnabledCarriersChangedMessage> subject;

        public ShipmentTypeProviderTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            subject = new Subject<EnabledCarriersChangedMessage>();
            mock.Provide<IObservable<IShipWorksMessage>>(subject);
        }

        [Fact]
        public void Constructor_PopulatesAvailableListOfShipments()
        {
            mock.Mock<IShipmentTypeManager>().SetupGet(x => x.EnabledShipmentTypeCodes)
                .Returns(new List<ShipmentTypeCode> { ShipmentTypeCode.Other, ShipmentTypeCode.Usps });

            ShipmentTypeProvider testObject = mock.Create<ShipmentTypeProvider>();

            Assert.Equal(2, testObject.Available.Count());
            Assert.Contains(ShipmentTypeCode.Other, testObject.Available);
            Assert.Contains(ShipmentTypeCode.Usps, testObject.Available);
        }

        [Fact]
        public void Constructor_UpdatesAvailableShipmentTypesFromManager_WhenEnabledCarriersChangedMessageReceived()
        {
            mock.Mock<IShipmentTypeManager>().SetupSequence(x => x.EnabledShipmentTypeCodes)
                .Returns(new List<ShipmentTypeCode> { ShipmentTypeCode.Other, ShipmentTypeCode.Usps })
                .Returns(new List<ShipmentTypeCode> { ShipmentTypeCode.FedEx, ShipmentTypeCode.Usps });

            ShipmentTypeProvider testObject = mock.Create<ShipmentTypeProvider>();

            subject.OnNext(new EnabledCarriersChangedMessage(null, new List<int>(), new List<int>()));

            Assert.Equal(2, testObject.Available.Count());
            Assert.Contains(ShipmentTypeCode.FedEx, testObject.Available);
            Assert.Contains(ShipmentTypeCode.Usps, testObject.Available);
        }

        [Fact]
        public void Constructor_RemovesExcludedShipmentTypes_WhenMessageContainsCarriersToExclude()
        {
            mock.Mock<IShipmentTypeManager>().SetupGet(x => x.EnabledShipmentTypeCodes)
                .Returns(new List<ShipmentTypeCode> { ShipmentTypeCode.Other, ShipmentTypeCode.Usps });

            ShipmentTypeProvider testObject = mock.Create<ShipmentTypeProvider>();

            subject.OnNext(new EnabledCarriersChangedMessage(null, new List<int>(), new List<int> { (int) ShipmentTypeCode.Other }));

            Assert.DoesNotContain(ShipmentTypeCode.Other, testObject.Available);
        }

        [Fact]
        public void Constructor_AddsShipmentTypes_WhenMessageContainsCarriersToAdd()
        {
            mock.Mock<IShipmentTypeManager>().SetupGet(x => x.EnabledShipmentTypeCodes)
                .Returns(new List<ShipmentTypeCode> { ShipmentTypeCode.Other, ShipmentTypeCode.Usps });

            ShipmentTypeProvider testObject = mock.Create<ShipmentTypeProvider>();

            subject.OnNext(new EnabledCarriersChangedMessage(null, new List<int> { (int) ShipmentTypeCode.FedEx }, new List<int>()));

            Assert.Contains(ShipmentTypeCode.FedEx, testObject.Available);
        }

        [Fact]
        public void Constructor_RemovesDuplicates_WhenAddedShipmentTypeAlreadyExistsInList()
        {
            mock.Mock<IShipmentTypeManager>().SetupGet(x => x.EnabledShipmentTypeCodes)
                .Returns(new List<ShipmentTypeCode> { ShipmentTypeCode.Other, ShipmentTypeCode.Usps });

            ShipmentTypeProvider testObject = mock.Create<ShipmentTypeProvider>();

            subject.OnNext(new EnabledCarriersChangedMessage(null, new List<int> { (int) ShipmentTypeCode.Other }, new List<int>()));

            Assert.Equal(2, testObject.Available.Count());
        }

        public void Dispose()
        {
            mock.Dispose();
            subject.Dispose();
        }
    }
}
