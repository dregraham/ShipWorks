using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Core.Stores.Content;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Content.Controls;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.ExtensionMethods;
using ShipWorks.Users.Security;
using Xunit;

namespace ShipWorks.Stores.Tests.Content
{
    public class CombineOrderOrchestratorTest : IDisposable
    {
        private readonly AutoMock mock;
        private IEnumerable<IOrderEntity> orders;

        public CombineOrderOrchestratorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            orders = new[]
            {
                mock.Create<IOrderEntity>(),
                mock.Create<IOrderEntity>()
            };

            mock.Mock<ICombineOrderGateway>()
                .Setup(x => x.LoadOrders(It.IsAny<IEnumerable<long>>()))
                .ReturnsAsync(() => GenericResult.FromSuccess(orders));

            mock.Mock<ISecurityContext>()
                .Setup(x => x.HasPermission(It.IsAny<PermissionType>(), It.IsAny<long?>()))
                .Returns(true);

            mock.Mock<IOrderCombinationUserInteraction>()
                .Setup(x => x.GetCombinationDetailsFromUser(It.IsAny<IEnumerable<IOrderEntity>>()))
                .Returns(GenericResult.FromSuccess(Tuple.Create(6L, "6-C")));

            mock.Mock<ICombineOrder>()
                .Setup(x => x.Combine(It.IsAny<long>(), It.IsAny<IEnumerable<IOrderEntity>>(), It.IsAny<string>(), It.IsAny<IProgressReporter>()))
                .ReturnsAsync(GenericResult.FromSuccess(23L));
        }

        [Fact]
        public async Task Combine_DelegatesToGateway_WhenValidOrderListIsPassedIn()
        {
            var testObject = mock.Create<CombineOrderOrchestrator>();

            await testObject.Combine(new long[] { 1, 2 });

            mock.Mock<ICombineOrderGateway>()
                .Verify(x => x.LoadOrders(ItIs.Enumerable(1L, 2L)));
        }

        [Fact]
        public async Task Combine_DisplaysError_IfLoadingFailed()
        {
            mock.Mock<ICombineOrderGateway>()
                .Setup(x => x.LoadOrders(It.IsAny<IEnumerable<long>>()))
                .ReturnsAsync(GenericResult.FromError<IEnumerable<IOrderEntity>>("Foo"));
            var testObject = mock.Create<CombineOrderOrchestrator>();

            await testObject.Combine(new long[] { 1, 2 });

            mock.Mock<IMessageHelper>()
                .Verify(x => x.ShowError("Foo"));
        }

        [Fact]
        public async Task Combine_DoesNotDelegateToViewModel_WhenLoadingFails()
        {
            mock.Mock<ICombineOrderGateway>()
                .Setup(x => x.LoadOrders(It.IsAny<IEnumerable<long>>()))
                .ReturnsAsync(GenericResult.FromError<IEnumerable<IOrderEntity>>("Foo"));
            var testObject = mock.Create<CombineOrderOrchestrator>();

            await testObject.Combine(new long[] { 1, 2 });

            mock.Mock<ICombineOrdersViewModel>()
                .Verify(x => x.GetCombinationDetailsFromUser(It.IsAny<IEnumerable<IOrderEntity>>()), Times.Never);
        }

        [Fact]
        public async Task Combine_ReturnsFailure_WhenLoadingFails()
        {
            mock.Mock<ICombineOrderGateway>()
                .Setup(x => x.LoadOrders(It.IsAny<IEnumerable<long>>()))
                .ReturnsAsync(GenericResult.FromError<IEnumerable<IOrderEntity>>("Foo"));
            var testObject = mock.Create<CombineOrderOrchestrator>();

            var result = await testObject.Combine(new long[] { 1, 2 });

            Assert.True(result.Failure);
        }

        [Fact]
        public async Task Combine_DelegatesToSecurityContext_WhenLoadingSucceeds()
        {
            orders = new[]
            {
                new OrderEntity { StoreID = 9 },
                new OrderEntity { StoreID = 10 },
            };

            var testObject = mock.Create<CombineOrderOrchestrator>();

            await testObject.Combine(new long[] { 1, 2 });

            mock.Mock<ISecurityContext>()
                .Verify(x => x.HasPermission(PermissionType.OrdersModify, 9));
        }

