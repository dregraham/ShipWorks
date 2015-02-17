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
        /// <summary>
        /// Initializes a new instance of the <see cref="UspsRatePromotionFootnote" /> class.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="showSingleAccountDialog">if set to <c>true</c> the dialog for the single account marketing
        /// message will be displayed when the Activate link is clicked; otherwise the normal activate discount dialog
        /// is displayed.</param>
        public UspsRatePromotionFootnote(ShipmentEntity shipment, bool showSingleAccountDialog)
        {
            InitializeComponent();
            Shipment = shipment;
            ShowSingleAccountDialog = showSingleAccountDialog;
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
        /// Gets or sets a value indicating whether to [show single account dialog] when 
        /// the activate link is clicked. 
        /// </summary>
        public bool ShowSingleAccountDialog { get; private set; }

        /// <summary>
        /// Link to activate the USPS discount
        /// </summary>
        private void OnActivateDiscount(object sender, EventArgs e)
        {
            if (ShowSingleAccountDialog)
            {
                using (SingleAccountMarketingDlg dlg = new SingleAccountMarketingDlg())
                {
                    dlg.Initialize(Shipment);
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        RaiseRateCriteriaChanged();
                    }
                }
            }
            else
            {
                using (UspsActivateDiscountDlg dlg = new UspsActivateDiscountDlg())
                {
                    dlg.Initialize(Shipment);
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        RaiseRateCriteriaChanged();
                    }
                }
            }
        }
    }
}
