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
        private const string PromotionalRateAgreementUrl = "https://www.ups.com/assets/resources/media/en_US/CQF_US.pdf";
        private const string TechnologyAgreementUrl = "https://www.ups.com/assets/resources/media/en_US/UTA.pdf";

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
    }
}
