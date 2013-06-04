using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Divelements.SandGrid.Rendering;

namespace ShipWorks.Data.Grid.Columns.DisplayTypes.Editors
{
    /// <summary>
    /// Editor for the DateTime display type
    /// </summary>
    public partial class GridDateDisplayEditor : GridColumnDisplayEditor
    {
        GridDateDisplayType displayType;

        TextFormattingInformation textFormat;

        DateTime exampleDate = new DateTime(2001, 3, 4, 13, 30, 0);

        /// <summary>
        /// To use visual inheritance there must be a constructor with zero arguments
        /// </summary>
        private GridDateDisplayEditor()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public GridDateDisplayEditor(GridDateDisplayType displayType) : 
            base(displayType)
        {
            InitializeComponent();

            if (displayType == null)
            {
                throw new ArgumentNullException("displayType");
            }

            textFormat = new TextFormattingInformation();
            textFormat.StringFormat = new StringFormat(StringFormatFlags.NoWrap);
            textFormat.TextFormatFlags = TextFormatFlags.Top;

            this.displayType = displayType;

            showDate.Checked = displayType.ShowDate;
            dateFormat.SelectedItem = displayType.DateFormat;
            todayYesterday.Checked = displayType.UseDescriptiveDates;

            showTime.Checked = displayType.TimeDisplayFormat != TimeDisplayFormat.None;
            radio12Hour.Checked = displayType.TimeDisplayFormat != TimeDisplayFormat.Military;
            radio24hour.Checked = !radio12Hour.Checked;

            UpdateDateControls();
            UpdateTimeControls();

            showTime.CheckedChanged += new EventHandler(OnChangeTimeDisplay);
            radio12Hour.CheckedChanged += new EventHandler(OnChangeTimeDisplay);
            radio24hour.CheckedChanged += new EventHandler(OnChangeTimeDisplay);

            showDate.CheckedChanged += new EventHandler(OnChangeDateDisplay);
            dateFormat.SelectedIndexChanged += new EventHandler(OnChangeDateDisplay);
            todayYesterday.CheckedChanged += new EventHandler(OnChangeDateDisplay);
        }

        /// <summary>
        /// Custom draw the date box
        /// </summary>
        private void OnDrawDateItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawFocusRectangle();

            if (e.Index >= 0)
            {
                string format = dateFormat.GetItemText(dateFormat.Items[e.Index]);

                IndependentText.DrawText(e.Graphics, exampleDate.ToString(format), e.Font, e.Bounds, textFormat, e.ForeColor);
            }
        }

        /// <summary>
        /// Date formatting has changed
        /// </summary>
        void OnChangeDateDisplay(object sender, EventArgs e)
        {
            UpdateDateControls();

            displayType.ShowDate = showDate.Checked;
            displayType.DateFormat = (string) dateFormat.SelectedItem;
            displayType.UseDescriptiveDates = todayYesterday.Checked;

            OnValueChanged();
        }

        /// <summary>
        /// Update the UI of the date controls
        /// </summary>
        private void UpdateDateControls()
        {
            dateFormat.Enabled = showDate.Checked;
            todayYesterday.Enabled = showDate.Checked;
        }

        /// <summary>
        /// Changing whether we should show time
        /// </summary>
        private void OnChangeTimeDisplay(object sender, EventArgs e)
        {
            UpdateTimeControls();

            if (showTime.Checked)
            {
                displayType.TimeDisplayFormat = radio12Hour.Checked ? TimeDisplayFormat.Standard : TimeDisplayFormat.Military;
            }
            else
            {
                displayType.TimeDisplayFormat = TimeDisplayFormat.None;
            }

            OnValueChanged();
        }

        /// <summary>
        /// Update the enabled\disabled state of the time controls
        /// </summary>
        private void UpdateTimeControls()
        {
            radio12Hour.Enabled = showTime.Checked;
            radio24hour.Enabled = showTime.Checked;
        }
    }
}
