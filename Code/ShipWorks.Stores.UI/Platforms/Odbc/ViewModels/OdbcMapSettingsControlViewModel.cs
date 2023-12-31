﻿using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows.Input;

namespace ShipWorks.Stores.UI.Platforms.Odbc.ViewModels
{
    /// <summary>
    /// Base class for mapping ODBC
    /// </summary>
    public abstract class OdbcMapSettingsControlViewModel : INotifyPropertyChanged, IOdbcMapSettingsControlViewModel
    {
        private string mapName = string.Empty;
        private IOdbcColumnSource selectedTable;
        private IEnumerable<IOdbcColumnSource> tables;
        private IOdbcColumnSource columnSource;
        private IOdbcColumnSource customQueryColumnSource;

        public event PropertyChangedEventHandler PropertyChanged;
        protected readonly PropertyChangedHandler Handler;
        protected readonly IMessageHelper messageHelper;
        private readonly Func<string, IOdbcColumnSource> columnSourceFactory;
        private IOdbcSchema schema;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcMapSettingsControlViewModel"/> class.
        /// </summary>
        protected OdbcMapSettingsControlViewModel(IMessageHelper messageHelper, Func<string, IOdbcColumnSource> columnSourceFactory)
        {
            this.messageHelper = messageHelper;
            this.columnSourceFactory = columnSourceFactory;

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
            set => Handler.Set(nameof(MapName), ref mapName, value);
        }

        /// <summary>
        /// The external odbc tables.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IEnumerable<IOdbcColumnSource> Tables
        {
            get => tables;
            set => Handler.Set(nameof(Tables), ref tables, value);
        }

        /// <summary>
        /// The selected external odbc table.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IOdbcColumnSource SelectedTable
        {
            get => selectedTable;
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
            get =>
                customQueryColumnSource ??
                (customQueryColumnSource = columnSourceFactory(CustomQueryColumnSourceName));
            set => Handler.Set(nameof(CustomQueryColumnSource), ref customQueryColumnSource, value);
        }

        /// <summary>
        /// The column source that will be used.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IOdbcColumnSource ColumnSource
        {
            get => columnSource;
            set
            {
                // Set map name for the user, if they have not altered it.
                // Starts by setting map name to selected data source name.
                // When a table is selected, if map name is untouched by user,
                // the map name is changed to "DataSourceName - SelectedColumnName"
                if (DataSource.Name != null &&
                    (MapName.Equals(DataSource.Name, StringComparison.InvariantCulture) ||
                     MapName.Equals($"{DataSource.Name} - {ColumnSource?.Name}", StringComparison.InvariantCulture)))
                {
                    MapName = value == null ? $"{DataSource.Name}" : $"{DataSource.Name} - {value.Name}";
                }

                Handler.Set(nameof(ColumnSource), ref columnSource, value);
            }
        }
        
        /// <summary>
        /// Whether the column source selected is table
        /// </summary>
        [Obfuscation(Exclude = true)]
        public abstract bool ColumnSourceIsTable { get; set; }

        /// <summary>
        /// Validates the required map settings.
        /// </summary>
        public virtual bool ValidateRequiredMapSettings()
        {
            if (ColumnSourceIsTable && SelectedTable == null)
            {
                messageHelper.ShowError("Please select a table before continuing to the next page.");
                return false;
            }

            if (ColumnSourceIsTable)
            {
                schema.Load(DataSource);
                IEnumerable<IOdbcColumnSource> currentTables = schema.Tables;

                if (currentTables.All(t => t.Name != SelectedTable.Name))
                {
                    messageHelper.ShowError("The selected table does not exist in the current data source");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Loads the external odbc tables.
        /// </summary>
        /// <param name="dataSource">ODBC Datasource</param>
        /// <param name="odbcSchema">Object to retrieve the list of column sources (tables). </param>
        /// <param name="columnSourceFromStore">Displays current datasource along with the one from the store.</param>
        /// <param name="store">Odbc Store Entity</param>
        public void Load(IOdbcDataSource dataSource, IOdbcSchema odbcSchema, string columnSourceFromStore, OdbcStoreEntity store)
        {
            MethodConditions.EnsureArgumentIsNotNull(dataSource, nameof(dataSource));
            MethodConditions.EnsureArgumentIsNotNull(odbcSchema, nameof(odbcSchema));

            try
            {
                odbcSchema.Load(dataSource);
            }
            catch (ShipWorksOdbcException ex)
            {
                messageHelper.ShowError(ex.Message);
            }

            DataSource = dataSource;
            schema = odbcSchema;

            LoadMapSettings(store);

            LoadAndSetColumnSource(columnSourceFromStore);
        }

        /// <summary>
        /// Populates ColumnSource and sets the selected table
        /// </summary>
        /// <param name="columnSourceFromStore">The column source from store.</param>
        protected void LoadAndSetColumnSource(string columnSourceFromStore)
        {
            try
            {
                Tables = schema.Tables as IList<IOdbcColumnSource> ?? schema.Tables.ToList();
            }
            catch (Exception)
            {
                messageHelper.ShowInformation("ShipWorks was unable to retrieve a list of tables from your ODBC source, you will have to write a query to map columns.");
                ColumnSourceIsTable = false;
            }
            
            if (ColumnSourceIsTable && !string.IsNullOrWhiteSpace(columnSourceFromStore))
            {
                IOdbcColumnSource loadedColumnSource = columnSourceFactory(columnSourceFromStore);
                IOdbcColumnSource table = Tables.FirstOrDefault(t => t.Name == loadedColumnSource.Name);

                if (table != null)
                {
                    SelectedTable = table;
                }
                else
                {
                    Tables = Tables.Concat(new[] { loadedColumnSource });
                    SelectedTable = loadedColumnSource;
                }
            }
        }

        /// <summary>
        /// Saves the map settings.
        /// </summary>
        public abstract void SaveMapSettings(OdbcStoreEntity store);

        /// <summary>
        /// Loads the map settings.
        /// </summary>
        public abstract void LoadMapSettings(OdbcStoreEntity store);

        /// <summary>
        /// The column source name to use for custom query
        /// </summary>
        public abstract string CustomQueryColumnSourceName { get; }
    }
}