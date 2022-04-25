namespace ShipWorks.Shipping.ShipEngine
{
    /// <summary>
    /// ShipEngine endpoints
    /// </summary>
    public static class ShipEngineEndpoints
    {
        public static string BaseUrl => "https://platform.shipengine.com";

        public static string DhlEcommerceAccountCreation => "v1/connections/carriers/dhl_ecommerce";

        public static string DhlExpressAccountCreation => "v1/connections/carriers/dhl_express";

        public static string DisconnectAmazonShippingAccount(string accountId) => $"v1/connections/carriers/amazon_shipping_us/{accountId}";

        public static string AsendiaAccountCreation => "v1/connections/carriers/asendia";

        public static string ListCarriers => "v1/carriers";

        public static string StampsAccountCreation => "v1/connections/carriers/stamps_com";

        public static string DisconnectStampsAccount(string carrierId) => $"v1/connections/carriers/stamps_com/{carrierId}";
    }
}
