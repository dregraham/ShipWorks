using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Core.UI;
using ShipWorks.Stores;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using ShipWorks.UI.Controls.ChannelConfirmDelete;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.Licensing.LicenseEnforcement;
using ShipWorks.Data.Utility;

namespace ShipWorks.UI.Controls.ChannelLimit
{
    /// <summary>
    /// ViewModel for the ChannelLimitDlg
    /// </summary>
    public class ChannelLimitViewModel : INotifyPropertyChanged, IChannelLimitViewModel
    {
        private readonly PropertyChangedHandler handler;
        public event PropertyChangedEventHandler PropertyChanged;
        private StoreTypeCode selectedStoreType;
        private ObservableCollection<StoreTypeCode> channelCollection;
        private ICustomerLicense license;
        private string errorMessage;
        private readonly IStoreManager storeManager;
        private readonly IChannelConfirmDeleteFactory confirmDeleteFactory;
        private readonly IWebBrowserFactory webBrowserFactory;
        private readonly IMessageHelper messageHelper;
        private readonly ILog log;
        private bool isDeleting;
        private IChannelLimitBehavior channelLimitBehavior;

        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelLimitViewModel(
            IStoreManager storeManager,
            IChannelConfirmDeleteFactory confirmDeleteFactory,
            Func<Type, ILog> logFactory,
            IWebBrowserFactory webBrowserFactory,
            IMessageHelper messageHelper)
        {
            this.storeManager = storeManager;
            this.confirmDeleteFactory = confirmDeleteFactory;
            this.webBrowserFactory = webBrowserFactory;
            this.messageHelper = messageHelper;

            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            // Set the selected store type to invalid
            SelectedStoreType = StoreTypeCode.Invalid;

            // Set the default enforcement context
            EnforcementContext = EnforcementContext.NotSpecified;

            log = logFactory(typeof (ChannelLimitViewModel));

            DeleteStoreClickCommand = new RelayCommand<Window>(DeleteChannel, CanExecuteDeleteStore);
            UpgradeClickCommand = new RelayCommand<Window>(UpgradeAccount);
        }

        /// <summary>
        /// Used to indicate if we are deleting a store
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool IsDeleting
        {
            get { return isDeleting; }
            set { handler.Set(nameof(IsDeleting), ref isDeleting, value); }
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
            set { handler.Set(nameof(SelectedStoreType), ref selectedStoreType, value); }
        }

        /// <summary>
        /// Delete Store ClickCommand
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand DeleteStoreClickCommand { get; }

        /// <summary>
        /// Upgrade Account ClickCommand
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand UpgradeClickCommand { get; }

        /// <summary>
        /// Channel being added
        /// </summary>
        public StoreTypeCode? ChannelToAdd { get; set; }

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
        /// Context of which the view model is being invoked in
        /// </summary>
        public EnforcementContext EnforcementContext { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title => channelLimitBehavior.Title;

        /// <summary>
        /// Since we already have a license and channelLimitBehavior internally,
        /// we should just call this...
        /// </summary>
        private void Load()
        {
            Load(license, channelLimitBehavior);
        }

        /// <summary>
        /// Loads the list of active stores
        /// </summary>
        public void Load(ICustomerLicense customerLicense, IChannelLimitBehavior behavior)
        {
            license = customerLicense;
            channelLimitBehavior = behavior;

            // Check to make sure we are getting a CustomerLicense
            if (license == null)
            {
                throw new ShipWorksLicenseException("Store licenses do not have channel limits.");
            }

            SelectedStoreType = StoreTypeCode.Invalid;

            license.Refresh();

            // if we don't have a store collection make one
            if (ChannelCollection == null)
            {
                ChannelCollection = new ObservableCollection<StoreTypeCode>();
            }

            channelLimitBehavior.PopulateChannels(channelCollection, ChannelToAdd);

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
        private bool CanExecuteDeleteStore(Window owner)
        {
            return SelectedStoreType != StoreTypeCode.Invalid;
        }

        /// <summary>
        /// Updates the error message to display to the user
        /// </summary>
        private void UpdateErrorMesssage()
        {
            IEnumerable<EnumResult<ComplianceLevel>> channelLimitCompliance = license.EnforceCapabilities(channelLimitBehavior.EditionFeature, EnforcementContext);

            EnumResult<ComplianceLevel> nonCompliantResult = channelLimitCompliance.FirstOrDefault(c => c.Value == ComplianceLevel.NotCompliant);

            ErrorMessage = nonCompliantResult == null ? "Please click next." : nonCompliantResult.Message;
        }

        /// <summary>
        /// Upgrade the account
        /// </summary>
        private void UpgradeAccount(Window owner)
        {
            Uri uri = new Uri(CustomerLicense.UpgradeUrl);
            IDialog browserDlg = webBrowserFactory.Create(uri, "Upgrade your plan", owner);
            browserDlg.ShowDialog();

            Load();

            if (IsCompliant())
            {
                owner?.Close();
            }
        }

        /// <summary>
        /// Delete the selected store
        /// </summary>
        private async void DeleteChannel(Window owner)
        {
            List<StoreTypeCode> localStoreTypeCodes =
                storeManager.GetAllStores().Select(s => (StoreTypeCode) s.TypeCode).Distinct().ToList();

            // If we are trying to delete the only store type in ShipWorks and they are not trying to add another one
            // display an error and don't delete
            if (localStoreTypeCodes.Count == 1 && localStoreTypeCodes.Contains(SelectedStoreType) && ChannelToAdd == null)
            {
                messageHelper.ShowError($"You cannot remove {EnumHelper.GetDescription(selectedStoreType)} because it is " +
                                        "the only channel in your ShipWorks database.");
                return;
            }

            IsDeleting = true;

            IChannelConfirmDeleteDlg deleteDlg = confirmDeleteFactory.GetConfirmDeleteDlg(selectedStoreType, owner);

            deleteDlg.ShowDialog();

            if (deleteDlg.DialogResult == true)
            {
                // Delete the channel
                try
                {
                    await DeleteChannelAsync();
                }
                catch (ShipWorksLicenseException ex)
                {
                    log.Error("Error deleting channel", ex);
                    messageHelper.ShowError("Error deleting Channel. Please try again.");
                }
                catch (SqlAppResourceLockException)
                {
                    messageHelper.ShowError("Unable to delete store while it is in the process of a download.");
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

            if (IsCompliant())
            {
                owner?.Close();
            }
        }

        /// <summary>
        /// Returns true if we are compliant with the enforcer
        /// </summary>
        private bool IsCompliant()
        {
            return
                license.EnforceCapabilities(channelLimitBehavior.EditionFeature, EnforcementContext)
                    .FirstOrDefault(c => c.Value == ComplianceLevel.NotCompliant) == null;
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
