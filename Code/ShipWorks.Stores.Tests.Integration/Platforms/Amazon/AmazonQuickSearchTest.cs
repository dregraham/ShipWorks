using System;
using System.Linq;
using Interapptive.Shared.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Startup;
using ShipWorks.Stores.Platforms.Amazon.Content;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using static ShipWorks.Stores.Tests.Integration.Helpers.DbHelpers;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Stores.Tests.Integration.Platforms.Amazon
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    [Trait("Category", "QuickSearch")]
    [Trait("Store", "Amazon")]
    public class AmazonQuickSearchTest : IDisposable
    {
        private readonly DataContext context;

        public AmazonQuickSearchTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));
        }

        [Fact]
        public void ReturnedSqlSearchesOrderTable_ForAmazonOrderID()
        {
            var builder = context.Mock.Mock<ISqlGenerationBuilder>();
            builder.Setup(x => x.RegisterParameter(AnyField, AnyString)).Returns("@p1");

            var order = Create.Order<AmazonOrderEntity>(context.Store, context.Customer)
                .Set(x => x.AmazonOrderID, "Foo")
                .Save();

            var searchSql = context.Mock.Create<AmazonQuickSearchSql>()
                .GenerateSql(builder.Object, "Foo")
                .Single(x => x.Contains("[AmazonOrder]"));

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
        public void ReturnedSqlSearchesOrderSearchTable_ForAmazonOrderID()
        {
            var builder = context.Mock.Mock<ISqlGenerationBuilder>();
            builder.Setup(x => x.RegisterParameter(AnyField, AnyString)).Returns("@p1");

            var order = Create.Order<AmazonOrderEntity>(context.Store, context.Customer).Save();

            var orderSearch = Create.Entity<AmazonOrderSearchEntity>()
                .Set(x => x.OrderID, order.OrderID)
                .Set(x => x.AmazonOrderID, "Foo")
                .Save();

            var searchSql = context.Mock.Create<AmazonQuickSearchSql>()
                .GenerateSql(builder.Object, "Foo")
                .Single(x => x.Contains("[AmazonOrderSearch]"));

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

        public void Dispose() => context.Dispose();
    }
}