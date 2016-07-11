﻿using GalaSoft.MvvmLight.Command;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.DataAccess;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Download;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows.Input;

namespace ShipWorks.Stores.UI.Platforms.Odbc
{
    /// <summary>
    /// ViewModel for OdbcMapSettingsControl
    /// </summary>
    public class OdbcMapSettingsControlViewModel : IOdbcMapSettingsControlViewModel, INotifyPropertyChanged
    {
        private const string CustomQueryColumnSourceName = "Custom Import";
        private readonly IOdbcSchema schema;
        private readonly IOdbcSampleDataCommand sampleDataCommand;
        private string mapName = string.Empty;
        private IOdbcColumnSource selectedTable;
        private bool columnSourceIsTable = true;
        private bool downloadStrategyIsLastModified = true;
        private IEnumerable<IOdbcColumnSource> columnSources;
        private DataTable queryResults;
        private string resultMessage;
        private const int NumberOfSampleResults = 25;
        private IOdbcColumnSource columnSource;
        private IOdbcColumnSource customQueryColumnSource;
        private readonly ILog log;
        private readonly IMessageHelper messageHelper;
        private readonly Func<string, IDialog> dialogFactory;
        private bool isQueryValid;
        private readonly PropertyChangedHandler handler;
        private string customQuery;
        private int downloadDaysBack = 30;
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcMapSettingsControlViewModel"/> class.
        /// </summary>
        public OdbcMapSettingsControlViewModel(IOdbcSchema schema, IOdbcSampleDataCommand sampleDataCommand, Func<Type, ILog> logFactory,
            IMessageHelper messageHelper, Func<string, IDialog> dialogFactory, Func<string, IOdbcColumnSource> columnSourceFactory)
        {
            this.schema = schema;
            this.sampleDataCommand = sampleDataCommand;
            this.messageHelper = messageHelper;
            this.dialogFactory = dialogFactory;
            ExecuteQueryCommand = new RelayCommand(ExecuteQuery);
            customQueryColumnSource = columnSourceFactory(CustomQueryColumnSourceName);
            log = logFactory(typeof(OdbcImportFieldMappingControlViewModel));

            NumbersUpTo30 = Enumerable.Range(1, 30).ToList();

            handler = new PropertyChangedHandler(this, () => PropertyChanged);
        }

        /// <summary>
        /// Gets the data source.
        /// </summary>
        public IOdbcDataSource DataSource { get; private set; }

        /// <summary>
        /// The name the map will be saved as.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string MapName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(mapName))
                {
                    mapName = ColumnSource == null ? DataSource.Name : $"{DataSource.Name} - {ColumnSource.Name}";
                }

                return mapName;
            }
            set { handler.Set(nameof(MapName), ref mapName, value); }
        }

        /// <summary>
        /// The external odbc tables.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IEnumerable<IOdbcColumnSource> ColumnSources
        {
            get { return columnSources; }
            set { handler.Set(nameof(ColumnSources), ref columnSources, value); }
        }

        /// <summary>
        /// The selected external odbc table.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IOdbcColumnSource SelectedTable
        {
            get { return selectedTable; }
            set
            {
                handler.Set(nameof(SelectedTable), ref selectedTable, value);

                ColumnSource = SelectedTable;
            }
        }

        /// <summary>
        /// The custom query column source.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IOdbcColumnSource CustomQueryColumnSource
        {
            get { return customQueryColumnSource; }
            set { handler.Set(nameof(CustomQueryColumnSource), ref customQueryColumnSource, value); }
        }

        /// <summary>
        /// The column source that will be used.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IOdbcColumnSource ColumnSource
        {
            get { return columnSource; }
            set
            {
                // Set map name for the user, if they have not altered it.
                // Starts by setting map name to selected data source name.
                // When a table is selected, if map name is untouched by user,
                // the map name is changed to "DataSourceName - SelectedColumnName"
                if (MapName != null && DataSource.Name != null &&
                    (MapName.Equals(DataSource.Name, StringComparison.InvariantCulture) ||
                     MapName.Equals($"{DataSource.Name} - {ColumnSource.Name}", StringComparison.InvariantCulture)))
                {
                    MapName = value == null ? $"{DataSource.Name}" : $"{DataSource.Name} - {value.Name}";
                }

                handler.Set(nameof(ColumnSource), ref columnSource, value);
            }
        }

        /// <summary>
        /// the custom query
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string CustomQuery
        {
            get { return customQuery; }
            set { handler.Set(nameof(CustomQuery), ref customQuery, value); }
        }

        /// <summary>
        /// Whether the column source selected is table
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ColumnSourceIsTable
        {
            get { return columnSourceIsTable; }
            set
            {
                handler.Set(nameof(ColumnSourceIsTable), ref columnSourceIsTable, value);

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
            set { handler.Set(nameof(DownloadStrategyIsLastModified), ref downloadStrategyIsLastModified, value); }
        }

        /// <summary>
        /// Command that executes the query
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand ExecuteQueryCommand { get; set; }

        /// <summary>
        /// The query results.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public DataTable QueryResults
        {
            get { return queryResults; }
            set { handler.Set(nameof(QueryResults), ref queryResults, value); }
        }

        /// <summary>
        /// Message to indicate failed query execution or no results
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ResultMessage
        {
            get { return resultMessage; }
            set { handler.Set(nameof(ResultMessage), ref resultMessage, value); }
        }

        /// <summary>
        /// List of numbers 0-25 for binding to number of items and number of attributes lists
        /// </summary>
        [Obfuscation(Exclude = true)]
        public List<int> NumbersUpTo30 { get; }

        /// <summary>
        /// The number of days back to begin downloading
        /// </summary>
        [Obfuscation(Exclude = true)]
        public int DownloadDaysBack
        {
            get { return downloadDaysBack; }
            set { handler.Set(nameof(DownloadDaysBack), ref downloadDaysBack, value); }
        }

        /// <summary>
        /// Loads the external odbc tables.
        /// </summary>
        public void Load(IOdbcDataSource dataSource)
        {
            MethodConditions.EnsureArgumentIsNotNull(dataSource);

            try
            {
                DataSource = dataSource;

                schema.Load(DataSource);
                ColumnSources = schema.Tables;
            }
            catch (ShipWorksOdbcException ex)
            {
                messageHelper.ShowError(ex.Message);
            }
        }

        /// <summary>
        /// Saves the map settings.
        /// </summary>
        public void SaveMapSettings(OdbcStoreEntity store)
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

            if (DownloadStrategyIsLastModified)
            {
                store.InitialDownloadDays = DownloadDaysBack;
            }
        }

        /// <summary>
        /// Validates the required map settings.
        /// </summary>
        public bool ValidateRequiredMapSettings()
        {
            if (ColumnSourceIsTable && SelectedTable == null)
            {
                messageHelper.ShowError("Please select a table before continuing to the next page.");
                return false;
            }

            if (!ColumnSourceIsTable && string.IsNullOrWhiteSpace(CustomQuery))
            {
                messageHelper.ShowError("Please enter a valid query before continuing to the next page.");
                return false;
            }

            if (!ColumnSourceIsTable)
            {
                ExecuteQuery();

                if (!isQueryValid)
                {
                    messageHelper.ShowError("Please enter a valid query before continuing to the next page.");
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
                messageHelper.ShowError(ex.Message);
                isQueryValid = false;
            }
        }
    }
}
