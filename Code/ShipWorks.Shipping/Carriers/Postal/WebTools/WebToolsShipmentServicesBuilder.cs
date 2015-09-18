using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.WebTools
{
    public class WebToolsShipmentServicesBuilder : IShipmentServicesBuilder
    {
        public Dictionary<int, string> BuildServiceTypeDictionary(IEnumerable<ShipmentEntity> shipments)
        {
            throw new NotImplementedException();
        }
    }
}