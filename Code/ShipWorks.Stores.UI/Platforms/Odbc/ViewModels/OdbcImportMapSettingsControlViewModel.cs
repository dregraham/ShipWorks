using System;
using System.ComponentModel;
using System.Reflection;
using GalaSoft.MvvmLight.Command;
using Interapptive.Shared.UI;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.DataAccess;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Download;

namespace ShipWorks.Stores.UI.Platforms.Odbc.ViewModels
{
    /// <summary>
    /// ViewModel for OdbcMapSettingsControl
    /// </summary>
    public class OdbcImportMapSettingsControlViewModel : OdbcMapSettingsControlViewModel, INotifyPropertyChanged
    {
        private bool columnSourceIsTable = true;
        private bool downloadStrategyIsLastModified = true;
        private readonly Func<string, IDialog> dialogFactory;
        private readonly IOdbcSampleDataCommand sampleDataCommand;
        private const int NumberOfSampleResults = 25;
        private bool isQueryValid;
        private readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcImportMapSettingsControlViewModel"/> class.
        /// </summary>
        public OdbcImportMapSettingsControlViewModel(Func<string, IDialog> dialogFactory, IOdbcSampleDataCommand sampleDataCommand,
            Func<Type, ILog> logFactory, IMessageHelper messageHelper, Func<string, IOdbcColumnSource> columnSourceFactory) :
            base(messageHelper, columnSourceFactory)
        {
            this.dialogFactory = dialogFactory;
            this.sampleDataCommand = sampleDataCommand;
            log = logFactory(typeof(OdbcImportFieldMappingControlViewModel));
            ExecuteQueryCommand = new RelayCommand(ExecuteQuery);
        }

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
        /// Saves the map settings.
        /// </summary>
        public override void SaveMapSettings(OdbcStoreEntity store)
        {
            store.ImportStrategy = DownloadStrategyIsLastModified ?
                (int) OdbcImportStrategy.ByModifiedTime :
                (int) OdbcImportStrategy.All;

            store.ImportSourceType = ColumnSourceIsTable ?
                (int) OdbcColumnSourceType.Table :
                (int) OdbcColumnSourceType.CustomQuery;

            store.ImportColumnSource = ColumnSourceIsTable ?
                SelectedTable.Name :
                CustomQuery;
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
                log.Error(ex.Message);
                MessageHelper.ShowError(ex.Message);
                isQueryValid = false;
            }
        }
    }
}
