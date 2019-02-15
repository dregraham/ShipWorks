using System;

namespace Interapptive.Shared.AutoUpdate
{
    /// <summary>
    /// struct to hold the data we send
    /// </summary>
    public struct UpdateWindowData
    {
        /// <summary>
        /// The day of the week the update should kick off
        /// </summary>
        public DayOfWeek AutoUpdateDayOfWeek { get; set; }

        /// <summary>
        /// The hour of the day the update should kick off
        /// </summary>
        public int AutoUpdateHourOfDay { get; set; }
    }
}
