using System.Collections.Generic;
using System.Linq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Close.Response.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.Close;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Close.Response
{
    /// <summary>
    /// An implementation of the ICarrierRequest representing the response from a FedEx SmartPost close request.
    /// </summary>
    public class FedExSmartPostCloseResponse : ICarrierResponse
    {
        private readonly IEnumerable<IFedExCloseResponseManipulator> manipulators;
        private readonly SmartPostCloseReply smartPostCloseReply;
        private readonly CarrierRequest request;
        private FedExEndOfDayCloseEntity closeEntity;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExSmartPostCloseResponse" /> class.
        /// </summary>
        /// <param name="manipulators">The manipulators.</param>
        /// <param name="closeReply">The close reply.</param>
        /// <param name="request">The request.</param>
        /// <param name="closeEntity">The close entity.</param>
        public FedExSmartPostCloseResponse(IEnumerable<IFedExCloseResponseManipulator> manipulators, SmartPostCloseReply closeReply, CarrierRequest request)
        {
            this.manipulators = manipulators;
            this.smartPostCloseReply = closeReply;
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
            get { return smartPostCloseReply; }
        }

        /// <summary>
        /// Gets the close entity.
        /// </summary>
        /// <value>The close entity.</value>
        public FedExEndOfDayCloseEntity CloseEntity
        {
            get { return closeEntity; }
        }

        /// <summary>
        /// Gets the manipulators.
        /// </summary>
        /// <value>The manipulators.</value>
        public IEnumerable<IFedExCloseResponseManipulator> Manipulators
        {
            get { return manipulators; }
        }


        /// <summary>
        /// Function that tells CarrierResponse to process a request's response
        /// </summary>
        public void Process()
        {
            if (smartPostCloseReply.Notifications != null && smartPostCloseReply.Notifications.Any(n => n.Code == "9804"))
            {
                // 9804 is code indicating nothing to process
                return;
            }

            // Check for errors
            if (smartPostCloseReply.HighestSeverity == NotificationSeverityType.ERROR || smartPostCloseReply.HighestSeverity == NotificationSeverityType.FAILURE)
            {
                throw new FedExApiCarrierException(smartPostCloseReply.Notifications);
            }

            // There are items to process so create a new close entity
            closeEntity = new FedExEndOfDayCloseEntity();
            foreach (IFedExCloseResponseManipulator manipulator in manipulators)
            {
                // Let the manipulators inspect the response and modify the close entity as needed
                // to process the response
                manipulator.Manipulate(this, closeEntity);
            }
        }

        /// <summary>
        /// Gets the carrier account entity.
        /// </summary>
        /// <value>The carrier account entity.</value>
        public IEntity2 CarrierAccountEntity { get; set; }
    }
}
