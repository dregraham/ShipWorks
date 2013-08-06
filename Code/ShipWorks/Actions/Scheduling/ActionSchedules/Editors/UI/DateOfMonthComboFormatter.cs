using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Actions.Scheduling.ActionSchedules.Editors.UI
{
    /// <summary>
    /// Formats days, grouping consecutive days (ie 2-4 vs 2,3,4)
    /// </summary>
    public class DateOfMonthComboFormatter
    {
        private readonly StringBuilder formattedDays = new StringBuilder();

        private int firstConsecutiveDay;

        private int previousDay;

        /// <summary>
        /// Formats the days. Turns 3 consecutive days to A-C vs A,B,C
        /// </summary>
        /// <param name="selectedDays">The selected days.</param>
        /// <returns></returns>
        public string FormatDays(List<int> selectedDays)
        {
            if (selectedDays == null || selectedDays.Count == 0)
            {
                return string.Empty;
            }

            firstConsecutiveDay = selectedDays.First();
            previousDay = selectedDays.First();

            for (int index = 1; index < selectedDays.Count; index++)
            {
                var day = selectedDays[index];
                if (day - 1 == previousDay)
                {
                    //consecutive day
                    previousDay = day;
                }
                else
                {
                    FormatLastOrNonConsecutiveDay(day);
                }
            }

            FormatLastOrNonConsecutiveDay(selectedDays.Last());

            return formattedDays.ToString(1, formattedDays.Length - 1).Trim();
        }

        /// <summary>
        /// Formats the last or non consecutive day.
        /// </summary>
        /// <param name="day">The day.</param>
        private void FormatLastOrNonConsecutiveDay(int day)
        {
            // non consecutive day
            if (previousDay == firstConsecutiveDay)
            {
                //previous day wasn't consecutive
                formattedDays.AppendFormat(", {0}", previousDay);
            }
            else if (previousDay - 1 == firstConsecutiveDay)
            {
                //Only two consecutive days in a row
                formattedDays.AppendFormat(", {0}, {1}", firstConsecutiveDay, previousDay);
                firstConsecutiveDay = day;
            }
            else
            {
                //previous day was consecutive
                formattedDays.AppendFormat(", {0}-{1}", firstConsecutiveDay, previousDay);
                firstConsecutiveDay = day;
            }

            firstConsecutiveDay = day;
            previousDay = day;
        }
    }
}