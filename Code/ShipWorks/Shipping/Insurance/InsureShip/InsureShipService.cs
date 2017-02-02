using System.Linq;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Insurance.InsureShip
{
    /// <summary>
    /// Service for interacting with InsureShip
    /// </summary>
    [Component]
    public class InsureShipService : IInsureShipService
    {
        private readonly ITangoWebClient tangoWebClient;
        readonly IShipmentTypeManager shipmentTypeFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public InsureShipService(IShipmentTypeManager shipmentTypeFactory, ITangoWebClient tangoWebClient)
        {
            this.shipmentTypeFactory = shipmentTypeFactory;
            this.tangoWebClient = tangoWebClient;
        }

        /// <summary>
        /// Insures the shipment with InsureShip and sets the InsuredWith property of the shipment based
        /// on the response from InsureShip.
        /// </summary>
        public void Insure(ShipmentEntity shipment, StoreEntity store)
        {
            InsureShipPolicy insureShipPolicy =
                new InsureShipPolicy(tangoWebClient.GetInsureShipAffiliate(store));
            insureShipPolicy.Insure(shipment);
        }

        /// <summary>
        /// Is the given shipment insured by InsureShip
        /// </summary>
        public bool IsInsuredByInsureShip(ShipmentEntity shipment)
        {
            ShipmentType shipmentType = shipmentTypeFactory.Get(shipment);

            return Enumerable.Range(0, shipmentType.GetParcelCount(shipment))
                .Select(i => shipmentType.GetParcelDetail(shipment, i).Insurance)
                .Any(choice => choice.Insured &&
                    choice.InsuranceProvider == InsuranceProvider.ShipWorks &&
                    choice.InsuranceValue > 0);
        }
    }
}
