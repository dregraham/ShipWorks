using System;
using Autofac.Extras.Moq;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Stores.Platforms.ProStores.Content;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.ExtensionMethods;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Stores.Tests.Platforms.ProStores.Content
{
    public class ProStoresQuickSearchSqlTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly Mock<ISqlGenerationBuilder> context;
        private readonly ProStoresQuickSearchSql testObject;

        public ProStoresQuickSearchSqlTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            context = mock.Mock<ISqlGenerationBuilder>();
            testObject = mock.Create<ProStoresQuickSearchSql>();
        }

        [Fact]
        public void StoreType_ReturnsProStores()
        {
            Assert.Equal(StoreTypeCode.ProStores, testObject.StoreType);
        }

        [Fact]
        public void GenerateSql_AddsOneParameter()
        {
            testObject.GenerateSql(context.Object, "Foo");

            context.Verify(x => x.RegisterParameter(ItIs.Field(ProStoresOrderFields.ConfirmationNumber), "Foo"));
        }

        [Fact]
        public void GenerateSql_ReturnsSearchSql()
        {
            context.Setup(x => x.RegisterParameter(It.IsAny<EntityField2>(), AnyObject)).Returns("p1");

            var result = testObject.GenerateSql(context.Object, "Foo");

            Assert.Contains("SELECT OrderId FROM [ProStoresOrder] WHERE ConfirmationNumber LIKE p1", result);
            Assert.Contains("SELECT OrderId FROM [ProStoresOrderSearch] WHERE ConfirmationNumber LIKE p1", result);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
