using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.AddressValidation
{
    public class ValidatedAddressManagerWrapper : IValidatedAddressManager
    {
        /// <summary>
        /// Validate a single shipment
        /// </summary>
        public void ValidateShipment(ShipmentEntity shipment, AddressValidator validator)
        {
            ValidatedAddressManager.ValidateShipment(shipment, validator);
        }
    }
}
