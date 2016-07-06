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
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.DataAccess;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;

namespace ShipWorks.Stores.UI.Platforms.Odbc
{
    public class OdbcMapSettingsControlViewModel : IOdbcMapSettingsControlViewModel, INotifyPropertyChanged
    {
        private const string CustomQueryColumnSourceName = "Custom Import";
        private readonly IOdbcSchema schema;
        private readonly IOdbcSampleDataCommand sampleDataCommand;
        private string mapName = string.Empty;
        private IOdbcColumnSource selectedTable;
        private bool isTableSelected = true;
        private bool isDownloadStrategyLastModified = true;
        private IEnumerable<IOdbcColumnSource> columnSources;
        private DataTable queryResults;
        private string resultMessage;
        private const int NumberOfSampleResults = 25;
        private IOdbcColumnSource columnSource;
        private IOdbcColumnSource customQueryColumnSource;
        private readonly ILog log;
        private readonly IMessageHelper messageHelper;

        private readonly PropertyChangedHandler handler;
        public event PropertyChangedEventHandler PropertyChanged;

        public OdbcMapSettingsControlViewModel(IOdbcSchema schema, IOdbcSampleDataCommand sampleDataCommand, Func<Type, ILog> logFactory,
            IMessageHelper messageHelper)
        {
            this.schema = schema;
            this.sampleDataCommand = sampleDataCommand;
            this.messageHelper = messageHelper;
            ExecuteQueryCommand = new RelayCommand(ExecuteQuery);
            customQueryColumnSource = new OdbcColumnSource(CustomQueryColumnSourceName);
            log = logFactory(typeof(OdbcImportFieldMappingControlViewModel));
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

        [Obfuscation(Exclude = true)]
        public IOdbcColumnSource CustomQueryColumnSource
        {
            get { return customQueryColumnSource; }
            set { handler.Set(nameof(CustomQueryColumnSource), ref customQueryColumnSource, value); }
        }

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
        /// Whether the column source selected is table
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool IsTableSelected
        {
            get { return isTableSelected; }
            set
            {
                handler.Set(nameof(IsTableSelected), ref isTableSelected, value);

                ColumnSource = value ? SelectedTable : CustomQueryColumnSource;
            }
        }

        /// <summary>
        /// Whether the download strategy is last modified.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool IsDownloadStrategyLastModified
        {
            get { return isDownloadStrategyLastModified; }
            set { handler.Set(nameof(IsDownloadStrategyLastModified), ref isDownloadStrategyLastModified, value); }
        }

        [Obfuscation(Exclude = true)]
        public ICommand ExecuteQueryCommand { get; set; }

        [Obfuscation(Exclude = true)]
        public DataTable QueryResults
        {
            get { return queryResults; }
            set { handler.Set(nameof(QueryResults), ref queryResults, value); }
        }

        [Obfuscation(Exclude = true)]
        public string ResultMessage
        {
            get { return resultMessage; }
            set { handler.Set(nameof(ResultMessage), ref resultMessage, value); }
        }

        public bool IsQueryValid { get; set; }


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

        public bool ValidateRequiredMapSettings()
        {
            if (IsTableSelected && SelectedTable == null)
            {
                messageHelper.ShowError("Please select a table before continuing to the next page.");
                return false;
            }

            if (!IsTableSelected && string.IsNullOrWhiteSpace(CustomQueryColumnSource.Query))
            {
                messageHelper.ShowError("Please enter a valid query before continuing to the next page.");
                return false;
            }

            if (!IsTableSelected)
            {
                ExecuteQuery();

                if (!IsQueryValid)
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
                QueryResults = sampleDataCommand.Execute(DataSource, CustomQueryColumnSource.Query, NumberOfSampleResults);
                if (QueryResults.Rows.Count == 0)
                {
                    ResultMessage = "Query returned no results";
                }
                IsQueryValid = true;
            }
            catch (ShipWorksOdbcException ex)
            {
                log.Error(ex.Message);
                messageHelper.ShowError(ex.Message);
                IsQueryValid = false;
            }
        }

    }
}
