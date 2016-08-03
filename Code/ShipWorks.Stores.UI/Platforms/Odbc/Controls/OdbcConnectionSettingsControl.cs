using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.UI.Platforms.Odbc.WizardPages;
using ShipWorks.Stores.UI.Platforms.Odbc.WizardPages.Import;
using ShipWorks.Stores.UI.Platforms.Odbc.WizardPages.Upload;
using ShipWorks.UI.Wizard;
using System;
using System.Windows.Forms;

namespace ShipWorks.Stores.UI.Platforms.Odbc.Controls
{
    /// <summary>
    /// Expose ODBC Store account settings to be shown in the Store Settings window
    /// </summary>
    public partial class OdbcConnectionSettingsControl : AccountSettingsControlBase
    {
        private OdbcStoreEntity odbcStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcConnectionSettingsControl"/> class.
        /// </summary>
        public OdbcConnectionSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Launches import settings wizard and saves to store.
        /// </summary>
        private void OnEditImportSettingsClick(object sender, EventArgs e)
        {
            using (ILifetimeScope scope = IoC.BeginLifetimeScope(ConfigureOdbcSettingsWizardDependencies))
            {
                WizardPage[] importPages =
                {
                    scope.Resolve<OdbcImportDataSourcePage>(),
                    scope.Resolve<OdbcImportMapSettingsPage>(),
                    scope.Resolve<OdbcImportMappingPage>(),
                    scope.Resolve<OdbcSetupFinishPage>()
                };

                using (OdbcSettingsWizard wizard = scope.Resolve<OdbcSettingsWizard>())
                {
                    wizard.LoadPages(odbcStore, importPages);
                    wizard.ShowDialog(this);
                }
            }
        }

        /// <summary>
        /// Launches the upload settings wizard and updates settings.
        /// </summary>
        private void OnEditUploadSettingsClick(object sender, EventArgs e)
        {
            using (ILifetimeScope scope = IoC.BeginLifetimeScope(ConfigureOdbcSettingsWizardDependencies))
            {
                WizardPage[] uploadPages =
                {
                    scope.Resolve<OdbcUploadShipmentStrategyPage>(),
                    scope.Resolve<OdbcUploadDataSourcePage>(),
                    scope.Resolve<OdbcUploadMapSettingsPage>(),
                    scope.Resolve<OdbcUploadMappingPage>(),
                    scope.Resolve<OdbcSetupFinishPage>()
                };

                using (OdbcSettingsWizard wizard = scope.Resolve<OdbcSettingsWizard>())
                {
                    wizard.LoadPages(odbcStore, uploadPages);
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
            odbcStore = store as OdbcStoreEntity;
            if (odbcStore == null)
            {
                throw new ArgumentException("OdbcStore expected.", "store");
            }
        }
    }
}
