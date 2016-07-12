using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.DataAccess;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;

namespace ShipWorks.Stores.UI.Platforms.Odbc.ViewModels
{
    public abstract class OdbcMapSettingsControlViewModel : IOdbcMapSettingsControlViewModel
    {
        private const string CustomQueryColumnSourceName = "Custom Import";
        private readonly IOdbcSampleDataCommand sampleDataCommand;
        private string mapName = string.Empty;
        private IOdbcColumnSource selectedTable;
        private IEnumerable<IOdbcColumnSource> tables;
        private DataTable queryResults;
        private string resultMessage;
        private const int NumberOfSampleResults = 25;
        private IOdbcColumnSource columnSource;
        private IOdbcColumnSource customQueryColumnSource;
        private readonly ILog log;
        private readonly IMessageHelper messageHelper;
        private bool isQueryValid;
        private string customQuery;

        public event PropertyChangedEventHandler PropertyChanged;
        protected readonly PropertyChangedHandler Handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcImportMapSettingsControlViewModel"/> class.
        /// </summary>
        protected OdbcMapSettingsControlViewModel(IOdbcSampleDataCommand sampleDataCommand, Func<Type, ILog> logFactory,
            IMessageHelper messageHelper, Func<string, IOdbcColumnSource> columnSourceFactory)
        {
            this.sampleDataCommand = sampleDataCommand;
            this.messageHelper = messageHelper;
            ExecuteQueryCommand = new RelayCommand(ExecuteQuery);
            customQueryColumnSource = columnSourceFactory(CustomQueryColumnSourceName);
            log = logFactory(typeof(OdbcImportFieldMappingControlViewModel));

            Handler = new PropertyChangedHandler(this, () => PropertyChanged);
        }

        /// <summary>
        /// Gets the data source.
        /// </summary>
        public IOdbcDataSource DataSource { get; protected set; }

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
            set { Handler.Set(nameof(MapName), ref mapName, value); }
        }

        /// <summary>
        /// The external odbc tables.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IEnumerable<IOdbcColumnSource> Tables
        {
            get { return tables; }
            set { Handler.Set(nameof(Tables), ref tables, value); }
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
                Handler.Set(nameof(SelectedTable), ref selectedTable, value);

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
            set { Handler.Set(nameof(CustomQueryColumnSource), ref customQueryColumnSource, value); }
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

                Handler.Set(nameof(ColumnSource), ref columnSource, value);
            }
        }

        /// <summary>
        /// the custom query
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string CustomQuery
        {
            get { return customQuery; }
            set { Handler.Set(nameof(CustomQuery), ref customQuery, value); }
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
            set { Handler.Set(nameof(QueryResults), ref queryResults, value); }
        }

        /// <summary>
        /// Message to indicate failed query execution or no results
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ResultMessage
        {
            get { return resultMessage; }
            set { Handler.Set(nameof(ResultMessage), ref resultMessage, value); }
        }

        /// <summary>
        /// Whether the column source selected is table
        /// </summary>
        [Obfuscation(Exclude = true)]
        public abstract bool ColumnSourceIsTable { get; set; }

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

        /// <summary>
        /// Loads the external odbc tables.
        /// </summary>
        public void Load(IOdbcDataSource dataSource, IEnumerable<IOdbcColumnSource> externalTables)
        {
            MethodConditions.EnsureArgumentIsNotNull(dataSource);
            MethodConditions.EnsureArgumentIsNotNull(externalTables);

            DataSource = dataSource;
            Tables = externalTables;
        }

        /// <summary>
        /// Saves the map settings.
        /// </summary>
        public abstract void SaveMapSettings(OdbcStoreEntity store);
    }
}