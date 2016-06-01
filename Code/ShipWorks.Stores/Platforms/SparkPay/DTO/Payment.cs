using Newtonsoft.Json;
using System;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.SparkPay.DTO
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)]
    public class Payment
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("customer_id")]
        public int CustomerId { get; set; }

        [JsonProperty("order_id")]
        public int OrderId { get; set; }

        [JsonProperty("customer_payment_method_id")]
        public int CustomerPaymentMethodId { get; set; }

        [JsonProperty("payment_method_id")]
        public int PaymentMethodId { get; set; }

        [JsonProperty("payment_type")]
        public string PaymentType { get; set; }

        [JsonProperty("is_approved")]
        public bool IsApproved { get; set; }

        [JsonProperty("is_declined")]
        public bool IsDeclined { get; set; }

        [JsonProperty("card_type")]
        public string CardType { get; set; }

        [JsonProperty("card_expiration_month")]
        public int CardExpirationMonth { get; set; }

        [JsonProperty("card_expiration_year")]
        public int CardExpirationYear { get; set; }

        [JsonProperty("cardholder_name")]
        public string CardholderName { get; set; }

        [JsonProperty("paid_at")]
        public DateTime PaidAt { get; set; }

        [JsonProperty("approved_at")]
        public string ApprovedAt { get; set; }

        [JsonProperty("authorization_code")]
        public string AuthorizationCode { get; set; }

        [JsonProperty("reject_reason")]
        public string RejectReason { get; set; }

        [JsonProperty("avs_code")]
        public string AvsCode { get; set; }

        [JsonProperty("payment_method_name")]
        public string PaymentMethodName { get; set; }

        [JsonProperty("transaction_type")]
        public string TransactionType { get; set; }

        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("payment_note")]
        public string PaymentNote { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("gift_certificate_id")]
        public object GiftCertificateId { get; set; }

        [JsonProperty("is_captured")]
        public bool IsCaptured { get; set; }

        [JsonProperty("transaction_id")]
        public string TransactionId { get; set; }

        [JsonProperty("is_void")]
        public bool IsVoid { get; set; }

        [JsonProperty("gateway_response_code")]
        public string GatewayResponseCode { get; set; }

        [JsonProperty("cvv_response_code")]
        public string CvvResponseCode { get; set; }

        [JsonProperty("sent_to_spark_pay")]
        public bool SentToSparkPay { get; set; }
    }
}
