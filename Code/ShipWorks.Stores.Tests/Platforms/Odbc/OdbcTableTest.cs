using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Stores.Platforms.Odbc;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc
{
    public class OdbcTableTest : IDisposable
    {
        private readonly AutoMock mock;

        public OdbcTableTest()
        {
            mock = AutoMock.GetLoose();
        }

        [Fact]
        public void Load_PopulatesTables()
        {
            DataTable tableData = new DataTable();
            tableData.Columns.Add("TABLE_NAME", typeof(string));
            tableData.Rows.Add("Order");

            // Mock up the connection
            Mock<DbConnection> connection = mock.Mock<DbConnection>();
            connection.Setup(c => c.GetSchema(It.IsAny<string>())).Returns(tableData);

            // Mock up the data source
            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            dataSource.Setup(d => d.CreateConnection()).Returns(connection.Object);

            OdbcSchema testObject = mock.Create<OdbcSchema>();

            testObject.Load(dataSource.Object);

            Assert.Equal(1, testObject.Tables.Count(t => t.Name == "Order"));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}