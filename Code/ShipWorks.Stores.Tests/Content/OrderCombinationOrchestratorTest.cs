using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Core.Stores.Content;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Content;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.ExtensionMethods;
using ShipWorks.Users.Security;
using Xunit;

namespace ShipWorks.Stores.Tests.Content
{
    public class OrderCombinationOrchestratorTest : IDisposable
    {
        readonly AutoMock mock;
        IEnumerable<IOrderEntity> orders;

        public OrderCombinationOrchestratorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            orders = new[]
            {
                mock.Create<IOrderEntity>(),
                mock.Create<IOrderEntity>()
            };

            mock.Mock<ICombineOrdersGateway>()
                .Setup(x => x.LoadOrders(It.IsAny<IEnumerable<long>>()))
                .Returns(() => GenericResult.FromSuccess(orders));

            mock.Mock<ISecurityContext>()
                .Setup(x => x.HasPermission(It.IsAny<PermissionType>(), It.IsAny<long?>()))
                .Returns(true);

            mock.Mock<ICombineOrdersViewModel>()
                .Setup(x => x.GetCombinationDetailsFromUser(It.IsAny<IEnumerable<IOrderEntity>>()))
                .Returns(GenericResult.FromSuccess(Tuple.Create(6L, "6-C")));

            mock.Mock<IOrderCombiner>()
                .Setup(x => x.Combine(It.IsAny<long>(), It.IsAny<IEnumerable<IOrderEntity>>(), It.IsAny<string>()))
                .Returns(GenericResult.FromSuccess(23L));
        }

        [Fact]
        public void Combine_DelegatesToGateway_WhenValidOrderListIsPassedIn()
        {
            var testObject = mock.Create<OrderCombinationOrchestrator>();

            testObject.Combine(new long[] { 1, 2 });

            mock.Mock<ICombineOrdersGateway>()
                .Verify(x => x.LoadOrders(ItIs.Enumerable(1L, 2L)));
        }

        [Fact]
        public void Combine_DisplaysError_IfLoadingFailed()
        {
            mock.Mock<ICombineOrdersGateway>()
                .Setup(x => x.LoadOrders(It.IsAny<IEnumerable<long>>()))
                .Returns(GenericResult.FromError<IEnumerable<IOrderEntity>>("Foo"));
            var testObject = mock.Create<OrderCombinationOrchestrator>();

            testObject.Combine(new long[] { 1, 2 });

            mock.Mock<IMessageHelper>()
                .Verify(x => x.ShowError("Foo"));
        }

        [Fact]
        public void Combine_DoesNotDelegateToViewModel_WhenLoadingFails()
        {
            mock.Mock<ICombineOrdersGateway>()
                .Setup(x => x.LoadOrders(It.IsAny<IEnumerable<long>>()))
                .Returns(GenericResult.FromError<IEnumerable<IOrderEntity>>("Foo"));
            var testObject = mock.Create<OrderCombinationOrchestrator>();

            testObject.Combine(new long[] { 1, 2 });

            mock.Mock<ICombineOrdersViewModel>()
                .Verify(x => x.GetCombinationDetailsFromUser(It.IsAny<IEnumerable<IOrderEntity>>()), Times.Never);
        }

        [Fact]
        public void Combine_ReturnsFailure_WhenLoadingFails()
        {
            mock.Mock<ICombineOrdersGateway>()
                .Setup(x => x.LoadOrders(It.IsAny<IEnumerable<long>>()))
                .Returns(GenericResult.FromError<IEnumerable<IOrderEntity>>("Foo"));
            var testObject = mock.Create<OrderCombinationOrchestrator>();

            var result = testObject.Combine(new long[] { 1, 2 });

            Assert.True(result.Failure);
        }

        [Fact]
        public void Combine_DelegatesToSecurityContext_WhenLoadingSucceeds()
        {
            orders = new[]
            {
                new OrderEntity { StoreID = 9 },
                new OrderEntity { StoreID = 10 },
            };

            var testObject = mock.Create<OrderCombinationOrchestrator>();

            testObject.Combine(new long[] { 1, 2 });

            mock.Mock<ISecurityContext>()
                .Verify(x => x.HasPermission(PermissionType.OrdersModify, 9));
        }

