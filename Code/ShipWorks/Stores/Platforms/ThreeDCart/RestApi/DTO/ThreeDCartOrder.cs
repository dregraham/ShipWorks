using System;
using System.Collections.Generic;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.ThreeDCart.RestApi.DTO
{
    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    public class ThreeDCartOrder
    {
        public int TotalCount { get; set; }

        public string InvoiceNumberPrefix { get; set; }

        public int InvoiceNumber { get; set; }

        public long OrderID { get; set; }

        public int CustomerID { get; set; }

        public DateTime OrderDate { get; set; }

        public int OrderStatusID { get; set; }

        public DateTime LastUpdate { get; set; }

        public string UserID { get; set; }

        public string SalesPerson { get; set; }

        public string ContinueURL { get; set; }

        public string BillingFirstName { get; set; }

        public string BillingLastName { get; set; }

        public string BillingCompany { get; set; }

        public string BillingAddress { get; set; }

        public string BillingAddress2 { get; set; }

        public string BillingCity { get; set; }

        public string BillingState { get; set; }

        public string BillingZipCode { get; set; }

        public string BillingCountry { get; set; }

        public string BillingPhoneNumber { get; set; }

        public string BillingEmail { get; set; }

        public string BillingPaymentMethod { get; set; }

        public bool BillingOnLinePayment { get; set; }

        public string BillingPaymentMethodID { get; set; }

        public IEnumerable<ThreeDCartShipment> ShipmentList { get; set; }

        public IEnumerable<ThreeDCartOrderItem> OrderItemList { get; set; }

        public decimal OrderDiscount { get; set; }

        public decimal SalesTax { get; set; }

        public decimal SalesTax2 { get; set; }

        public decimal SalesTax3 { get; set; }

        public decimal OrderAmount { get; set; }

        public double AffiliateCommission { get; set; }

        public IList<ThreeDCartTransaction> TransactionList { get; set; }

        public string CardType { get; set; }

        public IEnumerable<ThreeDCartQuestion> QuestionList { get; set; }

        public string CustomerComments { get; set; }

        public string InternalComments { get; set; }

        public string ExternalComments { get; set; }

        public bool IsSubOrder { get; set; }

        public bool HasSubOrders { get; set; }
    }
}