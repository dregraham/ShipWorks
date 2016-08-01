using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;

namespace ShipWorks.Stores.UI.Platforms.Odbc.ViewModels
{
    /// <summary>
    /// View Model for the <see cref="WizardPages.OdbcImportFieldMappingControl" />
    /// </summary>
    public interface IOdbcImportFieldMappingControlViewModel
    {
        /// <summary>
        /// Loads the specified store.
        /// </summary>
        void Load(OdbcStoreEntity store);
     
        /// <summary>
        /// Save the Map to the given store.
        /// </summary>
        void Save(OdbcStoreEntity store);

        /// <summary>
        /// Checks the required fields have value.
        /// </summary>
        bool ValidateRequiredMappingFields();

        /// <summary>
        /// The column source.
        /// </summary>
        IOdbcColumnSource ColumnSource { get; }
    }
}