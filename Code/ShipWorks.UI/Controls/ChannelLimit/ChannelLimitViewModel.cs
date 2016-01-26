using GalaSoft.MvvmLight.Command;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Core.UI;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.UI.Controls.ChannelLimit
{
    /// <summary>
    /// ViewModel for the ChannelLimitDlg
    /// </summary>
    class ChannelLimitViewModel
    {
        private readonly PropertyChangedHandler handler;
        public event PropertyChangedEventHandler PropertyChanged;
        private ActiveStore selectedStore;
        private ObservableCollection<ActiveStore> storeCollection;
        private readonly ICustomerLicense license;
        private readonly ITangoWebClient tangoWebClient;
        private string errorMessage;

        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelLimitViewModel(ICustomerLicense license, ITangoWebClient tangoWebClient)
        {
            this.license = license;
            this.tangoWebClient = tangoWebClient;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            // Check to make sure we are getting a CustomerLicense
            if (license.IsLegacy)
            {
                throw new ShipWorksLicenseException("Store licenses do not have channel limits.");
            }

            // Wire up the delete store click to the delete store method
            DeleteStoreClickCommand = new RelayCommand(DeleteStore, CanExecuteDeleteStore);
        }

        /// <summary>
        /// Loads the list of active stores
        /// </summary>
        public void Load()
        {
            storeCollection = new ObservableCollection<ActiveStore>(license.GetActiveStores());

            ErrorMessage = "";
        }

        /// <summary>
        /// The selected store
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { handler.Set(nameof(ErrorMessage), ref errorMessage, value); }
        }

        /// <summary>
        /// The selected store
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ActiveStore SelectedStore
        {
            get { return selectedStore; }
            set
            {
                handler.Set(nameof(SelectedStore), ref selectedStore, value);

                // Update the status of the delete button to ensure that its valid
                DeleteStoreClickCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Collection of stores
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ObservableCollection<ActiveStore> StoreCollection
        {
            get { return storeCollection; }
            set { handler.Set(nameof(StoreCollection), ref storeCollection, value); }
        }

        /// <summary>
        /// True if a store is selected
        /// </summary>
        [Obfuscation(Exclude = true)]
        private bool CanExecuteDeleteStore() => selectedStore != null;

        /// <summary>
        /// Delete Store ClickCommand
        /// </summary>
        [Obfuscation(Exclude = true)]
        public RelayCommand DeleteStoreClickCommand { get; }


        /// <summary>
        /// Delete the selected store
        /// </summary>
        public void DeleteStore()
        {
        }
    }
}
