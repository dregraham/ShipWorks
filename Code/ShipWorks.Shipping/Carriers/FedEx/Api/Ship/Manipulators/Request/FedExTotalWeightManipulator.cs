using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request.International;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request
{
    /// <summary>
    /// An implementation of the ICarrierRequestManipulator interface that will manipulate the total weight
    /// of the IFedExNativeShipmentRequest based on the shipment entity's total weight.
    /// </summary>
    public class FedExTotalWeightManipulator : IFedExShipRequestManipulator
    {
        /// <summary>
        /// Should the manipulator be applied
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment) => true;

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        public GenericResult<ProcessShipmentRequest> Manipulate(IShipmentEntity shipment, ProcessShipmentRequest request, int sequenceNumber)
        {
            InitializeRequest(request);

            request.RequestedShipment.TotalWeight = new Weight { Units = GetApiWeightUnit(shipment), Value = (decimal) shipment.TotalWeight };

            return GenericResult.FromSuccess(request);
        }

        /// <summary>
        /// Initializes the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="System.ArgumentNullException">request</exception>
        /// <exception cref="CarrierException">An unexpected request type was provided.</exception>
        private void InitializeRequest(ProcessShipmentRequest request) =>
            request.Ensure(x => x.RequestedShipment);

        /// <summary>
        /// Gets the FedEx API weight unit.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>The FedEx WeightUnits value.</returns>
        private WeightUnits GetApiWeightUnit(IShipmentEntity shipment)
        {
            WeightUnits weightUnits = WeightUnits.LB;

            if (shipment.FedEx.WeightUnitType == (int) WeightUnitOfMeasure.Kilograms)
            {
                weightUnits = WeightUnits.KG;
            }

            return weightUnits;
        }
    }
}
