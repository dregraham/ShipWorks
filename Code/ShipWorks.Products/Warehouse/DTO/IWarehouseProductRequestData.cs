namespace ShipWorks.Products.Warehouse.DTO
{
    /// <summary>
    /// Basic data for a Warehouse Product request
    /// </summary>
    public interface IWarehouseProductRequestData
    {
        /// <summary>
        /// Id of the warehouse to which the request applies
        /// </summary>
        string WarehouseId { get; set; }
    }
}