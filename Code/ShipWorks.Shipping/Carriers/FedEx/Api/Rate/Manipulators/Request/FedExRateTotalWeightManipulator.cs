using System;
using Interapptive.Shared.Enums;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    /// <summary>
    /// An ICarrierRequestManipulator implementation that modifies the total weight property of the
    /// FedEx API's RateRequest object.
    /// </summary>
    public class FedExRateTotalWeightManipulator : ICarrierRequestManipulator
    {
        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        public void Manipulate(CarrierRequest request)
        {
            // Make sure all of the properties we'll be accessing have been created
            InitializeRequest(request);

            // We can safely cast this since we've passed initialization 
            RateRequest nativeRequest = request.NativeRequest as RateRequest;

            // Just need to assign the weight value in pounds (default the weight to .1 if none is given, so we can still get rates)
            nativeRequest.RequestedShipment.TotalWeight = new Weight
                {
                    Units = GetApiWeightUnit(request.ShipmentEntity), 
                    Value = (decimal)request.ShipmentEntity.TotalWeight > 0 ? (decimal)request.ShipmentEntity.TotalWeight : .1m,
                    ValueSpecified = true,
                    UnitsSpecified = true
                };

            
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

            // The native FedEx request type should be a ProcessShipmentRequest
            RateRequest nativeRequest = request.NativeRequest as RateRequest;
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
