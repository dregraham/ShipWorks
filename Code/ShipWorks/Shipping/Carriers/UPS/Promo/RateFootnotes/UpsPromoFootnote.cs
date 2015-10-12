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
        private void OnLinkActivate(object sender, EventArgs e)
        {
            using (UpsPromoDlg dlg = new UpsPromoDlg(upsPromo))
            {
                dlg.ShowDialog(this);
            }

            RateCache.Instance.Clear();
            Hide();
        }

        /// <summary>
        /// Associated with check mark.
        /// </summary>
        public override bool AssociatedWithAmountFooter => true;
    }
}
