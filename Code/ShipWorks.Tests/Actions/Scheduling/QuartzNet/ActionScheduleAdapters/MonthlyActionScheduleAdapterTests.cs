using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Actions.Scheduling.ActionSchedules;
using ShipWorks.Actions.Scheduling.QuartzNet.ActionScheduleAdapters;
using System;
using System.Linq;


namespace ShipWorks.Tests.Actions.Scheduling.QuartzNet.ActionScheduleAdapters
{
    [TestClass]
    public class MonthlyActionScheduleAdapterTests
    {
        MonthlyActionScheduleAdapter target;

        [TestInitialize]
        public void Initialize()
        {
            target = new MonthlyActionScheduleAdapter();
        }


        [TestMethod]
        public void FiresAtStartTimeOfDay()
        {
            var schedule = new MonthlyActionSchedule {
                StartDateTimeInUtc = DateTime.Parse("6/3/2013 03:31Z").ToUniversalTime(),
                //TODO: add recurring settings
            };

            var fireTimes = schedule.ComputeFireTimes(target, 5);

            Assert.IsTrue(fireTimes.All(
                x => x.ToUniversalTime().TimeOfDay == schedule.StartDateTimeInUtc.TimeOfDay
            ));
        }
    }
}
