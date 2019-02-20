using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.AutoUpdate;
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

        [Fact]
        public void OnStart_CallsGetUpdateWindow()
        {
            var testObject = mock.Create<Escalator>();
            testObject.OnStart();
            mock.Mock<IUpgradeTimeWindow>().Verify(b => b.CallGetUpdateWindow(), Times.Once);
        }

        [Fact]
        public void OnStart_AttemptsUpgrade_AfterShipWorksMessage_WhenMessageIsVersionNumber()
        {
            var testObject = mock.Create<Escalator>();
            testObject.OnStart();

            mock.Mock<IShipWorksCommunicationBridge>().Raise(b => b.OnMessage += null, "1.2.3");

            mock.Mock<IShipWorksUpgrade>().Verify(u => u.Upgrade(new Version("1.2.3")), Times.Once);
        }

        [Fact]
        public void OnStart_CallsUpdateWindow_AfterShipWorksMessage_WhenMessageIsUpdateWindowData()
        {
            var testObject = mock.Create<Escalator>();
            testObject.OnStart();

            string message = "{\"AutoUpdateDayOfWeek\":2,\"AutoUpdateHourOfDay\":3,\"TangoCustomerId\":\"id\"}";
            mock.Mock<IShipWorksCommunicationBridge>().Raise(b => b.OnMessage += null, message);

            mock.Mock<IUpgradeTimeWindow>().Verify(w => w.UpdateWindow(It.Is<UpdateWindowData>(d => d.TangoCustomerId == "id")), Times.Once);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
