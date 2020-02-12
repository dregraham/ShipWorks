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
        public void Get_Returns200_WhenInstanceIDIsNotNull()
        {
            var session = mock.Mock<IShipWorksSession>();
            session.SetupGet(x => x.InstanceID).Returns(Guid.NewGuid());

            var testObject = SetupTestObject(session.Object);

            var response = testObject.Get();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public void Get_ResponseContainsInstanceID_WhenSuccessful()
        {
            Guid instanceID = Guid.NewGuid();

            var session = mock.Mock<IShipWorksSession>();
            session.SetupGet(x => x.InstanceID).Returns(instanceID);

            var testObject = SetupTestObject(session.Object);
            var response = testObject.Get().Content.ReadAsAsync<HealthCheckResponse>();

            Assert.Equal(instanceID, response.Result.InstanceId);
        }

        [Fact]
        public void Get_Returns500_WhenShipWorksSessionIsNull()
        {
            var testObject = SetupTestObject(null);
            var response = testObject.Get();

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public void Get_Returns500_WhenInstanceIDIsNull()
        {
            var session = mock.Mock<IShipWorksSession>();
            session.SetupGet(x => x.InstanceID).Returns(Guid.Empty);

            var testObject = SetupTestObject(session.Object);
            var response = testObject.Get();

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        private static HealthCheckController SetupTestObject(IShipWorksSession session)
        {
            HealthCheckController testObject = new HealthCheckController(session);
            testObject.Request = new HttpRequestMessage();
            testObject.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = new HttpConfiguration();
            return testObject;
        }
    }
}
