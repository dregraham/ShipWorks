using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Business;
using ShipWorks.Core.Shipping;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.UPS;

namespace ShipWorks.Shipping.Carriers
{
    public class NullShipmentAdapter : IShipmentAdapter
    {
        public ShipmentTypeCode ShipmentType { get; set; }
        public bool IsProcessed
        {
            get { return false; }
            set { }
        }

        public long OriginAddressType
        {
            get { return 0; }
            set { }
        }

        public PersonAdapter OriginPersonAdapter
        {
            get { return new PersonAdapter(); }
            set { }
        }

        public PersonAdapter DestinationPersonAdapter
        {
            get { return new PersonAdapter(); }
            set { }
        }

    }
}
