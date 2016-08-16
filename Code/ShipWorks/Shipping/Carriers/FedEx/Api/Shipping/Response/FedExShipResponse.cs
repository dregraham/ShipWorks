using System.Collections.Generic;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Response
{
    /// <summary>
    /// This object is used to process the FedExShipmentResponse, saving labels and other shipment information
    /// to the shipment object. It is populated with the actual WSDL response object.
    /// </summary>
    public class FedExShipResponse : ICarrierResponse
    {
        private readonly ILabelRepository labelRepository;
        private readonly List<ICarrierResponseManipulator> shipmentManipulators;
        private readonly IFedExNativeShipmentReply nativeResponse;
        private readonly CarrierRequest request;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExShipResponse(IFedExNativeShipmentReply reply, CarrierRequest request, ShipmentEntity shipment, ILabelRepository labelRepository, List<ICarrierResponseManipulator> shipmentManipulators)
        {
            this.nativeResponse = reply;
            Shipment = shipment;
            this.labelRepository = labelRepository;
            this.shipmentManipulators = shipmentManipulators;
            this.request = request;
        }

        /// <summary>
        /// Gets the request the was used to generate the response.
        /// </summary>
        /// <value>The CarrierRequest object.</value>
        public CarrierRequest Request
        {
            get { return request; }
        }

        /// <summary>
        /// Gets the native response received from the carrier API.
        /// </summary>
        /// <value>The native response.</value>
        public object NativeResponse
        {
            get { return nativeResponse; }
        }

        /// <summary>
        /// Gets the label repository.
        /// </summary>
        /// <value>The label repository.</value>
        public ILabelRepository LabelRepository
        {
            get { return labelRepository; }
        }

        /// <summary>
        /// Gets the shipment manipulators.
        /// </summary>
        /// <value>The shipment manipulators.</value>
        public List<ICarrierResponseManipulator> ShipmentManipulators
        {
            get { return shipmentManipulators; }
        }

        /// <summary>
        /// The shipment entity whose information we sent to FedEx
        /// </summary>
        public ShipmentEntity Shipment
        {
            get;
            private set;
        }

        /// <summary>
        /// Function that tells FedExShipResponse to process the request for shipment.
        /// </summary>
        public void Process()
        {
            Verify();

            ApplyManipulators();
            labelRepository.SaveLabels(this);
        }

        /// <summary>
        /// Verify no severe errors were returned from FedEx.
        /// </summary>
        private void Verify()
        {
            if (nativeResponse.HighestSeverity == NotificationSeverityType.ERROR || nativeResponse.HighestSeverity == NotificationSeverityType.FAILURE)
            {
                throw new FedExApiCarrierException(nativeResponse.Notifications);
            }

            // This should never happen, but our users will let us know if it does
            if (nativeResponse.CompletedShipmentDetail.CompletedPackageDetails.Length != 1)
            {
                throw new CarrierException("Invalid number of package details returned for a shipment request.");
            }
        }

        /// <summary>
        /// Applies the manipulators to the shipment.
        /// </summary>
        protected virtual void ApplyManipulators()
        {
            foreach (ICarrierResponseManipulator manipulator in shipmentManipulators)
            {
                // Let each manipulator inspect/change the shipment as needed
                manipulator.Manipulate(this);
            }
        }

        /// <summary>
        /// Gets the carrier account entity.
        /// </summary>
        /// <value>The carrier account entity.</value>
        public IEntity2 CarrierAccountEntity { get; set; }
    }
}
