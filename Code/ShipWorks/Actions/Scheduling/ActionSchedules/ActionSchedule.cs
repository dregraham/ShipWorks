using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using ShipWorks.Actions.Scheduling.ActionSchedules.Editors;
using ShipWorks.Actions.Scheduling.ActionSchedules.Enums;

namespace ShipWorks.Actions.Scheduling.ActionSchedules
{
    /// <summary>
    /// Abstract ActionSchedule type
    /// </summary>
    [Serializable]
    public abstract class ActionSchedule
    {
        private DateTime? endDateTimeInUtc;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionSchedule"/> class.
        /// </summary>
        protected ActionSchedule()
        {
            DateTime utcNow = DateTime.UtcNow;

            // Set the start date to the top of the next hour by default
            StartDateTimeInUtc = utcNow.AddMinutes(60 - utcNow.Minute);
            StartDateTimeInUtc = StartDateTimeInUtc.AddSeconds(-StartDateTimeInUtc.Second);

            // Default the end time to the start time.
            EndDateTimeInUtc = StartDateTimeInUtc;
        }

        /// <summary>
        /// Gets the type of the schedule.
        /// </summary>
        /// <value>The type of the schedule.</value>
        [XmlElement("ScheduleType")]
        public abstract ActionScheduleType ScheduleType
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the start time (in UTC).
        /// </summary>
        /// <value>StartDateTimeInUtc
        /// The start time in UTC
        /// </value>
        [XmlElement("StartDateTimeInUtc")]
        public DateTime StartDateTimeInUtc
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the end on type of the schedule.
        /// </summary>
        /// <value>The end on type of the schedule.</value>
        [XmlElement("ActionEndsOnType")]
        public ActionEndsOnType EndsOnType
        {
            get; set;
        }
        
        /// <summary>
        /// Gets or sets the end time (in UTC).
        /// </summary>
        /// <value>EndDateTimeInUtc
        /// The end time in UTC
        /// </value>
        [XmlElement("EndDateTimeInUtc")]
        public DateTime? EndDateTimeInUtc
        {
            get
            {
                return endDateTimeInUtc;
            }
            set
            {
                endDateTimeInUtc = value;
            }

        }

        /// <summary>
        /// Creates and returns an ActionScheduleEditor for the specific ActionSchedule
        /// </summary>
        public abstract ActionScheduleEditor CreateEditor();

        /// <summary>
        /// Validates the schedule.  An exception is thrown to indicate validation failure.
        /// </summary>
        public virtual void Validate() { }
    }
}