using System.Collections.Generic;
using Interapptive.Shared.Business;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Represents an order's ship address that has been validated.
    /// </summary>
    public class ValidatedOrderShipAddress : ValidatedShipAddressBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidatedOrderShipAddress"/> class.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="enteredAddress">The entered address.</param>
        /// <param name="suggestedAddresses">The suggested addresses.</param>
        public ValidatedOrderShipAddress(OrderEntity order, ValidatedAddressEntity enteredAddress, IEnumerable<ValidatedAddressEntity> suggestedAddresses, AddressAdapter addressAdapter)
        {
            OriginalShippingAddress = addressAdapter;
            Entity = order;
            EnteredAddress = enteredAddress;
            SuggestedAddresses = suggestedAddresses;
        }

        public AddressAdapter OriginalShippingAddress { get; private set; }
        /// <summary>
        /// Uses the SqlAdapter to save the validated address to the database.
        /// </summary>
        /// <param name="sqlAdapter">The SQL adapter.</param>
        public override void Save(SqlAdapter sqlAdapter)
        {
            ValidatedAddressManager.SaveValidatedOrder(sqlAdapter, this);
        }
    }
}
