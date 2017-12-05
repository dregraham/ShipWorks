using System;
using System.Linq;
using Interapptive.Shared.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Startup;
using ShipWorks.Stores.Platforms.CommerceInterface.Content;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using static ShipWorks.Stores.Tests.Integration.Helpers.DbHelpers;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Stores.Tests.Integration.Platforms.CommerceInterface
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    [Trait("Category", "QuickSearch")]
    [Trait("Store", "Groupon")]
    public class CommerceInterfaceQuickSearchTest : IDisposable
    {
        private readonly DataContext context;

        public CommerceInterfaceQuickSearchTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));
        }

        [Fact]
        public void ReturnedSqlSearchesOrderTable()
        {
            var builder = context.Mock.Mock<ISqlGenerationBuilder>();
            builder.Setup(x => x.RegisterParameter(AnyField, AnyString)).Returns("@p1");

            var order = Create.Order<CommerceInterfaceOrderEntity>(context.Store, context.Customer)
                .Set(x => x.CommerceInterfaceOrderNumber, "Foo")
                .Save();

            var searchSql = context.Mock.Create<CommerceInterfaceQuickSearchSql>()
                .GenerateSql(builder.Object, "Foo")
                .Single(x => x.Contains("[CommerceInterfaceOrder]"));

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

            var order = Create.Order<CommerceInterfaceOrderEntity>(context.Store, context.Customer).Save();

            var orderSearch = Create.Entity<CommerceInterfaceOrderSearchEntity>()
                .Set(x => x.OrderID, order.OrderID)
                .Set(x => x.CommerceInterfaceOrderNumber, "Foo")
                .Save();

            var searchSql = context.Mock.Create<CommerceInterfaceQuickSearchSql>()
                .GenerateSql(builder.Object, "Foo")
                .Single(x => x.Contains("[CommerceInterfaceOrderSearch]"));

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