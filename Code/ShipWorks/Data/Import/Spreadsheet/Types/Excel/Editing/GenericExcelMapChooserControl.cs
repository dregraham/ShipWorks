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

namespace ShipWorks.Data.Import.Spreadsheet.Types.Excel.Editing
{
    /// <summary>
    /// Map chooser for Excel maps
    /// </summary>
    public partial class GenericExcelMapChooserControl : GenericSpreadsheetMapChooserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GenericExcelMapChooserControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Must be overridden by derived classes to create the map
        /// </summary>
        protected override GenericSpreadsheetMap CreateMap()
        {
            using (GenericExcelMapWizard wizard = new GenericExcelMapWizard(TargetSchema))
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
            using (GenericExcelMapEditorDlg dlg = new GenericExcelMapEditorDlg((GenericExcelMap) map))
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

            dlg.Filter = "ShipWorks Excel Map (*.swxlm)|*.swxlm";
            dlg.AddExtension = true;
            dlg.DefaultExt = ".swxlm";
            dlg.FileName = string.Format("{0}.swxlm", Map.Name);
        
            return dlg;
        }

        /// <summary>
        /// Must be overridden by derived classes to create the proper open dialog.  The result is passed to LoadMap
        /// </summary>
        protected override OpenFileDialog CreateOpenFileDialog()
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.Filter = "ShipWorks Excel Map (*.swxlm)|*.swxlm";

            return dlg;
        }

        /// <summary>
        /// Must be overridden by derived classes to load an existing map.  The filename and filterIndex come from the dialog returned by CreateOpenFileDialog.
        /// </summary>
        protected override GenericSpreadsheetMap LoadMap(string filename, int filterIndex)
        {
            return new GenericExcelMap(TargetSchema, File.ReadAllText(filename));
        }
    }
}
