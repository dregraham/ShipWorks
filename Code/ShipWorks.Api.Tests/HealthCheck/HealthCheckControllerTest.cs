using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Hosting;
using Autofac.Extras.Moq;
using ShipWorks.Api.HealthCheck;
using ShipWorks.ApplicationCore;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Api.Tests.HealthCheck
{
    public class HealthCheckControllerTest
    {
        private readonly AutoMock mock;

        public HealthCheckControllerTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void Get_Returns200()
        {
            var testObject = SetupTestObject(Guid.NewGuid());

            var response = testObject.Get();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public void Get_ResponseContainsInstanceID_WhenSuccessful()
        {
            Guid instanceID = Guid.NewGuid();

            var testObject = SetupTestObject(instanceID);
            var response = testObject.Get().Content.ReadAsAsync<HealthCheckResponse>();

            Assert.Equal(instanceID, response.Result.InstanceId);
        }

        private HealthCheckController SetupTestObject(Guid sessionID)
        {
            var session = mock.Mock<IShipWorksSession>();
            session.SetupGet(x => x.InstanceID).Returns(sessionID);

            HealthCheckController testObject = new HealthCheckController(session.Object);

            testObject.Request = new HttpRequestMessage();
            testObject.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = new HttpConfiguration();
            return testObject;
        }
    }
}
