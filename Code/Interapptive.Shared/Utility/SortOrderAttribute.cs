using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Attribute for specifying something's sort order
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public sealed class SortOrderAttribute : Attribute
    {
        int position;

        /// <summary>
        /// Constructor
        /// </summary>
        public SortOrderAttribute(int position)
        {
            this.position = position;
        }

        /// <summary>
        /// The position to be sorted in
        /// </summary>
        public int Position
        {
            get { return position; }
        }
    }
}
