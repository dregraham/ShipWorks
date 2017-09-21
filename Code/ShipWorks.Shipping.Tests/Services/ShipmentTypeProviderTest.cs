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
using Interapptive.Shared.Collections;

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
            
            mock.Mock<IShipmentTypeManager>().Verify(s => s.EnabledShipmentTypeCodes);
        }

        [Fact]
        public void Constructor_UpdatesAvailableShipmentTypesFromManager_WhenEnabledCarriersChangedMessageReceived()
        {
            mock.Mock<IShipmentTypeManager>().SetupSequence(x => x.EnabledShipmentTypeCodes)
                .Returns(new List<ShipmentTypeCode> { ShipmentTypeCode.Other, ShipmentTypeCode.Usps })
                .Returns(new List<ShipmentTypeCode> { ShipmentTypeCode.FedEx, ShipmentTypeCode.Usps });

            ShipmentTypeProvider testObject = mock.Create<ShipmentTypeProvider>();

            subject.OnNext(new EnabledCarriersChangedMessage(null, new List<ShipmentTypeCode>(), new List<ShipmentTypeCode>()));

            var carrierAdapter = mock.Mock<ICarrierShipmentAdapter>();
            carrierAdapter.SetupGet(c => c.ShipmentTypeCode).Returns(ShipmentTypeCode.Other);

            Assert.Equal(2, testObject.GetAvailableShipmentTypes(carrierAdapter.Object).Count());
            Assert.Contains(ShipmentTypeCode.FedEx, testObject.GetAvailableShipmentTypes(carrierAdapter.Object));
            Assert.Contains(ShipmentTypeCode.Usps, testObject.GetAvailableShipmentTypes(carrierAdapter.Object));
        }

        [Fact]
        public void Constructor_RemovesExcludedShipmentTypes_WhenMessageContainsCarriersToExclude()
        {
            mock.Mock<IShipmentTypeManager>().SetupGet(x => x.EnabledShipmentTypeCodes)
                .Returns(new List<ShipmentTypeCode> { ShipmentTypeCode.Other, ShipmentTypeCode.Usps });

            ShipmentTypeProvider testObject = mock.Create<ShipmentTypeProvider>();

            subject.OnNext(new EnabledCarriersChangedMessage(null, new List<ShipmentTypeCode>(), new[] { ShipmentTypeCode.Other }));

            var carrierAdapter = mock.Mock<ICarrierShipmentAdapter>();
            carrierAdapter.SetupGet(c => c.ShipmentTypeCode).Returns(ShipmentTypeCode.Other);

            Assert.DoesNotContain(ShipmentTypeCode.Other, testObject.GetAvailableShipmentTypes(carrierAdapter.Object));
        }

        [Fact]
        public void Constructor_AddsShipmentTypes_WhenMessageContainsCarriersToAdd()
        {
            mock.Mock<IShipmentTypeManager>().SetupGet(x => x.EnabledShipmentTypeCodes)
                .Returns(new List<ShipmentTypeCode> { ShipmentTypeCode.Other, ShipmentTypeCode.Usps });

            ShipmentTypeProvider testObject = mock.Create<ShipmentTypeProvider>();

            subject.OnNext(new EnabledCarriersChangedMessage(null, new[] { ShipmentTypeCode.FedEx }, new List<ShipmentTypeCode>()));

            var carrierAdapter = mock.Mock<ICarrierShipmentAdapter>();
            carrierAdapter.SetupGet(c => c.ShipmentTypeCode).Returns(ShipmentTypeCode.Other);

            Assert.Contains(ShipmentTypeCode.FedEx, testObject.GetAvailableShipmentTypes(carrierAdapter.Object));
        }

        [Fact]
        public void Constructor_RemovesDuplicates_WhenAddedShipmentTypeAlreadyExistsInList()
        {
            mock.Mock<IShipmentTypeManager>().SetupGet(x => x.EnabledShipmentTypeCodes)
                .Returns(new List<ShipmentTypeCode> { ShipmentTypeCode.Other, ShipmentTypeCode.Usps });

            ShipmentTypeProvider testObject = mock.Create<ShipmentTypeProvider>();

            subject.OnNext(new EnabledCarriersChangedMessage(null, new[] { ShipmentTypeCode.Other }, new List<ShipmentTypeCode>()));

            var carrierAdapter = mock.Mock<ICarrierShipmentAdapter>();
            carrierAdapter.SetupGet(c => c.ShipmentTypeCode).Returns(ShipmentTypeCode.Other);

            Assert.Equal(2, testObject.GetAvailableShipmentTypes(carrierAdapter.Object).Count());
        }

        [Fact]
        public void GetAvailableShipmentTypes_ReturnsOnlyAmazon_WhenShipmentTypeCodeIsAmazon()
        {
            mock.Mock<IShipmentTypeManager>().SetupGet(x => x.EnabledShipmentTypeCodes)
                .Returns(new List<ShipmentTypeCode> { ShipmentTypeCode.Other, ShipmentTypeCode.Usps });

            ShipmentTypeProvider testObject = mock.Create<ShipmentTypeProvider>();

            subject.OnNext(new EnabledCarriersChangedMessage(null, new[] { ShipmentTypeCode.Other }, new List<ShipmentTypeCode>()));

            var carrierAdapter = mock.Mock<ICarrierShipmentAdapter>();
            carrierAdapter.SetupGet(c => c.ShipmentTypeCode).Returns(ShipmentTypeCode.Amazon);

            Assert.Equal(1, testObject.GetAvailableShipmentTypes(carrierAdapter.Object).Count());
            Assert.Equal(ShipmentTypeCode.Amazon, testObject.GetAvailableShipmentTypes(carrierAdapter.Object).First());
        }


        [Fact]
        public void GetAvailableShipmentTypes_ReturnsNoneAmazon_WhenShipmentTypeCodeIsNotAmazon()
        {
            mock.Mock<IShipmentTypeManager>().SetupGet(x => x.EnabledShipmentTypeCodes)
                .Returns(new List<ShipmentTypeCode> { ShipmentTypeCode.Other, ShipmentTypeCode.Usps });

            ShipmentTypeProvider testObject = mock.Create<ShipmentTypeProvider>();

            subject.OnNext(new EnabledCarriersChangedMessage(null, new[] { ShipmentTypeCode.Other }, new List<ShipmentTypeCode>()));

            var carrierAdapter = mock.Mock<ICarrierShipmentAdapter>();
            carrierAdapter.SetupGet(c => c.ShipmentTypeCode).Returns(ShipmentTypeCode.Other);

            Assert.Equal(2, testObject.GetAvailableShipmentTypes(carrierAdapter.Object).Count());
            Assert.True(testObject.GetAvailableShipmentTypes(carrierAdapter.Object).None(c => c == ShipmentTypeCode.Amazon));
        }

        public void Dispose()
        {
            mock.Dispose();
            subject.Dispose();
        }
    }
}
