using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net
{
    /// <summary>
    /// Input parameters for creating an envelope indicium
    /// </summary>
    public class CreateEnvelopeIndiciumParameters
    {
        public object Item { get; set; }
        public string IntegratorTxID { get; set; }
        public RateV20 Rate { get; set; }
        public Address From { get; set; }
        public Address To { get; set; }
        public string CustomerID { get; set; }
        public CreateIndiciumModeV1 Mode { get; set; }
        public ImageType ImageType { get; set; }
        public int CostCodeId { get; set; }
        public bool HideFIM { get; set; }
        public string RateToken { get; set; }
        public string OrderId { get; set; }
    }
}