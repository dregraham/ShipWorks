using System;
using System.IO.Pipes;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.AutoUpdate;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Administration;
using ShipWorks.Escalator;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Tests.Data.Administration
{
    public class UpdateServiceTest : IDisposable
    {
        private AutoMock mock;
        private UpdateService testObject;
        private Guid sessionGuid;

        public UpdateServiceTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            sessionGuid = Guid.NewGuid();

            var session = mock.Mock<IShipWorksSession>();
            session.SetupGet(s => s.InstanceID).Returns(sessionGuid);

            var dataPath = mock.Mock<IDataPath>();
            dataPath.SetupGet(d => d.InstanceSettings).Returns(string.Empty);

            testObject = mock.Create<UpdateService>();
        }

        [Fact]
        public void Update_SendsMessageWithVersion()
        {
            mock.Mock<IServiceName>().Setup(s => s.GetInstanceID()).Returns(sessionGuid);
            var communicationBridge = mock.Mock<IShipWorksCommunicationBridge>();
            Version version = new Version(0, 0, 123);
            testObject.Update(version);
            
            communicationBridge.Verify(x => x.SendMessage(version.ToString()), Times.Once);
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}
