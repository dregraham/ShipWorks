using Xunit;
using Moq;
using Quartz;
using ShipWorks.Actions.Scheduling.QuartzNet;
using System;
using System.Linq;


namespace ShipWorks.Tests.Actions.Scheduling.QuartzNet
{
    public class IntervalCalendarTests
    {
        [Fact]
        public void IsTimeIncluded_ChainsWithBase()
        {
            var calendarBase = new Mock<ICalendar>().Object;

            var target = new IntervalCalendar(calendarBase) {
                StartTimeUtc = DateTimeOffset.Parse("6/3/2013Z").ToUniversalTime(),
                RepeatInterval = 1,
                RepeatIntervalUnit = IntervalUnit.Week
            };

            Assert.IsFalse(
                target.IsTimeIncluded(target.StartTimeUtc)
            );
        }


        [Fact]
        public void FirstWeekIsIncluded()
        {
            var target = new IntervalCalendar {
                StartTimeUtc = DateTimeOffset.Parse("6/3/2013Z").ToUniversalTime(),
                RepeatInterval = 4,
                RepeatIntervalUnit = IntervalUnit.Week
            };

            Assert.IsTrue(
                Enumerable.Range(0, 7).All(x => 
                    target.IsTimeIncluded(target.StartTimeUtc.AddDays(x))
                )
            );
        }

        [Fact]
        public void OffWeeksAreExcluded()
        {
            var target = new IntervalCalendar {
                StartTimeUtc = DateTimeOffset.Parse("6/3/2013Z").ToUniversalTime(),
                RepeatInterval = 3,
                RepeatIntervalUnit = IntervalUnit.Week
            };

            Assert.IsFalse(
                Enumerable.Range(7, 14).Any(x => 
                    target.IsTimeIncluded(target.StartTimeUtc.AddDays(x))
                )
            );
        }

        [Fact]
        public void IntervalWeekIsIncluded()
        {
            var target = new IntervalCalendar {
                StartTimeUtc = DateTimeOffset.Parse("6/3/2013Z").ToUniversalTime(),
                RepeatInterval = 2,
                RepeatIntervalUnit = IntervalUnit.Week
            };

            Assert.IsTrue(
                Enumerable.Range(14, 7).All(x =>
                    target.IsTimeIncluded(target.StartTimeUtc.AddDays(x))
                )
            );
        }
    }
}
