using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Search;
using ShipWorks.Stores.Communication;
using ShipWorks.Tests.Shared;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Stores.Tests.Orders
{
    public class OrderRepositoryTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly Mock<ISqlAdapter> sqlAdapter;

        public OrderRepositoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            var filterDefinition = mock.CreateMock<IFilterDefinition>();
            filterDefinition
                .Setup(d => d.GenerateRootSql(FilterTarget.Orders))
                .Returns("GeneratedSql");

            sqlAdapter = mock.FromFactory<ISqlAdapterFactory>()
                .Mock(f => f.Create());

            sqlAdapter.Setup(a => a.FetchQueryAsync<OrderEntity>(AnyString, null))
                .ReturnsAsync(new List<OrderEntity> {new OrderEntity(42)});
        }

        [Fact]
        public async Task FindOrder_ReturnsFetchedOrder()
        {
            var testObject = mock.Create<OrderRepository>();
            var order = await testObject.FindOrder("blah");

            Assert.Equal(42, order.OrderID);
        }

        [Fact]
        public async Task FindOrder_DownloadIsDelegatedToIOnDemandDownloader()
        {
            var downloader = mock.FromFactory<IOnDemandDownloaderFactory>()
                .Mock(f => f.CreateSingleScanOnDemandDownloader());

            var testObject = mock.Create<OrderRepository>();
            await testObject.FindOrder("blah");

            downloader.Verify(d => d.Download("blah"), Times.Once);
        }

        [Fact]
        public async Task FindOrder_DelegatesToSingleScanSearchDefProviderForSql()
        {
            var testObject = mock.Create<OrderRepository>();
            await testObject.FindOrder("blah");

            mock.Mock<ISingleScanSearchDefinitionProvider>().Verify(d => d.GenerateSql("blah"));
        }

        [Fact]
        public async Task FindOrder_DelegatesToSqlAdapterWithSqlFromSingleScanSearchDefProvider()
        {
            var testObject = mock.Create<OrderRepository>();
            mock.Mock<ISingleScanSearchDefinitionProvider>().Setup(d => d.GenerateSql("blah")).Returns("FooBar");

            await testObject.FindOrder("blah");

            sqlAdapter.Verify(a => a.FetchQueryAsync<OrderEntity>("FooBar", null));
        }

        [Fact]
        public async Task FindOrder_ReturnsNull_WhenOrderNotFound()
        {
            sqlAdapter.Setup(a => a.FetchQueryAsync<OrderEntity>(AnyString, null))
                .ReturnsAsync(new List<OrderEntity>());

            var testObject = mock.Create<OrderRepository>();
            var order = await testObject.FindOrder("blah");

            Assert.Null(order);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}