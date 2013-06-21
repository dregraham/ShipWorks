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

        List<Tuple<CheckBox,MonthType>> onTheMonthsList=new List<Tuple<CheckBox,MonthType>>(); 

        List<Tuple<CheckBox,MonthType>> daysMonthsList = new List<Tuple<CheckBox,MonthType>>(); 

        /// <summary>
        /// Initializes a new instance of the <see cref="MonthlyActionScheduleEditor"/> class.
        /// </summary>
        public MonthlyActionScheduleEditor()
        {
            InitializeComponent();

            daysDaySelector.PopupController = new PopupController(daysPanel);

            onTheMonthSelctor.PopupController = new PopupController(onTheMonthsPanel);
        }

        /// <summary>
        /// Populate the editor control with the ActionSchedule properties.
        /// </summary>
        /// <param name="actionSchedule">The ActionSchedule from which to populate the editor.</param>
        public override void LoadActionSchedule(ActionSchedule actionSchedule)
        {
            EnumHelper.BindComboBox<WeekOfMonthType>(onTheWeekOfMonth);

            onTheDaySelector.DataSource = Enum.GetValues(typeof(DayOfWeek));

            BindMonths(onTheMonthsList, onTheMonthsPanel);
            
        }

        /// <summary>
        /// Binds the months.
        /// </summary>
        /// <param name="monthsList">The months list.</param>
        /// <param name="monthsPanel">The months panel.</param>
        private void BindMonths(List<Tuple<CheckBox,MonthType>> monthsList, Panel monthsPanel)
        {
            // Loop through all the months in MonthTypeEnum.
            for (int monthIndex = 0; monthIndex < EnumHelper.GetEnumList<MonthType>().Count; monthIndex++)
            {
                var month = EnumHelper.GetEnumList<MonthType>()[monthIndex];

                int verticlePosition = 23 * monthIndex + 23;

                // Build monthsList with new checkboxes and related enums.
                var checkboxAndMonthType = new Tuple<CheckBox, MonthType>(new CheckBox()
                {
                    Text = month.Key, Location = new Point(3, verticlePosition)
                }, month.Value);

                checkboxAndMonthType.Item1.CheckedChanged += CheckChangedMonth;

                monthsList.Add(checkboxAndMonthType);

                // Add checkbox to panel
                monthsPanel.Controls.Add(checkboxAndMonthType.Item1);
            }
        }

        /// <summary>
        /// Check changed for month checkbox
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void CheckChangedMonth(object sender, EventArgs e)
        {
            Tuple<CheckBox, MonthType> selectedMonth = onTheMonthsList.SingleOrDefault(m => m.Item1 == sender);
            List<Tuple<CheckBox, MonthType>> monthsList = onTheMonthsList;
            
            if (selectedMonth==null)
            {
                selectedMonth = daysMonthsList.SingleOrDefault(m => m.Item1 == sender);
                monthsList = daysMonthsList;
            }

            if (VScroll)
            {
                
            }

            
        }

        /// <summary>
        /// Checks changed for select all months.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void CheckChangedSelectAllMonths(object sender, EventArgs e)
        {
            List<Tuple<CheckBox, MonthType>> monthsList = daysMonthsList;

            if (sender == onTheSelectAllMonths)
            {
                monthsList = onTheMonthsList;
            }

            bool isChecked = ((CheckBox) sender).Checked;

            foreach (var months in monthsList)
            {
                months.Item1.Checked = isChecked;
            }
        }
    }
}
