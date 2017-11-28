using System;
using System.Threading.Tasks;
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
        public async Task Create_ThrowsArgumentNullException_WithNullShipment()
        {
            var testObject = mock.Create<AmazonLabelService>();

            await Assert.ThrowsAsync<ArgumentNullException>(() => testObject.Create(null));
        }

        [Fact]
        public async Task Create_ReturnsLabelData_WithShipmentAndApiResults()
        {
            var labelData = new AmazonShipment();
            mock.Mock<IAmazonCreateShipmentRequest>().Setup(x => x.Submit(It.IsAny<ShipmentEntity>()))
                .Returns(labelData);

            var response = mock.Create<AmazonDownloadedLabelData>(TypedParameter.From(defaultShipment));
            mock.Provide<Func<ShipmentEntity, AmazonShipment, AmazonDownloadedLabelData>>(
                (s, a) => s == defaultShipment && a == labelData ? response : null);

            var testObject = mock.Create<AmazonLabelService>();
            var result = await testObject.Create(defaultShipment);

            Assert.Equal(response, result);
        }

        [Fact]
        public async Task Void_Calls_IAmazonShipmentRequest()
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
