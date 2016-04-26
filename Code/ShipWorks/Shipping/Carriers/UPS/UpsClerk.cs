using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS.InvoiceRegistration;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.WebServices.Registration;
using ShipWorks.Shipping.Carriers.UPS.OpenAccount.Api;
using ShipWorks.Shipping.Carriers.UPS.WebServices.OpenAccount;
using System;
using System.Linq;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// UpsClerk used to communicate with the UpsWebService to OpenAccount and Register
    /// </summary>
    public class UpsClerk : IUpsClerk
    {
        private const string NeedsInvoiceAuthStatusCode = "045";
        private const string DuplicateUserIDErrorCode = "9570100";
        private readonly IUpsOpenAccountRequestFactory openAccountRequestFactory;
        private readonly IUpsInvoiceRegistrationRequestFactory invoiceRegistrationRequestFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsClerk" /> class.
        /// </summary>
        public UpsClerk(
            UpsAccountEntity upsAccount,
            Func<UpsAccountEntity, IUpsOpenAccountRequestFactory> openAccountRequestFactory,
            IUpsInvoiceRegistrationRequestFactory invoiceRegistrationRequestFactory)
        {
            this.openAccountRequestFactory = openAccountRequestFactory(upsAccount);
            this.invoiceRegistrationRequestFactory = invoiceRegistrationRequestFactory;
        }

        /// <summary>
        /// Opens a new account with UPS. This will attempt to create a new account
        /// on the UPS system and link that account with the customer's profile
        /// </summary>
        public OpenAccountResponse OpenAccount(OpenAccountRequest request)
        {
            CarrierRequest openAccountRequest = openAccountRequestFactory.CreateOpenAccountRequest(request);

            ICarrierResponse carrierResponse = openAccountRequest.Submit();
            carrierResponse.Process();

            OpenAccountResponse nativeResponse = (OpenAccountResponse)carrierResponse.NativeResponse;

            // This links the newly created account to the customer's profile
            CarrierRequest linkAccountRequest = openAccountRequestFactory.CreateLinkNewAccountRequestFactory();
            ICarrierResponse linkAccountResponse = linkAccountRequest.Submit();
            linkAccountResponse.Process();

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
            // Three attempts are made to create the account in case there are user ID collisions
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    CarrierRequest registrationRequest = invoiceRegistrationRequestFactory.CreateInvoiceRegistrationRequest(accountEntity, invoiceAuthorizationData);

                    ICarrierResponse response = registrationRequest.Submit();
                    response.Process();

                    RegisterResponse nativeResponse = response.NativeResponse as RegisterResponse;

                    if (nativeResponse != null && nativeResponse.ShipperAccountStatus.Any(s => s.Code == NeedsInvoiceAuthStatusCode))
                    {
                        // The shipper code is 045 which means that we did not send
                        // invoice info but the account requires invoice info
                        return UpsRegistrationStatus.InvoiceAuthenticationRequired;
                    }

                    return UpsRegistrationStatus.Success;
                }
                catch (UpsWebServiceException upsEx)
                {
                    if (upsEx.Code != DuplicateUserIDErrorCode || i == 2)
                    {
                        // An exception occurred not related to the uniqueness of the User ID
                        // or we have done this 3 times...
                        throw;
                    }
                }
            }

            // This should never be able to happen. Either we returned above or threw above.
            throw new InvalidOperationException();
        }
    }
}
