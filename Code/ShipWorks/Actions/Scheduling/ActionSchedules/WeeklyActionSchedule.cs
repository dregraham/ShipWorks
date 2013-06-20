using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using ShipWorks.Actions.Scheduling.ActionSchedules.Editors;
using ShipWorks.Actions.Scheduling.ActionSchedules.Enums;

namespace ShipWorks.Actions.Scheduling.ActionSchedules
{
    /// <summary>
    /// Weekly Action Schedule
    /// </summary>
    [Serializable]
    public class WeeklyActionSchedule : ActionSchedule
    {
        private int frequencyInWeeks = 1;
        private List<DayOfWeek> executeOnDays = new List<DayOfWeek>(); 

        /// <summary>
        /// Constructor.
        /// </summary>
        public WeeklyActionSchedule()
        {
            ExecuteOnDays = new List<DayOfWeek>();
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
        [XmlElement("FrequencyInWeeks")]
        public int FrequencyInWeeks
        {
            get
            {
                return frequencyInWeeks;
            }
            set
            {
                // Fix any boundary conditions.
                if (value < 1)
                {
                    frequencyInWeeks = 1;
                }
                else if (value > 52)
                {
                    frequencyInWeeks = 52;
                }
                else
                {
                    frequencyInWeeks = value;
                }
            }
        }

        /// <summary>
        /// Days of the week for which this action should be executed
        /// </summary>
        [XmlElement("ExecuteOnDays")]
        public List<DayOfWeek> ExecuteOnDays
        {
            get
            {
                return executeOnDays;
            }
            set
            {
                if (value != null)
                {
                    executeOnDays = value;
                }
            }
        }

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
