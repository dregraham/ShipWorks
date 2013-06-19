using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using ShipWorks.Actions.Scheduling.ActionSchedules.Editors;

namespace ShipWorks.Actions.Scheduling.ActionSchedules
{
    /// <summary>
    /// One time Action Schedule
    /// </summary>
    public class OneTimeActionSchedule : ActionSchedule
    {
        /// <summary>
        /// ActionScheduleType - One time in this case
        /// </summary>
        [XmlElement("ScheduleType")]
        public override ActionScheduleType ScheduleType
        {
            get
            {
                return ActionScheduleType.OneTime;
            }
            set
            {
                // For serialization to work, we MUST have a setter, so don't delete this!
            }
        }

        /// <summary>
        /// There is no ActionScheduleEditor for OneTime, so don't call this!
        /// </summary>
        public override ActionScheduleEditor CreateEditor()
        {
            throw new NotImplementedException("OneTimeActionSchedules do not use an editor.");
        }
    }
}
