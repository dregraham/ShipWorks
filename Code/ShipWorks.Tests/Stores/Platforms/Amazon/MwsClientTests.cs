using System;
using Autofac;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Stores;
using ShipWorks.Stores.Platforms.Amazon.Mws;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Tests.Stores.Amazon
{
    public class MwsClientTests : IDisposable
    {
        private ShipmentEntity shipmentEntity;
        private AmazonOrderEntity orderEntity;
        private PostalShipmentEntity postalShipmentEntity;
        private OtherShipmentEntity otherShipmentEntity;
        private readonly AutoMock mock;
        private readonly AmazonStoreEntity store;

        public MwsClientTests()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            store = new AmazonStoreEntity { StoreTypeCode = StoreTypeCode.Amazon };
            orderEntity = new AmazonOrderEntity { OrderNumber = 123456 };
            postalShipmentEntity = new PostalShipmentEntity { Service = (int) PostalServiceType.FirstClass };
            otherShipmentEntity = new OtherShipmentEntity { Carrier = "Some other carrier", Service = "Fast Ground" };
            shipmentEntity = new ShipmentEntity { Order = orderEntity, TrackingNumber = "ABCD1234", ShipDate = DateTime.UtcNow, ShipmentType = (int) ShipmentTypeCode.Usps, Postal = postalShipmentEntity };
        }

        [Fact]
        public void ValidateSigning()
        {
            string stringToSign = "POST\nmws.amazonservices.com\n/Orders/2011-01-01\nAWSAccessKeyId=AKIAJU465C2O2U6WNQTA&Action=ListOrderItems&AmazonOrderId=SHIPWORKS_CONNECT_ATTEMPT&SellerId=A2OIXR4TA6XKOD&SignatureMethod=HmacSHA256&SignatureVersion=2&Timestamp=2011-08-19T15%3A55%3A39Z&Version=2011-01-01";
            string signature = "XKab7VXEklL8EYE0HcqRF97fgjqTNsM7NFe/gJ7eWLE=";

            string signed = RequestSignature.CreateRequestSignature(stringToSign, "0+NcTFU11/qooQriFFO7k0JxY64t/P38DIAAAgeW", SigningAlgorithm.SHA256);

            Assert.Equal(signature, signed);
        }

        [Fact]
        public void GetCarrierName_ReturnsUsps_WhenUspsAndFirstClass()
        {
            shipmentEntity.ShipmentType = (int) ShipmentTypeCode.Usps;

            mock.Mock<IShippingManager>()
                .Setup(x => x.GetCarrierName(ShipmentTypeCode.Usps))
                .Returns("USPS");

            var testObject = mock.Create<AmazonMwsClient>(TypedParameter.From(store));
            string carrierName = testObject.GetCarrierName(shipmentEntity, ShipmentTypeCode.Usps);

            Assert.Equal("USPS", carrierName);
        }

        [Fact]
        public void GetCarrierName_ReturnsUsps_WhenEndiciaAndFirstClass()
        {
            shipmentEntity.ShipmentType = (int) ShipmentTypeCode.Endicia;

            mock.Mock<IShippingManager>()
                .Setup(x => x.GetCarrierName(ShipmentTypeCode.Usps))
                .Returns("USPS");

            var testObject = mock.Create<AmazonMwsClient>(TypedParameter.From(store));

            string carrierName = testObject.GetCarrierName(shipmentEntity, ShipmentTypeCode.Usps);

            Assert.Equal("USPS", carrierName);
        }

        [Fact]
        public void GetCarrierName_ReturnsDhl_WhenEndiciaAndDhl()
        {
            var testObject = mock.Create<AmazonMwsClient>(TypedParameter.From(store));

            shipmentEntity.ShipmentType = (int) ShipmentTypeCode.Endicia;
            postalShipmentEntity.Service = (int) PostalServiceType.DhlParcelGround;

            string carrierName = testObject.GetCarrierName(shipmentEntity, ShipmentTypeCode.Endicia);

            Assert.Equal("DHL eCommerce", carrierName);
        }

        [Fact]
        public void GetCarrierName_ReturnsDhl_WhenUspsAndDhl()
        {
            var testObject = mock.Create<AmazonMwsClient>(TypedParameter.From(store));

            shipmentEntity.ShipmentType = (int) ShipmentTypeCode.Usps;
            postalShipmentEntity.Service = (int) PostalServiceType.DhlParcelGround;

            string carrierName = testObject.GetCarrierName(shipmentEntity, ShipmentTypeCode.Usps);

            Assert.Equal("DHL eCommerce", carrierName);
        }

        [Fact]
        public void GetCarrierName_ReturnsConsolidator_WhenEndiciaAndConsolidator()
        {
            var testObject = mock.Create<AmazonMwsClient>(TypedParameter.From(store));

            shipmentEntity.ShipmentType = (int) ShipmentTypeCode.Endicia;
            postalShipmentEntity.Service = (int) PostalServiceType.ConsolidatorDomestic;

            string carrierName = testObject.GetCarrierName(shipmentEntity, ShipmentTypeCode.Endicia);

            Assert.Equal("Consolidator", carrierName);
        }

        [Fact]
        public void GetCarrierName_ReturnsOtherCarrierDesc_WhenOther()
        {
            var testObject = mock.Create<AmazonMwsClient>(TypedParameter.From(store));

            shipmentEntity.ShipmentType = (int) ShipmentTypeCode.Other;
            shipmentEntity.Other = otherShipmentEntity;

            mock.Mock<IShippingManager>()
                .Setup(x => x.GetOtherCarrierDescription(shipmentEntity))
                .Returns(new CarrierDescription(shipmentEntity));

            string carrierName = testObject.GetCarrierName(shipmentEntity, ShipmentTypeCode.Endicia);

            Assert.Equal("Some other carrier", carrierName);
        }

        public void Dispose() => mock.Dispose();
    }
}
