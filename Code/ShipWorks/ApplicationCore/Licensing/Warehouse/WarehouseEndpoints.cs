namespace ShipWorks.ApplicationCore.Licensing.Warehouse
{
    /// <summary>
    /// Warehouse endpoints
    /// </summary>
    public static class WarehouseEndpoints
    {
        public const string Login = "api/auth/token/login";
        public const string Warehouses = "api/warehouses";
        public const string RefreshToken = "api/auth/token/refresh";
        public const string UploadSkus = "api/skus/import";
        public const string GenerateDataKey = "api/store/generateDataKey";
        public const string Stores = "api/stores";

        private const string linkWarehouse = "api/warehouses/{0}/link";
        private const string orders = "api/warehouses/{0}";

        /// <summary>
        /// Create a link warehouse endpoint
        /// </summary>
        public static string LinkWarehouse(string warehouseId) => string.Format(linkWarehouse, warehouseId);

        /// <summary>
        /// Create a Stores endpoint
        /// </summary>
        public static string UpdateStoreCredentials(string warehouseStoreID) =>
            $"{Stores}/{warehouseStoreID}/credentials";

        /// <summary>
        /// Create an orders endpoint with a warehouse store ID
        /// </summary>
        public static string Orders(string warehouseID) => string.Format(orders, warehouseID);
    }
}
