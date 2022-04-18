using System;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse
{
    /// <summary>
    /// Warehouse endpoints
    /// </summary>
    public static class WarehouseEndpoints
    {
        public const string Login = "api/auth/token/login";
        public const string Warehouses = "api/warehouses";
        public const string SqlConfig = "api/customer/warehouse/setSqlConfig";
        public const string RefreshToken = "api/auth/token/refresh";
        public const string UploadProducts = "api/products/import";
        public const string GenerateDataKey = "api/store/generateDataKey";
        public const string Stores = "api/stores";
        public const string OdbcStores = "api/stores/{0}/odbc";
        public const string GenericFile = "api/stores/{0}/genericFile";
        public const string SyncStores = "api/customer/store/sync";
        public const string UploadOrders = "api/orders";
        public const string AddProduct = "api/products";
        public const string SetActivationBulk = "api/products/activation";
        public const string GetConfig = "api/config";
        public const string GetSmsVerificationNumber = "api/config/smsVerificationNumber";
        public const string Tracking = "api/tracking";

        private const string notifyShipped = "api/customer/notifyShipped/{0}";
        private const string linkWarehouse = "api/warehouses/{0}/link";
        private const string orders = "api/warehouses/{0}/orders";
        private const string shipOrder = "api/orders/{0}/ship";
        private const string voidShipment = "api/orders/{0}/void";
        private const string rerouteOrderItems = "api/orders/{0}/rerouteItems";
        private const string getTrackingUpdates = "api/tracking/{0}";
        private const string initiateMonoauth = "api/ordersource/{0}/auth?RedirectUrl={1}/callbacks/{0}monoauth/subscribe";
         
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
        /// Create void order endpoint with given warehouseOrderID
        /// </summary>
        public static string VoidShipment(string warehouseOrderID) => string.Format(voidShipment, warehouseOrderID);

        /// <summary>
        /// Create a reroute order items endpoint with an warehouseOrderID
        /// </summary>
        public static string RerouteOrderItems(string warehouseOrderID) => string.Format(rerouteOrderItems, warehouseOrderID);

        /// <summary>
        /// Create a change product route
        /// </summary>
        public static string ChangeProduct(IProductVariantEntity product) => $"api/product/{product.HubProductId}";

        /// <summary>
        /// Create a get product route
        /// </summary>
        public static string GetProduct(string hubProductId) => $"api/product/{hubProductId}";

        /// <summary>
        /// Create a get products after sequence route
        /// </summary>
        public static string GetProductsAfterSequence(string warehouseId, long sequence) =>
            $"api/products/sync/{warehouseId}/after/{sequence}";

        public static string NotifyShipped(string salesOrderId) =>
            string.Format(notifyShipped, salesOrderId);

        /// <summary>
        /// Create a get tracking updates route
        /// </summary>
        public static string GetTrackingUpdatesAfter(DateTime lastSeenTimestamp) =>
            string.Format(getTrackingUpdates, lastSeenTimestamp.ToUniversalTime().ToString("o"));

        /// <summary>
        /// Get the URL to get the InitiateMonoauthUrl
        /// </summary>
        public static string GetInitiateMonoauthUrl(string orderSourceName, string redirectUrl) =>
            string.Format(initiateMonoauth, orderSourceName, redirectUrl);
    }
}
