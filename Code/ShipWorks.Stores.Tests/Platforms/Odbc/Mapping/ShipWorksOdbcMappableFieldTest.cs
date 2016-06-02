using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System;
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

        [Fact]
        public void LoadValue_ConvertsStringToLong()
        {
            ShipWorksOdbcMappableField testObject = new ShipWorksOdbcMappableField(OrderFields.OrderNumber, "Order Number");
            testObject.LoadValue("123");

            Assert.Equal(123L, testObject.Value);
        }

        [Fact]
        public void LoadValue_ConvertsStringToDateTime()
        {
            ShipWorksOdbcMappableField testObject = new ShipWorksOdbcMappableField(OrderFields.OrderDate, "Order Date");
            testObject.LoadValue("6/2/2016 9:39AM");

            Assert.Equal(new DateTime(2016, 6, 2, 9, 39, 00, DateTimeKind.Local), testObject.Value);
        }
    }
}