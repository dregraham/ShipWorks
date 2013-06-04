using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Import.Spreadsheet;

namespace ShipWorks.Data.Import.Spreadsheet.Editing
{
    /// <summary>
    /// Window for changing the date settings of a generic CSV map
    /// </summary>
    public partial class GenericSpreadsheetDateSettingsDlg : Form
    {
        GenericSpreadsheetMapDateSettings dateSettings;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericSpreadsheetDateSettingsDlg(GenericSpreadsheetMapDateSettings dateSettings)
        {
            InitializeComponent();

            this.dateSettings = dateSettings;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            dateTimeFormat.Items.Clear();
            dateTimeFormat.Items.AddRange(GenericSpreadsheetUtility.CommonDateTimeFormats);

            dateFormat.Items.Clear();
            dateFormat.Items.AddRange(GenericSpreadsheetUtility.CommonDateFormats);

            timeFormat.Items.Clear();
            timeFormat.Items.AddRange(GenericSpreadsheetUtility.CommonTimeFormats);

            TimeSpan localOffset = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now);
            comboTimezone.Items.Add(string.Format("Local (UTC {0}{1})", localOffset <= TimeSpan.Zero ? "-" : "+", localOffset.ToString(@"hh\:mm")));
            comboTimezone.Items.Add("GMT (UTC -00:00)");

            dateTimeFormat.Text = dateSettings.DateTimeFormat;
            dateFormat.Text = dateSettings.DateFormat;
            timeFormat.Text = dateSettings.TimeFormat;
            comboTimezone.SelectedIndex = (int) dateSettings.TimeZoneAssumption;
        }

        /// <summary>
        /// Saving results
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            dateSettings.DateTimeFormat = dateTimeFormat.Text;
            dateSettings.DateFormat = dateFormat.Text;
            dateSettings.TimeFormat = timeFormat.Text;
            dateSettings.TimeZoneAssumption = (GenericSpreadsheetTimeZoneAssumption) comboTimezone.SelectedIndex;

            DialogResult = DialogResult.OK;
        }
    }
}
