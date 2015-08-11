using Xunit;
using ShipWorks.Data.Model.EntityClasses;
using System;


namespace ShipWorks.ApplicationCore.Services.Tests
{
    public class ServiceStatusEntityExtensionsTests
    {
        ServiceStatusEntity target;

        public ServiceStatusEntityExtensionsTests()
        {
            target = new ServiceStatusEntity();
        }


        [Fact]
        public void StatusIsNeverStartedWhenStartTimeIsNull()
        {
            Assert.Equal(ServiceStatus.NeverStarted, target.GetStatus());
        }

        [Fact]
        public void StatusIsStoppedWhenStopTimeIsAfterStartTime()
        {
            target.LastStartDateTime = DateTime.UtcNow;
            target.LastStopDateTime = target.LastStartDateTime.Value.AddHours(1);

            Assert.Equal(ServiceStatus.Stopped, target.GetStatus());
        }

        [Fact]
        public void StatusIsRunningWhenStopTimeIsNull()
        {
            target.LastStartDateTime = DateTime.UtcNow;
            target.LastCheckInDateTime = target.LastStartDateTime.Value.AddMinutes(10);

            Assert.Equal(ServiceStatus.Running, target.GetStatus());
        }

        [Fact]
        public void StatusIsRunningWhenStopTimeIsBeforeStartTime()
        {
            target.LastStartDateTime = DateTime.UtcNow;
            target.LastCheckInDateTime = target.LastStartDateTime.Value.AddMinutes(10);
            target.LastStopDateTime = target.LastStartDateTime.Value.AddHours(-1);

            Assert.Equal(ServiceStatus.Running, target.GetStatus());
        }

        [Fact]
        public void StatusIsNotRespondingWhenCheckInTimeIsBeyondThreshold()
        {
            target.LastCheckInDateTime = DateTime.UtcNow.Add(-ServiceStatusManager.NotRunningTimeSpan);
            target.LastStartDateTime = target.LastCheckInDateTime.Value.AddHours(-1);

            Assert.Equal(ServiceStatus.NotResponding, target.GetStatus());
        }
    }
}
