using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Input;
using Autofac.Features.Indexed;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Newtonsoft.Json.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.DataAccess;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Download;
using ShipWorks.Stores.Platforms.Odbc.Mapping;

namespace ShipWorks.Stores.UI.Platforms.Odbc.ViewModels.Import
{
    /// <summary>
    /// ViewModel for OdbcMapSettingsControl
    /// </summary>
    public class OdbcImportMapSettingsControlViewModel : OdbcMapSettingsControlViewModel
    {
        private bool columnSourceIsTable = true;
        private bool isSubquery = true; 
        private OdbcImportStrategy importStrategy = OdbcImportStrategy.ByModifiedTime;
        private OdbcImportOrderItemStrategy importOrderItemStrategy = OdbcImportOrderItemStrategy.SingleLine;
        private readonly Func<string, IDialog> dialogFactory;
        private IOdbcFieldMap fieldMap;
        private readonly Func<IOpenFileDialog> openFileDialogFactory;
        private readonly IOdbcImportSettingsFile importSettingsFile;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcImportMapSettingsControlViewModel"/> class.
        /// </summary>
        public OdbcImportMapSettingsControlViewModel(Func<string, IDialog> dialogFactory,
            IMessageHelper messageHelper,
            Func<string, IOdbcColumnSource> columnSourceFactory,
            IOdbcFieldMap fieldMap,
            Func<IOpenFileDialog> openFileDialogFactory,
            IOdbcImportSettingsFile importSettingsFile) :
                base(messageHelper, columnSourceFactory)
        {
            this.dialogFactory = dialogFactory;
            this.fieldMap = fieldMap;
            this.openFileDialogFactory = openFileDialogFactory;
            this.importSettingsFile = importSettingsFile;
            OpenMapSettingsFileCommand = new RelayCommand(OpenMapSettingsFile);
        }

        /// <summary>
        /// Gets the load map command.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand OpenMapSettingsFileCommand { get; }

        /// <summary>
        /// Whether the column source selected is table
        /// </summary>
        [Obfuscation(Exclude = true)]
        public override bool ColumnSourceIsTable
        {
            get => columnSourceIsTable;
            set
            {
                Handler.Set(nameof(ColumnSourceIsTable), ref columnSourceIsTable, value);

                // Show warning dlg when query is selected
                if (!value)
                {
                    IDialog warningDlg = dialogFactory("OdbcCustomQueryWarningDlg");
                    warningDlg.ShowDialog();
                }

                ColumnSource = value ? SelectedTable : CustomQueryColumnSource;
            }
        }

        /// <summary>
        /// The stores import strategy
        /// </summary>
        [Obfuscation(Exclude = true)]
        public OdbcImportStrategy ImportStrategy
        {
            get => importStrategy;
            set => Handler.Set(nameof(ImportStrategy), ref importStrategy, value);
        }

        /// <summary>
        /// The stores import strategy
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool IsSubquery
        {
            get => isSubquery;
            set => Handler.Set(nameof(IsSubquery), ref isSubquery, value);
        }

        /// <summary>
        /// Loads the map from disk.
        /// </summary>
        private void OpenMapSettingsFile()
        {
            IOpenFileDialog fileDialog = openFileDialogFactory();
            fileDialog.DefaultExt = importSettingsFile.Extension;
            fileDialog.Filter = importSettingsFile.Filter;
            fileDialog.DefaultFileName = fieldMap.Name;

            if (fileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            using (Stream streamToOpen = fileDialog.CreateFileStream())
            using (StreamReader reader = new StreamReader(streamToOpen))
            {
                GenericResult<JObject> openResults = importSettingsFile.Open(reader);
                if (openResults.Success)
                {
                    ImportStrategy = importSettingsFile.OdbcImportStrategy;

                    ColumnSourceIsTable = importSettingsFile.ColumnSourceType == OdbcColumnSourceType.Table;
                    LoadAndSetColumnSource(importSettingsFile.ColumnSource);
                    MapName = importSettingsFile.OdbcFieldMap.Name;
                    importOrderItemStrategy = importSettingsFile.OdbcImportItemStrategy;

                    fieldMap = importSettingsFile.OdbcFieldMap;

                    // upgrade the map to support alpha numeric order numbers just in case the user is trying to
                    // restore a map created prior to ShipWorks supporting alpha numeric order numbers
                    fieldMap.UpgradeToAlphanumericOrderNumbers();
                }
            }
        }

        /// <summary>
        /// Saves the map settings.
        /// </summary>
        public override void SaveMapSettings(OdbcStoreEntity store)
        {
            store.ImportStrategy = (int) ImportStrategy;

            if (ColumnSourceIsTable)
            {
                store.ImportColumnSourceType = (int) OdbcColumnSourceType.Table;
                store.ImportColumnSource = SelectedTable?.Name;
            }
            else
            {
                store.ImportColumnSourceType = IsSubquery ?
                    (int) OdbcColumnSourceType.CustomSubQuery :
                    (int) OdbcColumnSourceType.CustomParameterizedQuery;
            }

            store.ImportOrderItemStrategy = (int) importOrderItemStrategy;

            fieldMap.Name = MapName;
            store.ImportMap = fieldMap.Serialize();
        }

        /// <summary>
        /// Loads the map settings.
        /// </summary>
        public override void LoadMapSettings(OdbcStoreEntity store)
        {
            fieldMap.Load(store.ImportMap);
            MapName = fieldMap.Name;

            ImportStrategy = (OdbcImportStrategy) store.ImportStrategy;
            
            ColumnSourceIsTable = store.ImportColumnSourceType == (int) OdbcColumnSourceType.Table;
            IsSubquery = store.ImportColumnSourceType == (int) OdbcColumnSourceType.CustomSubQuery;

            importOrderItemStrategy = (OdbcImportOrderItemStrategy) store.ImportOrderItemStrategy;
        }

        /// <summary>
        /// The column source name to use for custom query
        /// </summary>
        public override string CustomQueryColumnSourceName => "Custom Import";
    }
}
