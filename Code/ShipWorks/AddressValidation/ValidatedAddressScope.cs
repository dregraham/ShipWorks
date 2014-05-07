using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Interapptive.Shared.Business;
using ShipWorks.Data;
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
        readonly Dictionary<long, List<ValidatedAddressEntity>> valueMap = new Dictionary<long, List<ValidatedAddressEntity>>();

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
        public static void StoreAddresses(long entityId, IEnumerable<ValidatedAddressEntity> addresses)
        {
            ValidatedAddressScope scope = Current;
            if (scope == null)
            {
                throw new InvalidOperationException("Cannot be used when there is no ValidatedAddressScope in scope.");
            }

            if (scope.valueMap.ContainsKey(entityId))
            {
                scope.valueMap[entityId] = addresses.ToList();
            }
            else
            {
                scope.valueMap.Add(entityId, addresses.ToList());
            }
        }

        /// <summary>
        /// Save the stored validated addresses to the database
        /// </summary>
        public void FlushAddressesToDatabase(SqlAdapter sqlAdapter, long entityId)
        {
            FlushAddressesToDatabase(new AdapterAddressValidationDataAccess(sqlAdapter), entityId);
        }

        /// <summary>
        /// Save the stored validated addresses to the database
        /// </summary>
        public void FlushAddressesToDatabase(IAddressValidationDataAccess dataAccess, long entityId)
        {
            if (!valueMap.ContainsKey(entityId))
            {
                return;
            }

            ValidatedAddressManager.DeleteExistingAddresses(dataAccess, entityId);

            foreach (ValidatedAddressEntity address in valueMap[entityId])
            {
                ValidatedAddressManager.SaveEntityAddress(dataAccess, entityId, address);
            }

            valueMap.Remove(entityId);
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
