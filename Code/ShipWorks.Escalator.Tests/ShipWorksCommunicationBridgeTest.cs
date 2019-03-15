using System;
using System.IO.Pipes;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.AutoUpdate;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Administration;
using ShipWorks.Tests.Shared;
using Xunit;


namespace ShipWorks.Escalator.Tests
{
    public class ShipWorksCommunicationBridgeTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly ShipWorksCommunicationBridge testObject;
        private readonly Guid sessionGuid;

        public ShipWorksCommunicationBridgeTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            sessionGuid = Guid.NewGuid();

            mock.Mock<IServiceName>().Setup(s => s.GetInstanceID()).Returns(sessionGuid);

            testObject = mock.Create<ShipWorksCommunicationBridge>(new TypedParameter(typeof(string), "instance"));

            var session = mock.Mock<IShipWorksSession>();
            session.SetupGet(s => s.InstanceID).Returns(sessionGuid);

            var dataPath = mock.Mock<IDataPath>();
            dataPath.SetupGet(d => d.InstanceSettings).Returns(string.Empty);
        }

        [Fact]
        public void IsAvailable_ReturnsTrue_WhenConnectionSuccessful()
        {
            testObject.StartPipeServer();
    
            Assert.True(testObject.IsAvailable());
        }

        [Fact]
        public void IsAvailable_ReturnsFalse_WhenConnectionIsNotSuccessful()
        {
            Assert.False(testObject.IsAvailable());
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}
