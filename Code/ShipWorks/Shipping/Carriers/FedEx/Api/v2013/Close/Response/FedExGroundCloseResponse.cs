using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.Close;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Close.Response.Manipulators;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Close.Response
{
    /// <summary>
    /// An implementation of the ICarrierRequest representing the response from a FedEx ground close request.
    /// </summary>
    public class FedExGroundCloseResponse : ICarrierResponse
    {
        private readonly GroundCloseReply closeReply;
        private FedExEndOfDayCloseEntity closeEntity;
        private readonly CarrierRequest request;
        private readonly IEnumerable<IFedExCloseResponseManipulator> manipulators;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExGroundCloseResponse" /> class.
        /// </summary>
        /// <param name="manipulators">The manipulators.</param>
        /// <param name="closeReply">The close reply.</param>
        /// <param name="request">The request.</param>
        public FedExGroundCloseResponse(IEnumerable<IFedExCloseResponseManipulator> manipulators, GroundCloseReply closeReply, CarrierRequest request)
        {
            this.closeReply = closeReply;
            this.request = request;
            this.manipulators = manipulators;
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
            get { return closeReply; }
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
            // 9804 is code indicating nothing to process
            if (closeReply.Notifications != null && closeReply.Notifications.Any(n => n.Code == "9804"))
            {
                return;
            }

            // Check for errors
            if (closeReply.HighestSeverity == NotificationSeverityType.ERROR || closeReply.HighestSeverity == NotificationSeverityType.FAILURE)
            {
                throw new FedExApiCarrierException(closeReply.Notifications);
            }

            // There are items to process, so create a new close entity
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
