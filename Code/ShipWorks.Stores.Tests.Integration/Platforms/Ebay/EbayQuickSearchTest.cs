using System;
using System.Linq;
using Interapptive.Shared.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Startup;
using ShipWorks.Stores.Platforms.Ebay.Content;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using static ShipWorks.Stores.Tests.Integration.Helpers.DbHelpers;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Stores.Tests.Integration.Platforms.Ebay
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    [Trait("Category", "QuickSearch")]
    [Trait("Store", "Ebay")]
    public class EbayQuickSearchTest : IDisposable
    {
        private readonly DataContext context;

        public EbayQuickSearchTest(DatabaseFixture db)
        {
            EbayOrderItemEntity.SetEffectiveCheckoutStatusAlgorithm(e => 0);
            EbayOrderItemEntity.SetEffectivePaymentMethodAlgorithm(e => 0);

            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));
        }

        [Fact]
        public void ReturnedSqlSearchesOrderTable_ForEbayBuyerID()
        {
            var builder = context.Mock.Mock<ISqlGenerationBuilder>();
            builder.Setup(x => x.RegisterParameter(AnyField, AnyString)).Returns("@p1");

            var order = Create.Order<EbayOrderEntity>(context.Store, context.Customer)
                .Set(x => x.EbayBuyerID, "Foo")
                .Save();

            var searchSql = context.Mock.Create<EbayQuickSearchSql>()
                .GenerateSql(builder.Object, "Foo")
                .Single(x => x.Contains("[EbayOrder]"));

            TestQuery(searchSql,
                command => command.AddParameterWithValue("p1", "Foo"),
                reader =>
                {
                    reader.Read();
                    Assert.Equal(order.OrderID, reader.GetInt64(reader.GetOrdinal("OrderID")));
                });

            TestQuery(searchSql,
                command => command.AddParameterWithValue("p1", "Bar"),
                reader => Assert.False(reader.Read()));
        }

        [Fact]
        public void ReturnedSqlSearchesOrderSearchTable_ForEbayBuyerID()
        {
            var builder = context.Mock.Mock<ISqlGenerationBuilder>();
            builder.Setup(x => x.RegisterParameter(AnyField, AnyString)).Returns("@p1");

            var order = Create.Order<EbayOrderEntity>(context.Store, context.Customer).Save();

            var orderSearch = Create.Entity<EbayOrderSearchEntity>()
                .Set(x => x.OrderID, order.OrderID)
                .Set(x => x.EbayBuyerID, "Foo")
                .Save();

            var searchSql = context.Mock.Create<EbayQuickSearchSql>()
                .GenerateSql(builder.Object, "Foo")
                .Single(x => x.Contains("[EbayOrderSearch]"));

            TestQuery(searchSql,
                command => command.AddParameterWithValue("p1", "Foo"),
                reader =>
                {
                    reader.Read();
                    Assert.Equal(orderSearch.OrderID, reader.GetInt64(reader.GetOrdinal("OrderID")));
                });

            TestQuery(searchSql,
                command => command.AddParameterWithValue("p1", "Bar"),
                reader => Assert.False(reader.Read()));
        }

        [Fact]
        public void ReturnedSqlSearchesOrderItemTable_ForSellingManagerRecord()
        {
            var builder = context.Mock.Mock<ISqlGenerationBuilder>();
            builder.Setup(x => x.RegisterParameter(AnyField, AnyString)).Returns("@p1");

            var order = Create.Order<EbayOrderEntity>(context.Store, context.Customer).Save();

            Create.Entity<EbayOrderItemEntity>()
                .Set(x => x.OrderID, order.OrderID)
                .Set(x => x.LocalEbayOrderID, order.OrderID)
                .Set(x => x.SellingManagerRecord, 6)
                .Save();

            var searchSql = context.Mock.Create<EbayQuickSearchSql>()
                .GenerateSql(builder.Object, "6")
                .Single(x => x.Contains("[EbayOrderItem]"));

            TestQuery(searchSql,
                command => command.AddParameterWithValue("p1", 6),
                reader =>
                {
                    reader.Read();
                    Assert.Equal(order.OrderID, reader.GetInt64(reader.GetOrdinal("OrderID")));
                });

            TestQuery(searchSql,
                command => command.AddParameterWithValue("p1", 7),
                reader => Assert.False(reader.Read()));
        }

        [Fact]
        public void ReturnedSqlSearchesOrderItemTable_ForCode()
        {
            var builder = context.Mock.Mock<ISqlGenerationBuilder>();
            builder.Setup(x => x.RegisterParameter(AnyField, AnyString)).Returns("@p1");

            var order = Create.Order<EbayOrderEntity>(context.Store, context.Customer).Save();

            Create.Entity<EbayOrderItemEntity>()
                .Set(x => x.OrderID, order.OrderID)
                .Set(x => x.LocalEbayOrderID, order.OrderID)
                .Set(x => x.Code, "Foo")
                .Save();

            var searchSql = context.Mock.Create<EbayQuickSearchSql>()
                .GenerateSql(builder.Object, "Foo")
                .Single(x => x.Contains("[OrderItem]"));

            TestQuery(searchSql,
                command => command.AddParameterWithValue("p1", "Foo"),
                reader =>
                {
                    reader.Read();
                    Assert.Equal(order.OrderID, reader.GetInt64(reader.GetOrdinal("OrderID")));
                });

            TestQuery(searchSql,
                command => command.AddParameterWithValue("p1", "Bar"),
                reader => Assert.False(reader.Read()));
        }

        public void Dispose() => context.Dispose();
    }
}