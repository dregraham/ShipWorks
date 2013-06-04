using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.SqlServer.Filters.DirtyCounts
{
    /// <summary>
    /// Represents the various way's top-level filter node's can join to other relevant top-level tables
    /// </summary>
    [Flags]
    public enum FilterNodeJoinType
    {
        None               = 0x00000000,

        CustomerToOrder    = 0x00000002,
        CustomerToItem     = 0x00000004,
        CustomerToShipment = 0x00000008,

        OrderToCustomer    = 0x00000100,
        OrderToItem        = 0x00000400,
        OrderToShipment    = 0x00000800,

        ItemToCustomer     = 0x00010000,
        ItemToOrder        = 0x00020000,

        ShipmentToCustomer = 0x01000000,
        ShipmentToOrder    = 0x02000000,
    }
}
