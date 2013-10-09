using System;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Environment;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.GlobalShipAddress;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.v2013.GlobalShipAddress.Request.Manipulators
{
    /// <summary>
    /// Adds ClientDetail to GlobalShipAddress request
    /// </summary>
    public class FedExGlobalShipAddressClientDetailManipulator: ICarrierRequestManipulator
    {
        private readonly FedExSettings fedExSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExGlobalShipAddressClientDetailManipulator" /> class.
        /// </summary>
        public FedExGlobalShipAddressClientDetailManipulator(ICarrierSettingsRepository settingsRepository)
        {
            fedExSettings = new FedExSettings(settingsRepository);
        }

        public void Manipulate(CarrierRequest request)
        {
            ValidateRequest(request);

            SearchLocationsRequest nativeRequest = request.NativeRequest as SearchLocationsRequest;

            FedExAccountEntity account = request.CarrierAccountEntity as FedExAccountEntity;
            
            nativeRequest.ClientDetail = new ClientDetail()
            {
                AccountNumber = account.AccountNumber,
                ClientProductId = fedExSettings.ClientProductId,
                ClientProductVersion = fedExSettings.ClientProductVersion,
                MeterNumber = account.MeterNumber
            };
        }

        private void ValidateRequest(CarrierRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            // The native FedEx request type should be a PostalCodeInquiryRequest
            SearchLocationsRequest nativeRequest = request.NativeRequest as SearchLocationsRequest;
            if (nativeRequest == null)
            {
                // Abort - we have an unexpected native request
                throw new CarrierException("An unexpected request type was provided.");
            }
        }
    }
}
