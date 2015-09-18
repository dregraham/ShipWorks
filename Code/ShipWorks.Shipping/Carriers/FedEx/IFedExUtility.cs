using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Enums;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    public interface IFedExUtility
    {
        List<FedExServiceType> GetValidServiceTypes(IEnumerable<ShipmentEntity> overriddenShipments);
    }
}