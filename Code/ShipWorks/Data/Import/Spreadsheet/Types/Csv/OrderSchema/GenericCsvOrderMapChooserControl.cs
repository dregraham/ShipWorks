using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Import.Spreadsheet.Types.Csv.Editing;
using System.IO;

namespace ShipWorks.Data.Import.Spreadsheet.Types.Csv.OrderSchema
{
    /// <summary>
    /// Form for choosing Order import maps for CSV
    /// </summary>
    public partial class GenericCsvOrderMapChooserControl : GenericCsvMapChooserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GenericCsvOrderMapChooserControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Create the window for opening existing maps
        /// </summary>
        protected override OpenFileDialog CreateOpenFileDialog()
        {
            OpenFileDialog dlg = base.CreateOpenFileDialog();

            dlg.Filter += "|" + GenericCsvOrderTrueShipImport.FileFilter;

            return dlg;
        }

        /// <summary>
        /// Load an existing map
        /// </summary>
        protected override GenericSpreadsheetMap LoadMap(string filename, int filterIndex)
        {
            if (filterIndex == 1)
            {
                return base.LoadMap(filename, filterIndex);
            }
            else
            {
                return GenericCsvOrderTrueShipImport.DeserializeTrueShipMap(File.ReadAllText(filename));
            }
        }
    }
}
