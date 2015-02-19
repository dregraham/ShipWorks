using System;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.Contracts;
using ShipWorks.Shipping.Carriers.Postal.Usps.Registration.Promotion;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// A dialog for activating the USPS shipment type and creating 
    /// a new account or converting an existing account.
    /// </summary>
    public partial class UspsConvertExistingAccountDlg : Form, IDiscountedAccountDlg
    {
        private readonly ILog log;
        private UspsAccountEntity accountToConvert;

        private static readonly string singleAccountMarketingMessage = 
            "You can now save up to 46% on USPS Priority Mail and Priority Mail Express Shipments with ShipWorks and " +
            "IntuiShip, all through one single USPS account. No " +
            "more switching between accounts to get the lowest rates!" + Environment.NewLine + Environment.NewLine +
            "ShipWorks offers these discounted rates through IntuiShip, a partner of the USPS.";

        private static readonly string normalMarketingMessage = 
            "You can now save up to 46% on USPS Priority Mail and Priority Mail Express Shipments with ShipWorks " +
            "and IntuiShip, all through one single Stamps.com account. " + Environment.NewLine + Environment.NewLine +
            "There are no additional monthly fees and the service, tracking, and labels are exactly the same. " +
            "The only difference is that you pay less for postage!";

        /// <summary>
        /// Initializes a new instance of the <see cref="UspsActivateDiscountDlg"/> class.
        /// </summary>
        public UspsConvertExistingAccountDlg()
        {
            InitializeComponent();

            log = LogManager.GetLogger(GetType());
        }

        /// <summary>
        /// Initializes the the form based on the given shipment.
        /// </summary>
        /// <param name="shipmentEntity">The shipment.</param>
        /// <param name="showSingleAccountMarketing"></param>
        public virtual void Initialize(ShipmentEntity shipmentEntity, bool showSingleAccountMarketing)
        {
            if (shipmentEntity == null || shipmentEntity.Postal == null || shipmentEntity.Postal.Usps == null)
            {
                throw new UspsException("Usps shipment is required to upgrade the account");
            }

            UspsAccountEntity uspsAccount = UspsAccountManager.GetAccount(shipmentEntity.Postal.Usps.UspsAccountID);

            if (uspsAccount == null)
            {
                throw new UspsException("Could not find account associated with the shipment");
            }

            if (uspsAccount.UspsReseller == (int)UspsResellerType.Express1)
            {
                throw new UspsException("Express1 accounts cannot be converted.");
            }

            accountToConvert = uspsAccount;

            labelConversionDescription.Text = showSingleAccountMarketing ? singleAccountMarketingMessage : normalMarketingMessage;
        }

        /// <summary>
        /// Called when the Start Saving button is clicked.
        /// </summary>
        private void OnStartSaving(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                // Convert the account with the USPS API and update the account entity's 
                // contract type to reflect the conversion, so the Activate discount dialog is 
                // not displayed again
                ConvertAccountToExpedited();

                // Set the ContractType to Unknown so that we rely on USPS to correctly tell us 
                // the contract type the next time we get rates or process.
                accountToConvert.ContractType = (int)UspsAccountContractType.Unknown;

                UspsAccountManager.SaveAccount(accountToConvert);

                FinishConvertingAccount();
            }
            catch (UspsException exception)
            {
                string message = "An error occurred, and ShipWorks was not able to convert your account at this time.";
                log.Error(string.Format("An error occurred trying to convert the USPS account ({0}) to get discounted postage.", accountToConvert.Username), exception);

                if (exception.Code == 0x005f0301)
                {
                    // Provide additional details when the error code is regarding a mult-user account
                    message = exception.Message;
                }
                MessageHelper.ShowError(this, message);
            }
        }

        /// <summary>
        /// Converts the account to an expedited account.
        /// </summary>
        private void ConvertAccountToExpedited()
        {
            try
            {
                // Need to convert the account to get discounted rates via the expedited contract/plan
                log.InfoFormat("Converting USPS account ({0}) to get discounted postage.", accountToConvert.Username);

                // ShipWorks3 must be used when converting an account, so always use the Intuiship promotion when converting an account. 
                IRegistrationPromotion promotion = new UspsIntuishipRegistrationPromotion();
                new UspsWebClient((UspsResellerType)accountToConvert.UspsReseller).ChangeToExpeditedPlan(accountToConvert, promotion.GetPromoCode()); 
            }
            catch (UspsApiException exception)
            {
                if (exception.Code == 0x005f0302)
                {
                    // The account is already converted, so there's nothing to do here
                    log.WarnFormat("The USPS account ({0}) has already been converted.", accountToConvert.Username);                    
                }
                else
                {
                    log.Error(string.Format("An error occurred trying to convert the USPS account ({0}) to get discounted postage.", accountToConvert.Username), exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Called when an account has been converted.
        /// </summary>
        private void FinishConvertingAccount()
        {
            RateCache.Instance.Clear();

            Cursor.Current = Cursors.Default;
            MessageHelper.ShowInformation(this, "Your account has been converted to take advantage of postage discounts.");

            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Called when the learn more link is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.LinkLabelLinkClickedEventArgs"/> instance containing the event data.</param>
        private void OnLearnMore(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageHelper.ShowInformation(this,
                                          "With IntuiShip you get some of the best postal rates available, saving you significant money on each of your domestic and " +
                                          "international Priority and Express shipments." + Environment.NewLine + Environment.NewLine + "Just add these rates to your " +
                                          "USPS account and ShipWorks will automatically utilize it for discounted rates from IntuiShip when creating postage labels." +
                                          Environment.NewLine + Environment.NewLine + "For more information, please contact us at www.interapptive.com/company/contact.html.");
        }
    }
}
