using System;
using Interapptive.Shared.Utility;
using ShipWorks.Actions.Scheduling.ActionSchedules.Editors;

namespace ShipWorks.Actions.Scheduling.ActionSchedules
{
    /// <summary>
    /// Abstract ActionSchedule type
    /// </summary>
    public abstract class ActionSchedule : SerializableObject
    {
        /// <summary>
        /// Gets the type of the schedule.
        /// </summary>
        /// <value>The type of the schedule.</value>
        public abstract ActionScheduleType ScheduleType { get; }

        /// <summary>
        /// Gets or sets the start time (in UTC).
        /// </summary>
        /// <value>StartDateTimeInUtc
        /// The start time in UTC
        /// </value>
        public DateTime StartDateTimeInUtc
        {
            get;
            set;
        }

        /// <summary>
        /// Creates and returns an ActionScheduleEditor for the specific ActionSchedule
        /// </summary>
        public abstract ActionScheduleEditor CreateEditor();

    }
}
