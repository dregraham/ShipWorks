﻿using System;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Orders.Split;
using ShipWorks.Tests.Shared;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Stores.Tests.Orders.Split
{
    public class OrderSplitUserInteractionTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly OrderSplitUserInteraction testObject;

        public OrderSplitUserInteractionTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<OrderSplitUserInteraction>();
        }

        [Fact]
        public async Task GetSplitDetailsFromUser_DelegatesToOrderSplitViewModel()
        {
            var order = new OrderEntity();

            await testObject.GetSplitDetailsFromUser(order, "Foo");

            mock.Mock<IOrderSplitViewModel>()
                .Verify(x => x.GetSplitDetailsFromUser(order, "Foo"));
        }

        [Fact]
        public async Task GetSplitDetailsFromUser_ReturnsValue_FromViewModel()
        {
            var viewModelResult = GenericResult.FromSuccess<OrderSplitDefinition>(null);

            mock.Mock<IOrderSplitViewModel>()
                .Setup(x => x.GetSplitDetailsFromUser(It.IsAny<OrderEntity>(), AnyString))
                .Returns(viewModelResult);

            var result = await testObject.GetSplitDetailsFromUser(new OrderEntity(), "Foo");

            Assert.Equal(viewModelResult, result);
        }

        [Fact]
        public async Task ShowSuccessConfirmation_DelegatesToOrderSplitSuccessViewModel()
        {
            var result = new[] { "Foo", "Bar" };

            await testObject.ShowSuccessConfirmation(result);

            mock.Mock<IOrderSplitSuccessViewModel>()
                .Verify(x => x.ShowSuccessConfirmation(result));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
