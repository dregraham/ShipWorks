using System;
using System.Net;
using Autofac.Extras.Moq;
using Interapptive.Shared.Net.RestSharp;
using Moq;
using RestSharp;
using ShipWorks.Api.HealthCheck;
using ShipWorks.ApplicationCore;
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
        public void IsRunning_ReturnsTrue_WhenEndpointReturns200_AndInstanceIDMatches()
        {
            var instanceGuid = Guid.NewGuid();

            var response = mock.Mock<IRestResponse<HealthCheckResponse>>();
            response.Setup(r => r.StatusCode).Returns(HttpStatusCode.OK);
            response.Setup(r => r.Data).Returns(new HealthCheckResponse(instanceGuid, "OK"));

            mock.Mock<IShipWorksSession>().Setup(s => s.InstanceID).Returns(instanceGuid);

            mock.FromFactory<IRestClientFactory>().Mock(x => x.Create())
                .Setup(x => x.Execute<HealthCheckResponse>(It.IsAny<IRestRequest>())).Returns(response);

            IHealthCheckClient testObject = mock.Create<HealthCheckClient>();

            Assert.True(testObject.IsRunning(8081, false));
        }

        [Fact]
        public void IsRunning_ReturnsFalse_WhenEndpointDoesNotReturn200()
        {
            var instanceGuid = Guid.NewGuid();

            var response = mock.Mock<IRestResponse<HealthCheckResponse>>();
            response.Setup(r => r.StatusCode).Returns(HttpStatusCode.InternalServerError);
            response.Setup(r => r.Data).Returns(new HealthCheckResponse(instanceGuid, "OK"));

            mock.Mock<IShipWorksSession>().Setup(s => s.InstanceID).Returns(instanceGuid);

            mock.FromFactory<IRestClientFactory>().Mock(x => x.Create())
                .Setup(x => x.Execute<HealthCheckResponse>(It.IsAny<IRestRequest>())).Returns(response);

            IHealthCheckClient testObject = mock.Create<HealthCheckClient>();

            Assert.False(testObject.IsRunning(8081, false));
        }

        [Fact]
        public void IsRunning_ReturnsFalse_WhenExceptionOccurs()
        {
            mock.FromFactory<IRestClientFactory>().Mock(x => x.Create())
                .Setup(x => x.Execute<HealthCheckResponse>(It.IsAny<IRestRequest>())).Throws(new Exception());

            IHealthCheckClient testObject = mock.Create<HealthCheckClient>();

            Assert.False(testObject.IsRunning(8081, false));
        }
    }
}
