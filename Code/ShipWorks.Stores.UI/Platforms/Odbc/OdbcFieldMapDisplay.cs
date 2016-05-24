using System.Reflection;
using ShipWorks.Stores.Platforms.Odbc.Mapping;

namespace ShipWorks.Stores.UI.Platforms.Odbc
{
    public class OdbcFieldMapDisplay
    {

        public OdbcFieldMapDisplay(string displayName, OdbcFieldMap map)
        {
            DisplayName = displayName;
            Map = map;
        }

        /// <summary>
        /// The Display Name
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the map.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public OdbcFieldMap Map { get; set; }
    }
}