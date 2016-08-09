using Autofac;
using Autofac.Features.Indexed;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Download;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using ShipWorks.Stores.Platforms.Odbc.Upload;
using ShipWorks.Stores.UI.Platforms.Odbc.WizardPages;
using ShipWorks.Stores.UI.Platforms.Odbc.WizardPages.Import;
using ShipWorks.Stores.UI.Platforms.Odbc.WizardPages.Upload;
using ShipWorks.UI.Wizard;
using System;
using System.IO;
using System.Windows.Forms;

namespace ShipWorks.Stores.UI.Platforms.Odbc.Controls
{
    /// <summary>
    /// Expose ODBC Store account settings to be shown in the Store Settings window
    /// </summary>
    public partial class OdbcConnectionSettingsControl : AccountSettingsControlBase
    {
        private readonly Func<IOdbcFieldMap> odbcFieldMapFunc;
        private readonly IIndex<FileDialogType, IFileDialog> fileDialogFactory;
        private readonly IOdbcImportSettingsFile importSettingsFile;
        private readonly IOdbcSettingsFile uploadSettingsFile;
        private OdbcStoreEntity store;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcConnectionSettingsControl"/> class.
        /// </summary>
        public OdbcConnectionSettingsControl(
            Func<IOdbcFieldMap> odbcFieldMapFunc,
            IIndex<FileDialogType, IFileDialog> fileDialogFactory,
            IOdbcImportSettingsFile importSettingsFile,
            IOdbcSettingsFile uploadSettingsFile)
        {
            this.odbcFieldMapFunc = odbcFieldMapFunc;
            this.fileDialogFactory = fileDialogFactory;
            this.importSettingsFile = importSettingsFile;
            this.uploadSettingsFile = uploadSettingsFile;
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
                    wizard.LoadPages(store, importPages);
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
                    wizard.LoadPages(store, uploadPages);
                    if (wizard.ShowDialog(this) == DialogResult.OK)
                    {
                        ToggleExportUploadMapButtonEnabled();
                    }
                }
            }
        }

        /// <summary>
        /// Load the data from the given store into the control
        /// </summary>
        public override void LoadStore(StoreEntity odbcStore)
        {
            store = odbcStore as OdbcStoreEntity;
            if (store == null)
            {
                throw new ArgumentException("OdbcStore expected.", "odbcStore");
            }

            ToggleExportUploadMapButtonEnabled();
        }

        /// <summary>
        /// Enable export upload button only if upload is enabled.
        /// </summary>
        private void ToggleExportUploadMapButtonEnabled()
        {
            exportUploadMap.Enabled = ((OdbcShipmentUploadStrategy) store.UploadStrategy) !=
                                      OdbcShipmentUploadStrategy.DoNotUpload;
        }

        /// <summary>
        /// Called when [export import map click].
        /// </summary>
        private void OnExportImportMapClick(object sender, EventArgs e)
        {
            IOdbcFieldMap fieldMap = odbcFieldMapFunc();
            fieldMap.Load(store.ImportMap);

            IFileDialog fileDialog = fileDialogFactory[FileDialogType.Save];
            fileDialog.DefaultExt = importSettingsFile.Extension;
            fileDialog.Filter = importSettingsFile.Filter;
            fileDialog.DefaultFileName = fieldMap.Name;

            if (fileDialog.ShowDialog()!=DialogResult.OK)
            {
                return;
            }

            using (Stream streamToSave = fileDialog.CreateFileStream())
            using(TextWriter writer = new StreamWriter(streamToSave))
            {
                importSettingsFile.OdbcImportStrategy = (OdbcImportStrategy) store.ImportStrategy;
                importSettingsFile.ColumnSourceType = (OdbcColumnSourceType) store.ImportColumnSourceType;
                importSettingsFile.ColumnSource = store.ImportColumnSource;
                importSettingsFile.OdbcFieldMap = fieldMap;
                importSettingsFile.Save(writer);
            }
        }

        /// <summary>
        /// Called when [export upload map].
        /// </summary>
        private void OnExportUploadMap(object sender, EventArgs e)
        {
            IOdbcFieldMap fieldMap = odbcFieldMapFunc();
            fieldMap.Load(store.UploadMap);

            IFileDialog fileDialog = fileDialogFactory[FileDialogType.Save];
            fileDialog.DefaultExt = uploadSettingsFile.Extension;
            fileDialog.Filter = uploadSettingsFile.Filter;
            fileDialog.DefaultFileName = fieldMap.Name;

            if (fileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            using (Stream streamToSave = fileDialog.CreateFileStream())
            using (TextWriter writer = new StreamWriter(streamToSave))
            {
                uploadSettingsFile.ColumnSourceType = (OdbcColumnSourceType)store.UploadColumnSourceType;
                uploadSettingsFile.ColumnSource = store.UploadColumnSource;
                uploadSettingsFile.OdbcFieldMap = fieldMap;
                uploadSettingsFile.Save(writer);
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
    }
}
