using Autofac.Extras.Moq;
using Interapptive.Shared.Business;
using Interapptive.Shared.Utility;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.Api;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Shipping.Carriers.Amazon.Enums;
using ShipWorks.Stores.Platforms.Amazon;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Amazon
{
    public class AmazonShipmentRequestDetailsFactoryTest
    {
        readonly AutoMock mock;
        readonly IAmazonOrder order;
        readonly ShipmentEntity shipmentEntity = new ShipmentEntity
        {
            OriginStreet1 = "123",
            OriginStreet2 = "456",
            OriginStreet3 = "789",

            TotalWeight = 1.23,
            Amazon = new AmazonShipmentEntity
            {
                DeclaredValue = 4,
                DimsHeight = 7,
                DimsLength = 8,
                DimsWidth = 9,
                DeliveryExperience = 1,
                Reference1 = "01234567890123456789",
                RequestedLabelFormat = (int) ThermalLanguage.None
            }
        };

        readonly AmazonShipmentRequestDetailsFactory amazonShipmentRequestDetailsFactory;

        public AmazonShipmentRequestDetailsFactoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            order = mock.Build<IAmazonOrder>();
            amazonShipmentRequestDetailsFactory = mock.Create<AmazonShipmentRequestDetailsFactory>();
        }

        [Fact]
        public void CreateReturns_ShipmentRequestDetailsWith_DeclaredValue()
        {
            ShipmentRequestDetails testObject = amazonShipmentRequestDetailsFactory.Create(shipmentEntity, order);

            Assert.Null(testObject.Insurance);
        }

        [Fact]
        public void CreateReturns_ShipmentRequestDetailsWith_Reference()
        {
            ShipmentRequestDetails testObject = amazonShipmentRequestDetailsFactory.Create(shipmentEntity, order);

            Assert.Equal(shipmentEntity.Amazon.Reference1.Truncate(14), testObject.LabelCustomization.CustomTextForLabel);
        }

        [Theory]
        [InlineData(ThermalLanguage.None, null)]
        [InlineData(ThermalLanguage.ZPL, "ZPL203")]
        public void CreateReturns_ShipmentRequestDetailsWith_RequestedLabelFormat(ThermalLanguage thermalLanguage, string expectedValue)
        {
            shipmentEntity.Amazon.RequestedLabelFormat = (int) thermalLanguage;
            ShipmentRequestDetails testObject = amazonShipmentRequestDetailsFactory.Create(shipmentEntity, order);

            Assert.Equal(expectedValue, testObject.ShippingServiceOptions.LabelFormat);
        }

        [Fact]
        public void CreateReturns_ShipmentRequestDetailsWith_TotalWeight()
        {
            ShipmentRequestDetails testObject = amazonShipmentRequestDetailsFactory.Create(shipmentEntity, order);

            Assert.Equal(1.23, testObject.Weight);
        }

        [Fact]
        public void CreateReturns_ShipmentRequestDetailsWith_Dims()
        {
            ShipmentRequestDetails testObject = amazonShipmentRequestDetailsFactory.Create(shipmentEntity, order);

            Assert.Equal(7, testObject.PackageDimensions.Height);
            Assert.Equal(8, testObject.PackageDimensions.Length);
            Assert.Equal(9, testObject.PackageDimensions.Width);
        }

        [Fact]
        public void CreateReturns_ShipmentRequestDetailsWith_FromAddress()
        {
            ShipmentRequestDetails testObject = amazonShipmentRequestDetailsFactory.Create(shipmentEntity, order);

            Assert.Equal("123", testObject.ShipFromAddress.AddressLine1);
            Assert.Equal("456", testObject.ShipFromAddress.AddressLine2);
            Assert.Equal("789", testObject.ShipFromAddress.AddressLine3);
        }

        [Fact]
        public void CreateReturns_NameIsCopied_WhenParseStatusIsUnknown()
        {
            shipmentEntity.OriginNameParseStatus = (int) PersonNameParseStatus.Unknown;
            shipmentEntity.OriginUnparsedName = string.Empty;
            shipmentEntity.OriginFirstName = "Foo";
            shipmentEntity.OriginLastName = "Bar";

            ShipmentRequestDetails testObject = amazonShipmentRequestDetailsFactory.Create(shipmentEntity, order);

            Assert.Equal("Foo Bar", testObject.ShipFromAddress.Name);
        }

        [Fact]
        public void CreateReturns_NameIsCopied_WhenParseStatusIsSimple()
        {
            shipmentEntity.OriginNameParseStatus = (int) PersonNameParseStatus.Simple;
            shipmentEntity.OriginUnparsedName = "Foo Bar";
            shipmentEntity.OriginFirstName = "Foo";
            shipmentEntity.OriginLastName = "Bar";

            ShipmentRequestDetails testObject = amazonShipmentRequestDetailsFactory.Create(shipmentEntity, order);

            Assert.Equal("Foo Bar", testObject.ShipFromAddress.Name);
        }

        [Fact]
        public void CreateReturns_ShipmentRequestDetailsWith_DeliveryExperience()
        {
            ShipmentRequestDetails testObject = amazonShipmentRequestDetailsFactory.Create(shipmentEntity, order);

            Assert.Equal(testObject.ShippingServiceOptions.DeliveryExperience, EnumHelper.GetApiValue((AmazonDeliveryExperienceType) 1));
        }
    }
}