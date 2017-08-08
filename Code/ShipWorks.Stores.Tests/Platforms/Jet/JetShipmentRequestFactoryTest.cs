using System;
using System.Linq;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Jet;
using ShipWorks.Stores.Platforms.Jet.DTO.Requests;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Jet
{
    public class JetShipmentRequestFactoryTest
    {
        private readonly AutoMock mock;
        private readonly JetShipmentRequestFactory testObject;
        private readonly ShipmentEntity shipment;

        public JetShipmentRequestFactoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<JetShipmentRequestFactory>();

            shipment = new ShipmentEntity
            {
                Processed = true,
                TrackingNumber = "abcd123",
                ShipmentType = (int)ShipmentTypeCode.Other,
                Order = new JetOrderEntity(),
                OriginPostalCode = "63040"
            };

            shipment.Order.OrderItems.Add(new JetOrderItemEntity() { MerchantSku = "123", Quantity = 3 });
        }

        [Fact]
        public void Create_PopulatesOrderDetails()
        {
            testObject.Create(shipment);

            mock.Mock<IOrderManager>().Verify(m => m.PopulateOrderDetails(shipment));
        }

        [Fact]
        public void Create_ReturnsJetShipmentRequestWithShipmentDate()
        {
            DateTime processedDate = DateTime.UtcNow.AddDays(-23);
            DateTime shipDate = DateTime.UtcNow.AddDays(-21);

            shipment.ProcessedDate = processedDate;
            shipment.ShipDate = shipDate;

            JetShipmentRequest result = testObject.Create(shipment);

            Assert.Equal(processedDate, result.Shipments.First().ResponseShipmentDate);
        }

        [Fact]
        public void Create_ReturnsJetShipmentRequestWithTrackingNumber()
        {
            JetShipmentRequest result = testObject.Create(shipment);

            Assert.Equal("abcd123", result.Shipments.First().ShipmentTrackingNumber);
        }

        [Fact]
        public void Create_ReturnsJetShipmentRequestWithShipFromZipCode()
        {
            JetShipmentRequest result = testObject.Create(shipment);

            Assert.Equal("63040", result.Shipments.First().ShipFromZipCode);
        }

        [Fact]
        public void Create_ReturnsJetShipmentRequestWithCarrier_WhenShipmentIsFedEx()
        {
            shipment.ShipmentType = (int)ShipmentTypeCode.FedEx;
            shipment.FedEx = new FedExShipmentEntity
            {
                Service = (int)FedExServiceType.FedExGround
            };

            JetShipmentRequest result = testObject.Create(shipment);

            Assert.Equal("FedEx", result.Shipments.First().Carrier);
        }

        [Fact]
        public void Create_ReturnsJetShipmentRequestWithCarrier_WhenShipmentIsFedExFreight()
        {
            shipment.ShipmentType = (int)ShipmentTypeCode.FedEx;
            shipment.FedEx = new FedExShipmentEntity
            {
                Service = (int)FedExServiceType.FedEx1DayFreight
            };

            JetShipmentRequest result = testObject.Create(shipment);

            Assert.Equal("FedEx Freight", result.Shipments.First().Carrier);
        }

        [Fact]
        public void Create_ReturnsJetShipmentRequestWithCarrier_WhenShipmentIsFedExSmartPost()
        {
            shipment.ShipmentType = (int) ShipmentTypeCode.FedEx;
            shipment.FedEx = new FedExShipmentEntity
            {
                Service = (int) FedExServiceType.SmartPost
            };

            JetShipmentRequest result = testObject.Create(shipment);

            Assert.Equal("FedEx SmartPost", result.Shipments.First().Carrier);
        }

        [Fact]
        public void Create_ReturnsJetShipmentRequestWithCarrier_WhenShipmentIsUps()
        {
            shipment.ShipmentType = (int)ShipmentTypeCode.UpsOnLineTools;
            shipment.Ups = new UpsShipmentEntity
            {
                Service = (int) UpsServiceType.UpsGround
            };

            JetShipmentRequest result = testObject.Create(shipment);

            Assert.Equal("UPS", result.Shipments.First().Carrier);
        }

        [Fact]
        public void Create_ReturnsJetShipmentRequestWithCarrier_WhenShipmentIsUpsMI()
        {
            shipment.ShipmentType = (int)ShipmentTypeCode.UpsOnLineTools;
            shipment.Ups = new UpsShipmentEntity
            {
                Service = (int)UpsServiceType.UpsMailInnovationsFirstClass
            };

            JetShipmentRequest result = testObject.Create(shipment);

            Assert.Equal("UPS Mail Innovations", result.Shipments.First().Carrier);
        }

        [Fact]
        public void Create_ReturnsJetShipmentRequestWithCarrier_WhenShipmentIsUpsSurePost()
        {
            shipment.ShipmentType = (int)ShipmentTypeCode.UpsOnLineTools;
            shipment.Ups = new UpsShipmentEntity
            {
                Service = (int)UpsServiceType.UpsSurePostLessThan1Lb
            };

            JetShipmentRequest result = testObject.Create(shipment);

            Assert.Equal("UPS SurePost", result.Shipments.First().Carrier);
        }

        [Fact]
        public void Create_ReturnsJetShipmentRequestWithCarrier_WhenShipmentIsUsps()
        {
            shipment.ShipmentType = (int)ShipmentTypeCode.Usps;
            shipment.Postal = new PostalShipmentEntity();

            JetShipmentRequest result = testObject.Create(shipment);

            Assert.Equal("USPS", result.Shipments.First().Carrier);
        }

        [Fact]
        public void Create_ReturnsJetShipmentRequestWithCarrier_WhenShipmentIsOnTrac()
        {
            shipment.ShipmentType = (int)ShipmentTypeCode.OnTrac;
            shipment.OnTrac = new OnTracShipmentEntity();

            JetShipmentRequest result = testObject.Create(shipment);
            
            Assert.Equal("OnTrac", result.Shipments.First().Carrier);
        }

        [Fact]
        public void Create_ReturnsJetShipmentRequestWithCarrier_WhenShipmentIsOther()
        {
            shipment.ShipmentType = (int)ShipmentTypeCode.Other;
            shipment.Other = new OtherShipmentEntity();

            JetShipmentRequest result = testObject.Create(shipment);

            Assert.Equal("Other", result.Shipments.First().Carrier);
        }
    }
}