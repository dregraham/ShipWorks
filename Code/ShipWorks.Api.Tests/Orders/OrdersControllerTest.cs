using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Hosting;
using Autofac.Extras.Moq;
using ShipWorks.Api.Orders;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Tests.Shared;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Api.Tests.Orders
{
    public class OrdersControllerTest
    {
        private readonly AutoMock mock;

        public OrdersControllerTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public async Task Get_Returns200_WhenOrderIsFound()
        {
            mock.Mock<IApiOrderRepository>()
                .Setup(x => x.GetOrders(AnyString))
                .Returns(new []
                {
                    new OrderEntity(1)
                });

            mock.Mock<IOrdersResponseFactory>().Setup(x => x.CreateOrdersResponse(AnyIOrder))
                .Returns(new OrderResponse());

            var testObject = mock.Create<OrdersController>();
            testObject.Request = new HttpRequestMessage();
            testObject.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = new HttpConfiguration();

            var response = await testObject.Get("1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Get_Returns404_WhenNoOrderIsFound()
        {
            mock.Mock<IApiOrderRepository>()
                .Setup(x => x.GetOrders(AnyString))
                .Returns(new OrderEntity[0]);

            var testObject = mock.Create<OrdersController>();
            testObject.Request = new HttpRequestMessage();
            testObject.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = new HttpConfiguration();

            var response = await testObject.Get("1");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Get_Returns409_WhenMultipleOrdersAreFound()
        {
            mock.Mock<IApiOrderRepository>()
                .Setup(x => x.GetOrders(AnyString))
                .Returns(new []
                {
                    new OrderEntity(1),
                    new OrderEntity(2),
                });

            var testObject = mock.Create<OrdersController>();
            testObject.Request = new HttpRequestMessage();
            testObject.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = new HttpConfiguration();

            var response = await testObject.Get("1");

            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }

        [Fact]
        public async Task Get_Returns500_WhenExceptionOccurs()
        {
            mock.Mock<IApiOrderRepository>()
                .Setup(x => x.GetOrders(AnyString))
                .Throws(new Exception());

            var testObject = mock.Create<OrdersController>();
            testObject.Request = new HttpRequestMessage();
            testObject.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = new HttpConfiguration();

            var response = await testObject.Get("1");

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }
    }
}
