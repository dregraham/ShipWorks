using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores
{
    /// <summary>
    /// Apply to StoreTypeCode enumeration values to provide information about the store type
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class StoreTypeIdentityAttribute : Attribute
    {
        string tangoIdentifier;
        string licenseSalt;

        /// <summary>
        /// Constructor
        /// </summary>
        public StoreTypeIdentityAttribute(string tangoIdentifier, string licenseSalt)
        {
            this.tangoIdentifier = tangoIdentifier;
            this.licenseSalt = licenseSalt;
        }

        /// <summary>
        /// The identifier in Tango.  May not be needed after converstion to Tango.net
        /// </summary>
        public string TangoIdentifier
        {
            get { return tangoIdentifier; }
        }

        /// <summary>
        /// License salt used for decripting and creating licenses
        /// </summary>
        public string LicenseSalt
        {
            get { return licenseSalt; }
        }
    }
}
