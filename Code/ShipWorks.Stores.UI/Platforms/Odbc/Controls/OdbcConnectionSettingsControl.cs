using Autofac;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.Upload;
using ShipWorks.Stores.UI.Platforms.Odbc.WizardPages.Import;
using ShipWorks.Stores.UI.Platforms.Odbc.WizardPages.Upload;
using System;
using System.Windows.Forms;

namespace ShipWorks.Stores.UI.Platforms.Odbc.Controls
{
    /// <summary>
    ///
    /// </summary>
    public partial class OdbcConnectionSettingsControl : AccountSettingsControlBase
    {
        private readonly IStoreManager storeManager;
        private OdbcStoreEntity odbcStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcConnectionSettingsControl"/> class.
        /// </summary>
        /// <param name="pages">The pages.</param>
        /// <param name="storeManager"></param>
        public OdbcConnectionSettingsControl(IStoreManager storeManager)
        {
            this.storeManager = storeManager;
            InitializeComponent();

            EnumHelper.BindComboBox<OdbcShipmentUploadStrategy>(uploadStrategy);
        }

        /// <summary>
        /// Launches import settings wizard and saves to store.
        /// </summary>
        private void OnEditImportSettingsClick(object sender, EventArgs e)
        {
            using (ILifetimeScope scope = IoC.BeginLifetimeScope(ConfigureOdbcSettingsWizardDependencies))
            {
                IOdbcWizardPage[] importPages =
                {
                    scope.Resolve<OdbcImportDataSourcePage>(),
                    scope.Resolve<OdbcImportMapSettingsPage>(),
                    scope.Resolve<OdbcImportMappingPage>()
                };

                using (OdbcSettingsWizard wizard = scope.Resolve<OdbcSettingsWizard>())
                {
                    wizard.LoadPages(odbcStore, importPages);
                    wizard.ShowDialog(this);
                }
            }
        }

        /// <summary>
        /// Configures the add store wizard dependencies.
        /// </summary>
        private static void ConfigureOdbcSettingsWizardDependencies(ContainerBuilder builder)
        {
            builder.RegisterType<OdbcSettingsWizard>()
                .AsSelf()
                .As<IWin32Window>()
                .SingleInstance();
        }

        /// <summary>
        /// Load the data from the given store into the control
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            OdbcStoreEntity odbcStoreEntity = store as OdbcStoreEntity;
            if (odbcStoreEntity != null)
            {
                odbcStore = odbcStoreEntity;
                uploadStrategy.SelectedValue = (OdbcShipmentUploadStrategy) odbcStore.UploadStrategy;
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the uploadStrategy control.
        /// </summary>
        private void OnUploadStrategySelectedIndexChanged(object sender, EventArgs e)
        {
            odbcStore.UploadStrategy = (int) uploadStrategy.SelectedValue;
            
            ToggleUploadSettingsEnabled();
        }

        /// <summary>
        /// EditUploadSettings button is enabled if there are settings to edit.
        /// </summary>
        private void ToggleUploadSettingsEnabled()
        {
            editUploadSettings.Enabled =
                            (OdbcShipmentUploadStrategy)uploadStrategy.SelectedValue == OdbcShipmentUploadStrategy.UseShipmentDataSource;
        }

        /// <summary>
        /// Launches the upload settings wizard and updates settings.
        /// </summary>
        private void OnEditUploadSettingsClick(object sender, EventArgs e)
        {
            using (ILifetimeScope scope = IoC.BeginLifetimeScope(ConfigureOdbcSettingsWizardDependencies))
            {
                IOdbcWizardPage[] uploadPages =
                {
                    scope.Resolve<OdbcUploadDataSourcePage>()
                };

                using (OdbcSettingsWizard wizard = scope.Resolve<OdbcSettingsWizard>())
                {
                    wizard.LoadPages(odbcStore, uploadPages);
                    wizard.ShowDialog(this);
                }
            }
        }
    }
}
