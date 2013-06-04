using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interapptive.Shared.Net.OAuth
{
    /// <summary>
    /// Comparer class for the pair type
    /// </summary>
    public class PairComparer : IComparer<Pair<string, string>>
    {
        /// <summary>
        /// Compare Function
        /// </summary>
        public int Compare(Pair<string, string> x, Pair<string, string> y)
        {
            if (x == null)
                throw new ArgumentNullException("x", "x required");
            if (y == null)
                throw new ArgumentNullException("y", "y required");
            if (x.Left == y.Left)
            {
                return string.Compare(x.Right, y.Right);
            }
            else
            {
                return string.Compare(x.Left, y.Left);
            }
        }
    }
}
