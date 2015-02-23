using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.AddressValidation
{
    public class ValidatedOrderAddress : ValidatedAddressBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidatedOrderAddress"/> class.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="enteredAddress">The entered address.</param>
        /// <param name="suggestedAddresses">The suggested addresses.</param>
        public ValidatedOrderAddress(OrderEntity order, ValidatedAddressEntity enteredAddress, IEnumerable<ValidatedAddressEntity> suggestedAddresses)
        {
            Entity = order;
            EnteredAddress = enteredAddress;
            SuggestedAddresses = suggestedAddresses;
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
