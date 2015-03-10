using System;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.RateFootnotes.Promotion
{
    /// <summary>
    /// A RateFootnoteControl for promoting the USPS shipping provider.
    /// </summary>
    public partial class UspsRatePromotionFootnote : RateFootnoteControl
    {
        private readonly ShipmentEntity shipment;
        private readonly bool showSingleAccountDialog;

        /// <summary>
        /// Initializes a new instance of the <see cref="UspsRatePromotionFootnote" /> class.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="showSingleAccountDialog">if set to <c>true</c> the dialog for the single account marketing
        /// message will be displayed when the Activate link is clicked; otherwise the normal activate discount dialog
        /// is displayed.</param>
        public UspsRatePromotionFootnote(ShipmentEntity shipment, bool showSingleAccountDialog)
        {
            this.shipment = shipment;
            this.showSingleAccountDialog = showSingleAccountDialog;
            InitializeComponent();
        }

        /// <summary>
        /// Associated with star.
        /// </summary>
        public override bool AssociatedWithAmountFooter
        {
            get { return true; }
        }

        /// <summary>
        /// Link to activate the USPS discount
        /// </summary>
        private void OnActivateDiscount(object sender, EventArgs e)
        {
            int originalShipmentTypeCode = shipment.ShipmentType;

            using (UspsActivateDiscountDlg dlg = GetDiscountDialog())
            {
                dlg.Initialize(shipment);

                // If they clicked ok and it didn't cause the shipment type to change, refresh rates.
                if (dlg.ShowDialog(this) == DialogResult.OK && originalShipmentTypeCode == shipment.ShipmentType)
                {
                    RaiseRateCriteriaChanged();
                }
            }
        }

        /// <summary>
        /// Gets the appropriate discount dialog based on ShowSingleAccountDialog.
        /// </summary>
        private UspsActivateDiscountDlg GetDiscountDialog()
        {
            return showSingleAccountDialog ? new SingleAccountMarketingDlg() : new UspsActivateDiscountDlg();
        }
    }
}
