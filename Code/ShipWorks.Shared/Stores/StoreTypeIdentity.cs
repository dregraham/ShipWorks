using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores
{
    /// <summary>
    /// Identity information about a store
    /// </summary>
    public class StoreTypeIdentity
    {
        // Caches the identities to save expensive attribute lookups
        static Dictionary<StoreTypeCode, StoreTypeIdentity> cache = new Dictionary<StoreTypeCode, StoreTypeIdentity>();

        // The store type code
        StoreTypeCode typeCode;

        // The user-visible name of the store
        string name;

        // The code used to identify the store type in tango.  may not be needed after converting to tango.net
        string tangoCode;

        // Used to encrypt and decrypt licenses
        string licenseSalt;

        /// <summary>
        /// Constructor
        /// </summary>
        private StoreTypeIdentity(StoreTypeCode typeCode)
        {
            // Get the field info reflected by the type code
            FieldInfo fieldInfo = GetReflectedFieldInfo(typeCode);

            if (fieldInfo == null)
            {
                throw new InvalidOperationException("Could not find field info for " + typeCode);
            }

            // Get the custom identity attribute
            StoreTypeIdentityAttribute attribute = Attribute.GetCustomAttribute(fieldInfo, typeof(StoreTypeIdentityAttribute)) as StoreTypeIdentityAttribute;

            if (attribute == null)
            {
                throw new InvalidOperationException("Could not found identity attribute for " + typeCode);
            }

            this.typeCode = typeCode;
            this.name = EnumHelper.GetDescription(typeCode);
            this.tangoCode = attribute.TangoIdentifier;
            this.licenseSalt = attribute.LicenseSalt;
        }

        /// <summary>
        /// The type code of the store
        /// </summary>
        public StoreTypeCode StoreTypeCode
        {
            get { return typeCode; }
        }

        /// <summary>
        /// The user-visible name of the store
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// The code used by tango to reference the store type.  May not be needed after converting to tango.net
        /// </summary>
        public string TangoCode
        {
            get { return tangoCode; }
        }

        /// <summary>
        /// Used to encrypt\decrypt licenses.
        /// </summary>
        public string LicenseSalt
        {
            get { return licenseSalt; }
        }

        /// <summary>
        /// Get the FieldInfo that represents the enum value, or null if its not found.
        /// </summary>
        private static FieldInfo GetReflectedFieldInfo(StoreTypeCode typeCode)
        {
            FieldInfo[] enumFields = typeof(StoreTypeCode).GetFields(BindingFlags.Public | BindingFlags.Static);

            return enumFields.SingleOrDefault(f => Convert.ToInt32(f.GetRawConstantValue()) == (int) typeCode);
        }

        /// <summary>
        /// Reads the identity from the given StoreTypeCode
        /// </summary>
        public static StoreTypeIdentity FromCode(StoreTypeCode typeCode)
        {
            StoreTypeIdentity identity = null;

            lock (cache)
            {
                if (!cache.TryGetValue(typeCode, out identity))
                {
                    identity = new StoreTypeIdentity(typeCode);
                    cache[typeCode] = identity;
                }
            }

            return identity;
        }
    }
}
