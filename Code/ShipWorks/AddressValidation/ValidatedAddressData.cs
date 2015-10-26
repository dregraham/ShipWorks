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

        private static readonly ValidatedAddressData notSet =
            new ValidatedAddressData(null, Enumerable.Empty<ValidatedAddressEntity>());

        /// <summary>
        /// Empty validation results
        /// </summary>
        public static ValidatedAddressData Empty => empty;

        /// <summary>
        /// Validation results that have not been explicitly set
        /// </summary>
        public static ValidatedAddressData NotSet => NotSet;

        /// <summary>
        /// Constructor
        /// </summary>
        public ValidatedAddressData(ValidatedAddressEntity original, IEnumerable<ValidatedAddressEntity> suggestions)
        {
            Original = original;

            List<ValidatedAddressEntity> allAddresses = new List<ValidatedAddressEntity>();
            if (Original != null)
            {
                allAddresses.Add(Original);
            }

            Suggestions = suggestions?.ToList() ?? Enumerable.Empty<ValidatedAddressEntity>();
            AllAddresses = allAddresses.Concat(Suggestions);
        }

        /// <summary>
        /// Original address, modified if necessary
        /// </summary>
        public ValidatedAddressEntity Original { get; private set; }

        /// <summary>
        /// List of suggested alternative addresses
        /// </summary>
        public IEnumerable<ValidatedAddressEntity> Suggestions { get; private set; }
        
        /// <summary>
        /// All addresses, with the original first
        /// </summary>
        public IEnumerable<ValidatedAddressEntity> AllAddresses { get; private set; }
    }
}