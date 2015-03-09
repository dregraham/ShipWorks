using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Allow address suggestions to be stored for saving later
    /// </summary>
    public class ValidatedAddressScope : IDisposable
    {
        // Maps the controls to their current value set in scope
        readonly Dictionary<long, Dictionary<string, List<ValidatedAddressEntity>>> valueMap = 
            new Dictionary<long, Dictionary<string, List<ValidatedAddressEntity>>>();
        
        /// <summary>
        /// Constructor
        /// </summary>
        public ValidatedAddressScope()
        {
            if (Current != null)
            {
                throw new InvalidOperationException("A ValidatedAddressScope is already in scope.");
            }

            Current = this;
        }

        /// <summary>
        /// Get the current object in scope.
        /// </summary>
        public static ValidatedAddressScope Current
        {
            get; 
            private set;
        }

        /// <summary>
        /// Store a collection of addresses that should be saved
        /// </summary>
        public static void StoreAddresses(long entityId, IEnumerable<ValidatedAddressEntity> addresses, string fieldPrefix)
        {
            ValidatedAddressScope scope = Current;
            if (scope == null)
            {
                throw new InvalidOperationException("Cannot be used when there is no ValidatedAddressScope in scope.");
            }

            // Set the prefix on the addresses
            List<ValidatedAddressEntity> addressList = addresses.ToList();
            addressList.ForEach(x => x.AddressPrefix = fieldPrefix);
            
            if (scope.valueMap.ContainsKey(entityId))
            {
                if (scope.valueMap[entityId].ContainsKey(fieldPrefix))
                {
                    scope.valueMap[entityId][fieldPrefix] = addressList;
                }
                else
                {
                    scope.valueMap[entityId].Add(fieldPrefix, addressList);
                }
            }
            else
            {
                scope.valueMap.Add(entityId, new Dictionary<string, List<ValidatedAddressEntity>>{ { fieldPrefix, addressList } });
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

        /// <summary>
        /// Terminate the scope
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Terminate the scope
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Current = null;   
            }
        }
    }
}
