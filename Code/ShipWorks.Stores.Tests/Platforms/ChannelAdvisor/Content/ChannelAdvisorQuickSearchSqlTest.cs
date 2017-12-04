using System;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Stores.Platforms.ChannelAdvisor.Content;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.ExtensionMethods;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.ChannelAdvisor.Content
{
    public class ChannelAdvisorQuickSearchSqlTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly Mock<ISqlGenerationContext> context;
        private readonly ChannelAdvisorQuickSearchSql testObject;

        public ChannelAdvisorQuickSearchSqlTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            context = mock.Mock<ISqlGenerationContext>();
            testObject = mock.Create<ChannelAdvisorQuickSearchSql>();
        }

        [Fact]
        public void GenerateSql_AddsTwoUsedFields()
        {
            testObject.GenerateSql(context.Object, "Foo");

            context.Verify(x => x.AddColumnUsed(ItIs.Field(ChannelAdvisorOrderFields.CustomOrderIdentifier)));
            context.Verify(x => x.AddColumnUsed(ItIs.Field(ChannelAdvisorOrderItemFields.MarketplaceBuyerID)));
        }

        [Fact]
        public void GenerateSql_AddsTwoParameters()
        {
            testObject.GenerateSql(context.Object, "Foo");

            context.Verify(x => x.RegisterParameter("Foo"), Times.Exactly(2));
        }

        [Fact]
        public void GenerateSql_ReturnsSearchSql()
        {
            context.SetupSequence(x => x.RegisterParameter(It.IsAny<object>()))
                .Returns("p1")
                .Returns("p2");

            var result = testObject.GenerateSql(context.Object, "Foo");

            Assert.Contains("SELECT OrderId FROM [ChannelAdvisorOrder] WHERE CustomOrderIdentifier LIKE p1", result);
            Assert.Contains("SELECT OrderId FROM [ChannelAdvisorOrderSearch] WHERE CustomOrderIdentifier LIKE p1", result);
            Assert.Contains("SELECT OrderId FROM [ChannelAdvisorOrderItem] WHERE MarketplaceBuyerID LIKE p2", result);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
