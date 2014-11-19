using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Shipping.Carriers.Api
{
    /// <summary>
    /// A generic abstract class for sumbitting requests to a shipping carrier's API where the type T is 
    /// intended to be the carrier-specific request medium (WSDL generated proxy-class, raw XML, etc.).
    /// </summary>
    public abstract class CarrierRequest
    {
        private readonly List<ICarrierRequestManipulator> requestManipulators;
        private readonly ShipmentEntity shipmentEntity;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="CarrierRequest" /> class.
        /// </summary>
        /// <param name="requestManipulators">The manipulators to use to manipulate the request prior to it being submitted.</param>
        /// <param name="shipmentEntity">The shipment entity the request is for.</param>
        public CarrierRequest(IEnumerable<ICarrierRequestManipulator> requestManipulators, ShipmentEntity shipmentEntity)
        {
            // Initialize our internal manipulator list based on the manipulators provided

            this.requestManipulators = requestManipulators == null ? new List<ICarrierRequestManipulator>() : new List<ICarrierRequestManipulator>(requestManipulators);

            this.shipmentEntity = shipmentEntity;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CarrierRequest" /> class. Allows for setting the native request 
        /// at creation.
        /// </summary>
        /// <param name="requestManipulators">The request manipulators.</param>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <param name="nativeRequest">The native request.</param>
        public CarrierRequest(IEnumerable<ICarrierRequestManipulator> requestManipulators, ShipmentEntity shipmentEntity, object nativeRequest)
            : this(requestManipulators, shipmentEntity)
        {
            NativeRequest = nativeRequest;
        }

        /// <summary>
        /// This is here to easily create a Moq object...
        /// </summary>
        public CarrierRequest()
        {
            
        }

        /// <summary>
        /// Gets the carrier account entity.
        /// </summary>
        /// <value>The carrier account entity.</value>
        public abstract IEntity2 CarrierAccountEntity { get; }

        /// <summary>
        /// If required by context, this represents the item order in the batch being processed.
        /// </summary>
        public int SequenceNumber { get; set; }

        /// <summary>
        /// Gets or sets the native request (WSDL generated proxy-class, raw XML, etc.) that the carrier request is representing. 
        /// </summary>
        /// <value>The native request.</value>
        public virtual object NativeRequest { get; protected set; }

        /// <summary>
        /// Gets the shipment entity.
        /// </summary>
        /// <value>The shipment entity.</value>
        public ShipmentEntity ShipmentEntity
        {
            get { return shipmentEntity; }
        }

        /// <summary>
        /// Gets the manipulators for this request.
        /// </summary>
        /// <value>The manipulators.</value>
        public IEnumerable<ICarrierRequestManipulator> Manipulators
        {
            get { return requestManipulators; }
        }

        /// <summary>
        /// Applies the manipulators to the request.
        /// </summary>
        protected virtual void ApplyManipulators()
        {
            foreach (ICarrierRequestManipulator manipulator in requestManipulators)
            {
                // Let each manipulator inspect/change the request as needed
                manipulator.Manipulate(this);
            }
        }

        /// <summary>
        /// Submits the request to the carrier API.
        /// </summary>
        /// <returns>An ICarrierResponse containing the carrier-specific results of the request.</returns>
        public abstract ICarrierResponse Submit();
    }
}
