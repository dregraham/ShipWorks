using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.UI;

namespace ShipWorks.Data.Import.Spreadsheet.Types.Csv.Editing
{
    /// <summary>
    /// Window for loading source columns from a sample CSV file
    /// </summary>
    public partial class GenericCsvChangeSourceSchemaDlg : Form
    {
        List<string> utilizedColumns;
        GenericCsvSourceSchema newSourceSchema;

        /// <summary>
        /// Constructor.  The map is only used to help warn the user if they are changing to a schema that loses columns
        /// </summary>
        public GenericCsvChangeSourceSchemaDlg(IEnumerable<string> utilizedColumns)
        {
            InitializeComponent();

            this.utilizedColumns = utilizedColumns.ToList();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// The new source schema to be used.  Only valid if DialogResult is OK
        /// </summary>
        public GenericCsvSourceSchema SourceSchema
        {
            get { return newSourceSchema; }
        }

        /// <summary>
        /// User OK'ing the changes
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            newSourceSchema = csvSourceSchemaControl.CurrentSchema;

            if (newSourceSchema == null)
            {
                MessageHelper.ShowInformation(this, "Please select a valid sample file to continue.");
                return;
            }

            if (!GenericSpreadsheetUtility.CheckForRemovedColumns(this, newSourceSchema, utilizedColumns))
            {
                return;
            }

            DialogResult = DialogResult.OK;
        }
    }
}
