using System;

namespace ShipWorks.Warehouse.DTO.Orders
{
    public class WarehouseOrderNote
    {
        /// <summary>
        /// Date the note was last edited
        /// </summary>
        public DateTime Edited { get; set; }
        
        /// <summary>
        /// The Text property of the Entity Note<br/><br/>
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The Visibility property of the Entity Note<br/><br/>
        /// </summary>
        public int Visibility { get; set; }
    }
}