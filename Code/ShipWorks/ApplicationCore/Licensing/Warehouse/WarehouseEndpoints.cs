﻿namespace ShipWorks.ApplicationCore.Licensing.Warehouse
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
        
        private const string linkWarehouse = "api/warehouses/{0}/link";
        private const string stores = "api/stores";
        private const string orders = "api/orders/{0}";

        /// <summary>
        /// Create a link warehouse endpoint
        /// </summary>
        public static string LinkWarehouse(string warehouseId) => string.Format(linkWarehouse, warehouseId);

        /// <summary>
        /// Create a Stores endpoint
        /// </summary>
        public static string Stores(string warehouseStoreID) =>
            string.IsNullOrWhiteSpace(warehouseStoreID) ? stores : $"{stores}/{warehouseStoreID}";

        public static string Orders(string warehouseID) => string.Format(orders, warehouseID);
    }
}
