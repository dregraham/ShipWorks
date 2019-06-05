using System;
using System.Collections.Generic;

namespace ShipWorks.Warehouse.DTO.Orders
{
    public class WarehouseOrderAddress
    {
        public string UnparsedName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string Street3 { get; set; }
        public string City { get; set; }
        public string StateProvCode { get; set; }
        public string PostalCode { get; set; }
        public string CountryCode { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
    }

    public abstract class WarehouseOrder
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

        public abstract IEnumerable<WarehouseOrderItem> Items { get; set; }
    }
}
