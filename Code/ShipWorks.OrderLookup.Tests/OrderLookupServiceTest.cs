using System;
using System.Data;
using System.Threading.Tasks;
using ShipWorks.Tests.Shared;
using Autofac.Extras.Moq;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Search;
using ShipWorks.Stores.Communication;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.OrderLookup.Tests
{
    public class OrderLookupServiceTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly Mock<IDbCommand> command;
        private readonly Mock<IDataReader> dataReader;
        private readonly Mock<ISqlAdapter> sqlAdapter;

        public OrderLookupServiceTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            var filterDefinition = mock.CreateMock<IFilterDefinition>();
            filterDefinition
                .Setup(d => d.GenerateRootSql(FilterTarget.Orders))
                .Returns("GeneratedSql");

            Mock<ISearchDefinitionProvider> searchDefinitionProvider = mock
                .FromFactory<ISearchDefinitionProviderFactory>()
                .Mock(m => m.Create(FilterTarget.Orders, true));

            searchDefinitionProvider
                .Setup(p => p.GetDefinition(AnyString))
                .Returns(filterDefinition.Object);

            dataReader = mock.CreateMock<IDataReader>();
            dataReader
                .SetupSequence(x => x.Read())
                .Returns(true)
                .Returns(false);
            dataReader
                .Setup(d => d.GetInt64(0))
                .Returns(42);

            command = mock.CreateMock<IDbCommand>();
            command.Setup(c => c.ExecuteReader())
                .Returns(dataReader.Object);

            var con = mock.CreateMock<IDbConnection>();
            con.Setup(c => c.CreateCommand())
                .Returns(command.Object);

            mock.Mock<ISqlSession>()
                .Setup(c => c.OpenConnection())
                .Returns(con.Object);

            sqlAdapter = mock.FromFactory<ISqlAdapterFactory>()
                .Mock(f => f.Create());

            sqlAdapter.Setup(a => a.FetchEntity(AnyOrder)).Callback<IEntity2>(o => o.IsNew = false);
        }

        [Fact]
        public async Task FindOrder_ReturnsFetchedOrder()
        {
            var testObject = mock.Create<OrderLookupService>();
            var order = await testObject.FindOrder("blah");

            Assert.Equal(42, order.OrderID);
        }

        [Fact]
        public async Task FindOrder_FetchesOrder_WithExpectedOrderNumber()
        {
            var testObject = mock.Create<OrderLookupService>();
            await testObject.FindOrder("blah");

            sqlAdapter.Verify(a => a.FetchEntity(It.Is<OrderEntity>(o => o.OrderID == 42)), Times.Once);
        }

        [Fact]
        public async Task FindOrder_DownloadIsDelegatedToIOnDemandDownloader()
        {
            var downloader = mock.FromFactory<IOnDemandDownloaderFactory>()
                .Mock(f => f.CreateSingleScanOnDemandDownloader());

            var testObject = mock.Create<OrderLookupService>();
            await testObject.FindOrder("blah");

            downloader.Verify(d => d.Download("blah"), Times.Once);
        }

        [Fact]
        public async Task FindOrder_CommandIsRanWithCorrectSql()
        {
            var testObject = mock.Create<OrderLookupService>();
            await testObject.FindOrder("blah");

            command
                .VerifySet(s => s.CommandText = "Select OrderId from [Order] o where GeneratedSql", Times.Once);
        }

        [Fact]
        public async Task FindOrder_ReturnsNull_WhenOrderNotFound()
        {
            dataReader.Setup(r => r.Read()).Returns(false);

            var testObject = mock.Create<OrderLookupService>();
            var order = await testObject.FindOrder("blah");

            Assert.Null(order);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}