using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System;
using ShipWorks.Data.Model.EntityClasses;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.Mapping
{
    public class ShipWorksOdbcMappableFieldTest
    {
        [Fact]
        public void JsonConstructor_PopulatesProperties()
        {
            ShipWorksOdbcMappableField testObject = new ShipWorksOdbcMappableField("OrderEntity", "OrderNumber", "Order Number");

            Assert.Equal("Order Number", testObject.DisplayName);
            Assert.Equal(false, testObject.IsRequired);
            Assert.Equal("OrderNumber", testObject.Name);
            Assert.Equal("OrderEntity.OrderNumber", testObject.QualifiedName);
            Assert.Equal("OrderEntity", testObject.ContainingObjectName);
        }

        [Fact]
        public void ConstructorWithFieldDescription_PopulatesProperties()
        {
            ShipWorksOdbcMappableField testObject = new ShipWorksOdbcMappableField(OrderFields.OrderNumber, OdbcOrderFieldDescription.Number);

            Assert.Equal("Order Number", testObject.DisplayName);
            Assert.Equal(false, testObject.IsRequired);
            Assert.Equal("OrderNumber", testObject.Name);
            Assert.Equal("OrderEntity.OrderNumber", testObject.QualifiedName);
            Assert.Equal("OrderEntity", testObject.ContainingObjectName);
        }

        [Fact]
        public void ConstructorWithFieldDescriptionAndIsRequired_PopulatesProperties()
        {
            ShipWorksOdbcMappableField testObject = new ShipWorksOdbcMappableField(OrderFields.OrderNumber, OdbcOrderFieldDescription.Number, true);

            Assert.Equal("Order Number", testObject.DisplayName);
            Assert.Equal(true, testObject.IsRequired);
            Assert.Equal("OrderNumber", testObject.Name);
            Assert.Equal("OrderEntity.OrderNumber", testObject.QualifiedName);
            Assert.Equal("OrderEntity", testObject.ContainingObjectName);
        }

        [Fact]
        public void ConstructorWithFieldAndDisplayName_PopulatesProperties()
        {
            ShipWorksOdbcMappableField testObject = new ShipWorksOdbcMappableField(OrderFields.OrderNumber, "Order Number");

            Assert.Equal("Order Number", testObject.DisplayName);
            Assert.Equal(false, testObject.IsRequired);
            Assert.Equal("OrderNumber", testObject.Name);
            Assert.Equal("OrderEntity.OrderNumber", testObject.QualifiedName);
            Assert.Equal("OrderEntity", testObject.ContainingObjectName);
        }

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
        public void LoadValue_UsesDefaultValue_WhenValueIsNullandFieldIsNotNullable()
        {
            ShipWorksOdbcMappableField testObject = new ShipWorksOdbcMappableField(OrderFields.OrderNumber, "Order Number");
            testObject.LoadValue((object)null);

            Assert.Equal(0L, testObject.Value);
        }

        [Fact]
        public void LoadValue_ThrowsShipWorksOdbcException_WhenFieldIsNotMappable()
        {
            ShipWorksOdbcMappableField testObject = new ShipWorksOdbcMappableField(OrderFields.OrderID, "OrderID");
            ShipWorksOdbcException ex = Assert.Throws<ShipWorksOdbcException>(() => testObject.LoadValue(2));

            Assert.Equal($"Invalid Map. '{testObject.QualifiedName}' should never be mapped.", ex.Message);
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

        [Fact]
        public void LoadValue_LoadsTrackingNumber_WhenEntityIsShipmentAndFieldIsTrackingNumber()
        {
            ShipWorksOdbcMappableField testObject = new ShipWorksOdbcMappableField(ShipmentFields.TrackingNumber, "Tracking Number");
            testObject.LoadValue(new ShipmentEntity() {TrackingNumber = "abcdefghijklmnopqrstuvwxyz"});

            Assert.Equal("abcdefghijklmnopqrstuvwxyz", testObject.Value);
        }
    }
}