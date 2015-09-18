using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    public class UspsShipmentServicesBuilder : IShipmentServicesBuilder
    {
        public Dictionary<int, string> BuildServiceTypeDictionary(IEnumerable<ShipmentEntity> shipments)
        {
            throw new NotImplementedException();
        }
    }
}