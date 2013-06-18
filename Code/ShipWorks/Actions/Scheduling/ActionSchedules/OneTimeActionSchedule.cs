using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public override ActionScheduleType ScheduleType
        {
            get
            {
                return ActionScheduleType.OneTime;
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
