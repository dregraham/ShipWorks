namespace ShipWorks.Warehouse.DTO.Orders
{
    /// <summary>
    /// Payment detail for a warehouse order
    /// </summary>
    public class WarehouseOrderPaymentDetail
    {
        /// <summary>
        /// Label of the payment detail
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Value of the payment detail
        /// </summary>
        public string Value { get; set; }
    }
}
