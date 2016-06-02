using System;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
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
        public void LoadExternalField_LoadsValueOnExternalField()
        {
            ExternalOdbcMappableField externalField = new ExternalOdbcMappableField(new OdbcTable("Order"), new OdbcColumn("Number"));
            ShipWorksOdbcMappableField shipWorksField = new ShipWorksOdbcMappableField(OrderFields.OrderNumber, "Order Number");
            OdbcFieldMapEntry entry = new OdbcFieldMapEntry(shipWorksField, externalField);

            OdbcRecord record = new OdbcRecord();
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

            OdbcRecord record = new OdbcRecord();
            record.AddField("Number", 123);

            entry.LoadExternalField(record);
            entry.CopyValueToShipWorksField();

            Assert.Equal(123, entry.ShipWorksField.Value);
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}