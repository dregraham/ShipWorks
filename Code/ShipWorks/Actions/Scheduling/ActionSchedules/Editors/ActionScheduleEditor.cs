using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.UI;

namespace ShipWorks.Actions.Scheduling.ActionSchedules.Editors
{
    /// <summary>
    /// Base class for ActionScheduleEditors.
    /// </summary>
    [TypeDescriptionProvider(typeof(UserControlTypeDescriptionProvider))]
    public abstract class ActionScheduleEditor : UserControl
    {
        /// <summary>
        /// Populate the editor control with the ActionSchedule properties.
        /// </summary>
        /// <param name="actionSchedule">The ActionSchedule from which to populate the editor.</param>
        public abstract void LoadActionSchedule(ActionSchedule actionSchedule);
    }
}
