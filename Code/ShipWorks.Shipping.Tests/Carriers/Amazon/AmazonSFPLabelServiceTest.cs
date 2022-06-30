using System;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.SFP;
using ShipWorks.Shipping.Carriers.Amazon.SFP.Platform;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Amazon
{
    public class AmazonSFPLabelServiceTest : IDisposable
    {
        private TelemetricResult<IDownloadedLabelData> telemetricResult = new TelemetricResult<IDownloadedLabelData>("testing");

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
            var labelData = mock.Create<AmazonSfpShipEngineDownloadedLabelData>();
            telemetricResult.SetValue(labelData);
            
            mock.Mock<IAmazonSfpLabelClient>()
                .Setup(l => l.Create(It.IsAny<ShipmentEntity>()))
                .ReturnsAsync(telemetricResult);

            var testObject = mock.Create<AmazonSFPLabelService>();
            var result = await testObject.Create(defaultShipment);

            Assert.True(result.Value is AmazonSfpShipEngineDownloadedLabelData);
        }

        [Fact]
        public void Void_Calls_AmazonSFPLabelService()
        {
            AmazonSFPLabelService testObject = mock.Create<AmazonSFPLabelService>();

            ShipmentEntity shipment = new ShipmentEntity();

            testObject.Void(shipment);

            mock.Mock<IAmazonSfpLabelClient>().Verify(x => x.Void(shipment), Times.Once);
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}
