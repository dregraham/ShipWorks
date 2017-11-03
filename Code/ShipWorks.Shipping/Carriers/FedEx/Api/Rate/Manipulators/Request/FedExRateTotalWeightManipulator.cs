using System;
using Interapptive.Shared.Enums;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    /// <summary>
    /// An ICarrierRequestManipulator implementation that modifies the total weight property of the
    /// FedEx API's RateRequest object.
    /// </summary>
    public class FedExRateTotalWeightManipulator : IFedExRateRequestManipulator
    {
        /// <summary>
        /// Does this manipulator apply to the shipment
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment, FedExRateRequestOptions options)
        {
            return true;
        }

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        public RateRequest Manipulate(IShipmentEntity shipment, RateRequest request)
        {
            // Make sure all of the properties we'll be accessing have been created
            InitializeRequest(request);

            // Just need to assign the weight value in pounds (default the weight to .1 if none is given, so we can still get rates)
            request.RequestedShipment.TotalWeight = new Weight
                {
                    Units = GetApiWeightUnit(shipment), 
                    Value = (decimal) shipment.TotalWeight > 0 ? (decimal) shipment.TotalWeight : .1m,
                    ValueSpecified = true,
                    UnitsSpecified = true
                };

            return request;
        }

        /// <summary>
        /// Initializes the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="System.ArgumentNullException">request</exception>
        /// <exception cref="CarrierException">An unexpected request type was provided.</exception>
        private void InitializeRequest(RateRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (request.RequestedShipment == null)
            {
                // We'll be manipulating properties of the requested shipment, so make sure it's been created
                request.RequestedShipment = new RequestedShipment();
            }
        }

        /// <summary>
        /// Gets the FedEx API weight unit.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>The FedEx WeightUnits value.</returns>
        private WeightUnits GetApiWeightUnit(IShipmentEntity shipment)
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
