using System;
using Autofac.Extras.Moq;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
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
        public void GetQualifiedName_ReturnsQualifiedName()
        {
            OdbcTable table = new OdbcTable("TableName");
            OdbcColumn column = new OdbcColumn("ColumnName");

            ExternalOdbcMappableField testObject = new ExternalOdbcMappableField(table, column);

            Assert.Equal("TableName.ColumnName", testObject.GetQualifiedName());
        }

        [Fact]
        public void DisplayName_ReturnsTableAndColumnName()
        {
            OdbcTable table = new OdbcTable("TableName");
            OdbcColumn column = new OdbcColumn("ColumnName");

            ExternalOdbcMappableField testObject = new ExternalOdbcMappableField(table, column);

            Assert.Equal("TableName ColumnName", testObject.DisplayName);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}