using System;
using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.UPS.LocalRating.RateFootnotes
{
    /// <summary>
    /// Footnote showing that local rating is disabled
    /// </summary>
    /// <seealso cref="ShipWorks.Shipping.Editing.Rating.RateFootnoteControl" />
    public partial class UpsLocalRatingDisabledFootnote : RateFootnoteControl
    {
        private readonly IFootnoteParameters parameters;
        private readonly UpsAccountEntity upsAccount;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsLocalRatingDisabledFootnote"/> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="upsAccount">The ups account.</param>
        public UpsLocalRatingDisabledFootnote(IFootnoteParameters parameters, UpsAccountEntity upsAccount)
        {
            this.parameters = parameters;
            this.upsAccount = upsAccount;
            InitializeComponent();
        }

        /// <summary>
        /// Open the UpsAccountEditorDlg with the local rating tab selected
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
