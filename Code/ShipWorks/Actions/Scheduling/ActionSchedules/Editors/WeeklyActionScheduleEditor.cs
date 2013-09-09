using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.UI;

namespace ShipWorks.Actions.Scheduling.ActionSchedules.Editors
{
    /// <summary>
    /// Editor for Weekly action schedules
    /// </summary>
    [ToolboxItem(false)]
    [TypeDescriptionProvider(typeof(UserControlTypeDescriptionProvider))]
    public partial class WeeklyActionScheduleEditor : ActionScheduleEditor
    {
        WeeklyActionSchedule weeklyActionSchedule;

        public WeeklyActionScheduleEditor()
        {
            InitializeComponent();

            // Add weeks to the combobox.  Only allowing 1-52.
            for (int i = 1; i <= 52; i++)
            {
                recurrsEveryNumberOfWeeks.Items.Add(i);
            }
        }

        /// <summary>
        /// Loads the control with the request ActionSchedule
        /// </summary>
        public override void LoadActionSchedule(ActionSchedule actionSchedule)
        {
            weeklyActionSchedule = (WeeklyActionSchedule)actionSchedule;


            // Set the selected week
            recurrsEveryNumberOfWeeks.SelectedIndexChanged -= OnRecurrenceWeeksChanged;
            recurrsEveryNumberOfWeeks.SelectedItem = weeklyActionSchedule.FrequencyInWeeks;
            recurrsEveryNumberOfWeeks.SelectedIndexChanged += OnRecurrenceWeeksChanged;

            // Set the days
            SetDaysCheckedEvent(false);
            sunday.Checked = weeklyActionSchedule.ExecuteOnDays.Any(d => d == DayOfWeek.Sunday);
            monday.Checked = weeklyActionSchedule.ExecuteOnDays.Any(d => d == DayOfWeek.Monday);
            tuesday.Checked = weeklyActionSchedule.ExecuteOnDays.Any(d => d == DayOfWeek.Tuesday);
            wednesday.Checked = weeklyActionSchedule.ExecuteOnDays.Any(d => d == DayOfWeek.Wednesday);
            thursday.Checked = weeklyActionSchedule.ExecuteOnDays.Any(d => d == DayOfWeek.Thursday);
            friday.Checked = weeklyActionSchedule.ExecuteOnDays.Any(d => d == DayOfWeek.Friday);
            saturday.Checked = weeklyActionSchedule.ExecuteOnDays.Any(d => d == DayOfWeek.Saturday);
            SetDaysCheckedEvent(true);
        }

        /// <summary>
        /// Add/Remove the day of week checkboxes CheckedChanged event so that we can populate correctly.
        /// </summary>
        /// <param name="enable"></param>
        private void SetDaysCheckedEvent(bool enable)
        {
            if (enable)
            {
                sunday.CheckedChanged += OnDayCheckedChanged;
                monday.CheckedChanged += OnDayCheckedChanged;
                tuesday.CheckedChanged += OnDayCheckedChanged;
                wednesday.CheckedChanged += OnDayCheckedChanged;
                thursday.CheckedChanged += OnDayCheckedChanged;
                friday.CheckedChanged += OnDayCheckedChanged;
                saturday.CheckedChanged += OnDayCheckedChanged;
            }
            else
            {
                sunday.CheckedChanged -= OnDayCheckedChanged;
                monday.CheckedChanged -= OnDayCheckedChanged;
                tuesday.CheckedChanged -= OnDayCheckedChanged;
                wednesday.CheckedChanged -= OnDayCheckedChanged;
                thursday.CheckedChanged -= OnDayCheckedChanged;
                friday.CheckedChanged -= OnDayCheckedChanged;
                saturday.CheckedChanged -= OnDayCheckedChanged; 
            }
        }

        /// <summary>
        /// Saves the control days selected to the ActionSchedule
        /// </summary>
        public void SaveExecuteOnDays()
        {
            // Clear out the previous selected days.
            weeklyActionSchedule.ExecuteOnDays.Clear();

            // And add the newly selected days.
            if (sunday.Checked)
            {
                weeklyActionSchedule.ExecuteOnDays.Add(DayOfWeek.Sunday);
            }

            if (monday.Checked)
            {
                weeklyActionSchedule.ExecuteOnDays.Add(DayOfWeek.Monday);
            }

            if (tuesday.Checked)
            {
                weeklyActionSchedule.ExecuteOnDays.Add(DayOfWeek.Tuesday);
            }

            if (wednesday.Checked)
            {
                weeklyActionSchedule.ExecuteOnDays.Add(DayOfWeek.Wednesday);
            }

            if (thursday.Checked)
            {
                weeklyActionSchedule.ExecuteOnDays.Add(DayOfWeek.Thursday);
            }

            if (friday.Checked)
            {
                weeklyActionSchedule.ExecuteOnDays.Add(DayOfWeek.Friday);
            }

            if (saturday.Checked)
            {
                weeklyActionSchedule.ExecuteOnDays.Add(DayOfWeek.Saturday);
            }
        }

        /// <summary>
        /// Update the schedule when the recurrence weeks changes.
        /// </summary>
        private void OnRecurrenceWeeksChanged(object sender, EventArgs e)
        {
            // Update the recurrance weeks
            weeklyActionSchedule.FrequencyInWeeks = (int)recurrsEveryNumberOfWeeks.SelectedItem;
        }

        /// <summary>
        /// Update the schedule when the days change.
        /// </summary>
        private void OnDayCheckedChanged(object sender, EventArgs e)
        {
            SaveExecuteOnDays();
        }
    }
}
