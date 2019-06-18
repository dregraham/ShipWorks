using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ShipWorks.Warehouse.DTO.Orders
{
    [Obfuscation]
    public class WarehouseOrderAddress
    {
        public string UnparsedName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string Street1 { get; set; } = string.Empty;
        public string Street2 { get; set; } = string.Empty;
        public string Street3 { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string StateProvCode { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Fax { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
    }

    [Obfuscation]
    public class WarehouseOrder
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected WarehouseOrder()
        {
            Charges = new List<WarehouseOrderCharge>();
            PaymentDetails = new List<WarehouseOrderPaymentDetail>();
            Notes = new List<WarehouseOrderNote>();
        }

        public long HubSequence { get; set; }
        public string HubOrderId { get; set; }

        public string OrderID { get; set; }

        public string StoreID { get; set; }

        public long WarehouseCustomerID { get; set; }

        public int StoreType { get; set; }

        public string Warehouse { get; set; }

        public int Zone { get; set; }

        public string OrderNumber { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal OrderTotal { get; set; }

        public DateTime OnlineLastModified { get; set; }
        public string OnlineCustomerID { get; set; }
        public string OnlineStatus { get; set; }
        public string OnlineStatusCode { get; set; }
        public string RequestedShipping { get; set; }

        public WarehouseOrderAddress BillAddress { get; set; }
        public WarehouseOrderAddress ShipAddress { get; set; }
        public string ChannelOrderID { get; set; }
        public DateTime? ShipByDate { get; set; }
        public List<WarehouseOrderCharge> Charges { get; set; }
        public List<WarehouseOrderPaymentDetail> PaymentDetails { get; set; }
        public List<WarehouseOrderNote> Notes { get; set; }

        public IEnumerable<WarehouseOrderItem> Items { get; set; }

        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; set; }
    }
}
