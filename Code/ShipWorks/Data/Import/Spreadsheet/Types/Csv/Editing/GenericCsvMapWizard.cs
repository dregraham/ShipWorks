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

namespace ShipWorks.Data.Import.Spreadsheet.Types.Csv.Editing
{
    /// <summary>
    /// Wizard for creating a CSV import map
    /// </summary>
    public partial class GenericCsvMapWizard : WizardForm
    {
        GenericSpreadsheetTargetSchema targetSchema;
        GenericCsvMap csvMap;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericCsvMapWizard(GenericSpreadsheetTargetSchema targetSchema)
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
        public GenericCsvMap Map
        {
            get { return csvMap; }
        }

        /// <summary>
        /// Stepping next from the sample map page
        /// </summary>
        private void OnStepNextSampleMap(object sender, WizardStepEventArgs e)
        {
            var sourceSchema = csvSchemaControl.CurrentSchema;

            if (sourceSchema == null)
            {
                MessageHelper.ShowInformation(this, "Please select a valid sample file to continue.");
                e.NextPage = CurrentPage;
                return;
            }

            csvMap = new GenericCsvMap(targetSchema);
            csvMap.Name = Path.GetFileNameWithoutExtension(csvSchemaControl.SampleFilename);
            csvMap.SourceSchema = sourceSchema;
            csvMapEditor.LoadMap(csvMap);
        }

        /// <summary>
        /// Settings next from the map settings page
        /// </summary>
        private void OnStepNextSettingsPage(object sender, WizardStepEventArgs e)
        {
            if (!csvMapEditor.SaveToMap())
            {
                e.NextPage = CurrentPage;
                return;
            }
        }
    }
}
