using System.Collections.Generic;
using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.Magento.DTO.Interfaces;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotZero
{
    public class Payment : IPayment
    {
        [JsonProperty("accountStatus")]
        public string AccountStatus { get; set; }

        [JsonProperty("additionalData")]
        public string AdditionalData { get; set; }

        [JsonProperty("additionalInformation")]
        public IList<string> AdditionalInformation { get; set; }

        [JsonProperty("addressStatus")]
        public string AddressStatus { get; set; }

        [JsonProperty("amountAuthorized")]
        public int AmountAuthorized { get; set; }

        [JsonProperty("amountCanceled")]
        public int AmountCanceled { get; set; }

        [JsonProperty("amountOrdered")]
        public double AmountOrdered { get; set; }

        [JsonProperty("amountPaid")]
        public int AmountPaid { get; set; }

        [JsonProperty("amountRefunded")]
        public int AmountRefunded { get; set; }

        [JsonProperty("anetTransMethod")]
        public string AnetTransMethod { get; set; }

        [JsonProperty("baseAmountAuthorized")]
        public int BaseAmountAuthorized { get; set; }

        [JsonProperty("baseAmountCanceled")]
        public int BaseAmountCanceled { get; set; }

        [JsonProperty("baseAmountOrdered")]
        public double BaseAmountOrdered { get; set; }

        [JsonProperty("baseAmountPaid")]
        public int BaseAmountPaid { get; set; }

        [JsonProperty("baseAmountPaidOnline")]
        public int BaseAmountPaidOnline { get; set; }

        [JsonProperty("baseAmountRefunded")]
        public int BaseAmountRefunded { get; set; }

        [JsonProperty("baseAmountRefundedOnline")]
        public int BaseAmountRefundedOnline { get; set; }

        [JsonProperty("baseShippingAmount")]
        public double BaseShippingAmount { get; set; }

        [JsonProperty("baseShippingCaptured")]
        public int BaseShippingCaptured { get; set; }

        [JsonProperty("baseShippingRefunded")]
        public int BaseShippingRefunded { get; set; }

        [JsonProperty("ccApproval")]
        public string CcApproval { get; set; }

        [JsonProperty("ccAvsStatus")]
        public string CcAvsStatus { get; set; }

        [JsonProperty("ccCidStatus")]
        public string CcCidStatus { get; set; }

        [JsonProperty("ccDebugRequestBody")]
        public string CcDebugRequestBody { get; set; }

        [JsonProperty("ccDebugResponseBody")]
        public string CcDebugResponseBody { get; set; }

        [JsonProperty("ccDebugResponseSerialized")]
        public string CcDebugResponseSerialized { get; set; }

        [JsonProperty("ccExpMonth")]
        public string CcExpMonth { get; set; }

        [JsonProperty("ccExpYear")]
        public string CcExpYear { get; set; }

        [JsonProperty("ccLast4")]
        public string CcLast4 { get; set; }

        [JsonProperty("ccNumberEnc")]
        public string CcNumberEnc { get; set; }

        [JsonProperty("ccOwner")]
        public string CcOwner { get; set; }

        [JsonProperty("ccSecureVerify")]
        public string CcSecureVerify { get; set; }

        [JsonProperty("ccSsIssue")]
        public string CcSsIssue { get; set; }

        [JsonProperty("ccSsStartMonth")]
        public string CcSsStartMonth { get; set; }

        [JsonProperty("ccSsStartYear")]
        public string CcSsStartYear { get; set; }

        [JsonProperty("ccStatus")]
        public string CcStatus { get; set; }

        [JsonProperty("ccStatusDescription")]
        public string CcStatusDescription { get; set; }

        [JsonProperty("ccTransId")]
        public string CcTransId { get; set; }

        [JsonProperty("ccType")]
        public string CcType { get; set; }

        [JsonProperty("echeckAccountName")]
        public string EcheckAccountName { get; set; }

        [JsonProperty("echeckAccountType")]
        public string EcheckAccountType { get; set; }

        [JsonProperty("echeckBankName")]
        public string EcheckBankName { get; set; }

        [JsonProperty("echeckRoutingNumber")]
        public string EcheckRoutingNumber { get; set; }

        [JsonProperty("echeckType")]
        public string EcheckType { get; set; }

        [JsonProperty("entityId")]
        public int EntityId { get; set; }

        [JsonProperty("lastTransId")]
        public string LastTransId { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("parentId")]
        public int ParentId { get; set; }

        [JsonProperty("poNumber")]
        public string PoNumber { get; set; }

        [JsonProperty("protectionEligibility")]
        public string ProtectionEligibility { get; set; }

        [JsonProperty("quotePaymentId")]
        public int QuotePaymentId { get; set; }

        [JsonProperty("shippingAmount")]
        public double ShippingAmount { get; set; }

        [JsonProperty("shippingCaptured")]
        public int ShippingCaptured { get; set; }

        [JsonProperty("shippingRefunded")]
        public int ShippingRefunded { get; set; }

        [JsonProperty("extensionAttributes")]
        public IExtensionAttributes ExtensionAttributes { get; set; }
    }
}