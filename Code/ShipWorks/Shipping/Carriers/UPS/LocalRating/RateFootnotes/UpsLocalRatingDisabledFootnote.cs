using System;
using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.UPS.LocalRating.RateFootnotes
{
    public partial class UpsLocalRatingDisabledFootnote : RateFootnoteControl
    {
        private readonly IFootnoteParameters parameters;
        private readonly UpsAccountEntity upsAccount;

        public UpsLocalRatingDisabledFootnote(IFootnoteParameters parameters, UpsAccountEntity upsAccount)
        {
            this.parameters = parameters;
            this.upsAccount = upsAccount;
            InitializeComponent();

            if (upsAccount.LocalRatingEnabled)
            {
                footnote.Text = "ShipWorks did not find any UPS rates for this shipment. Please review and update your UPS local rates.";
                accountSettingsLink.Visible = false;
                accountSettingsLink.Enabled = false;
            }
            else
            {
                footnote.Text = "UPS local rating needs to be enabled to compare UPS rates. To setup local rating for this account,";
                accountSettingsLink.Visible = true;
                accountSettingsLink.Enabled = true;
            }
        }

        /// <summary>
        /// Bring up the UpsPromoDlg
        /// </summary>
        private void OnClickLink(object sender, EventArgs e)
        {
            using (ILifetimeScope scope = IoC.BeginLifetimeScope())
            {
                using (UpsAccountEditorDlg dlg = scope.Resolve<UpsAccountEditorDlg>(TypedParameter.From(upsAccount)))
                {
                    if (dlg.Tabs.TabPages.ContainsKey("tabPageLocalRating"))
                    {
                        dlg.Tabs.SelectedTab = dlg.Tabs.TabPages["tabPageLocalRating"];
                    }
                    
                    dlg.ShowDialog(this);
                }
            }

            UpsAccountManager.CheckForChangesNeeded();

            RateCache.Instance.Clear();
            
            parameters.ReloadRatesAction.Invoke();
        }
    }
}
