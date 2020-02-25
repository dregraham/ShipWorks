using ShipWorks.UI.Wizard;

namespace ShipWorks.Shipping.Carriers.UPS.OneBalance
{
    /// <summary>
    /// Wizard page for a user to select whether they would like to create a One Balance UPS account or a standard
    /// UPS account.
    /// </summary>
    public partial class OneBalanceSelectionPage : WizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OneBalanceSelectionPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Whether or not the user has chosen to setup One Balance
        /// </summary>
        public bool SetupOneBalance => createOneBalanceOption.Checked;
    }
}
