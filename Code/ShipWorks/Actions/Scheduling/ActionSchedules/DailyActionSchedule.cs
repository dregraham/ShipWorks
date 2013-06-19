using System;
using System.Xml.Serialization;
using ShipWorks.Actions.Scheduling.ActionSchedules.Editors;

namespace ShipWorks.Actions.Scheduling.ActionSchedules
{
    /// <summary>
    /// An implementation of the abstract ActionSchedule class for scheduling an actions that
    /// runs once a day.
    /// </summary>
    [Serializable]
    public class DailyActionSchedule : ActionSchedule
    {
        private int frequencyInDays;

        /// <summary>
        /// Initializes a new instance of the <see cref="DailyActionSchedule"/> class.
        /// </summary>
        public DailyActionSchedule()
        {
            frequencyInDays = 1;
        }

        /// <summary>
        /// Gets the type of the schedule.
        /// </summary>
        /// <value>The type of the schedule.</value>
        public override ActionScheduleType ScheduleType
        {
            get { return ActionScheduleType.Daily; }
        }

        /// <summary>
        /// Gets or sets the frequency in number of days that an action should be executed.
        /// </summary>
        [XmlElement("FrequencyInDays")]
        public int FrequencyInDays
        {
            get { return frequencyInDays; }
            set
            {
                if (value <= 0)
                {
                    throw new ActionScheduleException("The frequency of the daily action schedule must be a positive integer value.");
                }

                frequencyInDays = value;
            }
        }

        /// <summary>
        /// Creates and returns an ActionScheduleEditor for the specific ActionSchedule
        /// </summary>
        /// <returns>A DailyActionScheduleEditor object.</returns>
        public override ActionScheduleEditor CreateEditor()
        {
            DailyActionScheduleEditor editor = new DailyActionScheduleEditor();
            editor.LoadActionSchedule(this);

            return editor;
        }
    }
}
