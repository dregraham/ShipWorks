﻿using System;
using Autofac.Extras.Moq;
using Moq;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Api.Orders;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Api.Tests.Orders
{
    public class ApiOrderRepositoryTest
    {
        private readonly AutoMock mock;
        private readonly ApiOrderRepository testObject;
        private readonly Mock<ISqlAdapterFactory> sqlAdapterFactory;
        private readonly Mock<ISqlAdapter> adapter;

        public ApiOrderRepositoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            adapter = mock.Mock<ISqlAdapter>();

            sqlAdapterFactory = mock.Mock<ISqlAdapterFactory>();
            sqlAdapterFactory.Setup(a => a.Create()).Returns(adapter);

            testObject = mock.Create<ApiOrderRepository>();
        }

        [Fact]
        public void GetOrders_DelegatesToSqlAdapterFactory_ForSqlAdapter()
        {
            mock.Mock<ISqlSession>().Setup(a => a.CanConnect()).Returns(true);
            testObject.GetOrders("11123-sdf");

            sqlAdapterFactory.Verify(a => a.Create());
        }

        [Fact]
        public void GetOrders_DelegatesToSqlAdapter_WithQuery()
        {
            mock.Mock<ISqlSession>().Setup(a => a.CanConnect()).Returns(true);
            testObject.GetOrders("11123-sdf");

            adapter.Verify(a => a.FetchQuery(It.IsAny<EntityQuery<OrderEntity>>(), It.IsAny<EntityCollection<OrderEntity>>()));
        }

        [Fact]
        public void GetOrders_ThrowsException_WhenCannotConnectToDatabase()
        {
            mock.Mock<ISqlSession>().Setup(a => a.CanConnect()).Returns(false);
            var ex = Assert.Throws<Exception>(() => testObject.GetOrders("11123-sdf"));
            Assert.Equal("Unable to connect to ShipWorks database.", ex.Message);
        }
    }
}
