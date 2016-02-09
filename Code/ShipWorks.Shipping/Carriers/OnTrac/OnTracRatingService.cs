using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.OnTrac.Enums;
using ShipWorks.Shipping.Carriers.OnTrac.Net.Rates;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.OnTrac
{
    /// <summary>
    /// Rating service for OnTrac
    /// </summary>
    public class OnTracRatingService : IRatingService
    {
        private readonly OnTracShipmentType onTracShipmentType;

        /// <summary>
        /// Initializes a new instance of the <see cref="OnTracRatingService"/> class.
        /// </summary>
        /// <param name="onTracShipmentType">Type of the on trac shipment.</param>
        public OnTracRatingService(OnTracShipmentType onTracShipmentType)
        {
            this.onTracShipmentType = onTracShipmentType;
        }

        /// <summary>
        /// Gets rates for the given shipment
        /// </summary>
        /// <param name="shipment">The shipment</param>
        /// <returns>RateGroup for the shipment</returns>
        /// <exception cref="OnTracException">An OnTrac account is required to view rates.</exception>
        /// <exception cref="ShippingException"></exception>
        public RateGroup GetRates(ShipmentEntity shipment)
        {
            try
            {
                OnTracAccountEntity account = OnTracShipmentType.GetAccountForShipment(shipment);
                OnTracRates rateRequest = new OnTracRates(account);
                return rateRequest.GetRates(shipment,
                    onTracShipmentType.GetAvailableServiceTypes()
                        .Cast<OnTracServiceType>()
                        .Union(new List<OnTracServiceType> { (OnTracServiceType) shipment.OnTrac.Service }));
            }
            catch (OnTracException ex)
            {
                if (ex.Message == "No OnTrac account is selected for the shipment.")
                {
                    // Provide a message with additional context
                    throw new ShippingException("An OnTrac account is required to view rates.", ex);
                }

                throw new ShippingException(ex.Message);
            }
        }
    }
}
