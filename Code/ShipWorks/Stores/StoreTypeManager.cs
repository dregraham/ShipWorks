using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores
{
    /// <summary>
    /// Manager of all the StoreTypes available in ShipWorks
    /// </summary>
    public static class StoreTypeManager
    {
        private static Lazy<Dictionary<StoreTypeCode, bool>> genericModuleStoreTypeMap =
            new Lazy<Dictionary<StoreTypeCode, bool>>(LoadGenericModuleStoreTypeDictionary);

        /// <summary>
        /// Returns all store types in ShipWorks
        /// </summary>
        public static List<StoreType> StoreTypes
        {
            get
            {
                return Enum.GetValues(typeof(StoreTypeCode))
                    .OfType<StoreTypeCode>()
                    .Except(new[] { StoreTypeCode.Invalid })
                    .Where(IsStoreTypeEnabled)
                    .Select(GetType)
                    .OrderBy(x => x.StoreTypeName, StringComparer.OrdinalIgnoreCase)
                    .ToList();
            }
        }

        /// <summary>
        /// Get the StoreType instance of the specified StoreEntity
        /// </summary>
        public static StoreType GetType(StoreEntity store)
        {
            MethodConditions.EnsureArgumentIsNotNull(store, nameof(store));

            return GetType((StoreTypeCode) store.TypeCode, store);
        }

        /// <summary>
        /// The indexer of the class based on store type
        /// </summary>
        public static StoreType GetType(StoreTypeCode typeCode) =>
            GetType(typeCode, null);

        /// <summary>
        /// Get the ShipmentType based on the given type code
        /// </summary>
        public static StoreType GetType(StoreTypeCode typeCode, StoreEntity store) =>
            GetType(typeCode, store, IoC.UnsafeGlobalLifetimeScope);

        /// <summary>
        /// Return the StoreType for the given store type.  If store is not null,
        /// then any "instance" methods of the StoreType will use it.
        /// </summary>
        public static StoreType GetType(StoreTypeCode typeCode, StoreEntity store, ILifetimeScope lifetimeScope)
        {
            if (lifetimeScope.IsRegisteredWithKey<StoreType>(typeCode))
            {
                return lifetimeScope.ResolveKeyed<StoreType>(typeCode, TypedParameter.From(store));
            }

            throw new InvalidOperationException("Invalid store type. " + typeCode);
        }

        /// <summary>
        /// Determines whether the store type is disabled. This is only temporary, so we can continue
        /// to release ShipWorks until supporting materials for the new store types are ready.
        /// </summary>
        private static bool IsStoreTypeEnabled(StoreTypeCode typeCode)
        {
            // Don't show in ShipWorks until marketing materials and other ancillary
            // materials are ready to go
            List<StoreTypeCode> disabledTypes = new List<StoreTypeCode>
            {

            };

            return !disabledTypes.Contains(typeCode);
        }

        /// <summary>
        /// Load the dictionary of StoreTypeCodes and whether or not they are GenericModuleBased
        /// </summary>
        /// <returns></returns>
        private static Dictionary<StoreTypeCode, bool> LoadGenericModuleStoreTypeDictionary()
        {
            Dictionary<StoreTypeCode, bool> result = new Dictionary<StoreTypeCode, bool>();
            IEnumerable<EnumEntry<StoreTypeCode>> storeTypeCodes = EnumHelper.GetEnumList<StoreTypeCode>();

            Type genericModuleType = typeof(GenericModuleStoreType);

            foreach (EnumEntry<StoreTypeCode> storeTypeCode in storeTypeCodes)
            {
                result.Add(storeTypeCode.Value, genericModuleType.IsAssignableFrom(GetType(storeTypeCode.Value).GetType()));
            }

            return result;
        }

        /// <summary>
        /// Check to see if the given StoreTypeCode is GenericModule based
        /// </summary>
        public static bool IsStoreTypeCodeGenericModuleBased(StoreTypeCode storeTypeCode)
        {
            if (genericModuleStoreTypeMap.Value.ContainsKey(storeTypeCode))
            {
                return genericModuleStoreTypeMap.Value[storeTypeCode];
            }

            return false;
        }
    }
}
