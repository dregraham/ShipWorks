using System;
using Autofac.Extras.Moq;
using Moq;
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
        public void QuanifiedName_ReturnsQualifiedName()
        {
            Mock<IOdbcTable> table = mock.Mock<IOdbcTable>();
            table.SetupGet(t => t.Name).Returns("TableName");
            OdbcColumn column = new OdbcColumn("ColumnName");

            ExternalOdbcMappableField testObject = new ExternalOdbcMappableField(table.Object, column);

            Assert.Equal("TableName.ColumnName", testObject.QualifiedName);
        }

        [Fact]
        public void DisplayName_ReturnsTableAndColumnName()
        {
            Mock<IOdbcTable> table = mock.Mock<IOdbcTable>();
            table.SetupGet(t => t.Name).Returns("TableName");
            OdbcColumn column = new OdbcColumn("ColumnName");

            ExternalOdbcMappableField testObject = new ExternalOdbcMappableField(table.Object, column);

            Assert.Equal("TableName ColumnName", testObject.DisplayName);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}