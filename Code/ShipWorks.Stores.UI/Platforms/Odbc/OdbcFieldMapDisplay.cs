using System.Collections.ObjectModel;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System.Reflection;

namespace ShipWorks.Stores.UI.Platforms.Odbc
{
    /// <summary>
    /// Used primarily for the view.
    /// </summary>
    public class OdbcFieldMapDisplay
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcFieldMapDisplay"/> class.
        /// </summary>
        public OdbcFieldMapDisplay(string displayName, OdbcFieldMap map)
        {
            DisplayName = displayName;
            Entries = new ObservableCollection<IOdbcFieldMapEntry>(map.Entries);
            Index = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcFieldMapDisplay"/> class.
        /// </summary>
        public OdbcFieldMapDisplay(string displayName, OdbcFieldMap map, int index)
        {
            DisplayName = displayName;
            Entries = new ObservableCollection<IOdbcFieldMapEntry>(map.Entries);
            Index = index;
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
        public ObservableCollection<IOdbcFieldMapEntry> Entries { get; set; }

        /// <summary>
        /// The index of the filed
        /// </summary>
        public int Index { get; set; }
    }
}