using Autofac.Features.Indexed;
using GalaSoft.MvvmLight.Command;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Newtonsoft.Json.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.DataAccess;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Download;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Input;

namespace ShipWorks.Stores.UI.Platforms.Odbc.ViewModels.Import
{
    /// <summary>
    /// ViewModel for OdbcMapSettingsControl
    /// </summary>
    public class OdbcImportMapSettingsControlViewModel : OdbcMapSettingsControlViewModel
    {
        private bool columnSourceIsTable = true;
        private bool downloadStrategyIsLastModified = true;
        private readonly Func<string, IDialog> dialogFactory;
        private readonly IOdbcSampleDataCommand sampleDataCommand;
        private IOdbcFieldMap fieldMap;
        private const int NumberOfSampleResults = 25;
        private bool isQueryValid;
        private readonly IIndex<FileDialogType, IFileDialog> fileDialogFactory;
        private readonly IOdbcImportSettingsFile importSettingsFile;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcImportMapSettingsControlViewModel"/> class.
        /// </summary>
        public OdbcImportMapSettingsControlViewModel(Func<string, IDialog> dialogFactory,
            IOdbcSampleDataCommand sampleDataCommand,
            IMessageHelper messageHelper,
            Func<string, IOdbcColumnSource> columnSourceFactory,
            IOdbcFieldMap fieldMap,
            IIndex<FileDialogType, IFileDialog> fileDialogFactory,
            IOdbcImportSettingsFile importSettingsFile) :
                base(messageHelper, columnSourceFactory)
        {
            this.dialogFactory = dialogFactory;
            this.sampleDataCommand = sampleDataCommand;
            this.fieldMap = fieldMap;
            this.fileDialogFactory = fileDialogFactory;
            this.importSettingsFile = importSettingsFile;
            OpenMapSettingsFileCommand = new RelayCommand(OpenMapSettingsFile);
            ExecuteQueryCommand = new RelayCommand(ExecuteQuery);
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
            get { return columnSourceIsTable; }
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
        /// Whether the download strategy is last modified.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool DownloadStrategyIsLastModified
        {
            get { return downloadStrategyIsLastModified; }
            set { Handler.Set(nameof(DownloadStrategyIsLastModified), ref downloadStrategyIsLastModified, value); }
        }

        /// <summary>
        /// Loads the map from disk.
        /// </summary>
        private void OpenMapSettingsFile()
        {

            IFileDialog fileDialog = fileDialogFactory[FileDialogType.Open];
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
                    DownloadStrategyIsLastModified = importSettingsFile.OdbcImportStrategy ==
                                                     OdbcImportStrategy.ByModifiedTime;
                    ColumnSourceIsTable = importSettingsFile.ColumnSourceType == OdbcColumnSourceType.Table;
                    LoadAndSetColumnSource(importSettingsFile.ColumnSource);
                    MapName = importSettingsFile.OdbcFieldMap.Name;

                    fieldMap = importSettingsFile.OdbcFieldMap;
                }
            }
        }

        /// <summary>
        /// Saves the map settings.
        /// </summary>
        public override void SaveMapSettings(OdbcStoreEntity store)
        {
            store.ImportStrategy = DownloadStrategyIsLastModified ?
                (int) OdbcImportStrategy.ByModifiedTime :
                (int) OdbcImportStrategy.All;

            store.ImportColumnSourceType = ColumnSourceIsTable ?
                (int) OdbcColumnSourceType.Table :
                (int) OdbcColumnSourceType.CustomQuery;

            store.ImportColumnSource = ColumnSourceIsTable ?
                SelectedTable?.Name :
                CustomQuery;

            fieldMap.Name = MapName;
            store.ImportMap = fieldMap.Serialize();
        }

        /// <summary>
        /// Validates the required map settings.
        /// </summary>
        public override bool ValidateRequiredMapSettings()
        {
            if (!base.ValidateRequiredMapSettings())
            {
                return false;
            }

            if (!ColumnSourceIsTable)
            {
                ExecuteQuery();

                if (!isQueryValid)
                {
                    MessageHelper.ShowError("Please enter a valid query before continuing to the next page.");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Loads the map settings.
        /// </summary>
        public override void LoadMapSettings(OdbcStoreEntity store)
        {
            fieldMap.Load(store.ImportMap);
            MapName = fieldMap.Name;

            DownloadStrategyIsLastModified = store.ImportStrategy == (int)OdbcImportStrategy.ByModifiedTime;

            ColumnSourceIsTable = store.ImportColumnSourceType == (int)OdbcColumnSourceType.Table;
        }

        /// <summary>
        /// The column source name to use for custom query
        /// </summary>
        public override string CustomQueryColumnSourceName => "Custom Import";

        /// <summary>
        /// Executes the query.
        /// </summary>
        private void ExecuteQuery()
        {
            QueryResults = null;
            ResultMessage = string.Empty;

            try
            {
                QueryResults = sampleDataCommand.Execute(DataSource, CustomQuery, NumberOfSampleResults);

                if (QueryResults.Rows.Count == 0)
                {
                    ResultMessage = "Query returned no results";
                }

                isQueryValid = true;
            }
            catch (ShipWorksOdbcException ex)
            {
                MessageHelper.ShowError(ex.Message);
                isQueryValid = false;
            }
        }
    }
}
