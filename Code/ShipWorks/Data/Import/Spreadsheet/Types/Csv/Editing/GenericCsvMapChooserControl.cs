using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Import.Spreadsheet.Editing;
using System.IO;

namespace ShipWorks.Data.Import.Spreadsheet.Types.Csv.Editing
{
    /// <summary>
    /// Map chooser for CSV maps
    /// </summary>
    public partial class GenericCsvMapChooserControl : GenericSpreadsheetMapChooserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GenericCsvMapChooserControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Must be overridden by derived classes to create the map
        /// </summary>
        protected override GenericSpreadsheetMap CreateMap()
        {
            using (GenericCsvMapWizard wizard = new GenericCsvMapWizard(TargetSchema))
            {
                if (wizard.ShowDialog(this) == DialogResult.OK)
                {
                    return wizard.Map;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Must be overridden by derived classes to edit an existing map
        /// </summary>
        protected override bool EditMap(GenericSpreadsheetMap map)
        {
            using (GenericCsvMapEditorDlg dlg = new GenericCsvMapEditorDlg((GenericCsvMap) map))
            {
                return (dlg.ShowDialog(this) == DialogResult.OK);
            }
        }

        /// <summary>
        /// Must be overridden by derived classes to create the proper dialog for saving a map
        /// </summary>
        protected override SaveFileDialog CreateSaveFileDialog()
        {
            SaveFileDialog dlg = new SaveFileDialog();

            dlg.Filter = "ShipWorks CSV Map (*.swcsvm)|*.swcsvm";
            dlg.AddExtension = true;
            dlg.DefaultExt = ".swm";
            dlg.FileName = string.Format("{0}.swcsvm", Map.Name);
        
            return dlg;
        }

        /// <summary>
        /// Must be overridden by derived classes to create the proper open dialog.  The result is passed to LoadMap
        /// </summary>
        protected override OpenFileDialog CreateOpenFileDialog()
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.Filter = "ShipWorks CSV Map (*.swcsvm)|*.swcsvm";

            return dlg;
        }

        /// <summary>
        /// Must be overridden by derived classes to load an existing map.  The filename and filterIndex come from the dialog returned by CreateOpenFileDialog.
        /// </summary>
        protected override GenericSpreadsheetMap LoadMap(string filename, int filterIndex)
        {
            return new GenericCsvMap(TargetSchema, File.ReadAllText(filename));
        }
    }
}
