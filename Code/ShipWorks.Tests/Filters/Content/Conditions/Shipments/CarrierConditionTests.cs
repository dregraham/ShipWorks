using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Filters.Content.Conditions.Shipments;
using ShipWorks.Shipping;
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
                    .Setup(m => m.ShipmentTypeCodes)
                    .Returns(new [] { ShipmentTypeCode.Amazon });

                mock.Mock<IShippingManager>()
                    .Setup(m => m.IsShipmentTypeConfigured(It.IsAny<ShipmentTypeCode>()))
                    .Returns(true);

                var testObject = mock.Create<CarrierCondition>();

                var valueChoices = testObject.ValueChoices;

                Assert.True(valueChoices.SingleOrDefault(c=>c.Value == ShipmentTypeCode.Amazon) != null);
            }
        }

        [Fact]
        public void ValueChoices_DoesNotContainAmazon_AmazonNotConfigured()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IShipmentTypeManager>()
                    .Setup(m => m.ShipmentTypeCodes)
                    .Returns(new [] { ShipmentTypeCode.Amazon });

                mock.Mock<IShippingManager>()
                    .Setup(m => m.IsShipmentTypeConfigured(It.IsAny<ShipmentTypeCode>()))
                    .Returns(false);

                var testObject = mock.Create<CarrierCondition>();

                var valueChoices = testObject.ValueChoices;

                Assert.True(valueChoices.SingleOrDefault(c => c.Value == ShipmentTypeCode.Amazon) == null);
            }
        }

        [Fact]
        public void ValueChoices_ContainsOther_AmazonNotConfigured()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IShipmentTypeManager>()
                    .Setup(m => m.ShipmentTypeCodes)
                    .Returns(new [] { ShipmentTypeCode.Amazon, ShipmentTypeCode.Other });

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