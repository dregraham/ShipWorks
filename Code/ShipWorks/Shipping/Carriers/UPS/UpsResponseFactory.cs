using System.Collections.Generic;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS.InvoiceRegistration.Api.Response;
using ShipWorks.Shipping.Carriers.UPS.InvoiceRegistration.Api.Response.Manipulators;
using ShipWorks.Shipping.Carriers.UPS.OpenAccount.Api.Response;
using ShipWorks.Shipping.Carriers.UPS.OpenAccount.Api.Response.Manipulators;
using ShipWorks.Shipping.Carriers.UPS.WebServices.OpenAccount;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.WebServices.Registration;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// UPS ICarrierResponseFactory implementation for creating response to UPS API calls
    /// </summary>
    public class UpsResponseFactory : ICarrierResponseFactory
    {
        /// <summary>
        /// Creates an ICarrierResponse that will be to represent the carrier-specific result of a
        /// carrier API request when subscribe a shipper to UpsOpenAccount use the UpsOpenAccount services.
        /// </summary>
        /// <param name="nativeResponse">The native response.</param>
        /// <param name="request">The request object that submitted the API request.</param>
        /// <returns>An ICarrierResponse representing the response of a void request.</returns>
        public ICarrierResponse CreateSubscriptionResponse(object nativeResponse, CarrierRequest request)
        {
            OpenAccountResponse openAccountResponse = nativeResponse as OpenAccountResponse;
            if (openAccountResponse == null)
            {
                // We can't create a UpsOpenAccountResponse without a OpenAccountResponse type
                throw new CarrierException("An unexpected response type was provided to create a UpsOpenAccountSubscriptionResponse.");
            }

            List<ICarrierResponseManipulator> manipulators = new List<ICarrierResponseManipulator>()
            {
                new UpsOpenAccountCreateUpsAccountEntityManipulator()
            };

            return new UpsOpenAccountResponse(openAccountResponse, request, manipulators);
        }

        /// <summary>
        /// Creates an ICarrierResponse that will be to represent the carrier-specific result of a
        /// carrier API request when registering a new user.
        /// </summary>
        /// <param name="nativeResponse">The native response.</param>
        /// <param name="request">The request object that submitted the API request.</param>
        /// <returns>
        /// An ICarrierResponse representing the response of a void request.
        /// </returns>
        /// <exception cref="CarrierException">An unexpected response type was provided to create a UpsInvoiceRegistrationResponse.</exception>
        public ICarrierResponse CreateRegisterUserResponse(object nativeResponse, CarrierRequest request)
        {
            RegisterResponse registerResponse = nativeResponse as RegisterResponse;
            if (registerResponse == null)
            {
                // We can't create a UpsInvoiceRegistrationResponse without a OpenAccountResponse type
                throw new CarrierException("An unexpected response type was provided to create a UpsInvoiceRegistrationResponse.");
            }

            List<ICarrierResponseManipulator> manipulators = new List<ICarrierResponseManipulator>()
            {
                new UpsSaveCredentialsManipulator()
            };

            return new UpsInvoiceRegistrationResponse(registerResponse, request, manipulators);
        }
    }
}
