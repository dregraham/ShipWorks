using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Insurance.InsureShip
{
    public interface IInsureShipRequestFactory
    {
        void CreateInsureShipmentRequest(ShipmentEntity shipmentEntity, InsureShipAffiliate insureShipAffiliate);
    }
}
