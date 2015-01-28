using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// User control for selecting what type of rates to return for ups
    /// </summary>
    public partial class UpsAccountRateTypeControl : UserControl
    {
        UpsAccountEntity account;

        private bool isInitialized = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsAccountRateTypeControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets a value indicating whether user is creating a new UPS account
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is creating new UPS account; otherwise, <c>false</c>.
        /// </value>
        private bool isCreatingNewUpsAccount;
       

        /// <summary>
        /// Initialize the control with the settings from the given account
        /// </summary>
        public void Initialize(UpsAccountEntity account, bool isCreatingNewUpsAccount)
        {
            this.account = account;

            if (!isInitialized)
            {
                isInitialized = true;

                this.isCreatingNewUpsAccount = isCreatingNewUpsAccount;

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
        /// Save the UI selection to the account entity
        /// </summary>
        /// <exception cref="CarrierException">
        /// Negotiated Rate not valid for new account.
        /// or
        /// Invoice Number required.
        /// </exception>
        public void RegisterAndSaveToEntity()
        {
            UpsRateType selectedRateType = (UpsRateType) rateType.SelectedValue;

            if (selectedRateType == UpsRateType.Negotiated)
            {
                if (isCreatingNewUpsAccount)
                {
                    throw new CarrierException("Negotiated Rate not valid for new account.");
                }

                if (!account.InvoiceAuth)
                {
                    if (string.IsNullOrWhiteSpace(authorizationControl.InvoiceAuthorizationData.InvoiceNumber))
                    {
                        throw new CarrierException("Invoice Number required.");
                    }

                    RegisterAccount();
                }
            }

            account.RateType = (int)selectedRateType;
        }

        /// <summary>
        /// Registers the account.
        /// </summary>
        /// <exception cref="CarrierException"></exception>
        private void RegisterAccount()
        {
            try
            {
                UpsClerk clerk = new UpsClerk(account);

                clerk.RegisterAccount(account, authorizationControl.InvoiceAuthorizationData);
            }
            catch (UpsWebServiceException ex)
            {
                string errorMessage = ex.Message + Environment.NewLine + Environment.NewLine + "Note: UPS will lock out accounts for a 24 hour period if your invoice information cannot be authenticated after two attempts.";
                throw new CarrierException(errorMessage,ex);
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
