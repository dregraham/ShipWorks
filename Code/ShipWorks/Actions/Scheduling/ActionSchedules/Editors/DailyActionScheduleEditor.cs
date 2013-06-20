using System;
using System.ComponentModel;
using Interapptive.Shared.Utility;
using System.Globalization;

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

            days.Text = dailyActionSchedule.FrequencyInDays.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Update the ActionSchedule from the control properties.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public void SaveActionSchedule()
        {
            if (!days.NumericValue.HasValue || days.NumericValue.Value <= 0)
            {
                throw new ActionScheduleException("A positive numeric value must be provided for the number of days.");
            }

            dailyActionSchedule.FrequencyInDays = days.NumericValue.Value;
        }
    }
}
