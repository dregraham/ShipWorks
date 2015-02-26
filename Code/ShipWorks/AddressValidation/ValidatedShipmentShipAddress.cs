using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Represents a shimpent's ship address that has been validated.
    /// </summary>
    public class ValidatedShipmentShipAddress : ValidatedShipAddressBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidatedShipmentShipAddress"/> class.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="enteredAddress">The entered address.</param>
        /// <param name="suggestedAddresses">The suggested addresses.</param>
        public ValidatedShipmentShipAddress(ShipmentEntity shipment, ValidatedAddressEntity enteredAddress, IEnumerable<ValidatedAddressEntity> suggestedAddresses)
        {
            Entity = shipment;
            EnteredAddress = enteredAddress;
            SuggestedAddresses = suggestedAddresses.ToList();
        }
        
        /// <summary>
        /// Uses the SqlAdapter to save the validated address to the database.
        /// </summary>
        /// <param name="sqlAdapter">The SQL adapter.</param>
        public override void Save(SqlAdapter sqlAdapter)
        {
            ValidatedAddressManager.SaveValidatedShipmentAddress(sqlAdapter, this);
        }
    }
}
