using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Business;
using ShipWorks.Core.Shipping;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.UPS;

namespace ShipWorks.Shipping.Carriers.Postal
{
    public class PostalWebShipmentAdapter : ShipmentAdapter
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PostalWebShipmentAdapter(ShipmentEntity shipmentEntity) : base(shipmentEntity)
        {
        }
    }
}
