using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Actions.Scheduling.QuartzNet;
using ShipWorks.Actions.Triggers;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Tests.Actions.Scheduling.QuartzNet
{
    [TestClass]
    public class QuartzSchedulingEngineTest
    {
        private QuartzSchedulingEngine testObject;

        [TestInitialize]
        public void Initialize()
        {
            testObject = new QuartzSchedulingEngine();
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void Schedule_ThrowsNotImplementedException_Test()
        {
            // This will obviously need to change as the Quartz scheduling engine gets implemented
            testObject.Schedule(new ActionEntity(), new CronTrigger());
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void IsExistingJob_ThrowsNotImplementedException_Test()
        {
            // This will obviously need to change as the Quartz scheduling engine gets implemented
            testObject.IsExistingJob(new ActionEntity(), new CronTrigger());
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void GetTrigger_ThrowsNotImplementedException_Test()
        {
            // This will obviously need to change as the Quartz scheduling engine gets implemented
            testObject.GetTrigger(new ActionEntity());
        }
    }
}
