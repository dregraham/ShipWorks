using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.Mapping
{
    public class ShipWorksOdbcMappableFieldTest
    {
        [Fact]
        public void GetQualifiedName_ReturnsQualifiedName()
        {
            ShipWorksOdbcMappableField testObject = new ShipWorksOdbcMappableField(OrderFields.OrderNumber, "Order Number");

            Assert.Equal("OrderEntity.OrderNumber", testObject.GetQualifiedName());
        }

        [Fact]
        public void DisplayName_ReturnsDisplayName()
        {
            ShipWorksOdbcMappableField testObject = new ShipWorksOdbcMappableField(OrderFields.OrderNumber, "Order Number");

            Assert.Equal("Order Number", testObject.DisplayName);
        }
    }
}