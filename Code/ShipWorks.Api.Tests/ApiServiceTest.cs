using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using Owin;
using ShipWorks.Api.Configuration;
using ShipWorks.Api.HealthCheck;
using ShipWorks.Api.Infrastructure;
using ShipWorks.ApplicationCore.ExecutionMode;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Api.Tests
{
    public class ApiServiceTest
    {
        private readonly AutoMock mock;
        private readonly Mock<ITimer> timer;

        public ApiServiceTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            timer = mock.Mock<ITimer>();
        }

        [Fact]
        public void InitializeForCurrentDatabase_StopsAndStartsTheTimer_WhenTimeEllapses()
        {
            mock.Mock<IHealthCheckClient>()
                .Setup(h => h.IsRunning(8081))
                .Returns(false);

            var testObject = mock.Create<ApiService>();
            testObject.InitializeForCurrentDatabase(null);

            timer.Verify(t => t.Start(), Times.Once);
            timer.Verify(t => t.Stop(), Times.Never);

            timer.Raise(r => r.Elapsed += null, EventArgs.Empty as ElapsedEventArgs);

            timer.Verify(t => t.Stop(), Times.Once);
            timer.Verify(t => t.Start(), Times.Exactly(2));
        }

        [Fact]
        public void InitializeForCurrentDatabase_StartsWebApp_WhenServiceIsNotRunning()
        {
            mock.Mock<IHealthCheckClient>()
                .Setup(h => h.IsRunning(8081))
                .Returns(false);

            var testObject = mock.Create<ApiService>();
            
            testObject.InitializeForCurrentDatabase(null);
            timer.Raise(r => r.Elapsed += null, EventArgs.Empty as ElapsedEventArgs);

            mock.Mock<IWebApp>().Verify(a => a.Start("http://+:8081/", It.IsAny<Action<IAppBuilder>>()), Times.Once);
        }

        [Fact]
        public void InitializeForCurrentDatabase_DoesNotStartWebApp_WhenServiceIsRunning()
        {
            mock.Mock<IHealthCheckClient>()
                .Setup(h => h.IsRunning(8081))
                .Returns(true);

            var testObject = mock.Create<ApiService>();
            testObject.InitializeForCurrentDatabase(null);
            timer.Raise(r => r.Elapsed += null, EventArgs.Empty as ElapsedEventArgs);

            mock.Mock<IWebApp>().Verify(a => a.Start("http://+:8081/", It.IsAny<Action<IAppBuilder>>()), Times.Never);
        }

        [Fact]
        public void InitializeForCurrentDatabase_DisposesOldService_WhenServiceIsNotRunning()
        {
            mock.Mock<IHealthCheckClient>()
                .Setup(h => h.IsRunning(8081))
                .Returns(false);

            var startedWebApp = mock.CreateMock<IDisposable>();
            mock.Mock<IWebApp>().Setup(a => a.Start(It.IsAny<string>(), It.IsAny<Action<IAppBuilder>>()))
                .Returns(startedWebApp.Object);

            var testObject = mock.Create<ApiService>();

            testObject.InitializeForCurrentDatabase(null);

            timer.Raise(r => r.Elapsed += null, EventArgs.Empty as ElapsedEventArgs);
            
            startedWebApp.Verify(a => a.Dispose(), Times.Never);
            timer.Raise(r => r.Elapsed += null, EventArgs.Empty as ElapsedEventArgs);
            startedWebApp.Verify(a => a.Dispose(), Times.Once);
        }
    }
}
