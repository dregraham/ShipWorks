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
    /// Hourly Action Schedule
    /// </summary>
    [Serializable]
    public class HourlyActionSchedule : ActionSchedule
    {
        private int frequencyInHours = 1;

        /// <summary>
        /// ActionScheduleType - Hourly in this case
        /// </summary>
        [XmlElement("ScheduleType")]
        public override ActionScheduleType ScheduleType
        {
            get
            {
                return ActionScheduleType.Hourly;
            }
            set
            {
                // For serialization to work, we MUST have a setter, so don't delete this!
            }
        }

        /// <summary>
        /// Number of hours for which this action should be executed
        /// </summary>
        [XmlElement("FrequencyInHours")]
        public int FrequencyInHours
        {
            get
            {
                return frequencyInHours;
            }
            set
            {
                // Fix any boundary conditions.
                if (value < 1)
                {
                    frequencyInHours = 1;
                }
                else if (value > 23)
                {
                    frequencyInHours = 23;
                }
                else
                {
                    frequencyInHours = value;
                }
            }
        }

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
