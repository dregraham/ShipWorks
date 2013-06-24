using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Actions.Scheduling.ActionSchedules.Enums;
using ShipWorks.UI;
using Interapptive.Shared.Utility;

namespace ShipWorks.Actions.Scheduling.ActionSchedules.Editors
{
    /// <summary>
    /// Monthly ActionScheduleEditor
    /// </summary>
    public partial class MonthlyActionScheduleEditor : ActionScheduleEditor
    {
        private MonthlyActionSchedule monthlyActionSchedule;

        /// <summary>
        /// Initializes a new instance of the <see cref="MonthlyActionScheduleEditor"/> class.
        /// </summary>
        public MonthlyActionScheduleEditor()
        {
            InitializeComponent();

            dayWeek.SelectedIndexChanged -= OnDayWeekSelectedIndexChanged;
            EnumHelper.BindComboBox<WeekOfMonthType>(dayWeek);
            dayWeek.SelectedIndexChanged += OnDayWeekSelectedIndexChanged;

            dayDayOfWeek.SelectedIndexChanged -= OnDayDayOfWeekSelectedIndexChanged;
            dayDayOfWeek.DataSource = Enum.GetValues(typeof(DayOfWeek));
            dayDayOfWeek.SelectedIndexChanged += OnDayDayOfWeekSelectedIndexChanged;

        }

        /// <summary>
        /// Populate the editor control with the ActionSchedule properties.
        /// </summary>
        /// <param name="actionSchedule">The ActionSchedule from which to populate the editor.</param>
        public override void LoadActionSchedule(ActionSchedule actionSchedule)
        {
            monthlyActionSchedule = (MonthlyActionSchedule) actionSchedule;

            dateSelected.CheckedChanged -= OnDateSelectedCheckedChanged;
            dateSelected.Checked = (monthlyActionSchedule.CalendarType == MonthlyCalendarType.Date);
            dateSelected.CheckedChanged += OnDateSelectedCheckedChanged;


            dateDayOfMonth.SelectDays(monthlyActionSchedule.ExecuteOnDates);
            dateMonth.SelectMonths(monthlyActionSchedule.ExecuteOnDateMonths);

            daySelected.Checked = (monthlyActionSchedule.CalendarType == MonthlyCalendarType.Day);

            dayWeek.SelectedIndexChanged -= OnDayWeekSelectedIndexChanged;
            dayWeek.SelectedValue = monthlyActionSchedule.ExecuteOnWeek;
            dayWeek.SelectedIndexChanged += OnDayWeekSelectedIndexChanged;

            dayDayOfWeek.SelectedIndexChanged -= OnDayDayOfWeekSelectedIndexChanged;
            dayDayOfWeek.SelectedItem = monthlyActionSchedule.ExecuteOnDay;
            dayDayOfWeek.SelectedIndexChanged += OnDayDayOfWeekSelectedIndexChanged;
            
            dayMonth.SelectMonths(monthlyActionSchedule.ExecuteOnDayMonths);

            dateMonth.MonthChanged = SaveValues;
            dayMonth.MonthChanged = SaveValues;
            dateDayOfMonth.DateChanged = SaveValues;
        }

        /// <summary>
        /// Saves the values to monthlyActionSchedule
        /// </summary>
        private void SaveValues()
        {
            monthlyActionSchedule.CalendarType = dateSelected.Checked ? MonthlyCalendarType.Date : MonthlyCalendarType.Day;

            monthlyActionSchedule.ExecuteOnDates = dateDayOfMonth.GetSelectedDays();
            monthlyActionSchedule.ExecuteOnDateMonths = dateMonth.GetSelectedMonths();

            monthlyActionSchedule.ExecuteOnWeek = ((EnumEntry<WeekOfMonthType>) dayWeek.SelectedItem).Value;
            monthlyActionSchedule.ExecuteOnDay = (DayOfWeek) dayDayOfWeek.SelectedItem;
            monthlyActionSchedule.ExecuteOnDayMonths = dayMonth.GetSelectedMonths();
        }

        /// <summary>
        /// Called when [date selected checked changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnDateSelectedCheckedChanged(object sender, EventArgs e)
        {
            SaveValues();
        }

        /// <summary>
        /// Called when [day week selected index changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnDayWeekSelectedIndexChanged(object sender, EventArgs e)
        {
            SaveValues();
        }

        /// <summary>
        /// Called when [day day of week selected index changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnDayDayOfWeekSelectedIndexChanged(object sender, EventArgs e)
        {
            SaveValues();
        }
    }
}
