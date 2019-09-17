using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Stores.Management;
using ShipWorks.UI.Wizard;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericFile.Formats.Csv;
using Interapptive.Shared.UI;
using ShipWorks.Data.Import.Spreadsheet.OrderSchema;

namespace ShipWorks.Stores.Platforms.GenericFile.WizardPages
{
    /// <summary>
    /// Wizard page for setting up the excel import mapping for Generic File
    /// </summary>
    public partial class GenericStoreExcelSetupPage : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GenericStoreExcelSetupPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Stepping into the
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            if (e.FirstTime)
            {
                excelMapChooser.Initialize(new GenericSpreadsheetOrderSchema());
            }

            var store = GetStore<GenericFileStoreEntity>();

            e.Skip = store.FileFormat != (int) GenericFileFormat.Excel || !string.IsNullOrWhiteSpace(store.FlatImportMap);
        }

        /// <summary>
        /// Stepping next
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            if (excelMapChooser.Map == null)
            {
                MessageHelper.ShowInformation(this, "Please configure an import map before proceeding.");
                e.NextPage = this;
                return;
            }

            GetStore<GenericFileStoreEntity>().FlatImportMap = excelMapChooser.Map.SerializeToXml();
        }
    }
}
