using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Actions.Scheduling.ActionSchedules.Editors;

namespace ShipWorks.Actions.Scheduling.ActionSchedules
{
    /// <summary>
    /// Monthly ActionSchedule
    /// </summary>
    class MonthlyActionSchedule : ActionSchedule
    {
        /// <summary>
        /// Gets the type of the schedule. DO NOT SET
        /// </summary>
        /// <value>
        /// The type of the schedule. 
        /// </value>
        public override ActionScheduleType ScheduleType
        {
            get
            {
                return ActionScheduleType.Monthly;
            }
            set
            {
                // needed for reflection. do not use.
            }
        }

        /// <summary>
        /// Creates and returns an ActionScheduleEditor for the specific ActionSchedule
        /// </summary>
        /// <returns></returns>
        public override ActionScheduleEditor CreateEditor()
        {
            var monthlyActionScheduleEditor = new MonthlyActionScheduleEditor();
            monthlyActionScheduleEditor.LoadActionSchedule(this);
            return monthlyActionScheduleEditor;
        }
    }
}
