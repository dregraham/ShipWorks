using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.GlobalShipAddress;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.GlobalShipAddress.Request.Manipulators
{
    /// <summary>
    /// Adds ClientDetail to GlobalShipAddress request
    /// </summary>
    public class FedExGlobalShipAddressClientDetailManipulator : IFedExGlobalShipAddressRequestManipulator
    {
        private readonly IFedExSettingsRepository settingsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExGlobalShipAddressClientDetailManipulator" /> class.
        /// </summary>
        public FedExGlobalShipAddressClientDetailManipulator(IFedExSettingsRepository settingsRepository)
        {
            this.settingsRepository = settingsRepository;
        }

        /// <summary>
        /// Manipulate the request
        /// </summary>
        public GenericResult<SearchLocationsRequest> Manipulate(IShipmentEntity shipment, SearchLocationsRequest request)
        {
            var account = settingsRepository.GetAccountReadOnly(shipment);

            if (account == null)
            {
                return new CarrierException("A FedEx account is required to select a Hold at location.");
            }

            var settings = new FedExSettings(settingsRepository);

            request.ClientDetail = new ClientDetail()
            {
                AccountNumber = account.AccountNumber,
                ClientProductId = settings.ClientProductId,
                ClientProductVersion = settings.ClientProductVersion,
                MeterNumber = account.MeterNumber
            };

            return request;
        }
    }
}
