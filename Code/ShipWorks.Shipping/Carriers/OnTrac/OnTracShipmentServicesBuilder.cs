using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.OnTrac
{
    public class OnTracShipmentServicesBuilder : IShipmentServicesBuilder
    {
        public Dictionary<int, string> BuildServiceTypeDictionary(IEnumerable<ShipmentEntity> shipments)
        {
            throw new NotImplementedException();
        }
    }
}