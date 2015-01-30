using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Stamps;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Registration;
using Interapptive.Shared.UI;
using log4net;
using log4net.Core;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// A UserControl that allows a customer to convert an existing Stamps.com account to an
    /// USPS (Stamps.com Expedited) account via the Stamps.com API.
    /// </summary>
    public partial class UspsConvertAccountToExpeditedControl : UserControl
    {
        private readonly ILog log;
        private StampsAccountEntity accountToConvert;

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
        /// Initializes the account to convert with the specified stamps account.
        /// </summary>
        /// <param name="stampsAccount">The account to be converted.</param>
        /// <exception cref="StampsException">Only Stamps.com accounts can be converted.</exception>
        public void Initialize(StampsAccountEntity stampsAccount)
        {
            if (stampsAccount.StampsReseller == (int)StampsResellerType.Express1)
            {
                throw new StampsException("Express1 accounts cannot be converted.");
            }

            accountToConvert = stampsAccount;
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

                // Convert the account with the Stamps.com API and update the account entity's 
                // contract type to reflect the conversion, so the Activate discount dialog is 
                // not displayed again
                ConvertAccountToExpedited();

                // Set the ContractType to Unknown so that we rely on Stamps to correctly tell us 
                // the contract type the next time we get rates or process.
                accountToConvert.ContractType = (int)StampsAccountContractType.Unknown;

                StampsAccountManager.SaveAccount(accountToConvert);

                // Notify any listeners
                if (AccountConverted != null)
                {
                    AccountConverted(this, new UspsAccountConvertedEventArgs(accountToConvert));
                }
            }
            catch (StampsException exception)
            {
                string message = "An error occurred, and ShipWorks was not able to convert your account at this time.";
                log.Error(string.Format("An error occurred trying to convert the Stamps.com account ({0}) to get discounted postage.", accountToConvert.Username), exception);

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
                log.InfoFormat("Converting Stamps.com account ({0}) to get discounted postage.", accountToConvert.Username);

                IRegistrationPromotion promotion = new RegistrationPromotionFactory().CreateRegistrationPromotion();
                new StampsApiSession().ChangeToExpeditedPlan(accountToConvert, promotion.GetPromoCode(PostalAccountRegistrationType.Expedited)); 
            }
            catch (StampsApiException exception)
            {
                if (exception.Code == 0x005f0302)
                {
                    // The account is already converted, so there's nothing to do here
                    log.WarnFormat("The Stamps.com account ({0}) has already been converted.", accountToConvert.Username);                    
                }
                else
                {
                    log.Error(string.Format("An error occurred trying to convert the Stamps.com account ({0}) to get discounted postage.", accountToConvert.Username), exception);
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
                                          "With IntuiShip you get some of the best postal rates available, saving you significant money on each of your domestic and " +
                                          "international Priority and Express shipments." + Environment.NewLine + Environment.NewLine + "Just add these rates to your " +
                                          "Stamps.com account and ShipWorks will automatically utilize it for discounted rates from IntuiShip when creating postage labels." +
                                          Environment.NewLine + Environment.NewLine + "For more information, please contact us at www.interapptive.com/company/contact.html.");
        }
    }
}
