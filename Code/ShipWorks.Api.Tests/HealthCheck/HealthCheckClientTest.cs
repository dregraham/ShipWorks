using System;
using System.Net;
using Autofac.Extras.Moq;
using Interapptive.Shared.Net.RestSharp;
using Moq;
using RestSharp;
using ShipWorks.Api.HealthCheck;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Api.Tests.HealthCheck
{
    public class HealthCheckClientTest
    {
        private readonly AutoMock mock;

        public HealthCheckClientTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void IsRunning_ReturnsTrue_WhenEndpointReturns200()
        {
            var response = mock.Mock<IRestResponse>();
            response.Setup(r => r.StatusCode).Returns(HttpStatusCode.OK);

            var client = mock.Mock<IRestClient>();
            client.Setup(c => c.Execute(It.IsAny<IRestRequest>())).Returns(response);
            mock.Mock<IRestClientFactory>().Setup(f => f.Create()).Returns(client);

            IHealthCheckClient testObject = mock.Create<HealthCheckClient>();

            Assert.True(testObject.IsRunning());
        }

        [Fact]
        public void IsRunning_ReturnsFalse_WhenEndpointDoesNotReturn200()
        {
            var response = mock.Mock<IRestResponse>();
            response.Setup(r => r.StatusCode).Returns(HttpStatusCode.InternalServerError);

            var client = mock.Mock<IRestClient>();
            client.Setup(c => c.Execute(It.IsAny<IRestRequest>())).Returns(response);
            mock.Mock<IRestClientFactory>().Setup(f => f.Create()).Returns(client);

            IHealthCheckClient testObject = mock.Create<HealthCheckClient>();

            Assert.False(testObject.IsRunning());
        }

        [Fact]
        public void IsRunning_ReturnsFalse_WhenExceptionOccurs()
        {
            var response = mock.Mock<IRestResponse>();
            response.Setup(r => r.StatusCode).Returns(HttpStatusCode.OK);

            var client = mock.Mock<IRestClient>();
            client.Setup(c => c.Execute(It.IsAny<IRestRequest>())).Throws(new Exception());
            mock.Mock<IRestClientFactory>().Setup(f => f.Create()).Returns(client);

            IHealthCheckClient testObject = mock.Create<HealthCheckClient>();

            Assert.False(testObject.IsRunning());
        }
    }
}
