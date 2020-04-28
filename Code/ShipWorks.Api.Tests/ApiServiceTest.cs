using System;
using System.Timers;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using Owin;
using ShipWorks.Api.Configuration;
using ShipWorks.Api.HealthCheck;
using ShipWorks.Api.Infrastructure;
using ShipWorks.Tests.Shared;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Api.Tests
{
    public class ApiServiceTest
    {
        private readonly AutoMock mock;
        private readonly Mock<ITimer> timer;
        private readonly ApiSettings apiSettings = new ApiSettings();

        public ApiServiceTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            timer = mock.Mock<ITimer>();
            mock.Mock<IApiSettingsRepository>()
                .Setup(r => r.Load())
                .Returns(() => apiSettings);
        }

        [Fact]
        public void InitializeForCurrentDatabase_StopsAndStartsTheTimer_WhenTimeEllapses()
        {
            mock.Mock<IHealthCheckClient>()
                .Setup(h => h.IsRunning(8081, false))
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
                .Setup(h => h.IsRunning(8081, false))
                .Returns(false);

            var testObject = mock.Create<ApiService>();
            
            testObject.InitializeForCurrentDatabase(null);
            timer.Raise(r => r.Elapsed += null, EventArgs.Empty as ElapsedEventArgs);

            mock.Mock<IWebApp>().Verify(a => a.Start("http://+:8081/", It.IsAny<Action<IAppBuilder>>()), Times.Once);
        }

        [Fact]
        public void InitializeForCurrentDatabase_StartsWebApp_WithCorrectPort()
        {
            apiSettings.Port = 42;

            mock.Mock<IHealthCheckClient>()
               .Setup(h => h.IsRunning(8081, false))
               .Returns(false);

            var testObject = mock.Create<ApiService>();

            testObject.InitializeForCurrentDatabase(null);
            timer.Raise(r => r.Elapsed += null, EventArgs.Empty as ElapsedEventArgs);

            mock.Mock<IWebApp>().Verify(a => a.Start("http://+:42/", It.IsAny<Action<IAppBuilder>>()), Times.Once);
        }

        [Fact]
        public void InitializeForCurrentDatabase_DoesNotStartWebApp_WhenApiDisabled()
        {
            apiSettings.Enabled = false;


            mock.Mock<IHealthCheckClient>()
               .Setup(h => h.IsRunning(8081, false))
               .Returns(false);

            var testObject = mock.Create<ApiService>();

            testObject.InitializeForCurrentDatabase(null);
            timer.Raise(r => r.Elapsed += null, EventArgs.Empty as ElapsedEventArgs);

            mock.Mock<IWebApp>().Verify(a => a.Start(AnyString, It.IsAny<Action<IAppBuilder>>()), Times.Never);
        }

        [Fact]
        public void InitializeForCurrentDatabase_DoesNotStartWebApp_WhenServiceIsRunning()
        {
            mock.Mock<IHealthCheckClient>()
                .Setup(h => h.IsRunning(8081, false))
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
                .Setup(h => h.IsRunning(8081, false))
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
