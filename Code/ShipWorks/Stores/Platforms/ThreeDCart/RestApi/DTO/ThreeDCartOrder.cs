using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.ThreeDCart.RestApi.DTO
{
    public class ThreeDCartOrder
    {
        [JsonProperty("TotalCount")]
        public int TotalCount { get; set; }

        [JsonProperty("InvoiceNumberPrefix")]
        public string InvoiceNumberPrefix { get; set; }

        [JsonProperty("InvoiceNumber")]
        public int InvoiceNumber { get; set; }

        [JsonProperty("OrderID")]
        public long OrderID { get; set; }

        [JsonProperty("CustomerID")]
        public int CustomerID { get; set; }

        [JsonProperty("OrderDate")]
        public DateTime OrderDate { get; set; }

        [JsonProperty("OrderStatusID")]
        public int OrderStatusID { get; set; }

        [JsonProperty("LastUpdate")]
        public DateTime LastUpdate { get; set; }

        [JsonProperty("UserID")]
        public string UserID { get; set; }

        [JsonProperty("SalesPerson")]
        public string SalesPerson { get; set; }

        [JsonProperty("ContinueURL")]
        public string ContinueURL { get; set; }

        [JsonProperty("BillingFirstName")]
        public string BillingFirstName { get; set; }

        [JsonProperty("BillingLastName")]
        public string BillingLastName { get; set; }

        [JsonProperty("BillingCompany")]
        public string BillingCompany { get; set; }

        [JsonProperty("BillingAddress")]
        public string BillingAddress { get; set; }

        [JsonProperty("BillingAddress2")]
        public string BillingAddress2 { get; set; }

        [JsonProperty("BillingCity")]
        public string BillingCity { get; set; }

        [JsonProperty("BillingState")]
        public string BillingState { get; set; }

        [JsonProperty("BillingZipCode")]
        public string BillingZipCode { get; set; }

        [JsonProperty("BillingCountry")]
        public string BillingCountry { get; set; }

        [JsonProperty("BillingPhoneNumber")]
        public string BillingPhoneNumber { get; set; }

        [JsonProperty("BillingEmail")]
        public string BillingEmail { get; set; }

        [JsonProperty("BillingPaymentMethod")]
        public string BillingPaymentMethod { get; set; }

        [JsonProperty("BillingOnLinePayment")]
        public bool BillingOnLinePayment { get; set; }

        [JsonProperty("BillingPaymentMethodID")]
        public string BillingPaymentMethodID { get; set; }

        [JsonProperty("ShipmentList")]
        public IEnumerable<ThreeDCartShipment> ShipmentList { get; set; }

        [JsonProperty("OrderItemList")]
        public IEnumerable<ThreeDCartOrderItem> OrderItemList { get; set; }

        [JsonProperty("OrderDiscount")]
        public decimal OrderDiscount { get; set; }

        [JsonProperty("SalesTax")]
        public decimal SalesTax { get; set; }

        [JsonProperty("SalesTax2")]
        public decimal SalesTax2 { get; set; }

        [JsonProperty("SalesTax3")]
        public decimal SalesTax3 { get; set; }

        [JsonProperty("OrderAmount")]
        public decimal OrderAmount { get; set; }

        [JsonProperty("AffiliateCommission")]
        public double AffiliateCommission { get; set; }

        [JsonProperty("TransactionList")]
        public IList<ThreeDCartTransaction> TransactionList { get; set; }

        [JsonProperty("CardType")]
        public string CardType { get; set; }

        //[JsonProperty("CardNumber")]
        //public string CardNumber { get; set; }

        //[JsonProperty("CardName")]
        //public string CardName { get; set; }

        //[JsonProperty("CardExpirationMonth")]
        //public string CardExpirationMonth { get; set; }

        //[JsonProperty("CardExpirationYear")]
        //public string CardExpirationYear { get; set; }

        //[JsonProperty("CardIssueNumber")]
        //public string CardIssueNumber { get; set; }

        //[JsonProperty("CardStartMonth")]
        //public string CardStartMonth { get; set; }

        //[JsonProperty("CardStartYear")]
        //public string CardStartYear { get; set; }

        //[JsonProperty("CardAddress")]
        //public string CardAddress { get; set; }

        //[JsonProperty("CardVerification")]
        //public string CardVerification { get; set; }

        //[JsonProperty("RewardPoints")]
        //public string RewardPoints { get; set; }

        [JsonProperty("QuestionList")]
        public IEnumerable<ThreeDCartQuestion> QuestionList { get; set; }

        //[JsonProperty("Referer")]
        //public string Referer { get; set; }

        //[JsonProperty("IP")]

        //public string IP { get; set; }

        [JsonProperty("CustomerComments")]
        public string CustomerComments { get; set; }

        [JsonProperty("InternalComments")]
        public string InternalComments { get; set; }

        [JsonProperty("ExternalComments")]
        public string ExternalComments { get; set; }

        public bool isSubOrder { get; set; }

        public bool hasSubOrders { get; set; }
    }
}