using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.Upload;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.UI.Platforms.Odbc.WizardPages.Upload
{
    /// <summary>
    /// Odbc add store wizard page dealing with uploading shipment details
    /// </summary>
    public partial class OdbcUploadShipmentStrategyPage : AddStoreWizardPage, IOdbcWizardPage
    {
        private OdbcStoreEntity store;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcUploadShipmentStrategyPage"/> class.
        /// </summary>
        public OdbcUploadShipmentStrategyPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the position.
        /// </summary>
        public int Position => 3;

        /// <summary>
        /// Called when [stepping into].
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            store = GetStore<OdbcStoreEntity>();
            
            OdbcShipmentUploadStrategy uploadStrategy = (OdbcShipmentUploadStrategy)store.UploadStrategy;

            doNotUpload.Checked = uploadStrategy == OdbcShipmentUploadStrategy.DoNotUpload;
            useShipmentDataSource.Checked = uploadStrategy == OdbcShipmentUploadStrategy.UseShipmentDataSource;
            useImportDataSource.Checked = uploadStrategy == OdbcShipmentUploadStrategy.UseImportDataSource;
        }

        /// <summary>
        /// Called when [step next].
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            if (doNotUpload.Checked)
            {
                store.UploadStrategy = (int) OdbcShipmentUploadStrategy.DoNotUpload;
                store.UploadColumnSourceType = 0;
                store.UploadMap = string.Empty;
                store.UploadColumnSource = string.Empty;
            }
            else if (useImportDataSource.Checked)
            {
                store.UploadStrategy = (int) OdbcShipmentUploadStrategy.UseImportDataSource;
            }
            else if (useShipmentDataSource.Checked)
            {
                store.UploadStrategy = (int) OdbcShipmentUploadStrategy.UseShipmentDataSource;
            }
        }

        /// <summary>
        /// Called when [step back].
        /// </summary>
        private void OnStepBack(object sender, WizardStepEventArgs e)
        {
            if (doNotUpload.Checked)
            {
                store.UploadStrategy = (int)OdbcShipmentUploadStrategy.DoNotUpload;
            }
            else if (useImportDataSource.Checked)
            {
                store.UploadStrategy = (int)OdbcShipmentUploadStrategy.UseImportDataSource;
            }
            else if (useShipmentDataSource.Checked)
            {
                store.UploadStrategy = (int)OdbcShipmentUploadStrategy.UseShipmentDataSource;
            }
        }
    }
}
