using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS.InvoiceRegistration;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
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
        /// Registers the account without invoice information.
        /// </summary>
        /// <param name="accountEntity">The account entity.</param>
        /// <exception cref="UpsException">A unique UserID could not be generated.  Please try again.</exception>
        public UpsRegistrationStatus RegisterAccount(UpsAccountEntity accountEntity)
        {
            return RegisterAccount(accountEntity, null);
        }

        /// <summary>
        /// Registers the account using the invoice information provided.
        /// </summary>
        /// <param name="accountEntity">The account entity.</param>
        /// <param name="invoiceAuthorizationData">The invoice authorization data.</param>
        /// <exception cref="UpsException">A unique UserID could not be generated.  Please try again.</exception>
        public UpsRegistrationStatus RegisterAccount(UpsAccountEntity accountEntity, UpsOltInvoiceAuthorizationData invoiceAuthorizationData)
        {
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
                    return UpsRegistrationStatus.Success;
                }
                catch (UpsWebServiceException upsEx)
                {
                    if (upsEx.Code == "9570100")
                    {
                        // The user ID provided is already taken; make a note of it so it can
                        // be used if the account was not created after three attempts
                        lastUniqueException = upsEx;
                    }
                    else if (upsEx.Code == "0495")
                    {
                        // Ups requires invoice information
                        return UpsRegistrationStatus.InvoiceAuthenticationRequired;
                    }
                    else
                    {
                        // An exception occurred not related to the uniqueness of the User ID
                        throw;
                    }
                }
            }

            if (!isAccountCreated)
            {
                throw new UpsException("A unique User ID could not be generated.  Please try again.", lastUniqueException);
            }

            return UpsRegistrationStatus.Failed;
        }
    }
}
