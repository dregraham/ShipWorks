using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.Data.Import.Spreadsheet.Types.Csv.Editing
{
    /// <summary>
    /// Window for editing the file format settings of a map
    /// </summary>
    public partial class GenericCsvFileFormatSettingsDlg : Form
    {
        GenericCsvSourceSchema sourceSchema;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericCsvFileFormatSettingsDlg(GenericCsvSourceSchema sourceSchema)
        {
            InitializeComponent();

            this.sourceSchema = sourceSchema;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            fileFormatControl.Delimiter = sourceSchema.Delimiter;
            fileFormatControl.Quotes = sourceSchema.Quotes;
            fileFormatControl.QuoteEscape = sourceSchema.QuotesEscape;
            fileFormatControl.Encoding = sourceSchema.Encoding;
        }

        /// <summary>
        /// User is OK'ing the changes
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            sourceSchema.Delimiter = fileFormatControl.Delimiter;
            sourceSchema.Quotes = fileFormatControl.Quotes;
            sourceSchema.QuotesEscape = fileFormatControl.QuoteEscape;
            sourceSchema.Encoding = fileFormatControl.Encoding;

            DialogResult = DialogResult.OK;
        }
    }
}
