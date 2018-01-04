using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request
{
    /// <summary>
    /// Adds web auth to the FedEx Ship request
    /// </summary>
    public class FedExShippingWebAuthenticationDetailManipulator : IFedExShipRequestManipulator
    {
        private readonly IFedExSettingsRepository settingsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExShippingWebAuthenticationDetailManipulator" /> class.
        /// </summary>
        public FedExShippingWebAuthenticationDetailManipulator(IFedExSettingsRepository settingsRepository)
        {
            this.settingsRepository = settingsRepository;
        }

        /// <summary>
        /// Does this manipulator apply to this shipment
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment, int sequenceNumber) => true;

        /// <summary>
        /// Manipulates the specified request by setting the WebAuthenticationDetail property of a IFedExNativeShipmentRequest object.
        /// </summary>
        public GenericResult<ProcessShipmentRequest> Manipulate(IShipmentEntity shipment, ProcessShipmentRequest request, int sequenceNumber)
        {
            request.WebAuthenticationDetail = FedExRequestManipulatorUtilities.CreateShippingWebAuthenticationDetail(settingsRepository);

            return request;
        }
    }
}
