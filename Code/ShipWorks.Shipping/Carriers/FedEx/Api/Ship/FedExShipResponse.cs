using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Response
{
    /// <summary>
    /// This object is used to process the FedExShipmentResponse, saving labels and other shipment information
    /// to the shipment object. It is populated with the actual WSDL response object.
    /// </summary>
    public class FedExShipResponse : IFedExShipResponse, ICarrierResponse
    {
        private readonly IFedExLabelRepository labelRepository;
        private readonly IEnumerable<IFedExShipResponseManipulator> manipulators;
        private readonly ShipmentEntity shipment;
        private readonly ProcessShipmentReply reply;

        public FedExShipResponse(object a, object b, object c, object d, object e)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExShipResponse(
            ShipmentEntity shipment,
            ProcessShipmentReply reply,
            IFedExLabelRepository labelRepository,
            IEnumerable<IFedExShipResponseManipulator> manipulators)
        {
            this.reply = reply;
            this.shipment = shipment;
            this.manipulators = manipulators;
            this.labelRepository = labelRepository;
        }

        //TODO: Remove these when 
        public ShipmentEntity Shipment => shipment;
        public ProcessShipmentReply NativeResponse => reply;

        public CarrierRequest Request
        {
            get { throw new System.NotImplementedException(); }
        }

        object ICarrierResponse.NativeResponse
        {
            get { throw new System.NotImplementedException(); }
        }

        /// <summary>
        /// Function that tells FedExShipResponse to process the request for shipment.
        /// </summary>
        public void Process() =>
            labelRepository.SaveLabels(shipment, reply);

        /// <summary>
        /// Applies the response manipulators.
        /// </summary>
        public GenericResult<IFedExShipResponse> ApplyManipulators() =>
            Verify()
                .Map(() => manipulators.Aggregate(shipment, (s, m) => m.Manipulate(reply, shipment)))
                .Map(x => this as IFedExShipResponse);

        /// <summary>
        /// Verify no severe errors were returned from FedEx.
        /// </summary>
        private Result Verify()
        {
            if (reply.HighestSeverity == NotificationSeverityType.ERROR || reply.HighestSeverity == NotificationSeverityType.FAILURE)
            {
                return reply.Notifications?.Any() == true ?
                    Result.FromError(new FedExApiCarrierException(reply.Notifications)) :
                    Result.FromError(new CarrierException("An error occurred while attempting to process the shipment."));
            }

            // This should never happen, but our users will let us know if it does
            if (reply.CompletedShipmentDetail.CompletedPackageDetails.Length != 1)
            {
                Result.FromError(new CarrierException("Invalid number of package details returned for a shipment request."));
            }

            return Result.FromSuccess();
        }
    }
}
