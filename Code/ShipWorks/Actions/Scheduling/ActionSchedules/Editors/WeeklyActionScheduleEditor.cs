using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Utility;

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

            // Fix any boundary conditions.
            if (weeklyActionSchedule.RecurrenceWeeks < 1)
            {
                weeklyActionSchedule.RecurrenceWeeks = 1;
            }
            else if (weeklyActionSchedule.RecurrenceWeeks > 52)
            {
                weeklyActionSchedule.RecurrenceWeeks = 52;
            }

            // Set the selected week
            recurrsEveryNumberOfWeeks.SelectedItem = weeklyActionSchedule.RecurrenceWeeks;

            // Set the days
            sunday.Checked = weeklyActionSchedule.ExecuteOnDays.Any(d => d == DayOfWeek.Sunday);
            monday.Checked = weeklyActionSchedule.ExecuteOnDays.Any(d => d == DayOfWeek.Monday);
            tuesday.Checked = weeklyActionSchedule.ExecuteOnDays.Any(d => d == DayOfWeek.Tuesday);
            wednesday.Checked = weeklyActionSchedule.ExecuteOnDays.Any(d => d == DayOfWeek.Wednesday);
            thursday.Checked = weeklyActionSchedule.ExecuteOnDays.Any(d => d == DayOfWeek.Thursday);
            friday.Checked = weeklyActionSchedule.ExecuteOnDays.Any(d => d == DayOfWeek.Friday);
            saturday.Checked = weeklyActionSchedule.ExecuteOnDays.Any(d => d == DayOfWeek.Saturday);
        }

        /// <summary>
        /// Saves the control properties to the ActionSchedule
        /// </summary>
        public override void SaveActionSchedule()
        {
            // Update the recurrance weeks
            weeklyActionSchedule.RecurrenceWeeks = (int)recurrsEveryNumberOfWeeks.SelectedItem;

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
    }
}
