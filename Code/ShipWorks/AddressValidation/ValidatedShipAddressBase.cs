using System.Collections.Generic;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// A base class that can be used for saving validated ship addresses. This was introduced as a layer of indirection
    /// needed in the AddressValidationQueue to simplify the manner in which validated addresses were being saved.
    /// </summary>
    public abstract class ValidatedShipAddressBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidatedShipAddressBase"/> class.
        /// </summary>
        protected ValidatedShipAddressBase()
        {
            SuggestedAddresses = new List<ValidatedAddressEntity>();
        }

        /// <summary>
        /// Gets the entity the address is associated with.
        /// </summary>
        public IEntity2 Entity { get; protected set; }

        /// <summary>
        /// Gets the entered address.
        /// </summary>
        public ValidatedAddressEntity EnteredAddress { get; protected set; }

        /// <summary>
        /// Gets the suggessted addresses.
        /// </summary>
        public IEnumerable<ValidatedAddressEntity> SuggestedAddresses { get; protected set; }

        /// <summary>
        /// Gets the prefix to use when saving the address (i.e. Ship).
        /// </summary>
        public string Prefix
        {
            get { return "Ship"; }
        }

        /// <summary>
        /// Uses the SqlAdapter to save the validated address to the database.
        /// </summary>
        /// <param name="sqlAdapter">The SQL adapter.</param>
        public abstract void Save(SqlAdapter sqlAdapter);
    }
}