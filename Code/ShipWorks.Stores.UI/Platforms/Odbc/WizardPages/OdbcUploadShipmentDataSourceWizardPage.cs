using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.Upload;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.UI.Platforms.Odbc.WizardPages
{
    public partial class OdbcUploadShipmentDataSourceWizardPage : AddStoreWizardPage, IOdbcWizardPage
    {
        private OdbcStoreEntity store;

        public OdbcUploadShipmentDataSourceWizardPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the position.
        /// </summary>
        public int Position => 3;

        /// <summary>
        /// Called when [step next].
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            if (store == null)
            {
                store = GetStore<OdbcStoreEntity>();
            }

            if (uploadToSame.Checked)
            {
                store.ShipmentUploadStrategy = (int)OdbcShipmentUploadStrategy.UseImportDataSource;
            }
            else if (uploadToDifferent.Checked)
            {
                store.ShipmentUploadStrategy = (int)OdbcShipmentUploadStrategy.UseShipmentDataSource;
            }
            else
            {
                store.ShipmentUploadStrategy = (int)OdbcShipmentUploadStrategy.DoNotUpload;
            }
        }
    }
}
