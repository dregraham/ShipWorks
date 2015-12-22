using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Tests.Integration.MSTest.Actions.Scheduling
{
    /// <summary>
    /// Struct for storing test run info
    /// </summary>
    public struct TestTime
    {
        public DateTime LocalTime;
        public DateTime ExpectedFireTime;
    }

    /// <summary>
    /// Base class for ActionScheduleTests.
    /// </summary>
    public abstract class ActionScheduleTest : ShipWorksInitializer
    {
        public DateTime InitialRunDateTime { get; set; }
        public int FiresEvery { get; set; }
        public TimeSpan FireTimeGracePeriod { get; set; }
        public int TotalRunsMade { get; set; }
        public int TotalRunsExpected { get; set; }
        public List<TestTime> TestTimes { get; set; }
    }
}
