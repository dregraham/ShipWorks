using System.Collections.Generic;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.AddressValidation
{
    public abstract class ValidatedAddressBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidatedAddressBase"/> class.
        /// </summary>
        protected ValidatedAddressBase()
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
        /// Gets the prefix.
        /// </summary>
        public abstract string Prefix { get; }

        public abstract void Save(SqlAdapter sqlAdapter);
    }
}