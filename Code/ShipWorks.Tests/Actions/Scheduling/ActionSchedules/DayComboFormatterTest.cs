using System.Collections.Generic;
using Xunit;
using ShipWorks.Actions.Scheduling.ActionSchedules.Editors;
using ShipWorks.Actions.Scheduling.ActionSchedules.Editors.UI;

namespace ShipWorks.Tests.Actions.Scheduling.ActionSchedules
{
    public class DayComboFormatterTest
    {
        private readonly DateOfMonthComboFormatter testObject = new DateOfMonthComboFormatter();

        [Fact]
        public void FormatDays_WorksWithNonConsecutiveDays_Test()
        {
            string formattedDays = testObject.FormatDays(new List<int>
            {
                1, 3
            });

            Assert.AreEqual("1, 3", formattedDays);
        }

        [Fact]
        public void FormatDays_WorksWith2ConsecutiveDays_Test()
        {
            string formattedDays = testObject.FormatDays(new List<int>
            {
                1, 2
            });

            Assert.AreEqual("1, 2", formattedDays);
        }

        [Fact]
        public void FormatDays_WorksWith3ConsecutiveDays_Test()
        {
            string formattedDays = testObject.FormatDays(new List<int>
            {
                1, 2, 3
            });

            Assert.AreEqual("1-3", formattedDays);
        }

        [Fact]
        public void FormatDays_WorksWith3ConsecutiveDaysFollowedByNonConsecutiveDays_Test()
        {
            string formattedDays = testObject.FormatDays(new List<int>
            {
                1, 2, 3, 5, 7, 10
            });

            Assert.AreEqual("1-3, 5, 7, 10", formattedDays);
        }

        [Fact]
        public void FormatDays_WorksWith2GroupsOf3ConsecutiveDays_Test()
        {
            string formattedDays = testObject.FormatDays(new List<int>
            {
                1, 2, 3, 5, 6, 7
            });

            Assert.AreEqual("1-3, 5-7", formattedDays);
        }

        [Fact]
        public void FormatDays_WorksWith2GroupsOfConsecutiveDaysFollowedByNonConsecutiveDays_Test()
        {
            string formattedDays = testObject.FormatDays(new List<int>
            {
                1, 2, 3, 5, 6, 7, 8, 9, 11, 12
            });

            Assert.AreEqual("1-3, 5-9, 11, 12", formattedDays);
        }

        [Fact]
        public void FormatDays_WorksWith2GroupsOfConsecutiveDaysSeperatedByNonConsecutiveDays_Test()
        {
            string formattedDays = testObject.FormatDays(new List<int>
            {
                1, 2, 3, 5, 6, 8, 9, 10
            });

            Assert.AreEqual("1-3, 5, 6, 8-10", formattedDays);
        }

        [Fact]
        public void FormatDays_WorksWith2GroupsOfConsecutiveDaysPrefacedkSeperatedByNonConsecutiveDays_Test()
        {
            string formattedDays = testObject.FormatDays(new List<int>
            {
                1, 3, 4, 5, 6, 8, 9, 10, 20
            });

            Assert.AreEqual("1, 3-6, 8-10, 20", formattedDays);
        }
    }
}