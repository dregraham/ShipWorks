using Autofac.Extras.Moq;
using Interapptive.Shared.Business;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Shipping.Carriers.Amazon.Api;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Shipping.Carriers.Amazon.Enums;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Amazon
{
    public class AmazonShipmentRequestDetailsFactoryTest
    {
        readonly AmazonOrderEntity order = new AmazonOrderEntity();
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
                DeliveryExperience = 1
            }
        };
        readonly AmazonShipmentRequestDetailsFactory amazonShipmentRequestDetailsFactory = new AmazonShipmentRequestDetailsFactory();

        [Fact]
        public void CreateReturns_ShipmentRequestDetailsWith_DeclaredValue()
        {
            ShipmentRequestDetails testObject = amazonShipmentRequestDetailsFactory.Create(shipmentEntity, order);

            Assert.Equal(testObject.Insurance.Amount, 4);
        }

        [Fact]
        public void CreateReturns_ShipmentRequestDetailsWith_TotalWeight()
        {
            ShipmentRequestDetails testObject = amazonShipmentRequestDetailsFactory.Create(shipmentEntity, order);

            Assert.Equal(testObject.Weight, 1.23);
        }

        [Fact]
        public void CreateReturns_ShipmentRequestDetailsWith_Dims()
        {
            ShipmentRequestDetails testObject = amazonShipmentRequestDetailsFactory.Create(shipmentEntity, order);

            Assert.Equal(testObject.PackageDimensions.Height, 7);
            Assert.Equal(testObject.PackageDimensions.Length, 8);
            Assert.Equal(testObject.PackageDimensions.Width, 9);
        }

        [Fact]
        public void CreateReturns_ShipmentRequestDetailsWith_FromAddress()
        {
            ShipmentRequestDetails testObject = amazonShipmentRequestDetailsFactory.Create(shipmentEntity, order);

            Assert.Equal(testObject.ShipFromAddress.AddressLine1, "123");
            Assert.Equal(testObject.ShipFromAddress.AddressLine2, "456");
            Assert.Equal(testObject.ShipFromAddress.AddressLine3, "789");
        }

        [Fact]
        public void CreateReturns_NameIsCopied_WhenParseStatusIsUnknown()
        {
            shipmentEntity.OriginNameParseStatus = (int)PersonNameParseStatus.Unknown;
            shipmentEntity.OriginUnparsedName = string.Empty;
            shipmentEntity.OriginFirstName = "Foo";
            shipmentEntity.OriginLastName = "Bar";

            ShipmentRequestDetails testObject = amazonShipmentRequestDetailsFactory.Create(shipmentEntity, order);

            Assert.Equal(testObject.ShipFromAddress.Name, "Foo Bar");
        }

        [Fact]
        public void CreateReturns_NameIsCopied_WhenParseStatusIsSimple()
        {
            shipmentEntity.OriginNameParseStatus = (int)PersonNameParseStatus.Simple;
            shipmentEntity.OriginUnparsedName = "Foo Bar";
            shipmentEntity.OriginFirstName = "Foo";
            shipmentEntity.OriginLastName = "Bar";

            ShipmentRequestDetails testObject = amazonShipmentRequestDetailsFactory.Create(shipmentEntity, order);

            Assert.Equal(testObject.ShipFromAddress.Name, "Foo Bar");
        }

        [Fact]
        public void CreateReturns_ShipmentRequestDetailsWith_DeliveryExperience()
        {
            ShipmentRequestDetails testObject = amazonShipmentRequestDetailsFactory.Create(shipmentEntity, order);

            Assert.Equal(testObject.ShippingServiceOptions.DeliveryExperience, EnumHelper.GetApiValue((AmazonDeliveryExperienceType)1));
        }
    }
}