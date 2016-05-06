using System.Collections.Generic;
using Autofac.Features.Indexed;
using Interapptive.Shared.Net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS.InvoiceRegistration.Api.Request;
using ShipWorks.Shipping.Carriers.UPS.InvoiceRegistration.Api.Request.Manipulators;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;

namespace ShipWorks.Shipping.Carriers.UPS.InvoiceRegistration
{
    public class UpsInvoiceRegistrationRequestFactory : IUpsInvoiceRegistrationRequestFactory
    {
        private readonly ICarrierResponseFactory upsResponseFactory;
        private readonly IUpsServiceGateway upsServiceGateway;

        /// <summary>
        /// Prevents a default instance of the <see cref="UpsInvoiceRegistrationRequestFactory"/> class from being created.
        /// </summary>
        /// <param name="upsServiceGateway">The ups service gateway.</param>
        /// <param name="responseFactoryIndex">The ups response factory.</param>
        public UpsInvoiceRegistrationRequestFactory(IUpsServiceGateway upsServiceGateway, IIndex<ShipmentTypeCode, ICarrierResponseFactory> responseFactoryIndex)
        {
            this.upsServiceGateway = upsServiceGateway;
            upsResponseFactory = responseFactoryIndex[ShipmentTypeCode.UpsOnLineTools];
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