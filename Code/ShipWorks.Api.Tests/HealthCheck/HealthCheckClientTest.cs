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

            mock.FromFactory<IRestClientFactory>().Mock(x => x.Create())
                .Setup(x => x.Execute(It.IsAny<IRestRequest>())).Returns(response);

            IHealthCheckClient testObject = mock.Create<HealthCheckClient>();

            Assert.True(testObject.IsRunning());
        }

        [Fact]
        public void IsRunning_ReturnsFalse_WhenEndpointDoesNotReturn200()
        {
            var response = mock.Mock<IRestResponse>();
            response.Setup(r => r.StatusCode).Returns(HttpStatusCode.InternalServerError);

            mock.FromFactory<IRestClientFactory>().Mock(x => x.Create())
                .Setup(x => x.Execute(It.IsAny<IRestRequest>())).Returns(response);

            IHealthCheckClient testObject = mock.Create<HealthCheckClient>();

            Assert.False(testObject.IsRunning());
        }

        [Fact]
        public void IsRunning_ReturnsFalse_WhenExceptionOccurs()
        {
            var response = mock.Mock<IRestResponse>();
            response.Setup(r => r.StatusCode).Returns(HttpStatusCode.OK);

            mock.FromFactory<IRestClientFactory>().Mock(x => x.Create())
                .Setup(x => x.Execute(It.IsAny<IRestRequest>())).Throws(new Exception());

            IHealthCheckClient testObject = mock.Create<HealthCheckClient>();

            Assert.False(testObject.IsRunning());
        }
    }
}
