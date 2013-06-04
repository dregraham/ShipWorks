using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;

namespace ShipWorks.Shipping.Carriers.UPS.InvoiceRegistration
{
    public interface IUpsInvoiceRegistrationRequestFactory
    {
        /// <summary>
        /// Creates the invoice registration request.
        /// </summary>
        /// <returns></returns>
        CarrierRequest CreateInvoiceRegistrationRequest(UpsAccountEntity account, UpsOltInvoiceAuthorizationData invoiceAuthorizationData);
    }
}