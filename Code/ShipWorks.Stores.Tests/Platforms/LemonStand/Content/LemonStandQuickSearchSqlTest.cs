using System;
using Autofac.Extras.Moq;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Stores.Platforms.LemonStand.Content;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.ExtensionMethods;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Stores.Tests.Platforms.LemonStand.Content
{
    [Trait("Category", "QuickSearch")]
    [Trait("Store", "LemonStand")]
    public class LemonStandQuickSearchSqlTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly Mock<ISqlGenerationBuilder> context;
        private readonly LemonStandQuickSearchSql testObject;

        public LemonStandQuickSearchSqlTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            context = mock.Mock<ISqlGenerationBuilder>();
            testObject = mock.Create<LemonStandQuickSearchSql>();
        }

        [Fact]
        public void GenerateSql_AddsTwoParameters()
        {
            testObject.GenerateSql(context.Object, "Foo");

            context.Verify(x => x.RegisterParameter(ItIs.Field(LemonStandOrderFields.LemonStandOrderID), "Foo"));
        }

        [Fact]
        public void GenerateSql_ReturnsSearchSql()
        {
            context.SetupSequence(x => x.RegisterParameter(It.IsAny<EntityField2>(), AnyObject))
                .Returns("p1")
                .Returns("p2");

            var result = testObject.GenerateSql(context.Object, "Foo");

            Assert.Contains("SELECT OrderId FROM [LemonStandOrder] WHERE LemonStandOrderID LIKE p1", result);
            Assert.Contains("SELECT OrderId FROM [LemonStandOrderSearch] WHERE LemonStandOrderID LIKE p1", result);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
