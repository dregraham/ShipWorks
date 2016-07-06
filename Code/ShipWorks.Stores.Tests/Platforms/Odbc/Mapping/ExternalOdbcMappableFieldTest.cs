using Autofac.Extras.Moq;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
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
            OdbcColumn column = new OdbcColumn("ColumnName");

            ExternalOdbcMappableField testObject = new ExternalOdbcMappableField(column);

            Assert.Equal("TableName.ColumnName", testObject.QualifiedName);
        }

        [Fact]
        public void DisplayName_ReturnsTableAndColumnName()
        {
            OdbcColumn column = new OdbcColumn("ColumnName");

            ExternalOdbcMappableField testObject = new ExternalOdbcMappableField(column);

            Assert.Equal("TableName ColumnName", testObject.DisplayName);
        }

        [Fact]
        public void Value_IsNull_WhenRecordDoesNotContainColumn()
        {
            OdbcColumn column = new OdbcColumn("ColumnName");
            OdbcRecord record = new OdbcRecord(string.Empty);
            record.AddField("foo", "bar");

            ExternalOdbcMappableField testObject = new ExternalOdbcMappableField(column);

            testObject.LoadValue(record);

            Assert.Null(testObject.Value);
        }

        [Fact]
        public void LoadValue_ThrowsArgumentNullException_WithNullRecord()
        {
            OdbcColumn column = new OdbcColumn("ColumnName");

            ExternalOdbcMappableField testObject = new ExternalOdbcMappableField(column);

            Assert.Throws<ArgumentNullException>(() => testObject.LoadValue(null));
        }

        [Fact]
        public void LoadValue_LoadsValueFromOdbcRecord()
        {
            OdbcColumn column = new OdbcColumn("ColumnName");
            OdbcRecord record = new OdbcRecord(string.Empty);
            record.AddField("ColumnName", "bar");

            ExternalOdbcMappableField testObject = new ExternalOdbcMappableField(column);

            testObject.LoadValue(record);

            Assert.Equal("bar", testObject.Value);
        }

        [Fact]
        public void ResetValue_SetsValueToNull()
        {
            OdbcColumn column = new OdbcColumn("ColumnName");
            OdbcRecord record = new OdbcRecord(string.Empty);
            record.AddField("ColumnName", "bar");

            ExternalOdbcMappableField testObject = new ExternalOdbcMappableField(column);

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