using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.GlobalShipAddress;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.GlobalShipAddress.Request.Manipulators
{
    /// <summary>
    /// Add authentication details to the GlobalShipAddress manipulator
    /// </summary>
    public class FedExGlobalShipAddressWebAuthenticationDetailManipulator : IFedExGlobalShipAddressRequestManipulator
    {
        private readonly IFedExSettingsRepository settingsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExGlobalShipAddressWebAuthenticationDetailManipulator" /> class.
        /// </summary>
        public FedExGlobalShipAddressWebAuthenticationDetailManipulator(IFedExSettingsRepository settingsRepository)
        {
            this.settingsRepository = settingsRepository;
        }

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        public GenericResult<SearchLocationsRequest> Manipulate(IShipmentEntity shipment, SearchLocationsRequest request)
        {
            var settings = new FedExSettings(settingsRepository);

            request.WebAuthenticationDetail = new WebAuthenticationDetail
            {
                CspCredential = new WebAuthenticationCredential
                {
                    Key = settings.CspCredentialKey,
                    Password = settings.CspCredentialPassword
                },
                UserCredential = new WebAuthenticationCredential
                {
                    Key = settings.UserCredentialsKey,
                    Password = settings.UserCredentialsPassword
                }
            };

            return request;
        }
    }
}
