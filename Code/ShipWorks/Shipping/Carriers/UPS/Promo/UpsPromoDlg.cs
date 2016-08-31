using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using System;
using System.Windows.Forms;

namespace ShipWorks.Shipping.Carriers.UPS.Promo
{
    public partial class UpsPromoDlg : Form
    {
        private readonly IUpsPromo promo;
        private PromoAcceptanceTerms terms;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="promo"></param>
        public UpsPromoDlg(IUpsPromo promo)
        {
            this.promo = promo;
            InitializeComponent();
        }

        /// <summary>
        /// Apply the Ups promo to the account
        /// </summary>
        private void OnEnrollClick(object sender, EventArgs e)
        {
            // Get terms and conditions
            GetTerms();

            try
            {
                // Apply the promo
                promo.Terms.AcceptTerms();
                promo.Apply();
            }
            catch (UpsPromoException)
            {
                MessageHelper.ShowError(this, "An error occurred while attempting to apply the promo to your UPS account. Please try again later.");
                promo.RemindMe();
            }

            Close();
        }

        /// <summary>
        /// Decline the promo
        /// </summary>
        private void OnDeclineClick(object sender, EventArgs e)
        {
            // Decline the promo and close the dlg
            promo.Decline();
            Close();
        }

        /// <summary>
        /// Remind me later
        /// </summary>
        private void OnRemindMeClick(object sender, EventArgs e)
        {
            promo.RemindMe();
            Close();
        }

        /// <summary>
        /// Open the terms and conditions Url
        /// </summary>
        private void OnTermsClick(object sender, EventArgs e)
        {
            GetTerms();
            if (terms != null)
            {
                WebHelper.OpenUrl(
                    Uri.IsWellFormedUriString(terms.URL, UriKind.Absolute) && !terms.URL.StartsWith("http") ?
                    new Uri(terms.URL) :
                    new Uri($"http://{terms.URL}"), this);
            }
        }

        /// <summary>
        /// Update the enroll button
        /// </summary>
        private void OnAcceptTermsChanged(object sender, EventArgs e)
        {
            enroll.Enabled = acceptTerms.Checked;
            enroll.Focus();
        }

        private void GetTerms()
        {
            if (terms == null)
            {
                try
                {
                    terms = promo.Terms;
                }
                catch (UpsPromoException )
                {
                    MessageHelper.ShowError(this, "An error occurred while attempting to retrieve the terms and conditions of the promo. Please try again later.");
                    promo.RemindMe();
                    Close();
                }
            }
        }
    }
}
