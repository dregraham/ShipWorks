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
using GalaSoft.MvvmLight.Command;
using Interapptive.Shared.Utility;
using log4net;

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
        private readonly Func<IChannelConfirmDeleteFactory> confirmDeleteFactory;
        private readonly IMessageHelper messageHelper;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelLimitViewModel(ILicenseService licenseService, IStoreManager storeManager,
            Func<IChannelConfirmDeleteFactory> confirmDeleteFactory, Func<Type, ILog> logFactory, IMessageHelper messageHelper)
        {
            license = licenseService.GetLicenses().FirstOrDefault() as ICustomerLicense;
            this.storeManager = storeManager;
            this.confirmDeleteFactory = confirmDeleteFactory;
            this.messageHelper = messageHelper;

            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            // Set the selected store type to invalid
            SelectedStoreType = StoreTypeCode.Invalid;

            log = logFactory(typeof (ChannelLimitViewModel));
            
            // use MVVMLightExtras to troubleshooting purposes
            var foo = new EventToCommand();
            foo.Invoke();
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
            string upgradeAccountLink = "www.shipworks.com";

            try
            {
                System.Diagnostics.Process.Start(upgradeAccountLink);
            }
            catch (Win32Exception ex)
            {
                log.Error($"Failed to open URL '{upgradeAccountLink}'", ex);
                messageHelper.ShowError($"ShipWorks could not open the URL '{upgradeAccountLink}'.\n\n({ex.Message})");
                throw;
            }   
        }

        /// <summary>
        /// Delete the selected store
        /// </summary>
        private void DeleteChannel()
        {
            List<StoreTypeCode> localStoreTypeCodes =
                storeManager.GetAllStores().Select(s => (StoreTypeCode) s.TypeCode).Distinct().ToList();

            // If we are trying to delete the only store type in ShipWorks display an error and dont delete
            if (localStoreTypeCodes.Count == 1 && localStoreTypeCodes.Contains(SelectedStoreType))
            {
                UpdateErrorMesssage();
                ErrorMessage +=
                    $"\n \nYou cannot remove {EnumHelper.GetDescription(selectedStoreType)} because it is the only channel in your ShipWorks database.";
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
