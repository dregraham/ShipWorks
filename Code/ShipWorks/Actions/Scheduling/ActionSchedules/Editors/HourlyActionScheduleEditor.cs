using System.ComponentModel;
using System.Windows.Forms;
using Interapptive.Shared.Utility;

namespace ShipWorks.Actions.Scheduling.ActionSchedules.Editors
{
    /// <summary>
    /// Editor for OneTime action schedules
    /// </summary>
    [ToolboxItem(false)]
    [TypeDescriptionProvider(typeof(UserControlTypeDescriptionProvider))]
    public partial class HourlyActionScheduleEditor : ActionScheduleEditor
    {
        HourlyActionSchedule hourlyActionSchedule;

        /// <summary>
        /// Constructor
        /// </summary>
        public HourlyActionScheduleEditor()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Loads the control with the request ActionSchedule
        /// </summary>
        public override void LoadActionSchedule(ActionSchedule actionSchedule)
        {
            hourlyActionSchedule = (HourlyActionSchedule)actionSchedule;
            recurrsEveryNumberOfHours.Text = hourlyActionSchedule.RecurrenceHours.ToString();
        }

        /// <summary>
        /// Saves the control properties to the ActionSchedule
        /// </summary>
        public override void SaveActionSchedule()
        {
            hourlyActionSchedule.RecurrenceHours = int.Parse(recurrsEveryNumberOfHours.Text);
        }
    }
}
