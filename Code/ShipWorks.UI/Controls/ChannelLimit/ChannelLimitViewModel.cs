using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Interop;
using GalaSoft.MvvmLight.Command;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.LicenseEnforcement;
using ShipWorks.Core.UI;
using ShipWorks.Data.Utility;
using ShipWorks.Stores;
using ShipWorks.UI.Controls.ChannelConfirmDelete;
using ShipWorks.Users.Security;
using IWin32Window = System.Windows.Forms.IWin32Window;

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
        private readonly ISecurityContext securityContext;

        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelLimitViewModel(
            IStoreManager storeManager,
            IChannelConfirmDeleteFactory confirmDeleteFactory,
            Func<Type, ILog> logFactory,
            IWebBrowserFactory webBrowserFactory,
            IMessageHelper messageHelper,
            ISecurityContext securityContext)
        {
            this.storeManager = storeManager;
            this.confirmDeleteFactory = confirmDeleteFactory;
            this.webBrowserFactory = webBrowserFactory;
            this.messageHelper = messageHelper;
            this.securityContext = securityContext;

            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            // Set the selected store type to invalid
            SelectedStoreType = StoreTypeCode.Invalid;

            // Set the default enforcement context
            EnforcementContext = EnforcementContext.NotSpecified;

            log = logFactory(typeof(ChannelLimitViewModel));

            DeleteStoreClickCommand = new RelayCommand<ChannelLimitControl>(async control => await DeleteChannel(control), CanExecuteDeleteStore);
            UpgradeClickCommand = new RelayCommand<ChannelLimitControl>(UpgradeAccount);
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

            // We need to force the refresh at this point to make sure we have the most
            // recent store/channel information
            license.ForceRefresh();

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
            // We need to force the refresh at this point to make sure we have the most
            // recent store/channel information
            license.ForceRefresh();
        }

        /// <summary>
        /// True if a store is selected
        /// </summary>
        private bool CanExecuteDeleteStore(ChannelLimitControl owner)
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

            // If ControlOwner is null, it is the dialog being displayed. So show Close instead of Next.
            string button = (ControlOwner == null) ? "Close" : "Next";

            ErrorMessage = nonCompliantResult == null ? $"Please click {button}." : nonCompliantResult.Message;
        }

        /// <summary>
        /// Upgrade the account
        /// </summary>
        private void UpgradeAccount(ChannelLimitControl control)
        {
            Uri uri = new Uri(CustomerLicense.UpgradeUrl);

            IWin32Window owner = GetOwner(control);

            IDialog browserDlg = webBrowserFactory.Create(uri, "Upgrade your plan", owner, new Size(1053, 1010));

            browserDlg.ShowDialog();

            Load();

            if (IsCompliant())
            {
                (owner as ChannelLimitDlg)?.Close();
            }
        }

        /// <summary>
        /// Gets the IWin32Window owner from the control.
        /// </summary>
        private IWin32Window GetOwner(ChannelLimitControl control)
        {
            if (control == null)
            {
                return null;
            }

            // Get handle for wpf control
            HwndSource wpfHandle = PresentationSource.FromVisual(control) as HwndSource;

            if (wpfHandle != null)
            {
                // Get the ElementHost if the control is owned by one.
                ElementHost host = Control.FromChildHandle(wpfHandle.Handle) as ElementHost;

                if (host != null)
                {
                    // We go up the parent chain here because the dialogs opened with the wizard page
                    // as the owner were not appearing center screen. So we set the owner as the AddStoreWizard instead.
                    // ElementHost -> ActivationErrorWizardPage -> WizardPage -> AddStoreWizard
                    return host.Parent.Parent.Parent;
                }
            }

            // Wasn't owned by element host, so it must be the wpf dialog
            return wpfHandle?.RootVisual as ChannelLimitDlg;
        }

        /// <summary>
        /// Delete the selected store
        /// </summary>
        public async Task DeleteChannel(ChannelLimitControl control)
        {
            List<StoreTypeCode> localStoreTypeCodes =
                storeManager.GetAllStores().Select(s => (StoreTypeCode) s.TypeCode).Distinct().ToList();

            IWin32Window owner = GetOwner(control);

            // If we are trying to delete the only store type in ShipWorks and they are not trying to add another one
            // display an error and don't delete
            if (localStoreTypeCodes.Count == 1 && localStoreTypeCodes.Contains(SelectedStoreType) && ChannelToAdd == null)
            {
                messageHelper.ShowError(owner, $"You cannot remove {EnumHelper.GetDescription(selectedStoreType)} because it is " +
                                        "the only channel in your ShipWorks database.");
                return;
            }

            IsDeleting = true;

            IDialog deleteDlg = confirmDeleteFactory.GetConfirmDeleteDlg(selectedStoreType, owner);
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
                    messageHelper.ShowError(owner, "Error deleting Channel. Please try again.");
                }
                catch (SqlAppResourceLockException)
                {
                    messageHelper.ShowError(owner, "Unable to delete store while it is in the process of a download.");
                }
                catch (PermissionException)
                {
                    messageHelper.ShowError(owner, "Please log in as an administrator to delete this channel.");
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
                (owner as ChannelLimitDlg)?.Close();
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
            return TaskEx.Run(() => license.DeleteChannel(selectedStoreType, securityContext));
        }

        /// <summary>
        /// The owner, if using the control
        /// </summary>
        public IWin32Window ControlOwner { get; set; }
    }
}
