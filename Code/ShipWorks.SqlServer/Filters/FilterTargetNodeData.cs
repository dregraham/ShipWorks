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
        public List<byte[]> ColumnMasks { get; set; }

        public int JoinMask { get; set; }
    }
}
