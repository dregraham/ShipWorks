using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Odbc;

namespace ShipWorks.Stores.UI.Platforms.Odbc
{
    /// <summary>
    ///
    /// </summary>
    public partial class OdbcConnectionSettingsControl : AccountSettingsControlBase
    {
        private readonly IEnumerable<IOdbcWizardPage> importPages;
        private readonly IEnumerable<IOdbcWizardPage> uploadPages;
        private OdbcStoreEntity odbcStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcConnectionSettingsControl"/> class.
        /// </summary>
        /// <param name="pages">The pages.</param>
        public OdbcConnectionSettingsControl(IEnumerable<IOdbcWizardPage> pages)
        {
            InitializeComponent();

            IEnumerable<IOdbcWizardPage> wizardPages = pages as IOdbcWizardPage[] ?? pages.ToArray();

            // todo: change the 1 to 3 when doing story for other pages.
            importPages = wizardPages.Where(p => p.Position < 1);
            uploadPages = wizardPages.Where(p => p.Position >= 3);

        }

        /// <summary>
        /// Called when [click edit import settings].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnClickEditImportSettings(object sender, EventArgs e)
        {
            using (OdbcImportSettingsWizard wizard = new OdbcImportSettingsWizard(odbcStore, importPages))
            {
                wizard.ShowDialog(this);
            }
        }

        /// <summary>
        /// Load the data from the given store into the control
        /// </summary>
        /// <param name="store"></param>
        public override void LoadStore(StoreEntity store)
        {
            OdbcStoreEntity odbcStoreEntity = store as OdbcStoreEntity;
            if (odbcStoreEntity != null)
            {
                odbcStore = odbcStoreEntity;
            }
        }
    }
}
