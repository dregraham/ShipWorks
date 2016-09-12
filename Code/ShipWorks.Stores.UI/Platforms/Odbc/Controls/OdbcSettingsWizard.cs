using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.UI.Wizard;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ShipWorks.Stores.UI.Platforms.Odbc.Controls
{
    /// <summary>
    ///
    /// </summary>
    public partial class OdbcSettingsWizard : WizardForm, IStoreWizard
    {
        private OdbcStoreEntity originalOdbcStoreEntity;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcSettingsWizard"/> class.
        /// </summary>
        public OdbcSettingsWizard()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Loads the pages.
        /// </summary>
        /// <param name="odbcStore">The ODBC store.</param>
        /// <param name="wizardPages">The wizard pages.</param>
        public void LoadPages(OdbcStoreEntity odbcStore, IEnumerable<WizardPage> wizardPages)
        {
            originalOdbcStoreEntity = odbcStore;
            Store = EntityUtility.CloneEntity(odbcStore);

            Pages.AddRange(wizardPages.ToArray());
        }

        /// <summary>
        /// The store currently being configured by the wizard
        /// </summary>
        public StoreEntity Store { get; private set; }

        /// <summary>
        /// If wizard is Finished successfully, save store entity fields to originally passed in field.
        /// </summary>
        private void OnOdbcSettingsWizardClosed(object sender, FormClosedEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                OdbcStoreEntity storeToSave = (OdbcStoreEntity) Store;
                originalOdbcStoreEntity.ImportConnectionString = storeToSave.ImportConnectionString;
                originalOdbcStoreEntity.ImportMap = storeToSave.ImportMap;
                originalOdbcStoreEntity.ImportStrategy = storeToSave.ImportStrategy;
                originalOdbcStoreEntity.ImportColumnSourceType = storeToSave.ImportColumnSourceType;
                originalOdbcStoreEntity.ImportColumnSource = storeToSave.ImportColumnSource;
                originalOdbcStoreEntity.ImportOrderItemStrategy = storeToSave.ImportOrderItemStrategy;
                originalOdbcStoreEntity.UploadStrategy = storeToSave.UploadStrategy;
                originalOdbcStoreEntity.UploadMap = storeToSave.UploadMap;
                originalOdbcStoreEntity.UploadColumnSourceType = storeToSave.UploadColumnSourceType;
                originalOdbcStoreEntity.UploadColumnSource = storeToSave.UploadColumnSource;
                originalOdbcStoreEntity.UploadConnectionString = storeToSave.UploadConnectionString;
            }
        }
    }
}
