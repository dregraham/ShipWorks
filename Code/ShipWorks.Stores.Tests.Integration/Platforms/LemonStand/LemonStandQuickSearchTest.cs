using System;
using System.Linq;
using Interapptive.Shared.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Startup;
using ShipWorks.Stores.Platforms.LemonStand.Content;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using static ShipWorks.Stores.Tests.Integration.Helpers.DbHelpers;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Stores.Tests.Integration.Platforms.LemonStand
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    [Trait("Category", "QuickSearch")]
    [Trait("Store", "LemonStand")]
    public class LemonStandQuickSearchTest : IDisposable
    {
        private readonly DataContext context;

        public LemonStandQuickSearchTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));
        }

        [Fact]
        public void ReturnedSqlSearchesOrderTable()
        {
            var builder = context.Mock.Mock<ISqlGenerationBuilder>();
            builder.Setup(x => x.RegisterParameter(AnyField, AnyString)).Returns("@p1");

            var order = Create.Order<LemonStandOrderEntity>(context.Store, context.Customer)
                .Set(x => x.LemonStandOrderID, "Foo")
                .Save();

            var searchSql = context.Mock.Create<LemonStandQuickSearchSql>()
                .GenerateSql(builder.Object, "Foo")
                .Single(x => x.Contains("[LemonStandOrder]"));

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
        public void ReturnedSqlSearchesOrderSearchTable()
        {
            var builder = context.Mock.Mock<ISqlGenerationBuilder>();
            builder.Setup(x => x.RegisterParameter(AnyField, AnyString)).Returns("@p1");

            var order = Create.Order<LemonStandOrderEntity>(context.Store, context.Customer).Save();

            var orderSearch = Create.Entity<LemonStandOrderSearchEntity>()
                .Set(x => x.OrderID, order.OrderID)
                .Set(x => x.LemonStandOrderID, "Foo")
                .Save();

            var searchSql = context.Mock.Create<LemonStandQuickSearchSql>()
                .GenerateSql(builder.Object, "Foo")
                .Single(x => x.Contains("[LemonStandOrderSearch]"));

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