using System;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Editions.Freemium
{
    /// <summary>
    /// Wizard for upgrading from freemium free to freemium paid
    /// </summary>
    public partial class FreemiumUpgradeWizard : WizardForm
    {
        EditionRestrictionIssue issue;
        FreemiumFreeEdition edition;

        /// <summary>
        /// Constructor
        /// </summary>
        public FreemiumUpgradeWizard(EditionRestrictionIssue issue)
        {
            InitializeComponent();

            this.issue = issue;
            this.edition = issue.Edition as FreemiumFreeEdition;

            labelExplanation.Text = issue.GetDescription();

            string accountText = (edition.AccountType == FreemiumAccountType.DAZzle) ? "ShipWorks account" : "ShipWorks and Endicia accounts";

            wizardPageWelcome.Description = string.Format(wizardPageWelcome.Description, accountText);
            labelUpgradeAdvert.Text = string.Format(labelUpgradeAdvert.Text, accountText);
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            if (edition.AccountType == FreemiumAccountType.DAZzle)
            {
                panelElsUpgrade.Visible = false;
                panelDazzle.Top = panelElsUpgrade.Top;
            }
            else
            {
                panelDazzle.Visible = false;
                comboElsPlan.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Stepping next from the welcome page
        /// </summary>
        private void OnStepNextWelcome(object sender, WizardStepEventArgs e)
        {
            e.NextPage = wizardPageTerms;
        }

        /// <summary>
        /// Click the terms of service link
        /// </summary>
        private void OnClickTermsOfService(object sender, EventArgs e)
        {
            WebHelper.OpenUrl("http://www.interapptive.com/store/agreement.htm?s=sw", this);
        }

        /// <summary>
        /// Click the service fees link
        /// </summary>
        private void OnClickServiceFees(object sender, EventArgs e)
        {
            WebHelper.OpenUrl("http://www.interapptive.com/shipworks/pricing.html", this);
        }

        /// <summary>
        /// Click the Endicia service plan pricing link
        /// </summary>
        private void OnClickEndiciaPricing(object sender, LinkLabelLinkClickedEventArgs e)
        {
            WebHelper.OpenUrl("http://www.endicia.com/Pricing/?s=sw", this);
        }

        /// <summary>
        /// Stepping next from the interapptive terms page
        /// </summary>
        private void OnStepNextInterapptiveTerms(object sender, WizardStepEventArgs e)
        {
            if (!checkBoxTerms.Checked)
            {
                MessageHelper.ShowError(this, "You must agree to the Auctane terms of service to proceed.");

                e.NextPage = CurrentPage;
                return;
            }
        }

        /// <summary>
        /// Stepping next from the Endicia page - to actually signup
        /// </summary>
        private void OnStepNextEndicia(object sender, WizardStepEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                TangoWebClient.UpgradeFreemiumStore(edition.Store, comboElsPlan.SelectedIndex);
            }
            catch (ShipWorksLicenseException ex)
            {
                MessageHelper.ShowError(this, "An error occurred upgrading your account:\n\n" + ex.Message);
                e.NextPage = CurrentPage;
            }
            catch (TangoException ex)
            {
                MessageHelper.ShowError(this, "An error occurred upgrading your account:\n\n" + ex.Message);
                e.NextPage = CurrentPage;
            }
        }
    }
}
