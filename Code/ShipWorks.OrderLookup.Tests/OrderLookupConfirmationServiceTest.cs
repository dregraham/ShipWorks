using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.OrderLookup.Tests
{
    public class OrderLookupConfirmationServiceTest
    {
        private readonly AutoMock mock;
        private readonly OrderLookupConfirmationService testObject;

        public OrderLookupConfirmationServiceTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<OrderLookupConfirmationService>();
        }

        [Fact]
        public async Task ConfirmOrderReturnsNull_WhenNoOrderIDs()
        {
            Assert.Null(await testObject.ConfirmOrder(new List<long>()));
        }

        [Fact]
        public async Task ConfirmOrderReturnsFirstOrderID_WhenOrderIDsContainsOneOrderID()
        {
            Assert.Equal(123, await testObject.ConfirmOrder(new List<long>() { 123 }));
        }

        [Fact]
        public async Task ConfirmOrderDelegatesToOrderRepository()
        {
            await testObject.ConfirmOrder(new List<long>() { 123, 456, 789 });

            Mock<IOrderLookupOrderRepository> repo = mock.Mock<IOrderLookupOrderRepository>();

            repo.Verify(r => r.GetOrder(123));
            repo.Verify(r => r.GetOrder(456));
            repo.Verify(r => r.GetOrder(789));
        }
    }
}
