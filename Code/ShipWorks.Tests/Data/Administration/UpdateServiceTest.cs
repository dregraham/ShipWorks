using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using log4net;
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

            testObject = mock.Create<UpdateService>();
        }

        [Fact]
        public void IsAvailable_ReturnsTrue_WhenConnectionSuccessful()
        {
            using (NamedPipeServerStream pipeServer = new NamedPipeServerStream(sessionGuid.ToString()))
            {
                Assert.True(testObject.IsAvailable());
            }
        }

        [Fact]
        public void IsAvailable_ReturnsFalse_WhenConnectionIsNotSuccessful()
        {
            Assert.False(testObject.IsAvailable());
        }

        [Fact]
        public void Update_WritesUpdateInfoToStream()
        {
            var communictionBridge = new ShipWorksCommunicationBridge(sessionGuid.ToString(), mock.Mock<ILog>().Object);

            communictionBridge.OnMessage += (m) => Assert.Equal("0.0.123", m);

            Assert.True(testObject.IsAvailable());
            Assert.True(testObject.Update(new Version(0, 0, 123)).Success);
        }

        public void Dispose()
        {
            testObject?.Dispose();
        }
    }
}
