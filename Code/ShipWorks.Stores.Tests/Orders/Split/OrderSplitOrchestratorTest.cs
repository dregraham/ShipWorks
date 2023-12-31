﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Orders.Split;
using ShipWorks.Stores.Orders.Split.Local;
using ShipWorks.Tests.Shared;
using ShipWorks.Users.Security;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Stores.Tests.Orders.Split
{
    [SuppressMessage("SonarLint", "S112: Code should not throw base Exception",
        Justification = "We're testing that any exception causes expected behavior")]
    public class OrderSplitOrchestratorTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly OrderSplitOrchestrator testObject;
        private readonly OrderEntity order;
        private readonly OrderSplitDefinition orderSplitDefinition;

        public OrderSplitOrchestratorTest()
        {
            order = new OrderEntity { OrderID = 1006, StoreID = 1005, OrderNumber = 1234 };
            orderSplitDefinition = new OrderSplitDefinition(order, new Dictionary<long, decimal>(), new Dictionary<long, decimal>(), "Foo", OrderSplitterType.Local);

            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            testObject = mock.Create<OrderSplitOrchestrator>();
            mock.Mock<IOrderSplitGateway>()
                .Setup(x => x.LoadOrder(AnyLong))
                .ReturnsAsync(order);
            mock.Mock<ISecurityContext>()
                .Setup(x => x.RequestPermission(It.IsAny<PermissionType>(), AnyLong))
                .Returns(Result.FromSuccess());
            mock.Mock<IOrderSplitUserInteraction>()
                .Setup(x => x.GetSplitDetailsFromUser(It.IsAny<OrderEntity>(), AnyString))
                .ReturnsAsync(orderSplitDefinition);
            mock.Mock<IOrderSplitter>()
                .Setup(x => x.Split(It.IsAny<OrderSplitDefinition>(), It.IsAny<IProgressReporter>()))
                .ReturnsAsync(new Dictionary<long, string>());
            mock.Mock<IOrderSplitterFactory>()
                .Setup(x => x.Create(It.IsAny<OrderSplitterType>()))
                .Returns(mock.Create<IOrderSplitter>());
            mock.Mock<IAsyncMessageHelper>()
                .Setup(x => x.ShowProgressDialog(AnyString, AnyString))
                .ReturnsAsync(mock.CreateMock<ISingleItemProgressDialog>().Object);
        }

        [Fact]
        public async Task Split_CallsLoadOrder_OnGateway()
        {
            await testObject.Split(1006);

            mock.Mock<IOrderSplitGateway>().Verify(x => x.LoadOrder(1006));
        }

        [Fact]
        public async Task Split_RequestsPermission_WhenOrderLoadSucceeds()
        {
            await testObject.Split(1006);

            mock.Mock<ISecurityContext>()
                .Verify(x => x.RequestPermission(PermissionType.OrdersModify, 1005));
        }

        [Fact]
        public async Task Split_DoesNotRequestPermission_WhenOrderLoadFails()
        {
            mock.Mock<IOrderSplitGateway>().Setup(x => x.LoadOrder(AnyLong))
                .Returns(Task.FromException<OrderEntity>(new Exception("Foo")));

            await testObject.Split(1006).Recover(ex => null);

            mock.Mock<ISecurityContext>()
                .Verify(x => x.RequestPermission(It.IsAny<PermissionType>(), AnyLong), Times.Never);
        }

        [Fact]
        public async Task Split_GetsSuggestedOrderNumber_WhenUserHasPermission()
        {
            await testObject.Split(1006).Recover(ex => null);

            mock.Mock<IOrderSplitGateway>()
                .Verify(x => x.GetNextOrderNumber(1006, "1234"));
        }

        [Fact]
        public async Task Split_DoesNotGetSuggestedOrderNumber_WhenUserDoesNotHavePermission()
        {
            mock.Mock<ISecurityContext>()
                .Setup(x => x.RequestPermission(It.IsAny<PermissionType>(), AnyLong))
                .Returns(Result.FromError("Foo"));

            await testObject.Split(1006).Recover(ex => null);

            mock.Mock<IOrderSplitGateway>()
                .Verify(x => x.GetNextOrderNumber(1006, "1234"), Times.Never);
        }

        [Fact]
        public async Task Split_GetsDetailsFromUser_WhenSuggestedOrderNumberSucceeds()
        {
            mock.Mock<IOrderSplitGateway>()
                .Setup(x => x.GetNextOrderNumber(AnyLong, AnyString))
                .ReturnsAsync("Bar");

            await testObject.Split(1006).Recover(ex => null);

            mock.Mock<IOrderSplitUserInteraction>()
                .Verify(x => x.GetSplitDetailsFromUser(order, "Bar"));
        }

        [Fact]
        public async Task Split_DoesNotGetDetailsFromUser_WhenSuggestedOrderNumberFails()
        {
            mock.Mock<IOrderSplitGateway>()
                .Setup(x => x.GetNextOrderNumber(AnyLong, AnyString))
                .Returns(Task.FromException<string>(new Exception("Foo")));

            await testObject.Split(1006).Recover(ex => null);

            mock.Mock<IOrderSplitUserInteraction>()
                .Verify(x => x.GetSplitDetailsFromUser(It.IsAny<OrderEntity>(), AnyString), Times.Never);
        }

        [Fact]
        public async Task Split_CallsSplitOnOrderSplitter_WhenUserProvidesDetails()
        {
            await testObject.Split(1006).Recover(ex => null);

            mock.Mock<IOrderSplitter>()
                .Verify(x => x.Split(orderSplitDefinition, It.IsAny<IProgressReporter>()));
        }

        [Fact]
        public async Task Split_DoesNotCallSplitOnOrderSplitter_WhenUserCancelsDialog()
        {
            mock.Mock<IOrderSplitUserInteraction>()
                .Setup(x => x.GetSplitDetailsFromUser(It.IsAny<OrderEntity>(), AnyString))
                .Returns(Task.FromException<OrderSplitDefinition>(new Exception("Foo")));

            await testObject.Split(1006).Recover(ex => null);

            mock.Mock<IOrderSplitter>()
                .Verify(x => x.Split(It.IsAny<OrderSplitDefinition>(), It.IsAny<IProgressReporter>()), Times.Never);
        }

        [Fact]
        public async Task Split_ShowsSuccessDialog_WhenSplitSucceeds()
        {
            mock.Mock<IOrderSplitter>()
                .Setup(x => x.Split(It.IsAny<OrderSplitDefinition>(), It.IsAny<IProgressReporter>()))
                .ReturnsAsync(new Dictionary<long, string> { { 1, "Foo" }, { 2, "Bar" } });

            await testObject.Split(1006).Recover(ex => null);

            mock.Mock<IOrderSplitUserInteraction>()
                .Verify(x => x.ShowSuccessConfirmation(new[] { "Foo", "Bar" }));
        }

        [Fact]
        public async Task Split_ShowsErrorMessage_WhenSplitFails()
        {
            mock.Mock<IOrderSplitter>()
                .Setup(x => x.Split(It.IsAny<OrderSplitDefinition>(), It.IsAny<IProgressReporter>()))
                .Returns(Task.FromException<IDictionary<long, string>>(new Exception("Error!")));

            await testObject.Split(1006).Recover(ex => null);

            mock.Mock<IAsyncMessageHelper>()
                .Verify(x => x.ShowError("Error!"));
        }

        [Fact]
        public async Task Split_ReturnsSplitOrderIDs_WhenSuccessful()
        {
            mock.Mock<IOrderSplitter>()
                .Setup(x => x.Split(It.IsAny<OrderSplitDefinition>(), It.IsAny<IProgressReporter>()))
                .ReturnsAsync(new Dictionary<long, string> { { 1, "Foo" }, { 2, "Bar" } });

            var result = await testObject.Split(1006).Recover(ex => null);

            Assert.Equal(2, result.Count());
            Assert.Contains(1, result);
            Assert.Contains(2, result);
        }

        [Fact]
        public async Task Split_ReturnsFailure_WhenSplitFails()
        {
            mock.Mock<IOrderSplitter>()
                .Setup(x => x.Split(It.IsAny<OrderSplitDefinition>(), It.IsAny<IProgressReporter>()))
                .Returns(Task.FromException<IDictionary<long, string>>(new Exception("Error!")));

            await Assert.ThrowsAsync<Exception>(() => testObject.Split(1006));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
