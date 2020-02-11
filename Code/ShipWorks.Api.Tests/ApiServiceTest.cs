using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Api.HealthCheck;
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
                .Setup(h => h.IsRunning())
                .Returns(false);

            var testObject = mock.Create<ApiService>();
            testObject.InitializeForCurrentDatabase(null);
            timer.Raise(r => r.Elapsed += null, EventArgs.Empty as ElapsedEventArgs);

            timer.Verify(t => t.Stop(), Times.Once);
            timer.Verify(t => t.Start(), Times.Once);
        }
    }
}
