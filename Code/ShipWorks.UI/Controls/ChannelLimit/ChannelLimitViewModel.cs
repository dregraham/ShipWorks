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
using ShipWorks.Users.Audit;
using ShipWorks.UI.Controls.ChannelConfirmDelete;
using System.Collections.Generic;
using System;
using Interapptive.Shared.Utility;

namespace ShipWorks.UI.Controls.ChannelLimit
{
    /// <summary>
    /// ViewModel for the ChannelLimitDlg
    /// </summary>
    public class ChannelLimitViewModel : INotifyPropertyChanged
    {
        private readonly PropertyChangedHandler handler;
        public event PropertyChangedEventHandler PropertyChanged;
        private StoreTypeCode selectedStoreType;
        private ObservableCollection<StoreTypeCode> channelCollection;
        private readonly ICustomerLicense license;
        private readonly ITangoWebClient tangoWebClient;
        private string errorMessage;
        private IStoreManager storeManager;
        private IDeletionService deletionService;
        private Func<IChannelConfirmDeleteFactory> confirmDeleteFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelLimitViewModel(
            ILicenseService licenseService, 
            ITangoWebClient tangoWebClient, 
            IStoreManager storeManager, 
            IDeletionService deletionService,
            Func<IChannelConfirmDeleteFactory> confirmDeleteFactory)
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
            this.confirmDeleteFactory = confirmDeleteFactory;

            handler = new PropertyChangedHandler(this, () => PropertyChanged);
            
            // Wire up the delete store click to the delete store method
            DeleteStoreClickCommand = new RelayCommand(DeleteChannel, CanExecuteDeleteStore);

            // Set the selected store type to invalid
            SelectedStoreType = StoreTypeCode.Invalid;
        }

        /// <summary>
        /// Loads the list of active stores
        /// </summary>
        public void Load()
        {
            SelectedStoreType = StoreTypeCode.Invalid;

            license.Refresh();

            // if we dont have a store collection make one
            if (ChannelCollection == null)
            {
                ChannelCollection = new ObservableCollection<StoreTypeCode>();
            }

            // clear the collection
            ChannelCollection.Clear();

            // load the collection with the licenses active stores
            license.GetActiveStores().ToList().ForEach(s => ChannelCollection.Add(new ShipWorksLicense(s.StoreLicenseKey).StoreTypeCode));

            foreach (StoreEntity store in storeManager.GetAllStores())
            {
                // if we did not find a match add it to the collection 
                if (!ChannelCollection.Contains((StoreTypeCode)store.TypeCode))
                {
                    ChannelCollection.Add((StoreTypeCode)store.TypeCode);
                }
            }

            UpdateErrorMesssage();
        }

        /// <summary>
        /// Called when the Control is dismissed
        /// </summary>
        public void Dismiss()
        {
            license.Refresh();
        }

        /// <summary>
        /// The error message displayed to the user
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
        public StoreTypeCode SelectedStoreType
        {
            get { return selectedStoreType; }
            set
            {
                handler.Set(nameof(SelectedStoreType), ref selectedStoreType, value);

                // Update the status of the delete button to ensure that its valid
                DeleteStoreClickCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Collection of stores
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ObservableCollection<StoreTypeCode> ChannelCollection
        {
            get { return channelCollection; }
            set { handler.Set(nameof(ChannelCollection), ref channelCollection, value); }
        }

        /// <summary>
        /// True if a store is selected
        /// </summary>
        [Obfuscation(Exclude = true)]
        private bool CanExecuteDeleteStore() => SelectedStoreType != StoreTypeCode.Invalid;

        /// <summary>
        /// Delete Store ClickCommand
        /// </summary>
        [Obfuscation(Exclude = true)]
        public RelayCommand DeleteStoreClickCommand { get; }

        /// <summary>
        /// Updates the error message to display to the user
        /// </summary>
        private void UpdateErrorMesssage()
        {
            int numberToDelete = license.LicenseCapabilities.ActiveChannels - license.LicenseCapabilities.ChannelLimit;

            if (numberToDelete == 1)
            {
                ErrorMessage = $"You have exceeded your channel limit. Please upgrade your plan or delete 1 channel to continue using ShipWorks.";
            }
            else if (numberToDelete >= 2)
            {
                ErrorMessage = $"You have exceeded your channel limit. Please upgrade your plan or delete {numberToDelete} channels to continue using ShipWorks.";
            }     
        }

        /// <summary>
        /// Delete the selected store
        /// </summary>
        private void DeleteChannel()
        {
            IEnumerable<StoreTypeCode> localStoreTypeCodes = storeManager.GetAllStores().Select(s => (StoreTypeCode)s.TypeCode).Distinct();

            // If we are trying to delete the only store type in ShipWorks display an error and dont delete
            if (localStoreTypeCodes.Count() == 1 && localStoreTypeCodes.Contains(SelectedStoreType))
            {
                ErrorMessage = ErrorMessage + $" \n \nYou cannot remove {EnumHelper.GetDescription(selectedStoreType)} because it is the only channel in your ShipWorks database.";

                return;
            }

            IChannelConfirmDeleteDlg deleteDlg = confirmDeleteFactory().GetConfirmDeleteDlg(selectedStoreType);

            deleteDlg.ShowDialog();

            if (deleteDlg.DialogResult == true)
            {
                using (AuditBehaviorScope auditScope = new AuditBehaviorScope(AuditState.Disabled))
                {
                    // Get all of the stores that match the type we want to remove
                    IEnumerable<StoreEntity> localStoresToDelete = storeManager.GetAllStores().Where(s => s.TypeCode == (int)selectedStoreType);
                    
                    // Get a list of licenses we are about to delete
                    List<string> licensesToDelete = new List<string>();
                    localStoresToDelete.ToList().ForEach(s => licensesToDelete.Add(s.License));

                    // remove the local stores individually 
                    localStoresToDelete.ToList().ForEach(deletionService.DeleteStore);
                    
                    // tell tango to delete those active stores
                    licensesToDelete.ForEach(l => tangoWebClient.DeleteStore(license, l));
                }
            }

            // call load to refresh everything
            Load();
        }
    }
}
