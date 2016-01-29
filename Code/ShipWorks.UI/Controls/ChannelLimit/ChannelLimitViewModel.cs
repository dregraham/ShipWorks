using GalaSoft.MvvmLight.Command;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Core.UI;
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
using log4net;

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
        private string errorMessage;
        private readonly IStoreManager storeManager;
        private readonly Func<IChannelConfirmDeleteFactory> confirmDeleteFactory;
        private readonly ILog log;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelLimitViewModel(
            ILicenseService licenseService, 
            IStoreManager storeManager,
            Func<IChannelConfirmDeleteFactory> confirmDeleteFactory,
            Func<Type, ILog> logFactory)
        {
            license = licenseService.GetLicenses().FirstOrDefault() as ICustomerLicense;

            // Check to make sure we are getting a CustomerLicense
            if (license == null)
            {
                throw new ShipWorksLicenseException("Store licenses do not have channel limits.");
            }

            this.storeManager = storeManager;
            this.confirmDeleteFactory = confirmDeleteFactory;

            handler = new PropertyChangedHandler(this, () => PropertyChanged);
            
            // Wire up the delete store click to the delete store method
            DeleteStoreClickCommand = new RelayCommand(DeleteChannel, CanExecuteDeleteStore);

            // Set the selected store type to invalid
            SelectedStoreType = StoreTypeCode.Invalid;

            log = logFactory(typeof(ChannelLimitViewModel));
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
            ErrorMessage =
                $"You have exceeded your channel limit. Please upgrade your plan or delete {license.NumberOfChannelsOverLimit} channel(s) to continue using ShipWorks.";
        }

        /// <summary>
        /// Delete the selected store
        /// </summary>
        private void DeleteChannel()
        {
            List<StoreTypeCode> localStoreTypeCodes = storeManager.GetAllStores().Select(s => (StoreTypeCode)s.TypeCode).Distinct().ToList();

            // If we are trying to delete the only store type in ShipWorks display an error and dont delete
            if (localStoreTypeCodes.Count == 1 && localStoreTypeCodes.Contains(SelectedStoreType))
            {
                ErrorMessage += $" \n \nYou cannot remove {EnumHelper.GetDescription(selectedStoreType)} because it is the only channel in your ShipWorks database.";
                return;
            }

            IChannelConfirmDeleteDlg deleteDlg = confirmDeleteFactory().GetConfirmDeleteDlg(selectedStoreType);

            deleteDlg.ShowDialog();

            if (deleteDlg.DialogResult == true)
            {
                using (new AuditBehaviorScope(AuditState.Disabled))
                {
                    // Delete the channel
                    try
                    {
                        license.DeleteChannel(selectedStoreType);
                    }
                    catch (ShipWorksLicenseException ex)
                    {
                        log.Error("Error deleting channel", ex);
                        ErrorMessage += "\n\nError deleting Channel. Please try again.";
                    }
                }
            }

            try
            {
                // call load to refresh everything
                Load();
            }
            catch (ShipWorksLicenseException ex)
            {
                log.Error("Error getting channels to reload dialog", ex);
                ErrorMessage += "\n\nError getting channels from server.";
            }
        }
    }
}
