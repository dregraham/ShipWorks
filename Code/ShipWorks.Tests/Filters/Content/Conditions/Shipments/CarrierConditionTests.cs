using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Filters.Content.Conditions.Shipments;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Amazon.SFP;
using ShipWorks.Shipping.Carriers.Other;
using Xunit;

namespace ShipWorks.Tests.Filters.Content.Conditions.Shipments
{
    public class CarrierConditionTests
    {
        [Fact]
        public void ValueChoices_ContainsAmazon_AmazonConfigured()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IShipmentTypeManager>()
                    .Setup(m => m.ShipmentTypes)
                    .Returns(new List<ShipmentType> { mock.Create<AmazonSFPShipmentType>() });

                mock.Mock<IShippingManager>()
                    .Setup(m => m.IsShipmentTypeConfigured(It.IsAny<ShipmentTypeCode>()))
                    .Returns(true);

                var testObject = mock.Create<CarrierCondition>();

                var valueChoices = testObject.ValueChoices;

                Assert.True(valueChoices.SingleOrDefault(c=>c.Value == ShipmentTypeCode.AmazonSFP) != null);
            }
        }

        [Fact]
        public void ValueChoices_DoesNotContainAmazon_AmazonNotConfigured()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IShipmentTypeManager>()
                    .Setup(m => m.ShipmentTypes)
                    .Returns(new List<ShipmentType> { mock.Create<AmazonSFPShipmentType>() });

                mock.Mock<IShippingManager>()
                    .Setup(m => m.IsShipmentTypeConfigured(It.IsAny<ShipmentTypeCode>()))
                    .Returns(false);

                var testObject = mock.Create<CarrierCondition>();

                var valueChoices = testObject.ValueChoices;

                Assert.True(valueChoices.SingleOrDefault(c => c.Value == ShipmentTypeCode.AmazonSFP) == null);
            }
        }

        [Fact]
        public void ValueChoices_ContainsOther_AmazonNotConfigured()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IShipmentTypeManager>()
                    .Setup(m => m.ShipmentTypes)
                    .Returns(new List<ShipmentType> { mock.Create<AmazonSFPShipmentType>(), mock.Create<OtherShipmentType>() });

                mock.Mock<IShippingManager>()
                    .Setup(m => m.IsShipmentTypeConfigured(It.IsAny<ShipmentTypeCode>()))
                    .Returns(false);

                var testObject = mock.Create<CarrierCondition>();

                var valueChoices = testObject.ValueChoices;

                Assert.True(valueChoices.SingleOrDefault(c => c.Value == ShipmentTypeCode.Other) != null);
            }
        }

    }
}