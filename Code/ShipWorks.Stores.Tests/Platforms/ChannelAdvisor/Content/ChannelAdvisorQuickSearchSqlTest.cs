using System;
using Autofac.Extras.Moq;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Stores.Platforms.ChannelAdvisor.Content;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.ExtensionMethods;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Stores.Tests.Platforms.ChannelAdvisor.Content
{
    public class ChannelAdvisorQuickSearchSqlTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly Mock<ISqlGenerationBuilder> context;
        private readonly ChannelAdvisorQuickSearchSql testObject;

        public ChannelAdvisorQuickSearchSqlTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            context = mock.Mock<ISqlGenerationBuilder>();
            testObject = mock.Create<ChannelAdvisorQuickSearchSql>();
        }

        [Fact]
        public void GenerateSql_AddsTwoParameters()
        {
            testObject.GenerateSql(context.Object, "Foo");

            context.Verify(x => x.RegisterParameter(ItIs.Field(ChannelAdvisorOrderFields.CustomOrderIdentifier), "Foo"));
            context.Verify(x => x.RegisterParameter(ItIs.Field(ChannelAdvisorOrderItemFields.MarketplaceBuyerID), "Foo"));
        }

        [Fact]
        public void GenerateSql_ReturnsSearchSql()
        {
            context.SetupSequence(x => x.RegisterParameter(It.IsAny<EntityField2>(), AnyObject))
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
