using System;
using System.Reflection;

namespace ShipWorks.Warehouse.DTO.Orders
{
    /// <summary>
    /// Note for a warehouse order
    /// </summary>
    [Obfuscation]
    public class WarehouseOrderNote
    {
        /// <summary>
        /// Date the note was last edited
        /// </summary>
        public DateTime Edited { get; set; }

        /// <summary>
        /// The text of the note
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The visibility of the note
        /// </summary>
        public int Visibility { get; set; }
    }
}
