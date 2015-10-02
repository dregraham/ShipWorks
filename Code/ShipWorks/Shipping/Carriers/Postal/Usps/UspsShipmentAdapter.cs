using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Business;
using ShipWorks.Core.Shipping;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    public class UspsShipmentAdapter : ShipmentAdapter
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UspsShipmentAdapter(ShipmentEntity shipmentEntity) : base(shipmentEntity)
        {
        }
    }
}
