using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.AddressValidation
{
    public interface IValidatedAddressManager
    {
        /// <summary>
        /// Validate a single shipment
        /// </summary>
        void ValidateShipment(ShipmentEntity shipment, AddressValidator validator);
    }
}
