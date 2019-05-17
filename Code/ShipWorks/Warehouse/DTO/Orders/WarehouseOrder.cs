using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ShipWorks.Warehouse.DTO.Orders
{
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
        public string BillUnparsedName { get; set; }
        public string BillFirstName { get; set; }
        public string BillMiddleName { get; set; }
        public string BillLastName { get; set; }
        public string BillCompany { get; set; }
        public string BillStreet1 { get; set; }
        public string BillStreet2 { get; set; }
        public string BillStreet3 { get; set; }
        public string BillCity { get; set; }
        public string BillStateProvCode { get; set; }
        public string BillPostalCode { get; set; }
        public string BillCountryCode { get; set; }
        public string BillPhone { get; set; }
        public string BillFax { get; set; }
        public string BillEmail { get; set; }
        public string BillWebsite { get; set; }
        public string ShipUnparsedName { get; set; }
        public string ShipFirstName { get; set; }
        public string ShipMiddleName { get; set; }
        public string ShipLastName { get; set; }
        public string ShipCompany { get; set; }
        public string ShipStreet1 { get; set; }
        public string ShipStreet2 { get; set; }
        public string ShipStreet3 { get; set; }
        public string ShipCity { get; set; }
        public string ShipStateProvCode { get; set; }
        public string ShipPostalCode { get; set; }
        public string ShipCountryCode { get; set; }
        public string ShipPhone { get; set; }
        public string ShipFax { get; set; }
        public string ShipEmail { get; set; }
        public string ShipWebsite { get; set; }
        public string ChannelOrderID { get; set; }
        public DateTime? ShipByDate { get; set; }
        public List<WarehouseOrderCharge> Charges { get; set; }
        public List<WarehouseOrderPaymentDetail> PaymentDetails { get; set; }
        public List<WarehouseOrderNote> Notes { get; set; }
        
        public abstract IEnumerable<WarehouseOrderItem> Items { get; set; }
    }
}
