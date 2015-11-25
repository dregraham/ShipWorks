using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Express1;
using ShipWorks.Shipping.Carriers.Postal.Usps.Express1.Net;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// Express1 Usps Label Service
    /// </summary>
    public class Express1UspsLabelService : ILabelService
    {
        private readonly Express1UspsShipmentType express1UspsShipmentType;
        private readonly UspsShipmentType uspsShipmentType;

        /// <summary>
        /// Constructor
        /// </summary>
        public Express1UspsLabelService(Express1UspsShipmentType express1UspsShipmentType, UspsShipmentType uspsShipmentType)
        {
            this.express1UspsShipmentType = express1UspsShipmentType;
            this.uspsShipmentType = uspsShipmentType;
        }

        /// <summary>
        /// Creates the Usps(stamps.com) Express1 label
        /// </summary>
        /// <param name="shipment"></param>
        public void Create(ShipmentEntity shipment)
        {
            express1UspsShipmentType.ValidateShipment(shipment);

            try
            {
                // Express1 for USPS requires that postage be hidden per their negotiated
                // service agreement
                shipment.Postal.Usps.HidePostage = true;
                new Express1UspsWebClient().ProcessShipment(shipment);
            }
            catch (UspsException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Uses the Usps label service to void the Usps Express1 label
        /// </summary>
        /// <param name="shipment"></param>
        public void Void(ShipmentEntity shipment)
        {
            try
            {
                uspsShipmentType.CreateWebClient().VoidShipment(shipment);
            }
            catch (UspsException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }
    }
}