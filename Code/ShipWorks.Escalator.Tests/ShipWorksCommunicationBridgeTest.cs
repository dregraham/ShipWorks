using System;
using System.Threading.Tasks;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Administration;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Escalator.Tests
{
    public class ShipWorksCommunicationBridgeTest
    {
        private readonly Autofac.Extras.Moq.AutoMock mock;
        private readonly ShipWorksCommunicationBridge testObject;
        private readonly Guid sessionGuid;
        private readonly UpdateService updateService;

        public ShipWorksCommunicationBridgeTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            sessionGuid = Guid.NewGuid();

            testObject = new ShipWorksCommunicationBridge(sessionGuid.ToString());

            var session = mock.Mock<IShipWorksSession>();
            session.SetupGet(s => s.InstanceID).Returns(sessionGuid);

            updateService = mock.Create<UpdateService>();
        }

        [Fact]
        public void CommunicationBridgeOnMessage_RecievesMessagesFromShipWorksUpdater()
        {
            Version version = new Version(1, 2, 3, 4567);

            // spin up a task to simulate the shipworks UpdateService sending an update message
            Task.Factory.StartNew(() =>
            {
                updateService.Update(version);
            });

            testObject.OnMessage += (s) => Assert.Equal(version.ToString(), s);
        }
    }
}
