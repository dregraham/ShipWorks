using System;
using Autofac.Extras.Moq;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Stores.Platforms.CommerceInterface.Content;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.ExtensionMethods;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Stores.Tests.Platforms.CommerceInterface.Content
{
    public class CommerceInterfaceQuickSearchSqlTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly Mock<ISqlGenerationBuilder> context;
        private readonly CommerceInterfaceQuickSearchSql testObject;

        public CommerceInterfaceQuickSearchSqlTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            context = mock.Mock<ISqlGenerationBuilder>();
            testObject = mock.Create<CommerceInterfaceQuickSearchSql>();
        }

        [Fact]
        public void StoreType_ReturnsCommerceInterface()
        {
            Assert.Equal(StoreTypeCode.CommerceInterface, testObject.StoreType);
        }

        [Fact]
        public void GenerateSql_AddsTwoParameters()
        {
            testObject.GenerateSql(context.Object, "Foo");

            context.Verify(x => x.RegisterParameter(ItIs.Field(CommerceInterfaceOrderFields.CommerceInterfaceOrderNumber), "Foo"));
        }

        [Fact]
        public void GenerateSql_ReturnsSearchSql()
        {
            context.Setup(x => x.RegisterParameter(It.IsAny<EntityField2>(), AnyObject)).Returns("p1");

            var result = testObject.GenerateSql(context.Object, "Foo");

            Assert.Contains("SELECT OrderId FROM [CommerceInterfaceOrder] WHERE CommerceInterfaceOrderNumber LIKE p1", result);
            Assert.Contains("SELECT OrderId FROM [CommerceInterfaceOrderSearch] WHERE CommerceInterfaceOrderNumber LIKE p1", result);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
