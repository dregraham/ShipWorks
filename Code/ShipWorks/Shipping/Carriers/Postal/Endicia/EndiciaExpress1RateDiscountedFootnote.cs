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
    /// User control for displaying footnote about the savings obtained by using Express1
    /// </summary>
    public partial class EndiciaExpress1RateDiscountedFootnote : RateFootnoteControl
    {
        List<RateResult> originalRates;
        List<RateResult> discountedRates;

        /// <summary>
        /// Constructor
        /// </summary>
        public EndiciaExpress1RateDiscountedFootnote(List<RateResult> originalRates, List<RateResult> discountedRates)
        {
            InitializeComponent();

            this.originalRates = originalRates;
            this.discountedRates = discountedRates;
        }

        /// <summary>
        /// View the detailed Endicia vs. Express1 savings
        /// </summary>
        private void OnLinkViewSavings(object sender, EventArgs e)
        {
            using (EndiciaExpress1ActualSavingsDlg dlg = new EndiciaExpress1ActualSavingsDlg(originalRates, discountedRates))
            {
                dlg.ShowDialog(this);
            }
        }
    }
}
