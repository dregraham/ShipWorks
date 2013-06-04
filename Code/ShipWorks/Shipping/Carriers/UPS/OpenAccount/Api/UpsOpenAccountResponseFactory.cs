using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.OpenAccount.Api.Response;
using ShipWorks.Shipping.Carriers.UPS.OpenAccount.Api.Response.Manipulators;
using ShipWorks.Shipping.Carriers.UPS.WebServices.OpenAccount;

namespace ShipWorks.Shipping.Carriers.UPS.OpenAccount.Api
{
    /// <summary>
    /// An implementation of the ICarrierResponseFactory for UpsOpenAccount.
    /// </summary>
    public class UpsOpenAccountResponseFactory : ICarrierResponseFactory
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
                // We can't create a UpsOpenAccountSubscriptionResponse without a OpenAccountResponse type
                throw new CarrierException("An unexpected response type was provided to create a UpsOpenAccountSubscriptionResponse.");
            }

            List<ICarrierResponseManipulator> manipulators = new List<ICarrierResponseManipulator>()
            {
                new UpsOpenAccountCreateUpsAccountEntityManipulator()
            };

            return new UpsOpenAccountResponse(openAccountResponse, request, manipulators);
        }

        public ICarrierResponse CreateShipResponse(object nativeResponse, CarrierRequest request, ShipmentEntity shipmentEntity)
        {
            throw new NotImplementedException();
        }

        public ICarrierResponse CreateGlobalShipAddressResponse(object nativeResponse, CarrierRequest request)
        {
            throw new NotImplementedException();
        }

        public ICarrierResponse CreateGroundCloseResponse(object nativeResponse, CarrierRequest request)
        {
            throw new NotImplementedException();
        }

        public ICarrierResponse CreateSmartPostCloseResponse(object nativeResponse, CarrierRequest request)
        {
            throw new NotImplementedException();
        }

        public ICarrierResponse CreateVoidResponse(object nativeResponse, CarrierRequest request)
        {
            throw new NotImplementedException();
        }

        public ICarrierResponse CreateRegisterCspUserResponse(object nativeResponse, CarrierRequest request)
        {
            throw new NotImplementedException();
        }

        public ICarrierResponse CreateRateResponse(object nativeResponse, CarrierRequest request)
        {
            throw new NotImplementedException();
        }

        public ICarrierResponse CreateTrackResponse(object nativeResponse, CarrierRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
