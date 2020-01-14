using System;
using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Rakuten.DTO
{
    /// <summary>
    /// Payment entity returned by Rakuten
    /// </summary>
    [Obfuscation(Exclude = true)]
    public class RakutenPayment
    {   /// <summary>
        /// The payment ID
        /// </summary>
        [JsonProperty("orderPaymentId")]
        public string OrderPaymentID { get; set; }

        /// <summary>
        /// The payment status
        /// </summary>
        [JsonProperty("paymentStatus")]
        public string PaymentStatus { get; set; }

        /// <summary>
        /// The amount paid
        /// </summary>
        [JsonProperty("payAmount")]
        public string PayAmount { get; set; }

        /// <summary>
        /// The points used by the customer
        /// </summary>
        [JsonProperty("pointAmount")]
        public string PointAmount { get; set; }

        /// <summary>
        /// The payment date
        /// </summary>
        [JsonProperty("paymentDate")]
        public DateTime PaymentDate { get; set; }
    }
}
