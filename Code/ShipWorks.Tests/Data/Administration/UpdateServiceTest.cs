﻿using System;
using System.IO.Pipes;
using Autofac.Extras.Moq;
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
            mock.Mock<IServiceName>().Setup(s => s.GetInstanceID()).Returns(sessionGuid);
            var communictionBridge = mock.Create<ShipWorksCommunicationBridge>();
            communictionBridge.StartPipeServer();

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
