using System;
using System.Windows.Forms;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using ShipWorks.Shipping.Carriers.UPS.Promo.API;

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
                MessageHelper.ShowError(this, "Error applying promo, we will try again later.");
                promo.RemindMe();
            }

            Close();
        }

        /// <summary>
        /// Decline the promo
        /// </summary>
        private void OnDclineClick(object sender, EventArgs e)
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
            WebHelper.OpenUrl(Uri.IsWellFormedUriString(promo.Terms.URL, UriKind.Absolute) && !promo.Terms.URL.StartsWith("http") ?
                new Uri(promo.Terms.URL) : new Uri($"http://{promo.Terms.URL}"), this);
        }

        /// <summary>
        /// Update the enroll button
        /// </summary>
        private void OnAcceptTermsChanged(object sender, EventArgs e)
        {
            enroll.Enabled = acceptTerms.Checked;
        }

        private void GetTerms()
        {
            if (terms == null)
            {
                try
                {
                    terms = promo.GetAgreementTerms();
                }
                catch (UpsPromoException )
                {
                    MessageHelper.ShowError(this, "Error getting Terms and Conditions, we will try again later");
                    promo.RemindMe();
                    Close();
                }
            }
        }
    }
}
