using System;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Dhl.API.Stamps;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.DhlExpress.API.Stamps
{
    public class DhlExpressStampsLabelClientTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly DhlExpressStampsLabelClient testObject;

        public DhlExpressStampsLabelClientTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<DhlExpressStampsLabelClient>();
        }

        [Fact]
        public async Task CreateLabel_CallsWebClient()
        {
            var shipment = new ShipmentEntity();
            var response = new TelemetricResult<StampsLabelResponse>("foo");
            response.SetValue(new StampsLabelResponse());

            var webClient = mock.Mock<IDhlExpressStampsWebClient>();
            webClient.Setup(x => x.ProcessShipment(shipment)).ReturnsAsync(response);

            await testObject.Create(shipment).ConfigureAwait(false);

            webClient.Verify(x => x.ProcessShipment(shipment));
        }

        [Fact]
        public async Task CreateLabel_ReturnsStampsLabelData()
        {
            var shipment = new ShipmentEntity();
            var response = new TelemetricResult<StampsLabelResponse>("foo");
            response.SetValue(new StampsLabelResponse());

            var webClient = mock.Mock<IDhlExpressStampsWebClient>();
            webClient.Setup(x => x.ProcessShipment(shipment)).ReturnsAsync(response);

            var result = await testObject.Create(shipment).ConfigureAwait(false);

            Assert.IsAssignableFrom<StampsDownloadedLabelData>(result.Value);
        }

        [Fact]
        public void Void_CallsWebClient()
        {
            var webClient = mock.Mock<IDhlExpressStampsWebClient>();

            var shipment = new ShipmentEntity();

            testObject.Void(shipment);

            webClient.Verify(x => x.VoidShipment(shipment));
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}
