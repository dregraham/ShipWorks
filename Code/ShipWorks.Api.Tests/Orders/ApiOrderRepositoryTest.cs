using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Api.Orders;
using ShipWorks.Data.Connection;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Api.Tests.Orders
{
    public class ApiOrderRepositoryTest
    {
        private readonly AutoMock mock;
        private readonly IApiOrderRepository testObject;
        private readonly Mock<ISqlAdapterFactory> sqlAdapterFactory;
        private readonly Mock<ISqlAdapter> adapter;

        public ApiOrderRepositoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            adapter = mock.Mock<ISqlAdapter>();

            sqlAdapterFactory = mock.Mock<ISqlAdapterFactory>();
            sqlAdapterFactory.Setup(a => a.Create()).Returns(adapter);

            testObject = mock.Create<IApiOrderRepository>();
        }

        [Fact]
        public void GetOrders_DelegatesToSqlAdapterFactory_ForSqlAdapter()
        {
            testObject.GetOrders("11123-sdf");

            sqlAdapterFactory.Verify(a => a.Create());
        }
    }
}
