using System.Reflection;
using System.Windows.Forms;
using Autofac;
using GalaSoft.MvvmLight.Command;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.LocalRating.RateFootnotes;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.UI.Carriers.Ups.LocalRating
{
    /// <summary>
    /// View model for the Ups local rating footnote
    /// </summary>
    /// <seealso cref="ShipWorks.Shipping.Carriers.UPS.LocalRating.RateFootnotes.IUpsLocalRatingDisabledFootnoteViewModel" />
    [Component]
    public class UpsLocalRatingDisabledFootnoteViewModel : IUpsLocalRatingDisabledFootnoteViewModel
    {
        private readonly IMessenger messenger;
        private readonly IWin32Window owner;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsLocalRatingDisabledFootnoteViewModel"/> class.
        /// </summary>
        /// <param name="messenger">The messenger.</param>
        /// <param name="owner">The owner.</param>
        public UpsLocalRatingDisabledFootnoteViewModel(IMessenger messenger, IWin32Window owner)
        {
            this.messenger = messenger;
            this.owner = owner;
            OpenAccountSettings = new RelayCommand(OpenAccountSettingsAction);
        }

        /// <summary>
        /// Command to open the account settings dlg
        /// </summary>
        [Obfuscation(Exclude = true)]
        public RelayCommand OpenAccountSettings { get; }

        /// <summary>
        /// Gets or sets the ups account.
        /// </summary>
        public UpsAccountEntity UpsAccount { get; set; }

        /// <summary>
        /// Shipment adapter associated with the current rates
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICarrierShipmentAdapter ShipmentAdapter { get; set; }

        /// <summary>
        /// Opens the account settings dialog with the rating tab selected
        /// </summary>
        private void OpenAccountSettingsAction()
        {
            using (ILifetimeScope scope = IoC.BeginLifetimeScope())
            {
                using (UpsAccountEditorDlg dlg = scope.Resolve<UpsAccountEditorDlg>(TypedParameter.From(UpsAccount)))
                {
                    if (dlg.Tabs.TabPages.ContainsKey("tabPageLocalRating"))
                    {
                        dlg.Tabs.SelectedTab = dlg.Tabs.TabPages["tabPageLocalRating"];
                    }

                    dlg.ShowDialog(owner);
                }
            }

            UpsAccountManager.CheckForChangesNeeded();

            RateCache.Instance.Clear();

            messenger.Send(new ShipmentChangedMessage(this, ShipmentAdapter));
        }
    }
}