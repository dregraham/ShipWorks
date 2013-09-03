using System;
using Common.Logging;
using Interapptive.Shared.Utility;
using ShipWorks.Actions.Scheduling.ActionSchedules.Enums;

namespace ShipWorks.Actions.Scheduling.ActionSchedules.Editors
{
    /// <summary>
    /// Monthly ActionScheduleEditor
    /// </summary>
    public partial class MonthlyActionScheduleEditor : ActionScheduleEditor
    {
        static readonly ILog log = LogManager.GetLogger(typeof(MonthlyActionScheduleEditor));

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

            BindDayDayOfWeek();
        }

        /// <summary>
        /// Binds the dayDayOfWeek.
        /// </summary>
        private void BindDayDayOfWeek()
        {
            dayDayOfWeek.SelectedIndexChanged -= OnDayDayOfWeekSelectedIndexChanged;

            dayDayOfWeek.Items.Clear();

            foreach (object day in Enum.GetValues(typeof(DayOfWeek)))
            {
                dayDayOfWeek.Items.Add(day);
            }
            if ((WeekOfMonthType)dayWeek.SelectedValue == WeekOfMonthType.First || (WeekOfMonthType)dayWeek.SelectedValue == WeekOfMonthType.Last)
            {
                dayDayOfWeek.Items.Add("Day");
            }

            dayDayOfWeek.SelectedIndexChanged += OnDayDayOfWeekSelectedIndexChanged;
        }

        /// <summary>
        /// Populate the editor control with the ActionSchedule properties.
        /// </summary>
        /// <param name="actionSchedule">The ActionSchedule from which to populate the editor.</param>
        public override void LoadActionSchedule(ActionSchedule actionSchedule)
        {
            monthlyActionSchedule = (MonthlyActionSchedule)actionSchedule;

            dateSelected.CheckedChanged -= OnDateSelectedCheckedChanged;
            dateSelected.Checked = (monthlyActionSchedule.CalendarType == MonthlyCalendarType.Date);
            dateSelected.CheckedChanged += OnDateSelectedCheckedChanged;

            dateDayOfMonth.SelectDays(monthlyActionSchedule.ExecuteOnDates);
            dateMonth.SelectMonths(monthlyActionSchedule.ExecuteOnDateMonths);

            daySelected.Checked = (monthlyActionSchedule.CalendarType == MonthlyCalendarType.Day);

            dayWeek.SelectedIndexChanged -= OnDayWeekSelectedIndexChanged;
            dayWeek.SelectedValue = monthlyActionSchedule.ExecuteOnWeek;
            dayWeek.SelectedIndexChanged += OnDayWeekSelectedIndexChanged;
            BindDayDayOfWeek();

            dayDayOfWeek.SelectedIndexChanged -= OnDayDayOfWeekSelectedIndexChanged;
            SetDayDayOfWeek();
            dayDayOfWeek.SelectedIndexChanged += OnDayDayOfWeekSelectedIndexChanged;

            dayMonth.SelectMonths(monthlyActionSchedule.ExecuteOnDayMonths);

            dateMonth.MonthChanged = SaveValues;
            dayMonth.MonthChanged = SaveValues;
            dateDayOfMonth.DateChanged = SaveValues;

            SetCalendarTypeControlEnabledState(monthlyActionSchedule.CalendarType);
        }

        /// <summary>
        /// Sets the day day of week. This is called at the initial load and also when dayDayOfWeek rebinds due to dayWeek changing.
        /// </summary>
        private void SetDayDayOfWeek()
        {
            if (monthlyActionSchedule.ExecuteOnAnyDay)
            {
                if (!dayDayOfWeek.Items.Contains("Day"))
                {
                    log.Error("Day not contained in dayDayOfWeek. Likely because dayWeek does not have First or Lase selected.");
                }
                else
                {
                    dayDayOfWeek.SelectedItem = "Day";
                }
            }
            else
            {
                dayDayOfWeek.SelectedItem = monthlyActionSchedule.ExecuteOnDay;
            }
        }

        /// <summary>
        /// Saves the values to monthlyActionSchedule
        /// </summary>
        private void SaveValues()
        {
            monthlyActionSchedule.CalendarType = dateSelected.Checked ? MonthlyCalendarType.Date : MonthlyCalendarType.Day;

            monthlyActionSchedule.ExecuteOnDates = dateDayOfMonth.GetSelectedDays();
            monthlyActionSchedule.ExecuteOnDateMonths = dateMonth.GetSelectedMonths();

            monthlyActionSchedule.ExecuteOnWeek = ((EnumEntry<WeekOfMonthType>)dayWeek.SelectedItem).Value;

            if (dayDayOfWeek.SelectedItem == null)
            {
                monthlyActionSchedule.ExecuteOnAnyDay = false;
                monthlyActionSchedule.ExecuteOnDay = null;
            }
            else if (dayDayOfWeek.SelectedItem is Enum)
            {
                monthlyActionSchedule.ExecuteOnDay = (DayOfWeek)dayDayOfWeek.SelectedItem;
                monthlyActionSchedule.ExecuteOnAnyDay = false;
            }
            else
            {
                monthlyActionSchedule.ExecuteOnAnyDay = true;
                monthlyActionSchedule.ExecuteOnDay = null;
            }

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

            SetCalendarTypeControlEnabledState(MonthlyCalendarType.Date);
        }

        /// <summary>
        /// Called when [day week selected index changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnDayWeekSelectedIndexChanged(object sender, EventArgs e)
        {
            BindDayDayOfWeek();
            SetDayDayOfWeek();
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

        /// <summary>
        /// Called when day selected checked changed.
        /// </summary>
        private void OnDaySelectedCheckedChanged(object sender, EventArgs e)
        {
            SetCalendarTypeControlEnabledState(MonthlyCalendarType.Day);
        }

        /// <summary>
        /// Enable/disable the calender type controls based on current selection.
        /// </summary>
        /// <param name="monthlyCalendarType">The current calendar type.</param>
        private void SetCalendarTypeControlEnabledState(MonthlyCalendarType monthlyCalendarType)
        {
            dateDayOfMonth.Enabled = monthlyCalendarType == MonthlyCalendarType.Date;
            dateMonth.Enabled = monthlyCalendarType == MonthlyCalendarType.Date;

            dayWeek.Enabled = monthlyCalendarType == MonthlyCalendarType.Day;
            dayDayOfWeek.Enabled = monthlyCalendarType == MonthlyCalendarType.Day;
            dayMonth.Enabled = monthlyCalendarType == MonthlyCalendarType.Day;
        }
    }
}