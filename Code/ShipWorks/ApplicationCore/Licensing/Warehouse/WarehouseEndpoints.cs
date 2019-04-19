namespace ShipWorks.ApplicationCore.Licensing.Warehouse
{
    /// <summary>
    /// Warehouse endpoints
    /// </summary>
    public static class WarehouseEndpoints
    {
        private const string associateWarehouse = "api/warehouses/{0}/associate";
        public const string Login = "api/auth/token/login";
        public const string Warehouses = "api/warehouses";
        public const string RefreshToken = "api/auth/token/refresh";

        /// <summary>
        /// Create an associate warehouse endpoint
        /// </summary>
        public static string AssociateWarehouse(string warehouseId) => string.Format(associateWarehouse, warehouseId);
    }
}
