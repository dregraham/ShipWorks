using System;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using ShipWorks.Shipping.Carriers.OnTrac.Schemas.Shipment;
using ShipWorks.Shipping.Carriers.UPS.Enums;
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
        public OpenAccountRequest OpenAccountRequest { private get; set; }

        /// <summary>
        /// Gets or sets the invalid shipping address.
        /// </summary>
        /// <value>
        /// The invalid shipping address.
        /// </value>
        public Action<string> AccountCreated { private get; set; }

        /// <summary>
        /// Clicks the create UPS account.
        /// </summary>
        private void ClickCreateUpsAccount(object sender, EventArgs e)
        {
            UpsClerk clerk = new UpsClerk();
            string shipperNumber = CreateUpsAccount(clerk);

            if (!string.IsNullOrEmpty(shipperNumber))
            {
                AccountCreated(shipperNumber);
            }
        }

        /// <summary>
        /// Creates the ups account. Note the recursive call to correct the address.
        /// </summary>
        /// <param name="clerk">The clerk.</param>
        /// <returns></returns>
        private string CreateUpsAccount(IUpsClerk clerk)
        {
            return CreateUpsAccount(clerk, false);
        }

        /// <summary>
        /// Creates the ups account. Note the recursive call to correct the address.
        /// </summary>
        /// <param name="clerk">The clerk.</param>
        /// <returns></returns>
        private string CreateUpsAccount(IUpsClerk clerk, bool retrySmartPost)
        {
            string shipperNumber = string.Empty;

            try
            {
                OpenAccountResponse response = clerk.OpenAccount(OpenAccountRequest);

                shipperNumber = response.ShipperNumber;

                if (string.IsNullOrEmpty(shipperNumber))
                {
                    throw new UpsOpenAccountException("Ups didn't return a new account number.");
                }
            }
            catch (UpsOpenAccountPickupAddressException ex)
            {
                if (CorrectPickupAddress(ex.SuggestedAddress, OpenAccountRequest.PickupAddress))
                {
                    shipperNumber = CreateUpsAccount(clerk);
                }
            }
            catch (UpsOpenAccountBusinessAddressException ex)
            {
                if (CorrectBillingAddress(ex.SuggestedAddress, OpenAccountRequest.BillingAddress))
                {
                    shipperNumber = CreateUpsAccount(clerk);
                }
            }
            catch (UpsOpenAccountSoapException ex)
            {
                MessageHelper.ShowError(this, string.Format("Ups returned the following error: {0}", ex.Message));
            }
            catch (UpsOpenAccountException ex)
            {
                if (ex.ErrorCode == UpsOpenAccountErrorCode.SmartPickupError && !retrySmartPost)
                {
                    string correctedAddress = UpsUtility.CorrectSmartPickupError(OpenAccountRequest.PickupAddress.City);

                    if (!string.IsNullOrEmpty(correctedAddress))
                    {
                        OpenAccountRequest.PickupAddress.City = correctedAddress;

                        shipperNumber = CreateUpsAccount(clerk, true);
                    }

                    MessageHelper.ShowError(this, "UPS couldn't resolve the pickup address. If there are alternate spellings, try again using one of those.");
                }
                else
                {
                    MessageHelper.ShowError(this, ex.Message);   
                }
            }

            return shipperNumber;
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

            using (UpsOpenAccountInvalidAddressDlg invalidAddressDlg = new UpsOpenAccountInvalidAddressDlg())
            {
                invalidAddressDlg.SetAddress(addressCandidate, "Pickup");
                DialogResult result = invalidAddressDlg.ShowDialog();

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

            using (UpsOpenAccountInvalidAddressDlg invalidAddressDlg = new UpsOpenAccountInvalidAddressDlg())
            {
                invalidAddressDlg.SetAddress(addressCandidate, "Billing");
                DialogResult result = invalidAddressDlg.ShowDialog();

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