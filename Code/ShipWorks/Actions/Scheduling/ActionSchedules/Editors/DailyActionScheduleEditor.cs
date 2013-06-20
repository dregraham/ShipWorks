using System;
using System.ComponentModel;
using Interapptive.Shared.Utility;
using System.Globalization;
using Interapptive.Shared.UI;

namespace ShipWorks.Actions.Scheduling.ActionSchedules.Editors
{
    [ToolboxItem(false)]
    [TypeDescriptionProvider(typeof (UserControlTypeDescriptionProvider))]
    public partial class DailyActionScheduleEditor : ActionScheduleEditor
    {
        private DailyActionSchedule dailyActionSchedule;

        public DailyActionScheduleEditor()
        {
            InitializeComponent();

            // Add the number of days to the combo box; if they need something greater 30, a monthly
            // schedule should probably be used instead
            for (int i = 1; i <= 30; i++)
            {
                days.Items.Add(i);
            }
        }

        /// <summary>
        /// Populate the editor control with the ActionSchedule properties.
        /// </summary>
        /// <param name="actionSchedule">The ActionSchedule from which to populate the editor.</param>
        public override void LoadActionSchedule(ActionSchedule actionSchedule)
        {
            dailyActionSchedule = actionSchedule as DailyActionSchedule;

            if (dailyActionSchedule == null)
            {
                throw new InvalidOperationException("In invalid action schedule was provided to the daily action schedule editor; a daily action schedule was expected.");
            }

            // Fix any boundary conditions
            if (dailyActionSchedule.FrequencyInDays < 1)
            {
                dailyActionSchedule.FrequencyInDays = 1;
            }
            else if (dailyActionSchedule.FrequencyInDays > 30)
            {
                dailyActionSchedule.FrequencyInDays = 30;
            }

            // Unhook the event handler to set the selected frequency value
            days.SelectedIndexChanged -= OnDaysChanged;
            days.SelectedItem = dailyActionSchedule.FrequencyInDays;
            days.SelectedIndexChanged += OnDaysChanged;
        }

        /// <summary>
        /// Called when [days change].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnDaysChanged(object sender, EventArgs e)
        {
            dailyActionSchedule.FrequencyInDays = (int)days.SelectedItem;
        }
    }
}
