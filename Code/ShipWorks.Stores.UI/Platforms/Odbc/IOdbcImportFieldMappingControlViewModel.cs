using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows.Input;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;

namespace ShipWorks.Stores.UI.Platforms.Odbc
{
    /// <summary>
    /// View Model for the <see cref="WizardPages.OdbcImportFieldMappingControl" />
    /// </summary>
    public interface IOdbcImportFieldMappingControlViewModel
    {


        /// <summary>
        /// The columns from the selected external odbc table.
        /// </summary>
        ObservableCollection<OdbcColumn> Columns { get; set; }

        /// <summary>
        /// The selected field map.
        /// </summary>
        OdbcFieldMapDisplay SelectedFieldMap { get; set; }

        /// <summary>
        /// Save the Map to the given store.
        /// </summary>
        void Save(OdbcStoreEntity store);

        /// <summary>
        /// Checks the required fields have value.
        /// </summary>
        bool ValidateRequiredMappingFields();



        void LoadColumns(bool IsTableSelected, IOdbcColumnSource ColumnSource, IOdbcDataSource DataSource,
            IOdbcColumnSource CustomQueryColumnSource);

    }
}