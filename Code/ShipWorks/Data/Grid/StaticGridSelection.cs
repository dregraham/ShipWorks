using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Grid
{
    /// <summary>
    /// Simple grid selection implementation that is always empty
    /// </summary>
    public class StaticGridSelection : IGridSelection
    {
        List<long> selection;

        /// <summary>
        /// A selection that is always empty
        /// </summary>
        public StaticGridSelection()
        {
            selection = new List<long>();
        }

        /// <summary>
        /// A selection that is always the given set of keys
        /// </summary>
        public StaticGridSelection(List<long> keys)
        {
            this.selection = keys;
        }

        /// <summary>
        /// Never chnages - since the selection is static and never changes
        /// </summary>
        public int Version
        {
            get { return 0; }
        }

        /// <summary>
        /// Selection count
        /// </summary>
        public int Count
        {
            get { return selection.Count; }
        }

        /// <summary>
        /// The keys
        /// </summary>
        public IEnumerable<long> Keys
        {
            get { return selection; }
        }

        /// <summary>
        /// The keys
        /// </summary>
        public IEnumerable<long> OrderedKeys
        {
            get { return Keys; }
        }
    }
}
