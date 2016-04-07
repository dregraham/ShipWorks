using System;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using Interapptive.Shared.UI;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// User control for selecting what type of rates to return for ups
    /// </summary>
    public partial class UpsAccountRateTypeControl : UserControl
    {
        private UpsAccountEntity account;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsAccountRateTypeControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize the control with the settings from the given account
        /// </summary>
        public void Initialize(UpsAccountEntity account, bool isCreatingNewUpsAccount)
        {
            this.account = account;

            if (isCreatingNewUpsAccount)
            {
                panelNewAccount.Visible = true;
            }
            else
            {
                panelAuthorizationInstructions.Visible = true;
            }

            EnumHelper.BindComboBox<UpsRateType>(rateType);

            rateType.SelectedValue = (UpsRateType) account.RateType;

            HideOrShowNegotiatedRates();
        }

        /// <summary>
        /// Called when [rate type changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnRateTypeChanged(object sender, EventArgs e)
        {
            HideOrShowNegotiatedRates();
        }

        /// <summary>
        /// Hides the or show negotiated rates.
        /// </summary>
        private void HideOrShowNegotiatedRates()
        {
            bool showNegotiatedRatesDependentControls = (!account.InvoiceAuth && (UpsRateType)rateType.SelectedValue == UpsRateType.Negotiated);

            panelInvoiceAuthorizationHolder.Visible = showNegotiatedRatesDependentControls;
            authorizationControl.Visible = showNegotiatedRatesDependentControls;
        }

        /// <summary>
        /// Register the account and save
        /// </summary>
        public bool RegisterAndSaveToEntity()
        {
            // If the account has not done invoice auth and they have selected negotiated rates
            if (!account.InvoiceAuth && (UpsRateType)rateType.SelectedValue == UpsRateType.Negotiated)
            {
                try
                {
                    // Register the account using invoice auth
                    UpsClerk clerk = new UpsClerk(account);
                    clerk.RegisterAccount(account, authorizationControl.InvoiceAuthorizationData);
                }
                catch (UpsWebServiceException ex)
                {
                    string errorMessage = ex.Message + Environment.NewLine + Environment.NewLine +
                        "Note: UPS will lock out accounts for a 24 hour period if your invoice information cannot be authenticated after two attempts.";
                    MessageHelper.ShowError(this, errorMessage);
                    return false;
                }
            }
            account.RateType = (int)rateType.SelectedValue;
            return true;
        }
    }
}
