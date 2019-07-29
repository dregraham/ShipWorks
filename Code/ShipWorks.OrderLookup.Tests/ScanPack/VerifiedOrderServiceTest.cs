using System;
using Autofac.Extras.Moq;
using ShipWorks.Tests.Shared;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.OrderLookup.ScanPack;
using Xunit;
using ShipWorks.Users;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Orders;

namespace ShipWorks.OrderLookup.Tests.ScanPack
{
    public class VerifiedOrderServiceTest : IDisposable
    {
        private readonly AutoMock mock;

        private readonly Mock<IOrderRepository> orderRepository;
        private readonly Mock<IUserSession> userSession;
        private readonly Mock<IDateTimeProvider> dateTimeProvider;
        private readonly VerifiedOrderService testObject;

        public VerifiedOrderServiceTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            orderRepository = mock.Mock<IOrderRepository>();
            userSession = mock.Mock<IUserSession>();
            dateTimeProvider = mock.Mock<IDateTimeProvider>();

            testObject = mock.Create<VerifiedOrderService>();
        }

        [Fact]
        public void Save_VerifiedInfoIsSavedCorrectly()
        {
            OrderEntity order = new OrderEntity();
            userSession.SetupGet(s => s.User).Returns(new UserEntity { UserID = 42 });
            var now = DateTime.Now;
            dateTimeProvider.SetupGet(d => d.UtcNow).Returns(now);
            testObject.Save(order);

            Assert.True(order.Verified);
            Assert.Equal(42, order.VerifiedBy);
            Assert.Equal(now, order.VerifiedDate);
        }   
        
        [Fact]
        public void Save_SavingOrderDeligatedToRepository()
        {
            OrderEntity order = new OrderEntity();
            userSession.SetupGet(s => s.User).Returns(new UserEntity { UserID = 42 });
            
            testObject.Save(order);

            orderRepository.Verify(r => r.Save(order), Times.Once);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
