using System;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// User control for selecting what type of rates to return for ups
    /// </summary>
    public partial class UpsAccountRateTypeControl : UserControl
    {
        private bool isInitialized = false;

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
            if (!isInitialized)
            {
                isInitialized = true;

                if (isCreatingNewUpsAccount)
                {
                    panelNewAccount.Visible = true;
                }
                else
                {
                    panelAuthorizationInstructions.Visible = true;
                    if (account.InvoiceAuth)
                    {
                        panelAlreadyRegisterred.Visible = true;
                    }
                    else
                    {
                        authorizationControl.Visible = true;
                    }
                }

                EnumHelper.BindComboBox<UpsRateType>(rateType);

                rateType.SelectedValue = (UpsRateType) account.RateType;

                HideOrShowNegotiatedRates();
            }
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
            panelInvoiceAuthorizationHolder.Visible = ((UpsRateType) rateType.SelectedValue == UpsRateType.Negotiated);
        }
    }
}
