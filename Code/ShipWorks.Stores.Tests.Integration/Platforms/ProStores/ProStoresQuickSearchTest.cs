﻿using System;
using System.Linq;
using Interapptive.Shared.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Startup;
using ShipWorks.Stores.Platforms.ProStores.Content;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using static ShipWorks.Stores.Tests.Integration.Helpers.DbHelpers;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Stores.Tests.Integration.Platforms.ProStores
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class ProStoresQuickSearchTest : IDisposable
    {
        private readonly DataContext context;

        public ProStoresQuickSearchTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));
        }

        [Fact]
        public void ReturnedSqlSearchesOrderTable()
        {
            var builder = context.Mock.Mock<ISqlGenerationBuilder>();
            builder.Setup(x => x.RegisterParameter(AnyField, AnyString)).Returns("@p1");

            var order = Create.Order<ProStoresOrderEntity>(context.Store, context.Customer)
                .Set(x => x.ConfirmationNumber, "Foo")
                .Save();

            var searchSql = context.Mock.Create<ProStoresQuickSearchSql>()
                .GenerateSql(builder.Object, "Foo")
                .Single(x => x.Contains("[ProStoresOrder]"));

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

            var order = Create.Order<ProStoresOrderEntity>(context.Store, context.Customer).Save();

            var orderSearch = Create.Entity<ProStoresOrderSearchEntity>()
                .Set(x => x.OrderID, order.OrderID)
                .Set(x => x.ConfirmationNumber, "Foo")
                .Save();

            var searchSql = context.Mock.Create<ProStoresQuickSearchSql>()
                .GenerateSql(builder.Object, "Foo")
                .Single(x => x.Contains("[ProStoresOrderSearch]"));

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