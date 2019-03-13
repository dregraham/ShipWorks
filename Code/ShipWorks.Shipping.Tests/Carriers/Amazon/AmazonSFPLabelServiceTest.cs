using System;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.SFP;
using ShipWorks.Shipping.Carriers.Amazon.SFP.Api.DTOs;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Amazon
{
    public class AmazonSFPLabelServiceTest : IDisposable
    {
        readonly ShipmentEntity defaultShipment = new ShipmentEntity
        {
            Order = new AmazonOrderEntity(),
            AmazonSFP = new AmazonSFPShipmentEntity { ShippingServiceID = "something", CarrierName = "Foo" }
        };

        readonly AutoMock mock;

        public AmazonSFPLabelServiceTest()
        {
            mock = AutoMock.GetFromRepository(new MockRepository(MockBehavior.Loose) { DefaultValue = DefaultValue.Mock });
        }

        [Fact]
        public async Task Create_ThrowsArgumentNullException_WithNullShipment()
        {
            var testObject = mock.Create<AmazonSFPLabelService>();

            await Assert.ThrowsAsync<ArgumentNullException>(() => testObject.Create(null));
        }

        [Fact]
        public async Task Create_ReturnsLabelData_WithShipmentAndApiResults()
        {
            var labelData = new AmazonShipment();
            mock.Mock<IAmazonSFPCreateShipmentRequest>().Setup(x => x.Submit(It.IsAny<ShipmentEntity>()))
                .Returns(labelData);

            var response = mock.Create<AmazonSFPDownloadedLabelData>(TypedParameter.From(defaultShipment));
            mock.Provide<Func<ShipmentEntity, AmazonShipment, AmazonSFPDownloadedLabelData>>(
                (s, a) => s == defaultShipment && a == labelData ? response : null);

            var testObject = mock.Create<AmazonSFPLabelService>();
            var result = await testObject.Create(defaultShipment);

            Assert.Equal(response, result.Value);
        }

        [Fact]
        public void Void_Calls_IAmazonShipmentRequest()
        {
            AmazonSFPLabelService testObject = mock.Create<AmazonSFPLabelService>();

            ShipmentEntity shipment = new ShipmentEntity();

            testObject.Void(shipment);

            mock.Mock<IAmazonSFPCancelShipmentRequest>().Verify(x => x.Submit(shipment));
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}
