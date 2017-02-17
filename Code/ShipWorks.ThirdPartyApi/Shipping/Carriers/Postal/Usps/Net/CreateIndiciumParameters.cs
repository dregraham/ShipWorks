using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net
{
    /// <summary>
    /// Input parameters for creating an indicium
    /// </summary>
    public class CreateIndiciumParameters
    {
        public object Item { get; set; }
        public string IntegratorTxID { get; set; }
        public string TrackingNumber { get; set; }
        public RateV20 Rate { get; set; }
        public Address From { get; set; }
        public Address To { get; set; }
        public string CustomerID { get; set; }
        public CustomsV4 Customs { get; set; }
        public bool SampleOnly { get; set; }
        public PostageMode PostageMode { get; set; }
        public ImageType ImageType { get; set; }
        public EltronPrinterDPIType EltronPrinterDPIType { get; set; }
        public string Memo { get; set; }
        public int CostCodeId { get; set; }
        public bool DeliveryNotification { get; set; }
        public ShipmentNotification ShipmentNotification { get; set; }
        public int RotationDegrees { get; set; }
        public int? HorizontalOffset { get; set; }
        public bool HorizontalOffsetSpecified { get; set; }
        public int? VerticalOffset { get; set; }
        public bool VerticalOffsetSpecified { get; set; }
        public int? PrintDensity { get; set; }
        public bool PrintDensitySpecified { get; set; }
        public bool? PrintMemo { get; set; }
        public bool PrintMemoSpecified { get; set; }
        public bool? PrintInstructions { get; set; }
        public bool PrintInstructionsSpecified { get; set; }
        public bool RequestPostageHash { get; set; }
        public NonDeliveryOption NonDeliveryOption { get; set; }
        public Address RedirectTo { get; set; }
        public string OriginalPostageHash { get; set; }
        public bool? ReturnImageData { get; set; }
        public bool ReturnImageDataSpecified { get; set; }
        public string InternalTransactionNumber { get; set; }
        public PaperSizeV1 PaperSize { get; set; }
        public LabelRecipientInfo EmailLabelTo { get; set; }
        public bool PayOnPrint { get; set; }
        public int? ReturnLabelExpirationDays { get; set; }
        public bool ReturnLabelExpirationDaysSpecified { get; set; }
        public ImageDpi ImageDpi { get; set; }
        public string RateToken { get; set; }
        public string OrderId { get; set; }
    }
}