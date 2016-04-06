using System.Collections.Generic;
using Interapptive.Shared.Net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS.InvoiceRegistration.Api.Request;
using ShipWorks.Shipping.Carriers.UPS.InvoiceRegistration.Api.Request.Manipulators;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Carriers.UPS.UpsEnvironment;

namespace ShipWorks.Shipping.Carriers.UPS.InvoiceRegistration
{
    public class UpsInvoiceRegistrationRequestFactory : IUpsInvoiceRegistrationRequestFactory
    {
        private readonly UpsResponseFactory upsResponseFactory;
        private readonly IUpsServiceGateway upsServiceGateway;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsInvoiceRegistrationRequestFactory" /> class.
        /// </summary>
        public UpsInvoiceRegistrationRequestFactory()
            : this(new UpsServiceGateway(new UpsSettingsRepository()), new UpsResponseFactory())
        { }

        /// <summary>
        /// Prevents a default instance of the <see cref="UpsInvoiceRegistrationRequestFactory"/> class from being created.
        /// </summary>
        /// <param name="upsServiceGateway">The ups service gateway.</param>
        /// <param name="upsResponseFactory">The ups response factory.</param>
        private UpsInvoiceRegistrationRequestFactory(IUpsServiceGateway upsServiceGateway, UpsResponseFactory upsResponseFactory)
        {
            this.upsServiceGateway = upsServiceGateway;
            this.upsResponseFactory = upsResponseFactory;
        }

        /// <summary>
        /// Creates the invoice registration request.
        /// </summary>
        /// <returns></returns>
        public CarrierRequest CreateInvoiceRegistrationRequest(UpsAccountEntity account, UpsOltInvoiceAuthorizationData invoiceAuthorizationData)
        {
            var requestManipulators = new List<ICarrierRequestManipulator>
            {
                new UpsInvoiceRegistrationAddressManipulator(account, new NetworkUtility()),
                new UpsInvoiceRegistrationInvoiceInfoManipulator(invoiceAuthorizationData),
                new UpsInvoiceRegistrationNewProfileCredentialsManipulator(),
                new UpsInvoiceRegistrationShipperInfoManipulator(account)
            };

            return new UpsInvoiceRegistrationRequest(requestManipulators, upsServiceGateway, upsResponseFactory, account);
        }
    }
}