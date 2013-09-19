using System;
using System.Windows.Forms;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;
using ShipWorks.Shipping.Editing;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// User control for promoting Express1 discounted rates
    /// </summary>
    public partial class Express1RatePromotionFootnote : RateFootnoteControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Express1RatePromotionFootnote()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Link to activate the Express1 discount
        /// </summary>
        private void OnActivateDiscount(object sender, EventArgs e)
        {
            using (Express1ActivateDiscountDlg dlg = new Express1ActivateDiscountDlg(new Express1EndiciaSettingsFacade()))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    RaiseRateCriteriaChanged();
                }
            }
        }
    }
}
