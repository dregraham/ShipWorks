using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    public interface IUspsShipmentType
    {
        void ValidateShipment(ShipmentEntity shipment);
        bool ShouldRateShop(ShipmentEntity shipment);
        bool ShouldTestExpress1Rates(ShipmentEntity shipment);
        IUspsWebClient CreateWebClient();
        void UseAccountForShipment(UspsAccountEntity account, ShipmentEntity shipment);
        void UpdateDynamicShipmentData(ShipmentEntity shipment);
    }
}