using System;
using RestSharp;

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
        public const string OdbcStores = "api/stores/{0}/odbc";
        public const string GenericFile = "api/stores/{0}/genericFile";
        public const string UploadOrders = "api/orders";

        private const string linkWarehouse = "api/warehouses/{0}/link";
        private const string orders = "api/warehouses/{0}/orders";
        private const string shipOrder = "api/orders/{0}/ship";
        private const string voidShipment = "api/orders/{0}/void";

        /// <summary>
        /// Create a link warehouse endpoint
        /// </summary>
        public static string LinkWarehouse(string warehouseId) => string.Format(linkWarehouse, warehouseId);

        /// <summary>
        /// Get an ODBC store
        /// </summary>
        public static string GetOdbcStore(string warehouseStoreID) =>
            string.Format(OdbcStores, warehouseStoreID);

        /// <summary>
        /// Get an GenericFile store
        /// </summary>
        public static string GetGenericFileStore(string warehouseStoreID) =>
            string.Format(GenericFile, warehouseStoreID);

        /// <summary>
        /// Create a Stores endpoint
        /// </summary>
        public static string UpdateStoreCredentials(string warehouseStoreID) =>
            $"{Stores}/{warehouseStoreID}/credentials";

        /// <summary>
        /// Create an orders endpoint with a warehouse store ID
        /// </summary>
        public static string Orders(string warehouseID) => string.Format(orders, warehouseID);

        /// <summary>
        /// Create ship order endpoint with given warehouseOrderID
        /// </summary>
        public static string ShipOrder(string warehouseOrderID) => string.Format(shipOrder, warehouseOrderID);

        /// <summary>
        /// Create void order endpoint with given warehouseOrderid
        /// </summary>
        public static string VoidShipment(string warehouseOrderID) => string.Format(voidShipment, warehouseOrderID);
    }
}
