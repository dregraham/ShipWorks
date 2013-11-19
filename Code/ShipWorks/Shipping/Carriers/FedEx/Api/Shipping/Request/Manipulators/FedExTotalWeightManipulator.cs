using System;
using Interapptive.Shared.Enums;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    /// <summary>
    /// An implementation of the ICarrierRequestManipulator interface that will manipulate the total weight
    /// of the IFedExNativeShipmentRequest based on the shipment entity's total weight.
    /// </summary>
    public class FedExTotalWeightManipulator : FedExShippingRequestManipulatorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FedExTotalWeightManipulator" /> class.
        /// </summary>
        public FedExTotalWeightManipulator()
            : this(new FedExSettings(new FedExSettingsRepository()))
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExTotalWeightManipulator" /> class.
        /// </summary>
        /// <param name="fedExSettings">The fed ex settings.</param>
        public FedExTotalWeightManipulator(FedExSettings fedExSettings)
            : base(fedExSettings)
        {
        }

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        public override void Manipulate(CarrierRequest request)
        {
            // Make sure all of the properties we'll be accessing have been created
            InitializeRequest(request);

            // We can safely cast this since we've passed initialization 
            IFedExNativeShipmentRequest nativeRequest = request.NativeRequest as IFedExNativeShipmentRequest;

            // Just need to assign the weight value in pounds
            nativeRequest.RequestedShipment.TotalWeight = new Weight { Units =GetApiWeightUnit(request.ShipmentEntity), Value = (decimal)request.ShipmentEntity.TotalWeight };
        }

        /// <summary>
        /// Initializes the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="System.ArgumentNullException">request</exception>
        /// <exception cref="CarrierException">An unexpected request type was provided.</exception>
        private void InitializeRequest(CarrierRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            // The native FedEx request type should be a IFedExNativeShipmentRequest
            IFedExNativeShipmentRequest nativeRequest = request.NativeRequest as IFedExNativeShipmentRequest;
            if (nativeRequest == null)
            {
                // Abort - we have an unexpected native request
                throw new CarrierException("An unexpected request type was provided.");
            }

            if (nativeRequest.RequestedShipment == null)
            {
                // We'll be manipulating properties of the requested shipment, so make sure it's been created
                nativeRequest.RequestedShipment = new RequestedShipment();
            }
        }

        /// <summary>
        /// Gets the FedEx API weight unit.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>The FedEx WeightUnits value.</returns>
        private WeightUnits GetApiWeightUnit(ShipmentEntity shipment)
        {
            WeightUnits weightUnits = WeightUnits.LB;

            if (shipment.FedEx.WeightUnitType == (int)WeightUnitOfMeasure.Kilograms)
            {
                weightUnits = WeightUnits.KG;
            }

            return weightUnits;
        }
    }
}
