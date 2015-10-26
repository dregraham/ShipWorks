using ShipWorks.Data.Model.EntityClasses;
using System.Collections.Generic;
using System.Linq;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Results of validation
    /// </summary>
    public struct ValidatedAddressData
    {
        private static readonly ValidatedAddressData empty = 
            new ValidatedAddressData(null, Enumerable.Empty<ValidatedAddressEntity>());

        /// <summary>
        /// Empty validation results
        /// </summary>
        public static ValidatedAddressData Empty => empty;

        /// <summary>
        /// Constructor
        /// </summary>
        public ValidatedAddressData(ValidatedAddressEntity original, IEnumerable<ValidatedAddressEntity> suggestions)
        {
            Original = original;
            Suggestions = suggestions.ToList();
        }

        /// <summary>
        /// Original address, modified if necessary
        /// </summary>
        public ValidatedAddressEntity Original { get; private set; }

        /// <summary>
        /// List of suggested alternative addresses
        /// </summary>
        public IEnumerable<ValidatedAddressEntity> Suggestions { get; private set; }
    }
}