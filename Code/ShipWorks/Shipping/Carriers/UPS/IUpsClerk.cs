using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Carriers.UPS.WebServices.OpenAccount;
using ShipWorks.Shipping.Carriers.UPS.InvoiceRegistration;

namespace ShipWorks.Shipping.Carriers.UPS
{
    public interface IUpsClerk
    {
        /// <summary>
        /// Opens a new account with UPS. This will attempt to create a new account
        /// on the UPS system.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The response from UPS.</returns>
        OpenAccountResponse OpenAccount(OpenAccountRequest request);

        /// <summary>
        /// Registers the account using the invoice information provided.
        /// </summary>
        /// <param name="accountEntity">The account entity.</param>
        /// <param name="invoiceAuthorizationData">The invoice authorization data.</param>
        UpsRegistrationStatus RegisterAccount(UpsAccountEntity accountEntity, UpsOltInvoiceAuthorizationData invoiceAuthorizationData);

        /// <summary>
        /// Registers the account without invoice information
        /// </summary>
        /// <param name="accountEntity">The account entity.</param>
        UpsRegistrationStatus RegisterAccount(UpsAccountEntity accountEntity);
    }
}
