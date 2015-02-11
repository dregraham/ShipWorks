using System;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS.InvoiceRegistration;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using ShipWorks.Shipping.Carriers.UPS.OpenAccount;
using ShipWorks.Shipping.Carriers.UPS.OpenAccount.Api;
using ShipWorks.Shipping.Carriers.UPS.WebServices.OpenAccount;

namespace ShipWorks.Shipping.Carriers.UPS
{
    public class UpsClerk : IUpsClerk
    {
        private readonly IUpsOpenAccountRequestFactory openAccountRequestFactory;
        private readonly IUpsInvoiceRegistrationRequestFactory invoiceRegistrationRequestFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsClerk" /> class.
        /// </summary>
        public UpsClerk(UpsAccountEntity upsAccount)
            : this(new UpsOpenAccountRequestFactory(upsAccount), new UpsInvoiceRegistrationRequestFactory())
        { }


        /// <summary>
        /// Initializes a new instance of the <see cref="UpsClerk" /> class.
        /// </summary>
        /// <param name="openAccountRequestFactory">The open account request factory.</param>
        /// <param name="invoiceRegistrationRequestFactory">The invoice registration request factory.</param>
        public UpsClerk(IUpsOpenAccountRequestFactory openAccountRequestFactory,
            IUpsInvoiceRegistrationRequestFactory invoiceRegistrationRequestFactory)
        {
            this.openAccountRequestFactory = openAccountRequestFactory;
            this.invoiceRegistrationRequestFactory = invoiceRegistrationRequestFactory;
        }

        /// <summary>
        /// Opens a new account with UPS. This will attempt to create a new account
        /// on the UPS system.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The response from UPS.</returns>
        public OpenAccountResponse OpenAccount(OpenAccountRequest request)
        {
            CarrierRequest openAccountRequest = openAccountRequestFactory.CreateOpenAccountRequest(request);

            ICarrierResponse carrierResponse = openAccountRequest.Submit();
            carrierResponse.Process();
            
            OpenAccountResponse nativeResponse = (OpenAccountResponse)carrierResponse.NativeResponse;
            
            return nativeResponse;
        }

        /// <summary>
        /// Registers the account using the invoice information provided to get account based rates.
        /// </summary>
        /// <param name="accountEntity">The account entity.</param>
        /// <param name="invoiceAuthorizationData">The invoice authorization data.</param>
        /// <exception cref="UpsException">A unique UserID could not be generated.  Please try again.</exception>
        public void RegisterAccount(UpsAccountEntity accountEntity, UpsOltInvoiceAuthorizationData invoiceAuthorizationData)
        {
            ValidateInvoiceData(invoiceAuthorizationData);

            bool isAccountCreated = false;
            UpsWebServiceException lastUniqueException = null;

            // Three attempts are made to create the account in case there are user ID collisions
            for (int i = 0; i < 3 && !isAccountCreated; i++)
            {
                try
                {
                    CarrierRequest registrationRequest = invoiceRegistrationRequestFactory.CreateInvoiceRegistrationRequest(accountEntity, invoiceAuthorizationData);

                    ICarrierResponse response = registrationRequest.Submit();
                    response.Process();

                    isAccountCreated = true;
                }
                catch (UpsWebServiceException upsEx)
                {
                    if (upsEx.Code != "9570100")
                    {
                        // An exception occurred not related to the uniqueness of the User ID
                        throw;
                    }
                    else
                    {
                        // The user ID provided is already taken; make a note of it so it can
                        // be used if the account was not created after three attempts
                        lastUniqueException = upsEx;
                    }
                }
            }

            if (!isAccountCreated)
            {
                throw new UpsException("A unique User ID could not be generated.  Please try again.", lastUniqueException);
            }
        }

        /// <summary>
        /// Validates the invoice data - only validates that an invlice number is provided.
        /// </summary>
        /// <param name="invoiceAuthorizationData">The invoice authorization data.</param>
        /// <exception cref="UpsException"></exception>
        private static void ValidateInvoiceData(UpsOltInvoiceAuthorizationData invoiceAuthorizationData)
        {
            if (invoiceAuthorizationData == null)
            {
                throw new UpsException("Invoice authorization cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(invoiceAuthorizationData.InvoiceNumber))
            {
                throw new UpsException("An invoice number is required.");
            }
        }
    }
}
