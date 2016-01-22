using System;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.UI;
using log4net;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.Contracts;
using ShipWorks.Shipping.Carriers.Postal.Usps.Registration.Promotion;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// A UserControl that allows a customer to convert an existing USPS account to an Express1
    /// USPS account via the Stamps.com API.
    /// </summary>
    public partial class UspsConvertAccountToExpeditedControl : UserControl
    {
        private readonly ILog log;
        private UspsAccountEntity accountToConvert;

        public event EventHandler<UspsAccountConvertedEventArgs> AccountConverted;
        public event EventHandler AccountConverting;

        /// <summary>
        /// Initializes a new instance of the <see cref="UspsConvertAccountToExpeditedControl"/> class.
        /// </summary>
        public UspsConvertAccountToExpeditedControl()
        {
            InitializeComponent();
            log = LogManager.GetLogger(GetType());
        }

        /// <summary>
        /// Gets or sets the description text.
        /// </summary>
        public string DescriptionText
        {
            get { return labelConversionDescription.Text; }
            set { labelConversionDescription.Text = value; }
        }

        /// <summary>
        /// Gets or sets the link text.
        /// </summary>
        public string LinkText
        {
            get { return linkLabel1.Text; }
            set { linkLabel1.Text = value; }
        }

        /// <summary>
        /// Initializes the account to convert with the specified USPS account.
        /// </summary>
        /// <param name="uspsAccount">The account to be converted.</param>
        /// <exception cref="UspsException">Only USPS accounts can be converted.</exception>
        public void Initialize(UspsAccountEntity uspsAccount)
        {
            if (uspsAccount.UspsReseller == (int)UspsResellerType.Express1)
            {
                throw new UspsException("Express1 accounts cannot be converted.");
            }

            accountToConvert = uspsAccount;
        }

        /// <summary>
        /// Called when the Start Saving button is clicked.
        /// </summary>
        private void OnStartSaving(object sender, EventArgs e)
        {
            try
            {
                if (AccountConverting != null)
                {
                    // Allow the hosting control/form a chance to update the cursor
                    AccountConverting(this, EventArgs.Empty);
                }

                // Convert the account with the USPS API and update the account entity's
                // contract type to reflect the conversion, so the Activate discount dialog is
                // not displayed again
                ConvertAccountToExpedited();

                // Set the ContractType to Unknown so that we rely on USPS to correctly tell us
                // the contract type the next time we get rates or process.
                accountToConvert.ContractType = (int)UspsAccountContractType.Unknown;

                UspsAccountManager.SaveAccount(accountToConvert);

                // Notify any listeners
                if (AccountConverted != null)
                {
                    AccountConverted(this, new UspsAccountConvertedEventArgs(accountToConvert));
                }
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

                // ShipWorks667 must be used when converting an account, so always use the Express1 promotion when converting an account.
                IRegistrationPromotion promotion = new Express1RegistrationPromotion();
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
        /// Called when the learn more link is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.LinkLabelLinkClickedEventArgs"/> instance containing the event data.</param>
        private void OnLearnMore(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageHelper.ShowInformation(this,
                                          "With ShipWorks you get some of the best postal rates available, saving you significant money on each of your domestic and " + 
                                          "international Priority and Express shipments." + Environment.NewLine + Environment.NewLine + "Simply create a USPS account " + 
                                          "in ShipWorks and you will have access to discounted rates when creating postage labels." +
                                          Environment.NewLine + Environment.NewLine + "For more information, please contact us at www.interapptive.com/company/contact.html.");
        }
    }
}
