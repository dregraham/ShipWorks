using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS.OpenAccount.Api.Request;
using ShipWorks.Shipping.Carriers.UPS.UpsEnvironment;
using ShipWorks.Shipping.Carriers.UPS.WebServices.OpenAccount;

namespace ShipWorks.Shipping.Carriers.UPS.OpenAccount.Api
{
    public class UpsOpenAccountRequestFactory : IUpsOpenAccountRequestFactory
    {
        private readonly IUpsServiceGateway upsOpenAccountService;
        private readonly ICarrierResponseFactory responseFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsOpenAccountRequestFactory" /> class.
        /// </summary>
        public UpsOpenAccountRequestFactory()
            : this(new UpsServiceGateway(new UpsSettingsRepository()), new UpsResponseFactory())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsOpenAccountRequestFactory" /> class. This
        /// constructor is primarily for testing purposes.
        /// </summary>
        /// <param name="upsOpenAccountService">The UpsOpenAccount service.</param>
        /// <param name="responseFactory">The response factory.</param>
        public UpsOpenAccountRequestFactory(IUpsServiceGateway upsOpenAccountService, ICarrierResponseFactory responseFactory)
        {
            this.upsOpenAccountService = upsOpenAccountService;
            this.responseFactory = responseFactory;
        }

        /// <summary>
        /// Creates a request for opening a new account on UPS.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>A CarrierRequest object that can be used for submitting a request to
        /// to open a new UPS account via the UpsOpenAccount API.</returns>
        public CarrierRequest CreateOpenAccountRequest(OpenAccountRequest request)
        {
            return new UpsOpenAccountRequest(upsOpenAccountService, responseFactory, request);
        }
    }
}
