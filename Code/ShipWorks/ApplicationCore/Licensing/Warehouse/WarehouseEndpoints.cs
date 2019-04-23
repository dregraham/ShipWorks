﻿namespace ShipWorks.ApplicationCore.Licensing.Warehouse
{
    /// <summary>
    /// Warehouse endpoints
    /// </summary>
    public static class WarehouseEndpoints
    {
        private const string linkWarehouse = "api/warehouses/{0}/link";
        public const string Login = "api/auth/token/login";
        public const string Warehouses = "api/warehouses";
        public const string RefreshToken = "api/auth/token/refresh";

        /// <summary>
        /// Create a link warehouse endpoint
        /// </summary>
        public static string LinkWarehouse(string warehouseId) => string.Format(linkWarehouse, warehouseId);
    }
}
