using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;
using Interapptive.Shared.UI;
using System.IO;
using ShipWorks.Data.Import.Spreadsheet;

namespace ShipWorks.Data.Import.Spreadsheet.Types.Excel.Editing
{
    /// <summary>
    /// Wizard for creating a Excel import map
    /// </summary>
    public partial class GenericExcelMapWizard : WizardForm
    {
        GenericSpreadsheetTargetSchema targetSchema;
        GenericExcelMap excelMap;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericExcelMapWizard(GenericSpreadsheetTargetSchema targetSchema)
        {
            InitializeComponent();

            if (targetSchema == null)
            {
                throw new ArgumentNullException("targetSchema");
            }

            this.targetSchema = targetSchema;
        }

        /// <summary>
        /// Gets the map created by the wizard.  Only valid if DialogResult == OK
        /// </summary>
        public GenericExcelMap Map
        {
            get { return excelMap; }
        }

        /// <summary>
        /// Stepping next from the sample map page
        /// </summary>
        private void OnStepNextSampleMap(object sender, WizardStepEventArgs e)
        {
            var sourceSchema = excelSchemaControl.CurrentSchema;

            if (sourceSchema == null)
            {
                MessageHelper.ShowInformation(this, "Please select a valid sample file to continue.");
                e.NextPage = CurrentPage;
                return;
            }

            excelMap = new GenericExcelMap(targetSchema);
            excelMap.Name = Path.GetFileNameWithoutExtension(excelSchemaControl.SampleFilename);
            excelMap.SourceSchema = sourceSchema;
            excelMapEditor.LoadMap(excelMap);
        }

        /// <summary>
        /// Settings next from the map settings page
        /// </summary>
        private void OnStepNextSettingsPage(object sender, WizardStepEventArgs e)
        {
            if (!excelMapEditor.SaveToMap())
            {
                e.NextPage = CurrentPage;
                return;
            }
        }
    }
}
