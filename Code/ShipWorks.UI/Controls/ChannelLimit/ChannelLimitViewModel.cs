using GalaSoft.MvvmLight.Command;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Core.UI;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Interapptive.Shared.Utility;
using ShipWorks.Users.Audit;
using ShipWorks.Stores.Management;
using System.Windows.Forms;

namespace ShipWorks.UI.Controls.ChannelLimit
{
    /// <summary>
    /// ViewModel for the ChannelLimitDlg
    /// </summary>
    public class ChannelLimitViewModel
    {
        private readonly PropertyChangedHandler handler;
        public event PropertyChangedEventHandler PropertyChanged;
        private ActiveStore selectedStore;
        private ObservableCollection<ActiveStore> storeCollection;
        private readonly ICustomerLicense license;
        private readonly ITangoWebClient tangoWebClient;
        private string errorMessage;
        private IStoreManager storeManager;
        private IDeletionService deletionService;
        private IWin32Window owner;

        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelLimitViewModel(ILicenseService licenseService, ITangoWebClient tangoWebClient, IStoreManager storeManager, IDeletionService deletionService, IWin32Window owner)
        {
            license = licenseService.GetLicenses().FirstOrDefault() as ICustomerLicense;

            // Check to make sure we are getting a CustomerLicense
            if (license == null)
            {
                throw new ShipWorksLicenseException("Store licenses do not have channel limits.");
            }

            this.tangoWebClient = tangoWebClient;
            this.storeManager = storeManager;
            this.deletionService = deletionService;
            this.owner = owner;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
            
            // Wire up the delete store click to the delete store method
            DeleteStoreClickCommand = new RelayCommand(DeleteStore, CanExecuteDeleteStore);
        }

        /// <summary>
        /// Loads the list of active stores
        /// </summary>
        public void Load()
        {
            license.Refresh();

            storeCollection = new ObservableCollection<ActiveStore>(license.GetActiveStores());

            UpdateErrorMessate();
        }

        /// <summary>
        /// Called when the Control is dismissed
        /// </summary>
        public void Dismiss()
        {
            license.Refresh();
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
        /// Updates the error message to display to the user
        /// </summary>
        private void UpdateErrorMessate()
        {
            int numberToDelete = license.LicenseCapabilities.ActiveChannels - license.LicenseCapabilities.ChannelLimit;

            if (numberToDelete > 0)
            {

                ErrorMessage = $"You have exceeded your channel limit. Please upgrade your plan or delete {numberToDelete} store types to continue using ShipWorks.";
            }            
        }

        /// <summary>
        /// Delete the selected store
        /// </summary>
        private void DeleteStore()
        {
            // Grab the local store entity that matches the license of the selected active store
            StoreEntity store = storeManager.GetAllStores().Where(s => s.License == selectedStore.StoreLicenseKey).FirstOrDefault();

            if (store != null)
            {
                DeleteStoreEntity(store);
            }

            // Remove the store form tango 
            tangoWebClient.DeleteStore(license, selectedStore.StoreLicenseKey);

            license.Refresh();

            UpdateErrorMessate();
        }

        /// <summary>
        /// Deletes the store entity
        /// </summary>
        /// <param name="store"></param>
        private void DeleteStoreEntity(StoreEntity store)
        {
            MethodConditions.EnsureArgumentIsNotNull(store, nameof(store));
            using (StoreConfirmDeleteDlg dlg = new StoreConfirmDeleteDlg(store.StoreName))
            {
                if (dlg.ShowDialog(owner) == DialogResult.OK)
                {
                    using (AuditBehaviorScope auditScope = new AuditBehaviorScope(AuditState.Disabled))
                    {
                        deletionService.DeleteStore(store);
                    }
                }
            }
        }
    }
}
