using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.AddressValidation
{
    public class ValidatedShippingAddress : ValidatedAddressBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidatedShippingAddress"/> class.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="enteredAddress">The entered address.</param>
        /// <param name="suggestedAddresses">The suggested addresses.</param>
        public ValidatedShippingAddress(ShipmentEntity shipment, ValidatedAddressEntity enteredAddress, IEnumerable<ValidatedAddressEntity> suggestedAddresses)
        {
            Entity = shipment;
            EnteredAddress = enteredAddress;
            SuggestedAddresses = suggestedAddresses.ToList();
        }

        /// <summary>
        /// Gets the prefix.
        /// </summary>
        public override string Prefix
        {
            get { return "Ship"; }
        }
    }
}
