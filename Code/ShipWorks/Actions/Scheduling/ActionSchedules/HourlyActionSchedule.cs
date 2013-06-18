using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Actions.Scheduling.ActionSchedules.Editors;

namespace ShipWorks.Actions.Scheduling.ActionSchedules
{
    /// <summary>
    /// Hourly Action Schedule
    /// </summary>
    public class HourlyActionSchedule : ActionSchedule
    {
        /// <summary>
        /// ActionScheduleType - Hourly in this case
        /// </summary>
        public override ActionScheduleType ScheduleType
        {
            get
            {
                return ActionScheduleType.Hourly;
            }
        }

        /// <summary>
        /// Number of hours for which this action should be executed
        /// </summary>
        public int RecurrenceHours { get; set; }

        /// <summary>
        /// Create and return the HourlyActionScheduleEditor
        /// </summary>
        public override ActionScheduleEditor CreateEditor()
        {
            HourlyActionScheduleEditor hourlyActionScheduleEditor = new HourlyActionScheduleEditor();
            hourlyActionScheduleEditor.LoadActionSchedule(this);
            return hourlyActionScheduleEditor;
        }
    }
}
