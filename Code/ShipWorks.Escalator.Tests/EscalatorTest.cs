using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Escalator.Tests
{
    public class EscalatorTest : IDisposable
    {
        private readonly AutoMock mock;

        public EscalatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void OnStart_StartPipeServerIsCalled()
        {
            var testObject = mock.Create<Escalator>();
            testObject.OnStart();
            mock.Mock<IShipWorksCommunicationBridge>().Verify(b => b.StartPipeServer(), Times.Once);
        }
        
        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
