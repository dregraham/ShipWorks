﻿using Newtonsoft.Json;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.SparkPay.DTO
{
    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    public class OrderStatus
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("is_open")]
        public bool IsOpen { get; set; }

        [JsonProperty("is_declined")]
        public bool IsDeclined { get; set; }

        [JsonProperty("is_cancelled")]
        public bool IsCancelled { get; set; }

        [JsonProperty("is_shipped")]
        public bool IsShipped { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("email_template_id")]
        public int? EmailTemplateId { get; set; }

        [JsonProperty("updated_at")]
        public object UpdatedAt { get; set; }

        [JsonProperty("created_at")]
        public object CreatedAt { get; set; }

        [JsonProperty("is_fully_refunded")]
        public bool IsFullyRefunded { get; set; }

        [JsonProperty("is_partially_refunded")]
        public bool IsPartiallyRefunded { get; set; }

        [JsonProperty("is_quote_status")]
        public bool IsQuoteStatus { get; set; }
    }
}
