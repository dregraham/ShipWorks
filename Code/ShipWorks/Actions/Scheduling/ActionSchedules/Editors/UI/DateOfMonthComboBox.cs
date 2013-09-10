using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ShipWorks.UI;
using ShipWorks.UI.Controls;

namespace ShipWorks.Actions.Scheduling.ActionSchedules.Editors.UI
{
    /// <summary>
    /// A combobox with 31 selectable days
    /// </summary>
    public class DateOfMonthComboBox : PopupComboBox
    {
        private readonly DateOfMonthComboFormatter dayComboFormatter;

        private readonly List<Tuple<CheckBox, int>> dayList;

        private Panel dayPanel;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateOfMonthComboBox"/> class.
        /// </summary>
        public DateOfMonthComboBox()
        {
            dayList = new List<Tuple<CheckBox, int>>();

            DropDownMinimumHeight = 116;
            DropDownHeight = 116;
            DropDownWidth = 294;

            InitializeComponent();

            PopupController = new PopupController(dayPanel);

            BindDays();
            dayComboFormatter = new DateOfMonthComboFormatter();
        }

        /// <summary>
        /// Gets or sets the date changed.
        /// </summary>
        /// <value>
        /// The date changed.
        /// </value>
        public Action DateChanged
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the day combo formatter.
        /// </summary>
        /// <value>
        /// The day combo formatter.
        /// </value>
        public DateOfMonthComboFormatter DayComboFormatter
        {
            get
            {
                return dayComboFormatter;
            }
        }

        /// <summary>
        /// Selects the days.
        /// </summary>
        /// <param name="daysToSelect">The days to select.</param>
        public void SelectDays(List<int> daysToSelect)
        {
            foreach (var dayHolder in dayList)
            {
                dayHolder.Item1.Checked = daysToSelect != null && daysToSelect.Any(d => d == dayHolder.Item2);
            }
        }

        /// <summary>
        /// Gets the selected days.
        /// </summary>
        /// <returns></returns>
        public List<int> GetSelectedDays()
        {
            return (dayList
                .Where(dayHolder => dayHolder.Item1.Checked)
                .Select(dayHolder => dayHolder.Item2)).ToList();
        }

        /// <summary>
        /// Initialize Component
        /// </summary>
        private void InitializeComponent()
        {
            dayPanel = new Panel();
            dayPanel.SuspendLayout();
            SuspendLayout();
            // 
            // onTheMonthsPanel
            // 
            dayPanel.BackColor = SystemColors.ControlLightLight;
            dayPanel.Name = "dayPanel";
            dayPanel.Size = new Size(294, 116);
            dayPanel.TabIndex = 7;
            dayPanel.Visible = false;

            // 
            // MonthlyComboPopup
            // 

            Controls.Add(dayPanel);
            Name = "DayComboPopup";
            Size = new Size(294, 116);
            dayPanel.ResumeLayout(false);
            dayPanel.PerformLayout();
            ResumeLayout(false);
        }

        /// <summary>
        /// Binds the days.
        /// </summary>
        private void BindDays()
        {
            // Loop through all the months in MonthTypeEnum.
            for (int dayIndex = 0; dayIndex < 31; dayIndex++)
            {
                var day = dayIndex + 1;

                int verticlePosition = 23 * (dayIndex / 7) + 4;
                int horizontalPosition = 41 * (dayIndex % 7) + 4;

                // Build dayList with new checkboxes and related enums.
                var checkboxAndDay = new Tuple<CheckBox, int>(new CheckBox
                {
                    Text = day.ToString(),
                    Location = new Point(horizontalPosition, verticlePosition),
                    Width = 40
                }, day);

                checkboxAndDay.Item1.CheckedChanged += OnDayCheckChanged;

                dayList.Add(checkboxAndDay);

                // Add checkbox to panel
                dayPanel.Controls.Add(checkboxAndDay.Item1);
            }
        }

        /// <summary>
        /// Check changed for month checkbox
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnDayCheckChanged(object sender, EventArgs e)
        {
            if (DateChanged != null)
            {
                DateChanged();
            }

            Invalidate();
        }

        /// <summary>
        /// Called when [draw selected item].
        /// </summary>
        /// <param name="graphics">The graphics.</param>
        /// <param name="foreColor">Color of the fore.</param>
        /// <param name="bounds">The bounds.</param>
        protected override void OnDrawSelectedItem(Graphics graphics, Color foreColor, Rectangle bounds)
        {
            var formatter = new DateOfMonthComboFormatter();
            string text = formatter.FormatDays(GetSelectedDays());

            using (var stringFormat = new StringFormat())
            {
                stringFormat.Trimming = StringTrimming.EllipsisCharacter;

                using (var brush = new SolidBrush(ForeColor))
                {
                    graphics.DrawString(text, Font, brush, bounds, stringFormat);
                }
            }
        }
    }
}