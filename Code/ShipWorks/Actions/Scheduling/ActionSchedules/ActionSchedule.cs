using System;

namespace ShipWorks.Actions.Scheduling.ActionSchedules
{
    /// <summary>
    /// Abstract ActionSchedule type
    /// </summary>
    public abstract class ActionSchedule
    {
        /// <summary>
        /// Gets the type of the schedule.
        /// </summary>
        /// <value>
        /// The type of the schedule.
        /// </value>
        public abstract ActionScheduleType ScheduleType
        {
            get;
        }

        /// <summary>
        /// Gets or sets the start time.
        /// </summary>
        /// <value>
        /// The start time.
        /// </value>
        public DateTime StartTime
        {
            get;
            set;
        }
    }
}
