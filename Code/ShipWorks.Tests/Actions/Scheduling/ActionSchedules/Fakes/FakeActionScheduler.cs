using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Actions.Scheduling.ActionSchedules;

namespace ShipWorks.Tests.Actions.Scheduling.ActionSchedules.Fakes
{
    // Nothing to do here - just intended to have a concrete class for testing the action schedule constructor
    public class FakeActionScheduler : ActionSchedule
    {
        public override ShipWorks.Actions.Scheduling.ActionSchedules.Enums.ActionScheduleType ScheduleType
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override ShipWorks.Actions.Scheduling.ActionSchedules.Editors.ActionScheduleEditor CreateEditor()
        {
            throw new NotImplementedException();
        }
    }
}
