using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Interapptive.Shared.Business;
using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Shipping.Carriers.Postal.Express1.WebServices.CustomerService;

namespace ShipWorks.Shipping.Carriers.Postal.Express1.Registration
{
    /// <summary>
    /// Gateway class for integrating with Express1
    /// </summary>
    public abstract class Express1RegistrationGateway : IExpress1RegistrationGateway
    {
        private readonly IExpress1ConnectionDetails connectionDetails;

        /// <summary>
        /// Create a new instance of the Express1 gateway
        /// </summary>
        /// <param name="connectionDetails">Details that define the connection to use</param>
        protected Express1RegistrationGateway(IExpress1ConnectionDetails connectionDetails)
        {
            this.connectionDetails = connectionDetails;
        }

        /// <summary>
        /// Verifies that the specified username and password map to a valid account
        /// </summary>
        /// <param name="registration">Registration that defines the account to test</param>
        public abstract void VerifyAccount(Express1Registration registration);

        /// <summary>
        /// Registers an account with Express1.
        /// </summary>
        /// <param name="registration">The registration data.</param>
        /// <returns>Returns the result of the registration call to Express1 containing the
        /// customer credentials that were just created.</returns>
        /// <exception cref="System.ArgumentNullException">registration</exception>
        public Express1RegistrationResult Register(Express1Registration registration)
        {
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }

            try
            {
                CustomerRegistrationData customerData = CreateExpress1ApiRegistration(registration);

                // populate security information
                customerData.SecurityInfo = GetSecurityInfo();

                using (CustomerService service = CreateCustomerService("Signup"))
                {
                    CustomerCredentials credentials = service.RegisterCustomer(connectionDetails.ApiKey, GetCustomerInfoString(Guid.NewGuid().ToString()), customerData);

                    return new Express1RegistrationResult()
                    {
                        AccountNumber = credentials.AccountID,
                        Password = credentials.PassPhrase
                    };
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(Express1RegistrationException));
            }
        }

        /// <summary>
        /// Creates the registration structure needed by the Express1 API
        /// </summary>
        /// <param name="registration">ShipWorks version of the Express1 registration data</param>
        /// <returns></returns>
        private static CustomerRegistrationData CreateExpress1ApiRegistration(Express1Registration registration)
        {
            CustomerRegistrationData customerData = new CustomerRegistrationData();

            // TOS
            customerData.TermsAcceptedSpecified = true;
            customerData.TermsAccepted = true;

            // Contact Info
            customerData.Email = registration.Email;
            customerData.Phone = registration.Phone10Digits;
            customerData.ContactName = registration.Name;
            customerData.CompanyName = registration.Company;

            // Mailing address
            customerData.MailingAddress = CreateAddressInfo(registration.MailingAddress);

            // Payment for postage
            customerData.PaymentType = PaymentType.CreditCard;
            customerData.PaymentTypeSpecified = true;

            customerData.CreditCard = new CreditCardInfo();

            // card details
            customerData.CreditCard.CardNumber = registration.Payment.CreditCardAccountNumber;
            customerData.CreditCard.CardType = registration.Payment.ApiCardType;
            customerData.CreditCard.NameOnCard = registration.Name;
            customerData.CreditCard.CardTypeSpecified = true;
            customerData.CreditCard.ExpirationMonth = registration.Payment.CreditCardExpirationDate.Month.ToString("00");
            customerData.CreditCard.ExpirationYear = registration.Payment.CreditCardExpirationDate.Year.ToString();

            // card billing address
            customerData.CreditCard.BillingAddress = CreateAddressInfo(registration.Payment.CreditCardBillingAddress);

            return customerData;
        }

        /// <summary>
        /// Customer Service creation
        /// </summary>
        private CustomerService CreateCustomerService(string logName)
        {
            // configure the endpoint
            return new CustomerService(new ApiLogEntry(connectionDetails.ApiLogSource, logName))
                {
                    Url = connectionDetails.TestServer
                              ? "https://www.express1dev.com/Services/CustomerService.svc"
                              : "https://service.express1.com/Services/CustomerService.svc"
                };
        }

        /// <summary>
        /// Encrypts a string based on Express1's requirements 
        /// </summary>
        private string EncryptData(string dataString)
        {
            using (Rijndael rijndael = Rijndael.Create())
            {
                // per Express1
                rijndael.BlockSize = 128;
                rijndael.KeySize = 256;
                rijndael.Mode = CipherMode.CBC;
                rijndael.Padding = PaddingMode.PKCS7;

                // our secret stuff
                string key = connectionDetails.CertificateId.Substring(0, 32);
                string iv = "F9sl139fakvN#1kl";

                byte[] keyBytes = Encoding.ASCII.GetBytes(key);
                byte[] ivBytes = Encoding.ASCII.GetBytes(iv);
                byte[] dataBytes = Encoding.ASCII.GetBytes(dataString);

                using (MemoryStream memStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memStream, rijndael.CreateEncryptor(keyBytes, ivBytes), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(dataBytes, 0, dataBytes.Length);
                        cryptoStream.Close();

                        // return the encrypted data, base64 encoded
                        return Convert.ToBase64String(memStream.ToArray());
                    }
                }
            }
        }

        /// <summary>
        /// Get the encrypted customerInfo string
        /// </summary>
        public string GetCustomerInfoString(string customerID)
        {
            // create the tokenized string to encrypt
            string info = string.Format("{0}|{1}|Account", connectionDetails.FranchiseId, customerID);

            // encrypt per Express1's requirements
            return EncryptData(info);
        }

        /// <summary>
        /// Creates the SecurityInfo data to send to Express1 along with registration
        /// for fraud detection purposes
        /// </summary>
        private static SecurityInfo GetSecurityInfo()
        {
            return new SecurityInfo
                {
                    RemoteIPAddress = new NetworkUtility().GetPublicIPAddress(),
                    SessionID = ShipWorksSession.InstanceID.ToString(),
                    UserAgent = "ShipWorks"
                };
        }

        /// <summary>
        /// Populates an AddressInfo with data from a an account entity
        /// </summary>
        private static AddressInfo CreateAddressInfo(PersonAdapter fromAddress)
        {
            return new AddressInfo
            {
                Address1 = fromAddress.Street1,
                Address2 = fromAddress.Street2,
                City = fromAddress.City,
                PostalCode = fromAddress.PostalCode,
                State = fromAddress.StateProvCode
            };
        }
    }
}
