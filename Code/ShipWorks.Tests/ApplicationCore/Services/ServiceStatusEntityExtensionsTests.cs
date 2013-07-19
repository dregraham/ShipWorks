using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Data.Model.EntityClasses;
using System;


namespace ShipWorks.ApplicationCore.Services.Tests
{
    [TestClass]
    public class ServiceStatusEntityExtensionsTests
    {
        ServiceStatusEntity target;

        [TestInitialize]
        public void Initialize()
        {
            target = new ServiceStatusEntity();
        }


        [TestMethod]
        public void StatusIsNeverStartedWhenStartTimeIsNull()
        {
            Assert.AreEqual(ServiceStatus.NeverStarted, target.GetStatus());
        }

        [TestMethod]
        public void StatusIsStoppedWhenStopTimeIsAfterStartTime()
        {
            target.LastStartDateTime = DateTime.UtcNow;
            target.LastStopDateTime = target.LastStartDateTime.Value.AddHours(1);

            Assert.AreEqual(ServiceStatus.Stopped, target.GetStatus());
        }

        [TestMethod]
        public void StatusIsRunningWhenStopTimeIsNull()
        {
            target.LastStartDateTime = DateTime.UtcNow;
            target.LastCheckInDateTime = target.LastStartDateTime.Value.AddMinutes(10);

            Assert.AreEqual(ServiceStatus.Running, target.GetStatus());
        }

        [TestMethod]
        public void StatusIsRunningWhenStopTimeIsBeforeStartTime()
        {
            target.LastStartDateTime = DateTime.UtcNow;
            target.LastCheckInDateTime = target.LastStartDateTime.Value.AddMinutes(10);
            target.LastStopDateTime = target.LastStartDateTime.Value.AddHours(-1);

            Assert.AreEqual(ServiceStatus.Running, target.GetStatus());
        }

        [TestMethod]
        public void StatusIsNotRespondingWhenCheckInTimeIsBeyondThreshold()
        {
            target.LastCheckInDateTime = DateTime.UtcNow.Add(-ServiceStatusManager.NotRunningTimeSpan);
            target.LastStartDateTime = target.LastCheckInDateTime.Value.AddHours(-1);

            Assert.AreEqual(ServiceStatus.NotResponding, target.GetStatus());
        }
    }
}
