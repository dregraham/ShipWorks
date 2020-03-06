using System.Security.Policy;
using Interapptive.Shared.Net;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Shipping.Carriers.UPS.OneBalance
{
    /// <summary>
    /// Wizard page for showing UPS terms and conditions for One Balance
    /// </summary>
    public partial class OneBalanceTermsAndConditionsPage : WizardPage
    {
        private const string PromotionalRateAgreementUrl = "https://shipworks-static-resources.s3.amazonaws.com/CQF_US.pdf";
        private const string TechnologyAgreementUrl = "https://shipworks-static-resources.s3.amazonaws.com/UTA.pdf";

        /// <summary>
        /// Constructor
        /// </summary>
        public OneBalanceTermsAndConditionsPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Open the rate agreement url when clicked
        /// </summary>
        private void OnClickRateAgreement(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            WebHelper.OpenUrl(PromotionalRateAgreementUrl, this);
        }

        /// <summary>
        /// Open the technology agreement url when clicked
        /// </summary>
        private void OnClickTechnologyAgreement(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            WebHelper.OpenUrl(TechnologyAgreementUrl, this);
        }

        /// <summary>
        /// Opens the prohibited goods page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClickLinkProhibitedGoods(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            WebHelper.OpenUrl("https://www.ups.com/us/en/help-center/shipping-support/prohibited-items.page", this);
        }
    }
}
