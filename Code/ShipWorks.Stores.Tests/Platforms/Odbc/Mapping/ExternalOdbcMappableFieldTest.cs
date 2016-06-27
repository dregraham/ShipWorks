using Autofac.Extras.Moq;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.Mapping
{
    public class ExternalOdbcMappableFieldTest : IDisposable
    {
        private readonly AutoMock mock;

        public ExternalOdbcMappableFieldTest()
        {
            mock = AutoMock.GetLoose();
        }

        [Fact]
        public void QualifiedName_ReturnsQualifiedName()
        {
            OdbcColumnSource table = new OdbcColumnSource("TableName");
            OdbcColumn column = new OdbcColumn("ColumnName");

            ExternalOdbcMappableField testObject = new ExternalOdbcMappableField(table, column);

            Assert.Equal("TableName.ColumnName", testObject.QualifiedName);
        }

        [Fact]
        public void DisplayName_ReturnsTableAndColumnName()
        {
            OdbcColumnSource table = new OdbcColumnSource("TableName");
            OdbcColumn column = new OdbcColumn("ColumnName");

            ExternalOdbcMappableField testObject = new ExternalOdbcMappableField(table, column);

            Assert.Equal("TableName ColumnName", testObject.DisplayName);
        }

        [Fact]
        public void Value_IsNull_WhenRecordDoesNotContainColumn()
        {
            OdbcColumnSource table = new OdbcColumnSource("TableName");
            OdbcColumn column = new OdbcColumn("ColumnName");
            OdbcRecord record = new OdbcRecord(string.Empty);
            record.AddField("foo", "bar");

            ExternalOdbcMappableField testObject = new ExternalOdbcMappableField(table, column);

            testObject.LoadValue(record);

            Assert.Null(testObject.Value);
        }

        [Fact]
        public void LoadValue_ThrowsArgumentNullException_WithNullRecord()
        {
            OdbcColumnSource table = new OdbcColumnSource("TableName");
            OdbcColumn column = new OdbcColumn("ColumnName");

            ExternalOdbcMappableField testObject = new ExternalOdbcMappableField(table, column);

            Assert.Throws<ArgumentNullException>(() => testObject.LoadValue(null));
        }

        [Fact]
        public void LoadValue_LoadsValueFromOdbcRecord()
        {
            OdbcColumnSource table = new OdbcColumnSource("TableName");
            OdbcColumn column = new OdbcColumn("ColumnName");
            OdbcRecord record = new OdbcRecord(string.Empty);
            record.AddField("ColumnName", "bar");

            ExternalOdbcMappableField testObject = new ExternalOdbcMappableField(table, column);

            testObject.LoadValue(record);

            Assert.Equal("bar", testObject.Value);
        }

        [Fact]
        public void ResetValue_SetsValueToNull()
        {
            OdbcColumnSource table = new OdbcColumnSource("TableName");
            OdbcColumn column = new OdbcColumn("ColumnName");
            OdbcRecord record = new OdbcRecord(string.Empty);
            record.AddField("ColumnName", "bar");

            ExternalOdbcMappableField testObject = new ExternalOdbcMappableField(table, column);

            testObject.LoadValue(record);

            Assert.Equal("bar", testObject.Value);

            testObject.ResetValue();

            Assert.Null(testObject.Value);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}