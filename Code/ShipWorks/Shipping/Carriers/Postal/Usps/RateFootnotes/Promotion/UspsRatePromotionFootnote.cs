using System;
using System.Windows.Forms;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.RateFootnotes.Promotion
{
    public partial class UspsRatePromotionFootnote : RateFootnoteControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UspsRatePromotionFootnote"/> class.
        /// </summary>
        public UspsRatePromotionFootnote()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// Link to activate the USPS (Stamps.com Expedited) discount
        /// </summary>
        private void OnActivateDiscount(object sender, EventArgs e)
        {
            using (UspsActivateDiscountDlg dlg = new UspsActivateDiscountDlg())
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