        [Fact]
        public async Task Combine_ReturnsFailure_WhenUserDoesNotHavePermission()
        {
            mock.Mock<ISecurityContext>()
                .Setup(x => x.HasPermission(It.IsAny<PermissionType>(), It.IsAny<long?>()))
                .Returns(false);

            var testObject = mock.Create<CombineOrderOrchestrator>();

            var result = await testObject.Combine(new long[] { 1, 2 });

            Assert.True(result.Failure);
        }

        [Fact]
        public async Task Combine_DelegatesToViewModel_WhenUserHasPermission()
        {
            var testObject = mock.Create<CombineOrderOrchestrator>();

            await testObject.Combine(new long[] { 1, 2 });

            mock.Mock<IOrderCombinationUserInteraction>()
                .Verify(x => x.GetCombinationDetailsFromUser(orders));
        }

        [Fact]
        public async Task Combine_ReturnsFailure_WhenViewModelReturnsFailure()
        {
            mock.Mock<IOrderCombinationUserInteraction>()
                .Setup(x => x.GetCombinationDetailsFromUser(It.IsAny<IEnumerable<IOrderEntity>>()))
                .Returns(GenericResult.FromError<Tuple<long, string>>("Foo"));
            var testObject = mock.Create<CombineOrderOrchestrator>();

            var result = await testObject.Combine(new long[] { 1, 2 });

            Assert.True(result.Failure);
            Assert.Equal("Foo", result.Message);
        }

        [Fact]
        public async Task Combine_ShowsErrorMessage_WhenCombinationFails()
        {
            mock.Mock<ICombineOrder>()
                .Setup(x => x.Combine(It.IsAny<long>(), It.IsAny<IEnumerable<IOrderEntity>>(), It.IsAny<string>(), It.IsAny<IProgressReporter>()))
                .ReturnsAsync(GenericResult.FromError<long>("Error"));

            var testObject = mock.Create<CombineOrderOrchestrator>();

            await testObject.Combine(new long[] { 1, 2 });

            mock.Mock<IMessageHelper>()
                .Verify(x => x.ShowError("Error"));
        }

        [Fact]
        public async Task Combine_ReturnsFailure_WhenCombinationFails()
        {
            mock.Mock<ICombineOrder>()
                .Setup(x => x.Combine(It.IsAny<long>(), It.IsAny<IEnumerable<IOrderEntity>>(), It.IsAny<string>(), It.IsAny<IProgressReporter>()))
                .ReturnsAsync(GenericResult.FromError<long>("Error"));

            var testObject = mock.Create<CombineOrderOrchestrator>();

            var result = await testObject.Combine(new long[] { 1, 2 });

            Assert.True(result.Failure);
            Assert.Equal("Error", result.Message);
        }

        [Fact]
        public async Task DelegatesToCombineOrders()
        {
            var testObject = mock.Create<CombineOrderOrchestrator>();

            await testObject.Combine(new long[] { 1, 2 });

            mock.Mock<ICombineOrder>()
                .Verify(x => x.Combine(6, orders, "6-C", It.IsAny<IProgressReporter>()));
        }

        [Fact]
        public async Task Combine_ShowsSuccessNotification_WhenCombinationSucceeds()
        {
            orders = new[] { 1006, 2006 }
                .Select(x => new OrderEntity { OrderNumber = x });

            var testObject = mock.Create<CombineOrderOrchestrator>();

            await testObject.Combine(new long[] { 1006, 2006 });

            mock.Mock<IOrderCombinationUserInteraction>()
                .Verify(x => x.ShowSuccessConfirmation("6-C", orders));
        }

        [Fact]
        public async Task Combine_ReturnsIdOfNewOrder_WhenCombinationSucceeds()
        {
            var testObject = mock.Create<CombineOrderOrchestrator>();

            var result = await testObject.Combine(new long[] { 1, 2 });

            Assert.True(result.Success);
            Assert.Equal(23L, result.Value);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}