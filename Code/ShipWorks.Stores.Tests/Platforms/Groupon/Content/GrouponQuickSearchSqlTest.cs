using System;
using Autofac.Extras.Moq;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Stores.Platforms.Groupon.Content;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.ExtensionMethods;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Stores.Tests.Platforms.Groupon.Content
{
    [Trait("Category", "QuickSearch")]
    [Trait("Store", "Groupon")]
    public class GrouponQuickSearchSqlTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly Mock<ISqlGenerationBuilder> context;
        private readonly GrouponQuickSearchSql testObject;

        public GrouponQuickSearchSqlTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            context = mock.Mock<ISqlGenerationBuilder>();
            testObject = mock.Create<GrouponQuickSearchSql>();
        }

        [Fact]
        public void StoreType_ReturnsGroupon()
        {
            Assert.Equal(StoreTypeCode.Groupon, testObject.StoreType);
        }

        [Fact]
        public void GenerateSql_AddsTwoParameters()
        {
            testObject.GenerateSql(context.Object, "Foo");

            context.Verify(x => x.RegisterParameter(ItIs.Field(GrouponOrderFields.GrouponOrderID), "Foo"));
            context.Verify(x => x.RegisterParameter(ItIs.Field(GrouponOrderFields.ParentOrderID), "Foo"));
        }

        [Fact]
        public void GenerateSql_ReturnsSearchSql()
        {
            context.SetupSequence(x => x.RegisterParameter(It.IsAny<EntityField2>(), AnyObject))
                .Returns("p1")
                .Returns("p2");

            var result = testObject.GenerateSql(context.Object, "Foo");

            Assert.Contains("SELECT OrderId FROM [GrouponOrder] WHERE GrouponOrderID LIKE p1 OR ParentOrderID LIKE p2", result);
            Assert.Contains("SELECT OrderId FROM [GrouponOrderSearch] WHERE GrouponOrderID LIKE p1 OR ParentOrderID LIKE p2", result);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
