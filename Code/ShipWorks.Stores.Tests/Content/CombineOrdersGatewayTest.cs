using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Collections;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.ExtensionMethods;
using Xunit;

namespace ShipWorks.Stores.Tests.Content
{
    public class CombineOrdersGatewayTest : IDisposable
    {
        readonly AutoMock mock;

        public CombineOrdersGatewayTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public async Task LoadOrders_DelegatesToOrderManager()
        {
            var sqlAdapter = mock.FromFactory<ISqlAdapterFactory>()
                .Mock(x => x.Create())
                .Object;

            var testObject = mock.Create<CombineOrdersGateway>();

            await testObject.LoadOrders(new[] { 1L, 2L });

            mock.Mock<IOrderManager>()
                .Verify(x => x.LoadOrdersAsync(ItIs.Enumerable(1L, 2L), sqlAdapter));
        }

        [Fact]
        public async Task LoadOrders_ReturnsOrders_WhenLoadWasSuccessful()
        {
            var order1 = new OrderEntity();
            var order2 = new OrderEntity();

            mock.Mock<IOrderManager>()
                .Setup(x => x.LoadOrdersAsync(It.IsAny<IEnumerable<long>>(), It.IsAny<ISqlAdapter>()))
                .ReturnsAsync(new[] { order1, order2 });

            var testObject = mock.Create<CombineOrdersGateway>();

            var results = await testObject.LoadOrders(new[] { 1L, 2L });

            Assert.True(results.Success);
            Assert.True(results.Value.UnorderedSequenceEquals(new[] { order1, order2 }));
        }

        [Fact]
        public async Task LoadOrders_ReturnsFailure_WhenLoadFails()
        {
            mock.Mock<IOrderManager>()
                .Setup(x => x.LoadOrdersAsync(It.IsAny<IEnumerable<long>>(), It.IsAny<ISqlAdapter>()))
                .ThrowsAsync(new ORMException("Foo"));

            var testObject = mock.Create<CombineOrdersGateway>();

            var results = await testObject.LoadOrders(new[] { 1L, 2L });

            Assert.True(results.Failure);
            Assert.Equal("Foo", results.Message);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
