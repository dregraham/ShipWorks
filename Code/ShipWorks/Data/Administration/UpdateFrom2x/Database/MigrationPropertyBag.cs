using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Database
{
    /// <summary>
    /// Name/Value collection for migration runtime properties.
    /// 
    /// Expecting this to expand to contain unique properties per database
    /// involved in the migration. 
    /// </summary>
    public class MigrationPropertyBag
    {
        // underlying datastore
        Dictionary<string, object> properties = new Dictionary<string, object>();

        /// <summary>
        /// Get/Set values
        /// </summary>
        public object this[string name]
        {
            get { return properties[name]; }
            set { properties[name] = value; }
        }

        /// <summary>
        /// Get the names of the properties contained here
        /// </summary>
        public List<string> PropertyNames
        {
            get { return properties.Keys.ToList(); }
        }
    }
}
