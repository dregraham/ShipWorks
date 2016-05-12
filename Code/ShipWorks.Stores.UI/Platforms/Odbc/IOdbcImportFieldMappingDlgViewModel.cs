using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.Mapping;

namespace ShipWorks.Stores.UI.Platforms.Odbc
{
    public interface IOdbcImportFieldMappingDlgViewModel
    {
        OdbcFieldMap AddressFieldMap { get; set; }
        IEnumerable<OdbcColumn> Columns { get; set; }
        IEnumerable<OdbcFieldMap> FieldMaps { get; set; }
        OdbcFieldMap ItemFieldMap { get; set; }
        ICommand LoadMapCommand { get; set; }
        string MapName { get; set; }
        OdbcFieldMap OrderFieldMap { get; set; }
        ICommand SaveMapCommand { get; set; }
        OdbcFieldMap SelectedFieldMap { get; set; }
        OdbcTable SelectedTable { get; set; }
        IEnumerable<OdbcTable> Tables { get; set; }

        event PropertyChangedEventHandler PropertyChanged;

        void Load(List<OdbcTable> tables);
        void LoadStore(OdbcStoreEntity store);
    }
}