using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Actions.Scheduling.QuartzNet.ActionScheduleAdapters;


namespace ShipWorks.Tests.Actions.Scheduling.QuartzNet.ActionScheduleAdapters
{
    [TestClass]
    public class DailyActionScheduleAdapterTests
    {
        DailyActionScheduleAdapter target;

        [TestInitialize]
        public void Initialize()
        {
            target = new DailyActionScheduleAdapter();
        }
    }
}
