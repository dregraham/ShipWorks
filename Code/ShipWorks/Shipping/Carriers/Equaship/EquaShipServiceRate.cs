using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Carriers.EquaShip.Enums;

namespace ShipWorks.Shipping.Carriers.EquaShip
{
    /// <summary>
    /// EquaShip Shipping Rate
    /// </summary>
    public class EquaShipServiceRate
    {
        public EquaShipServiceType Service { get; set; }
        public double Rate { get; set; }
        public DateTime EstimatedDelivery { get; set; }
    }
}
