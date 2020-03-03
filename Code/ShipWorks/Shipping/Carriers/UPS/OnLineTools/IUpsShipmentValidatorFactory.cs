using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools
{
    /// <summary>
    /// Create a IUpsShipmentValidator
    /// </summary>
    public interface IUpsShipmentValidatorFactory
    {
        /// <summary>
        /// Create a IUpsShipmentValidator
        /// </summary>
        IUpsShipmentValidator Create(ShipmentEntity shipment);
    }
}
