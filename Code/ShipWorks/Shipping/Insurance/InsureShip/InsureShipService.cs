using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Insurance.InsureShip.Net.Insure;
using ShipWorks.Shipping.Insurance.InsureShip.Net.Void;

namespace ShipWorks.Shipping.Insurance.InsureShip
{
    /// <summary>
    /// Service for interacting with InsureShip
    /// </summary>
    [Component]
    public class InsureShipService : IInsureShipService
    {
        private readonly IShipmentTypeManager shipmentTypeFactory;
        private readonly IInsureShipInsureShipmentRequest insureShipment;
        private readonly IInsureShipVoidPolicyRequest voidShipment;

        /// <summary>
        /// Constructor
        /// </summary>
        public InsureShipService(
            IShipmentTypeManager shipmentTypeFactory,
            IInsureShipInsureShipmentRequest insureShipment,
            IInsureShipVoidPolicyRequest voidShipment)
        {
            this.voidShipment = voidShipment;
            this.insureShipment = insureShipment;
            this.shipmentTypeFactory = shipmentTypeFactory;
        }

        /// <summary>
        /// Insures the shipment with InsureShip and sets the InsuredWith property of the shipment based
        /// on the response from InsureShip.
        /// </summary>
        public Result Insure(ShipmentEntity shipment) =>
            insureShipment.CreateInsurancePolicy(shipment);

        /// <summary>
        /// Void the policy for the given shipment
        /// </summary>
        public Result Void(ShipmentEntity shipment) =>
            voidShipment.VoidInsurancePolicy(shipment);

        /// <summary>
        /// Is the given shipment insured by InsureShip
        /// </summary>
        public bool IsInsuredByInsureShip(ShipmentEntity shipment)
        {
            ShipmentType shipmentType = shipmentTypeFactory.Get(shipment);
            bool result = false;

            foreach (var i in Enumerable.Range(0, shipmentType.GetParcelCount(shipment)))
            {
                var choice = shipmentType.GetParcelDetail(shipment, i).Insurance;

                if (choice.Insured && 
                    choice.InsuranceProvider == InsuranceProvider.ShipWorks && 
                    InsuranceUtility.GetInsuranceCost(shipment, choice.InsuranceValue).ShipWorks > 0)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }
    }
}
