using System;
using System.Collections.Generic;
using Autofac.Features.Indexed;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS.OpenAccount.Api.Request;
using ShipWorks.Shipping.Carriers.UPS.OpenAccount.Api.Request.Manipulators;
using ShipWorks.Shipping.Carriers.UPS.UpsEnvironment;
using ShipWorks.Shipping.Carriers.UPS.WebServices.OpenAccount;

namespace ShipWorks.Shipping.Carriers.UPS.OpenAccount.Api
{
    public class UpsOpenAccountRequestFactory : IUpsOpenAccountRequestFactory
    {
        private readonly UpsAccountEntity upsAccount;
        private readonly IUpsServiceGateway upsOpenAccountService;
        private readonly ICarrierResponseFactory responseFactory;

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="UpsOpenAccountRequestFactory" /> class. This
        /// constructor is primarily for testing purposes.
        /// </summary>
        /// <param name="upsOpenAccountService">The UpsOpenAccount service.</param>
        /// <param name="responseFactoryIndex">The response factory.</param>
        /// <param name="upsAccount">The ups account.</param>
        public UpsOpenAccountRequestFactory(IUpsServiceGateway upsOpenAccountService, IIndex<ShipmentTypeCode, ICarrierResponseFactory> responseFactoryIndex, UpsAccountEntity upsAccount)
        {
            this.upsOpenAccountService = upsOpenAccountService;
            this.responseFactory = responseFactoryIndex[ShipmentTypeCode.UpsOnLineTools];
            this.upsAccount = upsAccount;
        }

        /// <summary>
        /// Creates a request for opening a new account on UPS.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>A CarrierRequest object that can be used for submitting a request to
        /// to open a new UPS account via the UpsOpenAccount API.</returns>
        public CarrierRequest CreateOpenAccountRequest(OpenAccountRequest request)
        {
            List<ICarrierRequestManipulator> requestManipulators = new List<ICarrierRequestManipulator>()
            {
                new UpsOpenAccountAddEndUserInformation()
            };

            return new UpsOpenAccountRequest(requestManipulators, upsOpenAccountService, responseFactory, request, upsAccount);
        }
    }
}
