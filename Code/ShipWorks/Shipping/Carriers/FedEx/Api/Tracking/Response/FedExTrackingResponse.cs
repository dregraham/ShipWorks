using System.Collections.Generic;
using System.Linq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Tracking.Response.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Track;
using ShipWorks.Shipping.Tracking;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Tracking.Response
{
    /// <summary>
    /// An implementation of the ICarrierRequest representing the response from a FedEx ground tracking request.
    /// </summary>
    public class FedExTrackingResponse : ICarrierResponse
    {
        private readonly TrackReply trackingReply;
        private readonly CarrierRequest request;
        private readonly IEnumerable<IFedExTrackingResponseManipulator> manipulators;
        private TrackingResult trackingResult;
        private ShipmentEntity shipment;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExTrackingResponse" /> class.
        /// </summary>
        /// <param name="manipulators">The manipulators.</param>
        /// <param name="trackingReply">The tracking reply.</param>
        /// <param name="request">The request.</param>
        public FedExTrackingResponse(IEnumerable<IFedExTrackingResponseManipulator> manipulators, ShipmentEntity shipment, TrackReply trackingReply, CarrierRequest request)
        {
            this.trackingReply = trackingReply;
            this.request = request;
            this.manipulators = manipulators;
            this.shipment = shipment;
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
            get { return trackingReply; }
        }

        
        /// <summary>
        /// Gets the manipulators.
        /// </summary>
        /// <value>The manipulators.</value>
        public IEnumerable<IFedExTrackingResponseManipulator> Manipulators
        {
            get { return manipulators; }
        }

        /// <summary>
        /// Gets the tracking result
        /// </summary>
        public TrackingResult TrackingResult
        {
            get { return trackingResult; }
            set { trackingResult = value; }
        }

        public ShipmentEntity Shipment
        {
            get { return shipment; }
        }

        /// <summary>
        /// Function that tells CarrierResponse to process a request's response
        /// </summary>
        public void Process()
        {
            // 9804 is code indicating nothing to process
            if (trackingReply.Notifications != null && trackingReply.Notifications.Any(n => n.Code == "9804"))
            {
                return;
            }

            // Check for errors
            if (trackingReply.HighestSeverity == NotificationSeverityType.ERROR || trackingReply.HighestSeverity == NotificationSeverityType.FAILURE)
            {
                throw new FedExApiCarrierException(trackingReply.Notifications);
            }
          
            foreach (IFedExTrackingResponseManipulator manipulator in manipulators)
            {
                // Let the manipulators inspect the response and modify the tracking entity as needed
                // to process the response
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
