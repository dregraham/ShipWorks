using System;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Administration;
using ShipWorks.Tests.Shared;
using Xunit;


namespace ShipWorks.Escalator.Tests
{
    public class ShipWorksCommunicationBridgeTest
    {
        private readonly AutoMock mock;
        private readonly ShipWorksCommunicationBridge testObject;
        private readonly Guid sessionGuid;
        private readonly UpdateService updateService;

        public ShipWorksCommunicationBridgeTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            sessionGuid = Guid.NewGuid();

            mock.Mock<IServiceName>().Setup(s => s.GetInstanceID()).Returns(sessionGuid);

            testObject = mock.Create<ShipWorksCommunicationBridge>();
            testObject.StartPipeServer();

            var session = mock.Mock<IShipWorksSession>();
            session.SetupGet(s => s.InstanceID).Returns(sessionGuid);

            var dataPath = mock.Mock<IDataPath>();
            dataPath.SetupGet(d => d.InstanceSettings).Returns(string.Empty);

            updateService = mock.Create<UpdateService>();
        }

        [Fact]
        public void CommunicationBridgeOnMessage_RecievesMessagesFromShipWorksUpdater()
        {
            Version version = new Version(1, 2, 3, 4567);

            bool messageReceived = false;

            testObject.OnMessage += (s) =>
            {
                Assert.Equal(version.ToString(), s.Trim('\0'));
                messageReceived = true;
            };

            updateService.Update(version);

            Functional.Retry(() => {
                Assert.True(messageReceived);
                return true;
            }, 5, TimeSpan.FromMilliseconds(250), ex => messageReceived == false);

            Assert.True(messageReceived);
        }
    }
}
