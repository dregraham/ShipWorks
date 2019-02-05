using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Administration;
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
                Assert.True(testObject.IsAvailable);
            }
        }

        [Fact]
        public void IsAvailable_ReturnsFalse_WhenConnectionIsNotSuccessful()
        {
            Assert.False(testObject.IsAvailable);
        }

        [Fact]
        public void Update_WritesUpdateInfoToStream()
        {
            var serverTask = StartServer();
            Assert.True(testObject.Update(new Version(0, 0, 123)).Success);
            Assert.Equal("0.0.123", serverTask.Result);
        }

        private Task<string> StartServer()
        {
            return Task.Factory.StartNew(() =>
            {
                var server = new NamedPipeServerStream(sessionGuid.ToString());
                server.WaitForConnection();
                StreamReader reader = new StreamReader(server);
                while (true)
                {
                    return reader.ReadLine();
                }
            });
        }

        public void Dispose()
        {
            testObject?.Dispose();
        }
    }
}
