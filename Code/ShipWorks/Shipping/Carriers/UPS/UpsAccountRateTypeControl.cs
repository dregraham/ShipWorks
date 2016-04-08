using System;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using Interapptive.Shared.UI;
using Autofac;
using ShipWorks.ApplicationCore;

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

            HideOrShowNegotiatedRatesControl();
        }

        /// <summary>
        /// Called when [rate type changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnRateTypeChanged(object sender, EventArgs e)
        {
            HideOrShowNegotiatedRatesControl();
        }

        /// <summary>
        /// Hides the or show negotiated rates control based on the accounts status
        /// </summary>
        /// <remarks>
        /// negotiated rates are hidden when invoice auth is true. If invoice auth
        /// is false and the user picks negotiated rates in the drop down
        /// then the negotiated rates control is visible
        /// </remarks>
        private void HideOrShowNegotiatedRatesControl()
        {
            bool showNegotiatedRatesDependentControls = (!account.InvoiceAuth && (UpsRateType)rateType.SelectedValue == UpsRateType.Negotiated);

            panelInvoiceAuthorizationHolder.Visible = showNegotiatedRatesDependentControls;
            authorizationControl.Visible = showNegotiatedRatesDependentControls;
        }

        /// <summary>
        /// Register the account and save
        /// </summary>
        /// <returns>true when successful or false when register fails</returns>
        public bool RegisterAndSaveToEntity()
        {
            // If the account has not done invoice auth and they have selected negotiated rates
            if (!account.InvoiceAuth && (UpsRateType)rateType.SelectedValue == UpsRateType.Negotiated)
            {
                try
                {
                    using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
                    {
                        IUpsClerk clerk = lifetimeScope.Resolve<IUpsClerk>(new TypedParameter(typeof(UpsAccountEntity), account));
                        clerk.RegisterAccount(account, authorizationControl.InvoiceAuthorizationData);
                    }

                    account.RateType = (int)rateType.SelectedValue;
                }
                catch (UpsWebServiceException ex)
                {
                    string errorMessage = ex.Message + Environment.NewLine + Environment.NewLine +
                        "Note: UPS will lock out accounts for a 24 hour period if your invoice information cannot be authenticated after two attempts.";
                    MessageHelper.ShowError(this, errorMessage);
                    return false;
                }
            }
            return true;
        }
    }
}
