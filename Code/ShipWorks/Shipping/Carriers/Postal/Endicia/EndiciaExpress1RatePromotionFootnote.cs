using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Shipping.Editing;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// User control for promoting Express1 discounted rates
    /// </summary>
    public partial class EndiciaExpress1RatePromotionFootnote : RateFootnoteControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EndiciaExpress1RatePromotionFootnote()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Link to activate the Express1 discount
        /// </summary>
        private void OnActivateDiscount(object sender, EventArgs e)
        {
            using (EndiciaExpress1ActivateDiscountDlg dlg = new EndiciaExpress1ActivateDiscountDlg())
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    RaiseRateCriteriaChanged();
                }
            }
        }
    }
}
