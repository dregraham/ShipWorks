using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Allow address suggestions to be stored for saving later
    /// </summary>
    public class ValidatedAddressScope : IValidatedAddressScope
    {
        // Maps the controls to their current value set in scope
        readonly Dictionary<long, Dictionary<string, List<ValidatedAddressEntity>>> valueMap =
            new Dictionary<long, Dictionary<string, List<ValidatedAddressEntity>>>();

        /// <summary>
        /// Clear validated addresses for the given entity and prefix
        /// </summary>
        public void ClearAddresses(long value, string prefix) =>
            StoreAddresses(value, Enumerable.Empty<ValidatedAddressEntity>(), prefix);

        /// <summary>
        /// Store a collection of addresses that should be saved
        /// </summary>
        public void StoreAddresses(long entityId, IEnumerable<ValidatedAddressEntity> addresses, string fieldPrefix)
        {
            // Set the prefix on the addresses
            List<ValidatedAddressEntity> addressList = addresses.ToList();
            addressList.ForEach(x => x.AddressPrefix = fieldPrefix);

            if (valueMap.ContainsKey(entityId))
            {
                if (valueMap[entityId].ContainsKey(fieldPrefix))
                {
                    valueMap[entityId][fieldPrefix] = addressList;
                }
                else
                {
                    valueMap[entityId].Add(fieldPrefix, addressList);
                }
            }
            else
            {
                valueMap.Add(entityId, new Dictionary<string, List<ValidatedAddressEntity>>{ { fieldPrefix, addressList } });
            }
        }

        /// <summary>
        /// Create a function that will get a list of validated addresses
        /// </summary>
        public IEnumerable<ValidatedAddressEntity> LoadValidatedAddresses(long entityId, string addressPrefix)
        {
            using (SqlAdapter sqlAdapter = new SqlAdapter())
            {
                return ValidatedAddressManager.GetSuggestedAddresses(sqlAdapter, entityId, addressPrefix);
            }
        }

        /// <summary>
        /// Save the stored validated addresses to the database
        /// </summary>
        public void FlushAddressesToDatabase(SqlAdapter sqlAdapter, long entityId, string prefix)
        {
            FlushAddressesToDatabase(new AdapterAddressValidationDataAccess(sqlAdapter), entityId, prefix);
        }

        /// <summary>
        /// Save the stored validated addresses to the database
        /// </summary>
        public void FlushAddressesToDatabase(IAddressValidationDataAccess dataAccess, long entityId, string prefix)
        {
            if (!valueMap.ContainsKey(entityId) || !valueMap[entityId].ContainsKey(prefix))
            {
                return;
            }

            ValidatedAddressManager.DeleteExistingAddresses(dataAccess, entityId, prefix);

            foreach (ValidatedAddressEntity address in valueMap[entityId][prefix])
            {
                ValidatedAddressManager.SaveEntityAddress(dataAccess, entityId, address);
            }

            valueMap[entityId].Remove(prefix);

            if (!valueMap[entityId].Any())
            {
                valueMap.Remove(entityId);
            }
        }
    }
}
