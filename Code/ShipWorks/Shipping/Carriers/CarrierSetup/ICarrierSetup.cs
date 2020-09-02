using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Warehouse.DTO.Configuration.ShippingSettings;

namespace ShipWorks.Shipping.CarrierSetup
{
    interface ICarrierSetup
    {
        void Setup(CarrierConfigurationPayload config);
    }
}
