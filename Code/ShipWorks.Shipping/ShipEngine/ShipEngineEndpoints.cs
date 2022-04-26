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

        public static string PurchaseLabelWithRate(string rateId) => $"v1/labels/rates/{rateId}";

        public static string PurchaseLabel => "v1/labels";

        public static string RateShipment => "v1/rates";

        public static string VoidLabel(string labelId) => $"v1/labels/{labelId}/void";

        public static string TrackLabel(string labelId) => $"v1/labels/{labelId}/track";

        public static string Track => "v1/tracking";
    }
}
