using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using ShipWorks.Actions.Scheduling.ActionSchedules.Editors;

namespace ShipWorks.Actions.Scheduling.ActionSchedules
{
    /// <summary>
    /// Weekly Action Schedule
    /// </summary>
    [Serializable]
    public class WeeklyActionSchedule : ActionSchedule
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public WeeklyActionSchedule()
        {
            ExecuteOnDays = new List<DayOfWeek>();
            RecurrenceWeeks = 1;
        }

        /// <summary>
        /// ActionScheduleType - Weekly in this case
        /// </summary>
        [XmlElement("ScheduleType")]
        public override ActionScheduleType ScheduleType
        {
            get
            {
                return ActionScheduleType.Weekly;
            }
            set
            {
                // For serialization to work, we MUST have a setter, so don't delete this!
            }
        }

        /// <summary>
        /// Number of weeks for which this action should be executed
        /// </summary>
        [XmlElement("RecurrenceWeeks")]
        public int RecurrenceWeeks { get; set; }

        /// <summary>
        /// Days of the week for which this action should be executed
        /// </summary>
        [XmlElement("ExecuteOnDays")]
        public List<DayOfWeek> ExecuteOnDays { get; set; }

        /// <summary>
        /// Create and return the WeeklyActionScheduleEditor
        /// </summary>
        public override ActionScheduleEditor CreateEditor()
        {
            WeeklyActionScheduleEditor weeklyActionScheduleEditor = new WeeklyActionScheduleEditor();
            weeklyActionScheduleEditor.LoadActionSchedule(this);
            return weeklyActionScheduleEditor;
        }
    }
}
