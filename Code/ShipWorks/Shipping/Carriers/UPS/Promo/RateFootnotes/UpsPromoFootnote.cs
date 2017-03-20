using ShipWorks.Shipping.Editing.Rating;
using System;

namespace ShipWorks.Shipping.Carriers.UPS.Promo.RateFootnotes
{
    /// <summary>
    /// Displays UpsPromo info
    /// </summary>
    public partial class UpsPromoFootnote : RateFootnoteControl
    {
        private readonly IFootnoteParameters parameters;
        private readonly TelemetricUpsPromo upsPromo;

        public UpsPromoFootnote(IFootnoteParameters parameters, TelemetricUpsPromo upsPromo)
        {
            InitializeComponent();
            this.parameters = parameters;
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

            parameters.ReloadRatesAction.Invoke();
        }

        /// <summary>
        /// Associated with check mark.
        /// </summary>
        public override bool AssociatedWithAmountFooter => true;
    }
}
