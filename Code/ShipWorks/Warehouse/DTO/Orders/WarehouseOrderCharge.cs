namespace ShipWorks.Warehouse.DTO.Orders
{
    /// <summary>
    /// Charge for a warehouse order
    /// </summary>
    public class WarehouseOrderCharge
    {
        /// <summary>
        /// Type of the order charge
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Description of the order charge
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Amount of the order charge
        /// </summary>
        public decimal Amount { get; set; }
    }
}