        [Fact]
        public void Combine_ReturnsFailure_WhenUserDoesNotHavePermission()
        {
            mock.Mock<ISecurityContext>()
                .Setup(x => x.HasPermission(It.IsAny<PermissionType>(), It.IsAny<long?>()))
                .Returns(false);

            var testObject = mock.Create<OrderCombinationOrchestrator>();

            var result = testObject.Combine(new long[] { 1, 2 });

            Assert.True(result.Failure);
        }

        [Fact]
        public void Combine_DelegatesToViewModel_WhenUserHasPermission()
        {
            var testObject = mock.Create<OrderCombinationOrchestrator>();

            testObject.Combine(new long[] { 1, 2 });

            mock.Mock<ICombineOrdersViewModel>()
                .Verify(x => x.GetCombinationDetailsFromUser(orders));
        }

        [Fact]
        public void Combine_ReturnsFailure_WhenViewModelReturnsFailure()
        {
            mock.Mock<ICombineOrdersViewModel>()
                .Setup(x => x.GetCombinationDetailsFromUser(It.IsAny<IEnumerable<IOrderEntity>>()))
                .Returns(GenericResult.FromError<Tuple<long, string>>("Foo"));
            var testObject = mock.Create<OrderCombinationOrchestrator>();

            var result = testObject.Combine(new long[] { 1, 2 });

            Assert.True(result.Failure);
            Assert.Equal("Foo", result.Message);
        }

        [Fact]
        public void Combine_ShowsErrorMessage_WhenCombinationFails()
        {
            mock.Mock<IOrderCombiner>()
                .Setup(x => x.Combine(It.IsAny<long>(), It.IsAny<IEnumerable<IOrderEntity>>(), It.IsAny<string>()))
                .Returns(GenericResult.FromError<long>("Error"));

            var testObject = mock.Create<OrderCombinationOrchestrator>();

            testObject.Combine(new long[] { 1, 2 });

            mock.Mock<IMessageHelper>()
                .Verify(x => x.ShowError("Error"));
        }

        [Fact]
        public void Combine_ReturnsFailure_WhenCombinationFails()
        {
            mock.Mock<IOrderCombiner>()
                .Setup(x => x.Combine(It.IsAny<long>(), It.IsAny<IEnumerable<IOrderEntity>>(), It.IsAny<string>()))
                .Returns(GenericResult.FromError<long>("Error"));

            var testObject = mock.Create<OrderCombinationOrchestrator>();

            var result = testObject.Combine(new long[] { 1, 2 });

            Assert.True(result.Failure);
            Assert.Equal("Error", result.Message);
        }

        [Fact]
        public void DelegatesToCombineOrders()
        {
            var testObject = mock.Create<OrderCombinationOrchestrator>();

            testObject.Combine(new long[] { 1, 2 });

            mock.Mock<IOrderCombiner>()
                .Verify(x => x.Combine(6, orders, "6-C"));
        }

        [Theory]
        [InlineData("Order #1 was", 1, null, null, null)]
        [InlineData("Orders #1 and #2 were", 1, 2, null, null)]
        [InlineData("Orders #1, #2, and #3 were", 1, 2, 3, null)]
        [InlineData("Orders #1, #2, #3, and #4 were", 1, 2, 3, 4)]
        public void Combine_ShowsSuccessNotification_WhenCombinationSucceeds(string expected, long? id1, long? id2, long? id3, long? id4)
        {
            orders = new[] { id1, id2, id3, id4 }
                .Where(x => x.HasValue)
                .Select(x => new OrderEntity { OrderNumber = x.Value });

            var testObject = mock.Create<OrderCombinationOrchestrator>();

            testObject.Combine(new long[] { 1, 2 });

            mock.Mock<IMessageHelper>()
                .Verify(x => x.ShowUserConditionalInformation(
                    "Combine Orders",
                    $"{expected} combined into Order #6-C",
                    UserConditionalNotificationType.CombineOrders));
        }

        [Fact]
        public void Combine_ReturnsIdOfNewOrder_WhenCombinationSucceeds()
        {
            var testObject = mock.Create<OrderCombinationOrchestrator>();

            var result = testObject.Combine(new long[] { 1, 2 });

            Assert.True(result.Success);
            Assert.Equal(23L, result.Value);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
