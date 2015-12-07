using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.OnTrac.Enums;
using ShipWorks.Shipping.Carriers.OnTrac.Net.Rates;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.OnTrac
{
    public class OnTracRatingService : IRatingService
    {
        private readonly OnTracShipmentType onTracShipmentType;

        public OnTracRatingService(OnTracShipmentType onTracShipmentType)
        {
            this.onTracShipmentType = onTracShipmentType;
        }

        public RateGroup GetRates(ShipmentEntity shipment)
        {
            try
            {
                OnTracAccountEntity account = OnTracShipmentType.GetAccountForShipment(shipment);
                OnTracRates rateRequest = new OnTracRates(account);
                return rateRequest.GetRates(shipment,
                    onTracShipmentType.GetAvailableServiceTypes()
                        .Cast<OnTracServiceType>()
                        .Union(new List<OnTracServiceType> {(OnTracServiceType) shipment.OnTrac.Service}));
            }
            catch (OnTracException ex)
            {
                if (ex.Message == "No OnTrac account is selected for the shipment.")
                {
                    // Provide a message with additional context
                    throw new OnTracException("An OnTrac account is required to view rates.", ex);
                }

                throw new ShippingException(ex.Message);
            }
        }
    }
}
