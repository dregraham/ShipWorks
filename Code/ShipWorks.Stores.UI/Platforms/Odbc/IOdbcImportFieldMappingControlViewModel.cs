using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Download;

namespace ShipWorks.Stores.UI.Platforms.Odbc
{
    /// <summary>
    /// View Model for the <see cref="WizardPages.OdbcImportFieldMappingControl" />
    /// </summary>
    public interface IOdbcImportFieldMappingControlViewModel
    {
        /// <summary>
        /// Save the Map to the given store.
        /// </summary>
        void Save(OdbcStoreEntity store);

        /// <summary>
        /// Checks the required fields have value.
        /// </summary>
        bool ValidateRequiredMappingFields();

        /// <summary>
        /// Loads the column source.
        /// </summary>
        void LoadColumnSource(IOdbcColumnSource source, OdbcDownloadStrategy downloadStrategy);

        /// <summary>
        /// The column source.
        /// </summary>
        IOdbcColumnSource ColumnSource { get; }
    }
}