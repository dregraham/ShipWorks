using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    public class Express1ShipmentServicesBuilder : IShipmentServicesBuilder
    {
        public Dictionary<int, string> BuildServiceTypeDictionary(IEnumerable<ShipmentEntity> shipments)
        {
            throw new NotImplementedException();
        }
    }
}