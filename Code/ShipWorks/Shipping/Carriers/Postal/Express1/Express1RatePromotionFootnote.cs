using System;
using System.Windows.Forms;
using ShipWorks.Shipping.Editing;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// User control for promoting Express1 discounted rates
    /// </summary>
    public partial class Express1RatePromotionFootnote : RateFootnoteControl
    {
        private readonly IExpress1SettingsFacade express1Settings;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="settings">Express1 settings facade</param>
        public Express1RatePromotionFootnote(IExpress1SettingsFacade settings)
        {
            InitializeComponent();

            express1Settings = settings;
        }

        /// <summary>
        /// Adds the carrier name text to the text of the control
        /// </summary>
        public override void SetCarrierName(string carrierName)
        {
            AddCarrierNameText(carrierName, label, linkControl);
        }

        /// <summary>
        /// Link to activate the Express1 discount
        /// </summary>
        private void OnActivateDiscount(object sender, EventArgs e)
        {
            using (Express1ActivateDiscountDlg dlg = new Express1ActivateDiscountDlg(express1Settings))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    RaiseRateCriteriaChanged();
                }
            }
        }

        /// <summary>
        /// Associated with star.
        /// </summary>
        public override bool AssociatedWithAmountFooter
        {
            get { return true; }
        }
    }
}
