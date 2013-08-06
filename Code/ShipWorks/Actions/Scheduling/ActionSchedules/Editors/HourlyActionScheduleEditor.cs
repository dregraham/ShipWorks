using System.ComponentModel;
using System.Windows.Forms;
using Interapptive.Shared.Utility;

namespace ShipWorks.Actions.Scheduling.ActionSchedules.Editors
{
    /// <summary>
    /// Editor for Hourly action schedules
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

            // Add hours to the combobox.  Only allowing 1-23.  If they need longer than 24 hours, they should use a Daily.
            for (int i = 1; i <= 23; i++)
            {
                recurrsEveryNumberOfHours.Items.Add(i);
            }
        }

        /// <summary>
        /// Loads the control with the request ActionSchedule
        /// </summary>
        public override void LoadActionSchedule(ActionSchedule actionSchedule)
        {
            hourlyActionSchedule = (HourlyActionSchedule)actionSchedule;

            // Set the selected hour
            recurrsEveryNumberOfHours.SelectedIndexChanged -= OnRecurrenceHoursChanged;
            recurrsEveryNumberOfHours.SelectedItem = hourlyActionSchedule.FrequencyInHours;
            recurrsEveryNumberOfHours.SelectedIndexChanged += OnRecurrenceHoursChanged;
        }

        /// <summary>
        /// Update the schedule to the newly selected recurrence hours
        /// </summary>
        private void OnRecurrenceHoursChanged(object sender, System.EventArgs e)
        {
            hourlyActionSchedule.FrequencyInHours = (int)recurrsEveryNumberOfHours.SelectedItem;
        }
    }
}
