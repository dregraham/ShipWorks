using Autofac.Extras.Moq;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.Mapping
{
    public class OdbcFieldMapEntryTest : IDisposable
    {
        private readonly AutoMock mock;

        public OdbcFieldMapEntryTest()
        {
            mock = AutoMock.GetLoose();
        }

        [Fact]
        public void Ctor_IndexIsZero_WhenNoIndexIsSent()
        {
            var testObject = new OdbcFieldMapEntry(null, null);
            Assert.Equal(0, testObject.Index);
        }

        [Fact]
        public void Ctor_IndexIs5_When5IsSentForIndex()
        {
            var testObject = new OdbcFieldMapEntry(null, null, 5);
            Assert.Equal(5, testObject.Index);
        }

        [Fact]
        public void LoadExternalField_LoadsValueOnExternalField()
        {
            ExternalOdbcMappableField externalField = new ExternalOdbcMappableField(new OdbcTable("Order"), new OdbcColumn("Number"));
            ShipWorksOdbcMappableField shipWorksField = new ShipWorksOdbcMappableField(OrderFields.OrderNumber, "Order Number");
            OdbcFieldMapEntry entry = new OdbcFieldMapEntry(shipWorksField, externalField);

            OdbcRecord record = new OdbcRecord(string.Empty);
            record.AddField("Number", 123);

            entry.LoadExternalField(record);

            Assert.Equal(123, entry.ExternalField.Value);
        }

        [Fact]
        public void CopyValueToShipWorksField_CopiesValueFromExternalFieldToShipWorksField()
        {
            ExternalOdbcMappableField externalField = new ExternalOdbcMappableField(new OdbcTable("Order"), new OdbcColumn("Number"));
            ShipWorksOdbcMappableField shipWorksField = new ShipWorksOdbcMappableField(OrderFields.OrderNumber, "Order Number");
            OdbcFieldMapEntry entry = new OdbcFieldMapEntry(shipWorksField, externalField);

            OdbcRecord record = new OdbcRecord(string.Empty);
            record.AddField("Number", 123);

            entry.LoadExternalField(record);
            entry.CopyExternalValueToShipWorksField();

            Assert.Equal(123L, entry.ShipWorksField.Value);
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}