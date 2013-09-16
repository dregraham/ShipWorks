using System;
using Interapptive.Shared.Business;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Account;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;
using ShipWorks.Shipping.Carriers.Postal.Express1;
using ShipWorks.Shipping.Carriers.Postal.Express1.WebServices.CustomerService;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps.Express1
{
    public class Express1StampsRegistration : Express1Registration
    {
        private StampsAccountEntity account;

        /// <summary>
        /// Creates a new Express1StampsRegistration instance
        /// </summary>
        public Express1StampsRegistration()
            : base(new Express1RegistrationGateway(new Express1StampsConnectionDetails()))
        {
        }

        /// <summary>
        /// Gets the shipment type code associated with this registration
        /// </summary>
        public override ShipmentTypeCode ShipmentTypeCode
        {
            get { return ShipmentTypeCode.Express1Stamps; }
        }

        /// <summary>
        /// Saves the account
        /// </summary>
        public override void SaveAccount()
        {
            account = new StampsAccountEntity { IsExpress1 = true };

            StampsAccountManager.SaveAccount(account);
        }

        public override void Signup(PersonAdapter accountAddress, Express1PaymentInfo paymentInfo)
        {
            if (accountAddress == null)
            {
                throw new ArgumentNullException("accountAddress");
            }

            if (paymentInfo == null)
            {
                throw new ArgumentNullException("paymentInfo");
            }

            CustomerRegistrationData customerData = new CustomerRegistrationData();

            // TOS
            customerData.TermsAcceptedSpecified = true;
            customerData.TermsAccepted = true;

            // Contact Info
            customerData.Email = accountAddress.Email;
            customerData.Phone = accountAddress.Phone10Digits;
            customerData.ContactName = new PersonName(accountAddress.FirstName, accountAddress.MiddleName, accountAddress.LastName).FullName;
            customerData.CompanyName = accountAddress.Company;

            // Mailing address
            customerData.MailingAddress = CreateAddressInfo(accountAddress);

            // Payment for postage
            customerData.PaymentType = PaymentType.CreditCard;
            customerData.PaymentTypeSpecified = true;

            customerData.CreditCard = new CreditCardInfo();

            // card detials
            customerData.CreditCard.CardNumber = paymentInfo.CreditCardAccountNumber;
            customerData.CreditCard.CardType = paymentInfo.ApiCardType;
            customerData.CreditCard.NameOnCard = customerData.ContactName;
            customerData.CreditCard.CardTypeSpecified = true;
            customerData.CreditCard.ExpirationMonth = paymentInfo.CreditCardExpirationDate.Month.ToString("00");
            customerData.CreditCard.ExpirationYear = paymentInfo.CreditCardExpirationDate.Year.ToString();

            // card billing address
            customerData.CreditCard.BillingAddress = CreateAddressInfo(paymentInfo.CreditCardBillingAddress);

            CustomerCredentials returnedCredentials = RegistrationGateway.Register(customerData);

            // Save the address 
            PersonAdapter.Copy(accountAddress, new PersonAdapter(account, ""));
            account.MailingPostalCode = account.PostalCode;

            // Account type and passwords
            account.Password = SecureText.Encrypt(returnedCredentials.PassPhrase, "Stamps");
            account.AccountNumber = returnedCredentials.AccountID;

            //TODO: Address the other data that we're saving for Endicia
            //// record if this is a Test account
            //account.TestAccount = Express1EndiciaUtility.UseTestServer;

            //// other data
            //account.CreatedByShipWorks = true;
            //account.AccountType = (int)EndiciaAccountType.Standard;
            //account.ApiInitialPassword = "";
            //account.SignupConfirmation = "";
            //account.WebPassword = "";
            //account.ScanFormAddressSource = (int)EndiciaScanFormAddressSource.Provider;

            //// Deafult description.  Blank until we get an account number
            //account.Description = EndiciaAccountManager.GetDefaultDescription(account);

            SaveAccount();
        }
    }
}
