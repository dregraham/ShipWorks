using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.Mapping
{
    public class ShipWorksOdbcMappableFieldTest
    {
        [Fact]
        public void QualifiedName_ReturnsQualifiedName()
        {
            ShipWorksOdbcMappableField testObject = new ShipWorksOdbcMappableField(OrderFields.OrderNumber, "Order Number");

            Assert.Equal("OrderEntity.OrderNumber", testObject.QualifiedName);
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

        [Fact]
        public void LoadValue_ConvertsStringToDecimal()
        {
            ShipWorksOdbcMappableField testObject = new ShipWorksOdbcMappableField(OrderFields.OrderTotal, "Order Total");
            testObject.LoadValue("2.51");

            Assert.Equal(2.51M, testObject.Value);
        }

        [Fact]
        public void LoadValue_ConvertsStringToDecimal_WhenStringHasCurrencySymbol()
        {
            ShipWorksOdbcMappableField testObject = new ShipWorksOdbcMappableField(OrderFields.OrderTotal, "Order Total");
            testObject.LoadValue("$2.51");

            Assert.Equal(2.51M, testObject.Value);
        }

        [Fact]
        public void LoadValue_ConvertsStringToDecimal_WhenStringHasCurrencySymbolWithWhiteSpace()
        {
            ShipWorksOdbcMappableField testObject = new ShipWorksOdbcMappableField(OrderFields.OrderTotal, "Order Total");
            testObject.LoadValue("$ 2.51");

            Assert.Equal(2.51M, testObject.Value);
        }

        [Fact]
        public void LoadValue_ConvertsStringToDecimal_WhenStringHasWhiteSpace()
        {
            ShipWorksOdbcMappableField testObject = new ShipWorksOdbcMappableField(OrderFields.OrderTotal, "Order Total");
            testObject.LoadValue(" 2.51  ");

            Assert.Equal(2.51M, testObject.Value);
        }

        [Fact]
        public void LoadValue_ConvertsStringToDecimal_WhenStringHasLeadingSign()
        {
            ShipWorksOdbcMappableField testObject = new ShipWorksOdbcMappableField(OrderFields.OrderTotal, "Order Total");
            testObject.LoadValue("-2.51");

            Assert.Equal(-2.51M, testObject.Value);
        }

        [Fact]
        public void LoadValue_ConvertsDoubleToDecimal()
        {
            ShipWorksOdbcMappableField testObject = new ShipWorksOdbcMappableField(OrderFields.OrderTotal, "Order Total");
            testObject.LoadValue(2.51D);

            Assert.Equal(2.51M, testObject.Value);
        }

        [Fact]
        public void LoadValue_ConvertsFloatToDecimal()
        {
            ShipWorksOdbcMappableField testObject = new ShipWorksOdbcMappableField(OrderFields.OrderTotal, "Order Total");
            testObject.LoadValue(2.51F);

            Assert.Equal(2.51M, testObject.Value);
        }
    }
}