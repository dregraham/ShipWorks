using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.UI.Platforms.Odbc.WizardPages.Import;
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
        }

        /// <summary>
        /// Called when [click edit import settings].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnClickEditImportSettings(object sender, EventArgs e)
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

                    if (wizard.ShowDialog(this) == DialogResult.OK)
                    {
                        storeManager.SaveStore(odbcStore);
                    }
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
