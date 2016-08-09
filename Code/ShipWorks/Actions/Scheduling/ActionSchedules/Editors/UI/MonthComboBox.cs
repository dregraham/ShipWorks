using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.Actions.Scheduling.ActionSchedules.Enums;
using ShipWorks.UI;
using ShipWorks.UI.Controls;

namespace ShipWorks.Actions.Scheduling.ActionSchedules.Editors.UI
{
    /// <summary>
    /// MonthComboBox is a combobox that shows a list of selectable months.
    /// </summary>
    public class MonthComboBox : PopupComboBox
    {
        private readonly List<Tuple<CheckBox, MonthType>> monthsList;

        private bool ignoreMonthCheckChanged;

        private Panel monthPanel;

        private CheckBox selectAll;

        /// <summary>
        /// Initializes a new instance of the <see cref="MonthComboBox"/> class.
        /// </summary>
        public MonthComboBox()
        {
            monthsList = new List<Tuple<CheckBox, MonthType>>();

            DropDownMinimumHeight = 294;
            DropDownHeight = 294;

            InitializeComponent();

            PopupController = new PopupController(monthPanel);

            BindMonths();
        }

        /// <summary>
        /// Gets or sets the month changed.
        /// </summary>
        public Action MonthChanged
        {
            get;
            set;
        }

        /// <summary>
        /// Selects the months.
        /// </summary>
        /// <param name="monthsToSelect">The months to select.</param>
        public void SelectMonths(List<MonthType> monthsToSelect)
        {
            foreach (var monthHolder in monthsList)
            {
                monthHolder.Item1.Checked = monthsToSelect != null && monthsToSelect.Any(m => m == monthHolder.Item2);
            }
        }

        /// <summary>
        /// Gets the selected months.
        /// </summary>
        public List<MonthType> GetSelectedMonths()
        {
            return (from monthHolder in monthsList
                    where monthHolder.Item1.Checked
                    select monthHolder.Item2).ToList();
        }

        /// <summary>
        /// Initialize Component
        /// </summary>
        private void InitializeComponent()
        {
            monthPanel = new Panel();
            selectAll = new CheckBox();
            monthPanel.SuspendLayout();
            SuspendLayout();
            //
            // onTheMonthsPanel
            //
            monthPanel.BackColor = SystemColors.ControlLightLight;
            monthPanel.Controls.Add(selectAll);
            monthPanel.Name = "monthPanel";
            monthPanel.Size = new Size(130, 306);
            monthPanel.TabIndex = 7;
            monthPanel.Visible = false;
            //
            // onTheSelectAllMonths
            //
            selectAll.AutoSize = true;
            selectAll.Location = new Point(3, 3);
            selectAll.Name = "selectAll";
            selectAll.Size = new Size(120, 17);
            selectAll.TabIndex = 1;
            selectAll.Text = "<Select All Months>";
            selectAll.UseVisualStyleBackColor = true;
            selectAll.CheckedChanged += OnSelectAllMonthsCheckChanged;
            //
            // MonthlyComboPopup
            //

            Controls.Add(monthPanel);
            Name = "MonthlyComboPopup";
            Size = new Size(316, 348);
            monthPanel.ResumeLayout(false);
            monthPanel.PerformLayout();
            ResumeLayout(false);
        }

        /// <summary>
        /// Binds the months.
        /// </summary>
        private void BindMonths()
        {
            EnumEntry<MonthType>[] months = EnumHelper.GetEnumList<MonthType>().ToArray();

            // Loop through all the months in MonthTypeEnum.
            for (int monthIndex = 0; monthIndex < months.Length; monthIndex++)
            {
                var month = months[monthIndex];

                int verticlePosition = 23 * monthIndex + 23;

                // Build monthsList with new checkboxes and related enums.
                var checkboxAndMonthType = new Tuple<CheckBox, MonthType>(new CheckBox
                {
                    Text = month.Key,
                    Location = new Point(3, verticlePosition)
                }, month.Value);

                checkboxAndMonthType.Item1.CheckedChanged += OnMonthCheckChanged;

                monthsList.Add(checkboxAndMonthType);

                // Add checkbox to panel
                monthPanel.Controls.Add(checkboxAndMonthType.Item1);
            }
        }

        /// <summary>
        /// Check changed for month checkbox
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnMonthCheckChanged(object sender, EventArgs e)
        {
            if (!ignoreMonthCheckChanged)
            {
                selectAll.CheckedChanged -= OnSelectAllMonthsCheckChanged;
                selectAll.Checked = monthsList.All(m => m.Item1.Checked);
                selectAll.CheckedChanged += OnSelectAllMonthsCheckChanged;

                if (MonthChanged != null)
                {
                    MonthChanged();
                }

                Invalidate();
            }
        }

        /// <summary>
        /// Checks changed for select all months.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnSelectAllMonthsCheckChanged(object sender, EventArgs e)
        {
            ignoreMonthCheckChanged = true;

            bool isChecked = ((CheckBox) sender).Checked;

            foreach (var months in monthsList)
            {
                months.Item1.Checked = isChecked;
            }

            if (MonthChanged != null)
            {
                MonthChanged();
            }

            ignoreMonthCheckChanged = false;
        }

        /// <summary>
        /// Draw the item that the user has currently selected
        /// </summary>
        protected override void OnDrawSelectedItem(Graphics graphics, Color foreColor, Rectangle bounds)
        {
            string text = string.Join(", ", GetSelectedMonths().Select(m => EnumHelper.GetDescription(m)));
            if (selectAll.Checked)
            {
                text = "Every Month";
            }

            using (StringFormat stringFormat = new StringFormat())
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