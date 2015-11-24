using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Amazon.Api;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Amazon
{ 
    /// <summary>
    /// Gets list of service types available for a selected Amazon shipment
    /// </summary>
    public class AmazonShipmentServicesBuilder : IShipmentServicesBuilder
    {
        private readonly IAmazonRatingService amazonRatingService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonShipmentServicesBuilder"/> class.
        /// </summary>
        public AmazonShipmentServicesBuilder(IAmazonRatingService amazonRatingService)
        {
            this.amazonRatingService = amazonRatingService;
        }

        /// <summary>
        /// Gets the AvailableServiceTypes for this shipment type and shipment along with their descriptions.
        /// </summary>
        public Dictionary<int, string> BuildServiceTypeDictionary(IEnumerable<ShipmentEntity> shipments)
        {
            List<ShipmentEntity> shipmentList = shipments?.ToList() ?? new List<ShipmentEntity>();
            ShipmentEntity shipment = shipmentList.FirstOrDefault(s => s.ShipmentTypeCode == ShipmentTypeCode.Amazon);

            if (shipmentList.Count() > 1 || shipment == null)
            {
                return new Dictionary<int, string>();
            }
            
            RateGroup rateGroup = amazonRatingService.GetRates(shipment);
            
            int index = 0;
            return rateGroup.Rates.ToDictionary(s => index++, s => s.Description);
        }
    }
}