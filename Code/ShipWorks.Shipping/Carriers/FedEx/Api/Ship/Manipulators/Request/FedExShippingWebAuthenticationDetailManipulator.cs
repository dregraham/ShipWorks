using System;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request
{
    /// <summary>
    /// Adds web auth to the FedEx Ship request
    /// </summary>
    public class FedExShippingWebAuthenticationDetailManipulator : IFedExShipRequestManipulator
    {
        private readonly FedExSettings fedExSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExShippingWebAuthenticationDetailManipulator" /> class using 
        /// a FedExSettings backed by the FedExSettingsRepository.
        /// </summary>
        public FedExShippingWebAuthenticationDetailManipulator()
            : this(new FedExSettings(new FedExSettingsRepository()))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExShippingWebAuthenticationDetailManipulator" /> class.
        /// </summary>
        /// <param name="fedExSettings">The FedEx settings.</param>
        public FedExShippingWebAuthenticationDetailManipulator(FedExSettings fedExSettings)
        {
            this.fedExSettings = fedExSettings;
        }

        /// <summary>
        /// Does this manipulator apply to this shipment
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment) => true;

        /// <summary>
        /// Manipulates the specified request by setting the WebAuthenticationDetail property of a IFedExNativeShipmentRequest object.
        /// </summary>
        public GenericResult<ProcessShipmentRequest> Manipulate(IShipmentEntity shipment, ProcessShipmentRequest request, int sequenceNumber)
        {
            // Make sure all of the properties we'll be accessing have been created
            ValidateRequest(request, shipment);

            request.WebAuthenticationDetail = FedExRequestManipulatorUtilities.CreateShippingWebAuthenticationDetail(fedExSettings);

            return GenericResult.FromSuccess(request);
        }

        /// <summary>
        /// Validates the request making sure it is not null and of the correct type.
        /// </summary>
        private void ValidateRequest(ProcessShipmentRequest request, IShipmentEntity shipment)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }
        }
    }
}
