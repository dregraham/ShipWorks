using System;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.RateFootnotes.Promotion
{
    public partial class UspsRatePromotionFootnote : RateFootnoteControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UspsRatePromotionFootnote"/> class.
        /// </summary>
        public UspsRatePromotionFootnote(ShipmentEntity shipment)
        {
            InitializeComponent();
            Shipment = shipment;
        }

        /// <summary>
        /// Associated with star.
        /// </summary>
        public override bool AssociatedWithAmountFooter
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the shipment.
        /// </summary>
        public ShipmentEntity Shipment { get; private set; }

        /// <summary>
        /// Link to activate the USPS (Stamps.com Expedited) discount
        /// </summary>
        private void OnActivateDiscount(object sender, EventArgs e)
        {
            using (UspsActivateDiscountDlg dlg = new UspsActivateDiscountDlg(Shipment))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    RaiseRateCriteriaChanged();
                }
            }
        }
    }
}
