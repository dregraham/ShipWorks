using System;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Shipping.Carriers.Amazon.Api;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Stores.Platforms.Amazon;
using ShipWorks.Stores.Platforms.Amazon.Mws;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Amazon
{
    public class AmazonCreateShipmentRequestTest : IDisposable
    {
        readonly AutoMock mock;
        readonly ShipmentEntity defaultShipment = new ShipmentEntity
        {
            Order = new AmazonOrderEntity(),
            Amazon = new AmazonShipmentEntity { ShippingServiceID = "something", CarrierName = "Foo" }
        };

        public AmazonCreateShipmentRequestTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            mock.Mock<IAmazonShipmentRequestDetailsFactory>()
                .Setup(x => x.Create(It.IsAny<ShipmentEntity>(), It.IsAny<IAmazonOrder>()))
                .Returns(new ShipmentRequestDetails { ShippingServiceOptions = new ShippingServiceOptions { DeclaredValue = new DeclaredValue() } });
        }

        [Theory]
        [InlineData("STAMPS_DOT_COM")]
        [InlineData("USPS")]
        public void Create_SetsDeclaredValueToZero_WhenCarrierIsUSPS(string carrier)
        {
            defaultShipment.Amazon.InsuranceValue = 65;
            defaultShipment.Amazon.CarrierName = carrier;

            var testObject = mock.Create<AmazonCreateShipmentRequest>();
            testObject.Submit(defaultShipment);

            mock.Mock<IAmazonShippingWebClient>()
                .Verify(x => x.CreateShipment(
                    It.Is<ShipmentRequestDetails>(s => s.ShippingServiceOptions.DeclaredValue.Amount == 0),
                    defaultShipment.Amazon));
        }

        [Theory]
        [InlineData("FEDEX")]
        [InlineData("UPS")]
        [InlineData("DHLMX")]
        [InlineData("DHLM")]
        [InlineData("DYNAMEX")]
        public void Create_SetsDeclaredValueToSpecifiedValue_WhenCarrierIsNotUSPS(string carrier)
        {
            defaultShipment.Amazon.InsuranceValue = 65;
            defaultShipment.Amazon.CarrierName = carrier;

            var testObject = mock.Create<AmazonCreateShipmentRequest>();
            testObject.Submit(defaultShipment);

            mock.Mock<IAmazonShippingWebClient>()
                .Verify(x => x.CreateShipment(
                    It.Is<ShipmentRequestDetails>(s => s.ShippingServiceOptions.DeclaredValue.Amount == 65),
                    defaultShipment.Amazon));
        }

        [Theory]
        [InlineData("FEDEX")]
        [InlineData("UPS")]
        [InlineData("DHLMX")]
        [InlineData("DHLM")]
        [InlineData("DYNAMEX")]
        public void Create_SetsDeclaredValueTo100_WhenCarrierIsNotUSPSAndDeclaredValueIsGreaterThan100(string carrier)
        {
            defaultShipment.Amazon.InsuranceValue = 101;
            defaultShipment.Amazon.CarrierName = carrier;

            var testObject = mock.Create<AmazonCreateShipmentRequest>();
            testObject.Submit(defaultShipment);

            mock.Mock<IAmazonShippingWebClient>()
                .Verify(x => x.CreateShipment(
                    It.Is<ShipmentRequestDetails>(s => s.ShippingServiceOptions.DeclaredValue.Amount == 100),
                    defaultShipment.Amazon));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
