using System;
using Autofac;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Amazon
{
    public class AmazonLabelServiceTest : IDisposable
    {
        readonly ShipmentEntity defaultShipment = new ShipmentEntity
        {
            Order = new AmazonOrderEntity(),
            Amazon = new AmazonShipmentEntity { ShippingServiceID = "something", CarrierName = "Foo" }
        };

        readonly AutoMock mock;

        public AmazonLabelServiceTest()
        {
            mock = AutoMock.GetFromRepository(new MockRepository(MockBehavior.Loose) { DefaultValue = DefaultValue.Mock });
        }

        [Fact]
        public void Create_ThrowsArgumentNullException_WithNullShipment()
        {
            var testObject = mock.Create<AmazonLabelService>();

            Assert.Throws<ArgumentNullException>(() => testObject.Create(null));
        }

        [Fact]
        public void Create_ReturnsLabelData_WithShipmentAndApiResults()
        {
            var labelData = new AmazonShipment();
            mock.Mock<IAmazonCreateShipmentRequest>().Setup(x => x.Submit(It.IsAny<ShipmentEntity>()))
                .Returns(labelData);

            var response = mock.Create<AmazonDownloadedLabelData>(TypedParameter.From(defaultShipment));
            mock.Provide<Func<ShipmentEntity, AmazonShipment, AmazonDownloadedLabelData>>(
                (s, a) => s == defaultShipment && a == labelData ? response : null);

            var testObject = mock.Create<AmazonLabelService>();
            var result = testObject.Create(defaultShipment);

            Assert.Equal(response, result);
        }

        [Fact]
        public void Void_Calls_IAmazonShipmentRequest()
        {
            AmazonLabelService testObject = mock.Create<AmazonLabelService>();

            ShipmentEntity shipment = new ShipmentEntity();

            testObject.Void(shipment);

            mock.Mock<IAmazonCancelShipmentRequest>().Verify(x => x.Submit(shipment));
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}
