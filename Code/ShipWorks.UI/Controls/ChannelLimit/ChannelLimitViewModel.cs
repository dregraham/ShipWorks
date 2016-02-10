using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using ShipWorks.Users.Audit;
using ShipWorks.UI.Controls.ChannelConfirmDelete;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Utility;
using ShipWorks.UI.Controls.WebBrowser;

namespace ShipWorks.UI.Controls.ChannelLimit
{
    /// <summary>
    /// ViewModel for the ChannelLimitDlg
    /// </summary>
    public partial class ChannelLimitViewModel : INotifyPropertyChanged
    {
        private readonly PropertyChangedHandler handler;
        public event PropertyChangedEventHandler PropertyChanged;
        private StoreTypeCode selectedStoreType;
        private ObservableCollection<StoreTypeCode> channelCollection;
        private readonly ICustomerLicense license;
        private string errorMessage;
        private readonly IStoreManager storeManager;
        private readonly IChannelConfirmDeleteFactory confirmDeleteFactory;
        private readonly WebBrowserFactory webBrowserFactory;
        private readonly IMessageHelper messagdHelper;
        private readonly ILog log;
        private bool isDeleting;

        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelLimitViewModel(
            ILicenseService licenseService, 
            IStoreManager storeManager,
            IChannelConfirmDeleteFactory confirmDeleteFactory, 
            Func<Type, ILog> logFactory,
            WebBrowserFactory webBrowserFactory,
            IMessageHelper messagdHelper)
        {
            license = licenseService.GetLicenses().FirstOrDefault() as ICustomerLicense;
            this.storeManager = storeManager;
            this.confirmDeleteFactory = confirmDeleteFactory;
            this.webBrowserFactory = webBrowserFactory;
            this.messagdHelper = messagdHelper;

            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            // Set the selected store type to invalid
            SelectedStoreType = StoreTypeCode.Invalid;

            log = logFactory(typeof (ChannelLimitViewModel));

            DeleteStoreClickCommand = new RelayCommand(DeleteChannel, CanExecuteDeleteStore);
        }

        /// <summary>
        /// Loads the list of active stores
        /// </summary>
        public void Load()
        {
            // Check to make sure we are getting a CustomerLicense
            if (license == null)
            {
                throw new ShipWorksLicenseException("Store licenses do not have channel limits.");
            }

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
            license.GetActiveStores()
                .ToList()
                .ForEach(s => ChannelCollection.Add(new ShipWorksLicense(s.StoreLicenseKey).StoreTypeCode));

            foreach (StoreEntity store in storeManager.GetAllStores())
            {
                // if we did not find a match add it to the collection 
                if (!ChannelCollection.Contains((StoreTypeCode) store.TypeCode))
                {
                    ChannelCollection.Add((StoreTypeCode) store.TypeCode);
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
        /// True if a store is selected
        /// </summary>
        private bool CanExecuteDeleteStore()
        {
            return SelectedStoreType != StoreTypeCode.Invalid;
        }

        /// <summary>
        /// Updates the error message to display to the user
        /// </summary>
        private void UpdateErrorMesssage()
        {
            string plural = license.NumberOfChannelsOverLimit > 1 ? "s" : string.Empty;
            ErrorMessage =
                $"You have exceeded your channel limit. Please upgrade your plan or delete {license.NumberOfChannelsOverLimit} channel{plural} to continue downloading orders and creating shipment labels.";
        }

        /// <summary>
        /// Upgrade the account
        /// </summary>
        private void UpgradeAccount()
        {
            Uri uri = new Uri("https://www.interapptive.com/account/changeplan.php");
            IDialog browserDlg = webBrowserFactory.Create(uri, "Upgrade your account");
            browserDlg.ShowDialog();
        }

        /// <summary>
        /// Delete the selected store
        /// </summary>
        private async void DeleteChannel()
        {
            IsDeleting = true;

            List<StoreTypeCode> localStoreTypeCodes =
                storeManager.GetAllStores().Select(s => (StoreTypeCode) s.TypeCode).Distinct().ToList();

            // If we are trying to delete the only store type in ShipWorks display an error and dont delete
            if (localStoreTypeCodes.Count == 1 && localStoreTypeCodes.Contains(SelectedStoreType))
            {
                messagdHelper.ShowError($"You cannot remove {EnumHelper.GetDescription(selectedStoreType)} because it is the only channel in your ShipWorks database.");
                return;
            }

            IsDeleting = true;

            IChannelConfirmDeleteDlg deleteDlg = confirmDeleteFactory.GetConfirmDeleteDlg(selectedStoreType);

            deleteDlg.ShowDialog();

            if (deleteDlg.DialogResult == true)
            {
                using (new AuditBehaviorScope(AuditState.Disabled))
                {
                    // Delete the channel
                    try
                    {
                        await DeleteChannelAsync();
                    }
                    catch (ShipWorksLicenseException ex)
                    {
                        log.Error("Error deleting channel", ex);
                        messagdHelper.ShowError("Error deleting Channel. Please try again.");
                    }
                    catch (SqlAppResourceLockException)
                    {
                        messagdHelper.ShowError("Unable to delete store while it is in the process of a download.");
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

            IsDeleting = false;
        }

        /// <summary>
        /// Deletes all of the stores for the selected channel
        /// </summary>
        private Task DeleteChannelAsync()
        {
            return TaskEx.Run(() => license.DeleteChannel(selectedStoreType));
        }
    }
}
