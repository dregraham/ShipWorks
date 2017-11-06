using System;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request
{
    /// <summary>
    /// An implementation of the ICarrierRequestManipulator interface that will manipulate the
    /// Version information of a IFedExNativeShipmentRequest object.
    /// </summary>
    public class FedExShippingVersionManipulator : IFedExShipRequestManipulator
    {
        private readonly FedExSettings fedExSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExShippingVersionManipulator" /> class.
        /// </summary>
        public FedExShippingVersionManipulator()
            : this(new FedExSettings(new FedExSettingsRepository()))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExShippingVersionManipulator" /> class.
        /// </summary>
        /// <param name="fedExSettings">The fed ex settings.</param>
        public FedExShippingVersionManipulator(FedExSettings fedExSettings)
        {
            this.fedExSettings = fedExSettings;
        }

        /// <summary>
        /// Does this manipulator apply to this shipment
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment, int sequenceNumber) => true;

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        public GenericResult<ProcessShipmentRequest> Manipulate(IShipmentEntity shipment, ProcessShipmentRequest request, int sequenceNumber)
        {
            // Make sure all of the properties we'll be accessing have been created
            ValidateRequest(request, shipment);

            request.Version = new VersionId
            {
                ServiceId = "ship",
                Major = int.Parse(FedExSettings.ShipVersionNumber),
                Minor = 0,
                Intermediate = 0
            };

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
