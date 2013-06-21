using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using ShipWorks.Actions.Scheduling.ActionSchedules.Editors;
using ShipWorks.Actions.Scheduling.ActionSchedules.Enums;

namespace ShipWorks.Actions.Scheduling.ActionSchedules
{
    /// <summary>
    /// Monthly ActionSchedule
    /// </summary>
    [Serializable]
    public class MonthlyActionSchedule : ActionSchedule
    {
        /// <summary>
        /// Gets the type of the schedule. DO NOT SET
        /// </summary>
        /// <value>
        /// The type of the schedule. 
        /// </value>
        public override ActionScheduleType ScheduleType
        {
            get
            {
                return ActionScheduleType.Monthly;
            }
            set
            {
                // needed for reflection. do not use.
            }
        }

        /// <summary>
        /// Creates and returns an ActionScheduleEditor for the specific ActionSchedule
        /// </summary>
        /// <returns></returns>
        public override ActionScheduleEditor CreateEditor()
        {
            var monthlyActionScheduleEditor = new MonthlyActionScheduleEditor();
            monthlyActionScheduleEditor.LoadActionSchedule(this);
            return monthlyActionScheduleEditor;
        }

        /// <summary>
        /// Gets or sets the type of the calendar.
        /// </summary>
        /// <value>
        /// The type of the calendar.
        /// </value>
        [XmlElement("CalendarType")]
        public MonthlyCalendarType CalendarType
        {
            get;
            set;
        }

        /// <summary>
        /// The numeric days to run if CalendarType is Date.
        /// </summary>
        /// <value>
        /// The execute on dates.
        /// </value>
        [XmlElement("ExecuteOnDates")]
        public List<int> ExecuteOnDates
        {
            get;
            set;
        }
        
        /// <summary>
        /// The months to run if CalendarType is Date.
        /// </summary>
        /// <value>
        /// The execute on date months.
        /// </value>
        [XmlElement("ExecuteOnDateMonths")]
        public List<MonthType> ExecuteOnDateMonths
        {
            get;
            set;
        }


        /// <summary>
        /// The day to run if CalendarType is Day.
        /// </summary>
        /// <value>
        /// The execute on day.
        /// </value>
        [XmlElement("ExecuteOnDay")]
        public DayOfWeek ExecuteOnDay
        {
            get;
            set;
        }

        /// <summary>
        /// The week in the month to run if CalendarType is Day.
        /// </summary>
        /// <value>
        /// The execute on week.
        /// </value>
        [XmlElement("ExecuteOnWeek")]
        public WeekOfMonthType ExecuteOnWeek
        {
            get;
            set;
        }

        /// <summary>
        /// The month to run if CalendarType is Day
        /// </summary>
        /// <value>
        /// The execute on day months.
        /// </value>
        [XmlElement("ExecuteOnDayMonths")]
        public List<MonthType> ExecuteOnDayMonths
        {
            get;
            set;
        }
    }
}
