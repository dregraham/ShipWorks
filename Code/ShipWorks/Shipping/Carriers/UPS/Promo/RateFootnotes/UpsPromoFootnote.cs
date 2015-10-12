using System;
using ShipWorks.Shipping.Carriers.UPS.Promo.API;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.UPS.Promo.RateFootnotes
{
    /// <summary>
    /// Displays UpsPromo info
    /// </summary>
    public partial class UpsPromoFootnote : RateFootnoteControl
    {
        private readonly IUpsPromo upsPromo;

        public UpsPromoFootnote(IUpsPromo upsPromo)
        {
            InitializeComponent();
            this.upsPromo = upsPromo;
        }

        /// <summary>
        /// Bring up the UpsPromoDlg
        /// </summary>
        private void OnLinkViewSavings(object sender, EventArgs e)
        {
            //using (somedlg dlg = new somedlg(upsPromo))
            //{
            //    dlg.ShowDialog(this);
            //}
        }

        /// <summary>
        /// Associated with check mark.
        /// </summary>
        public override bool AssociatedWithAmountFooter => true;
    }
}
