﻿using Autofac.Extras.Moq;
using Interapptive.Shared.Threading;
using Microsoft.Reactive.Testing;
using Moq;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.OrderLookup.Tests
{
    public class OrderLookupDataServiceTest
    {
        private readonly AutoMock mock;
        private readonly TestMessenger testMessenger;

        public OrderLookupDataServiceTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testMessenger = new TestMessenger();
            mock.Provide<IMessenger>(testMessenger);
        }

        [Fact]
        public void InitializeForCurrentSession_SubscribesToOrderFoundMessage()
        {
            OrderLookupDataService testObject = mock.Create<OrderLookupDataService>();

            testObject.InitializeForCurrentSession();

            OrderEntity order = new OrderEntity();
            testMessenger.Send(new OrderFoundMessage(this, order));
            
            Assert.Equal(order, testObject.Order);
        }
    }
}
