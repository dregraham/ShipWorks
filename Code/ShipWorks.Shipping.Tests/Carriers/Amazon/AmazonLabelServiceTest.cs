using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Shipping.Carriers.Amazon.Api;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Amazon.Mws;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Amazon
{
    public class AmazonLabelServiceTest : IDisposable
    {
        AutoMock mock = null;

        public AmazonLabelServiceTest()
        {
            mock = AutoMock.GetLoose();
        }

        [Fact]
        public void Create_ThrowsArgumentNullException_WithNullShipment()
        {
            var testObject = mock.Create<AmazonLabelService>();

            Assert.Throws<ArgumentNullException>(() => testObject.Create(null));
        }

        [Fact]
        public void Create_PopulatesOrder_WhenShipmentIsSet()
        {
            var shipment = new ShipmentEntity { Order = new AmazonOrderEntity() };

            var testObject = mock.Create<AmazonLabelService>();
            testObject.Create(shipment);

            mock.Mock<IOrderManager>()
                .Verify(x => x.PopulateOrderDetails(shipment));
        }

        [Fact]
        public void Create_ThrowsShippingException_WithNonAmazonOrder()
        {
            var testObject = mock.Create<AmazonLabelService>();

            Assert.Throws<ShippingException>(() => testObject.Create(new ShipmentEntity { Order = new OrderEntity() }));
        }

        [Fact]
        public void Create_SendsCreatedCredentialsToWebService_WhenShipmentIsSet()
        {
            var shipment = new ShipmentEntity {
                Amazon = new AmazonShipmentEntity(),
                Order = new AmazonOrderEntity()
            };
            var settings = mock.Create<AmazonMwsWebClientSettings>();

            mock.Mock<IAmazonMwsWebClientSettingsFactory>()
                .Setup(x => x.Create(shipment.Amazon))
                .Returns(settings);

            var testObject = mock.Create<AmazonLabelService>();
            testObject.Create(shipment);

            mock.Mock<IAmazonShippingWebClient>()
                .Verify(x => x.CreateShipment(It.IsAny<ShipmentRequestDetails>(), settings, It.IsAny<string>()));
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}
