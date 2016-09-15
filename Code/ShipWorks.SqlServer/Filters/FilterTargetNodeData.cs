using System;
using System.Collections.Generic;
using System.Text;

namespace ShipWorks.SqlServer
{
    /// <summary>
    /// DTO for Filter target node data
    /// </summary>
    public class FilterTargetNodeData
    {
        /// <summary>
        /// Column masks
        /// </summary>
        public List<byte[]> ColumnMasks { get; set; }

        /// <summary>
        /// Join mask
        /// </summary>
        public int JoinMask { get; set; }
    }
}
