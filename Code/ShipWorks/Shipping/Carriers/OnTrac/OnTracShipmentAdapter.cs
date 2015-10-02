using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Business;
using ShipWorks.Core.Shipping;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;

namespace ShipWorks.Shipping.Carriers.OnTrac
{
    public class OnTracShipmentAdapter : ShipmentAdapter
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OnTracShipmentAdapter(ShipmentEntity shipmentEntity) : base(shipmentEntity)
        {
        }
    }
}
