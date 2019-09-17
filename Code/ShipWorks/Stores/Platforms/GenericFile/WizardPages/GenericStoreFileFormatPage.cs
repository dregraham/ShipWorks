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
using Interapptive.Shared.UI;

namespace ShipWorks.Stores.Platforms.GenericFile.WizardPages
{
    /// <summary>
    /// Wizard page for choosing the type of import for a generic store
    /// </summary>
    public partial class GenericStoreFileFormatPage : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GenericStoreFileFormatPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Stepping into the
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            var store = GetStore<GenericFileStoreEntity>();

            // if we have a map then we know the file format and it must be a hub store
            e.Skip = string.IsNullOrWhiteSpace(store.FlatImportMap);
        }

        /// <summary>
        /// Stepping next
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            GetStore<GenericFileStoreEntity>().FileFormat = (int) SelectedFileFormat;
        }

        /// <summary>
        /// Get the selected import type
        /// </summary>
        private GenericFileFormat SelectedFileFormat
        {
            get
            {
                if (radioCsv.Checked)
                {
                    return GenericFileFormat.Csv;
                }

                if (radioXml.Checked)
                {
                    return GenericFileFormat.Xml;
                }

                if (radioExcel.Checked)
                {
                    return GenericFileFormat.Excel;
                }

                throw new InvalidOperationException("No import type selected?");
            }
        }
    }
}
