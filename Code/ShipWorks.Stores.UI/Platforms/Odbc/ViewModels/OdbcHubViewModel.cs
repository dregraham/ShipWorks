using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;

namespace ShipWorks.Stores.UI.Platforms.Odbc.ViewModels
{
    public class OdbcHubViewModel : ViewModelBase
    {
        public ObservableCollection<Store> Stores { get; set; }

        public Store SelectedStore
        {
            get { throw new System.NotImplementedException(); }
        }

        public ICommand NewStoreCommand
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}
