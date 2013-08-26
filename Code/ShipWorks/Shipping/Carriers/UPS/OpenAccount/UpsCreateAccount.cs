using System;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using ShipWorks.Shipping.Carriers.UPS.WebServices.OpenAccount;

namespace ShipWorks.Shipping.Carriers.UPS.OpenAccount
{
    public partial class UpsCreateAccount : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpsCreateAccount" /> class.
        /// </summary>
        public UpsCreateAccount()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the open account request.
        /// </summary>
        public OpenAccountRequest OpenAccountRequest
        {
            private get;
            set;
        }

        /// <summary>
        /// Gets or sets the invalid shipping address.
        /// </summary>
        /// <value>
        /// The invalid shipping address.
        /// </value>
        public Action AccountCreated
        {
            private get;
            set;
        }

        /// <summary>
        /// Clicks the create UPS account.
        /// </summary>
        private void ClickCreateUpsAccount(object sender, EventArgs e)
        {
            var clerk = new UpsClerk();
            if (CreateUpsAccount(clerk))
            {
                AccountCreated();
            }
        }

        /// <summary>
        /// Creates the ups account. Note the recursive call to correct the address.
        /// </summary>
        /// <param name="clerk">The clerk.</param>
        /// <returns></returns>
        private bool CreateUpsAccount(UpsClerk clerk)
        {
            bool isAccountCreated = false;
            try
            {
                clerk.OpenAccount(OpenAccountRequest);
                isAccountCreated = true;
            }
            catch (UpsOpenAccountPickupAddressException ex)
            {
                if (CorrectPickupAddress(ex.SuggestedAddress, OpenAccountRequest.PickupAddress))
                {
                    isAccountCreated = CreateUpsAccount(clerk);
                }
            }
            catch (UpsOpenAccountBusinessAddressException ex)
            {
                if (CorrectBillingAddress(ex.SuggestedAddress, OpenAccountRequest.BillingAddress))
                {
                    isAccountCreated = CreateUpsAccount(clerk);
                }

            }
            catch (UpsOpenAccountSoapException ex)
            {
                MessageHelper.ShowError(this, string.Format("Ups returned the following error: {0}", ex.Message));
            }
            catch (UpsOpenAccountException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
            }

            return isAccountCreated;
        }

        /// <summary>
        /// Validates the pickup address.
        /// </summary>
        /// <param name="addressCandidate">The address candidate.</param>
        /// <param name="pickupAddressType">Type of the pickup address.</param>
        /// <returns></returns>
        /// <exception cref="ShipWorks.Shipping.Carriers.UPS.OpenAccount.UpsOpenAccountInvalidAddressException"></exception>
        private static bool CorrectPickupAddress(AddressKeyCandidateType addressCandidate, PickupAddressType pickupAddressType)
        {
            bool isAddressCorrected = false;

            using (var invalidAddressDlg = new UpsOpenAccountInvalidAddressDlg())
            {
                invalidAddressDlg.SetAddress(addressCandidate, "Pickup");
                var result = invalidAddressDlg.ShowDialog();

                if (result == DialogResult.OK)
                {
                    pickupAddressType.StreetAddress = addressCandidate.StreetAddress;
                    pickupAddressType.City = addressCandidate.City;
                    pickupAddressType.StateProvinceCode = addressCandidate.State;
                    pickupAddressType.PostalCode = addressCandidate.PostalCode;
                    pickupAddressType.CountryCode = addressCandidate.CountryCode;

                    isAddressCorrected = true;
                }
            }

            return isAddressCorrected;
        }

        /// <summary>
        /// Validates the address.
        /// </summary>
        /// <param name="addressCandidate">The address candidate.</param>
        /// <param name="billingAddressType">Type of the billing address.</param>
        /// <returns></returns>
        /// <exception cref="ShipWorks.Shipping.Carriers.UPS.OpenAccount.UpsOpenAccountInvalidAddressException">If address suggested and user cancels, throw</exception>
        private static bool CorrectBillingAddress(AddressKeyCandidateType addressCandidate, BillingAddressType billingAddressType)
        {
            bool isAddressCorrected = false;

            using (var invalidAddressDlg = new UpsOpenAccountInvalidAddressDlg())
            {
                invalidAddressDlg.SetAddress(addressCandidate, "Billing");
                var result = invalidAddressDlg.ShowDialog();

                if (result == DialogResult.OK)
                {
                    billingAddressType.StreetAddress = addressCandidate.StreetAddress;
                    billingAddressType.City = addressCandidate.City;
                    billingAddressType.StateProvinceCode = addressCandidate.State;
                    billingAddressType.PostalCode = addressCandidate.PostalCode;
                    billingAddressType.CountryCode = addressCandidate.CountryCode;

                    isAddressCorrected = true;
                }
            }

            return isAddressCorrected;
        }
    }
}