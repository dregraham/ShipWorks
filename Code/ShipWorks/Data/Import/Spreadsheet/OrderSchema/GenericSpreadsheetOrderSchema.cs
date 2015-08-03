using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Import.Spreadsheet;

namespace ShipWorks.Data.Import.Spreadsheet.OrderSchema
{
    /// <summary>
    /// Schema definition for the Generic CSV input format
    /// </summary>
    public class GenericSpreadsheetOrderSchema : GenericSpreadsheetTargetSchema
    {
        List<GenericSpreadsheetTargetFieldGroup> groups = new List<GenericSpreadsheetTargetFieldGroup>();

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericSpreadsheetOrderSchema()
        {

        }

        /// <summary>
        /// A unique identifier to uniquely identify this schema
        /// </summary>
        public override string SchemaType
        {
            get { return "ShipWorksOrderImport"; }
        }

        /// <summary>
        /// The version of the schema
        /// </summary>
        public override Version SchemaVersion
        {
            get { return new Version(1, 0, 0, 0); }
        }

        /// <summary>
        /// Custom settings applicable to this schema
        /// </summary>
        public override GenericSpreadsheetTargetSchemaSettings CreateSettings()
        {
            return new GenericSpreadsheetOrderMapSettings();
        }

        /// <summary>
        /// The field groups that are available in this schema
        /// </summary>
        public override IEnumerable<GenericSpreadsheetTargetFieldGroup> FieldGroups
        {
            get
            {
                if (groups.Count == 0)
                {
                    CreateSchema();
                }

                return groups;
            }
        }

        /// <summary>
        /// Create the schema
        /// </summary>
        private void CreateSchema()
        {
            groups.AddRange(CreateSchemaFieldGroups());
        }

        /// <summary>
        /// Create all the target field groups for the schema
        /// </summary>
        protected virtual List<GenericSpreadsheetTargetFieldGroup> CreateSchemaFieldGroups()
        {
            return new List<GenericSpreadsheetTargetFieldGroup>
                {
                    CreateOrderGroup(),
                    CreateAddressGroup(),
                    CreateItemsGroup()
                };
        }

        /// <summary>
        /// Create the field group for 'Order'
        /// </summary>
        private GenericSpreadsheetTargetFieldGroup CreateOrderGroup()
        {
            return new GenericSpreadsheetTargetFieldGroup("Order",
                new GenericSpreadsheetTargetField[]
                {
                    new GenericSpreadsheetTargetField("Order.Number",               "Order Number",           typeof(long),       true),
                    new GenericSpreadsheetTargetField("Order.DateTime",             "Order Date and Time",    typeof(DateTime)),
                    new GenericSpreadsheetTargetField("Order.Date",                 "Order Date",             typeof(DateTime)),
                    new GenericSpreadsheetTargetField("Order.Time",                 "Order Time",             typeof(TimeSpan)),
                    new GenericSpreadsheetTargetField("Order.LocalStatus",          "Local Status",           typeof(string)),
                    new GenericSpreadsheetTargetField("Order.OnlineStatus",         "Store Status",          typeof(string)),
                    new GenericSpreadsheetTargetField("Order.RequestedShipping",    "Requested Shipping",     typeof(string)),
                    new GenericSpreadsheetTargetField("Order.CustomerNumber",       "Customer Number",        typeof(string)),
                    new GenericSpreadsheetTargetField("Order.NoteInternal",         "Note (Internal)",        typeof(string)),
                    new GenericSpreadsheetTargetField("Order.NotePublic",           "Note (Public)",          typeof(string)),
                    new GenericSpreadsheetTargetField("Order.ShippingAmount",       "Shipping Amount",        typeof(decimal)),
                    new GenericSpreadsheetTargetField("Order.HandlingAmount",       "Handling Amount",        typeof(decimal)),
                    new GenericSpreadsheetTargetField("Order.DiscountAmount",       "Discount Amount",        typeof(decimal)),
                    new GenericSpreadsheetTargetField("Order.InsuranceAmount",      "Insurance Amount",       typeof(decimal)),
                    new GenericSpreadsheetTargetField("Order.OtherAmount",          "Other Amount",           typeof(decimal)),
                    new GenericSpreadsheetTargetField("Order.TaxAmount",            "Tax Amount",             typeof(decimal)),
                    new GenericSpreadsheetTargetField("Order.PaymentMethod",        "Payment Method",         typeof(string)),
                    new GenericSpreadsheetTargetField("Order.PaymentReference",     "Payment Reference",      typeof(string)),
                    new GenericSpreadsheetTargetField("Order.CreditCardType",       "Credit Card Type",       typeof(string)),
                    new GenericSpreadsheetTargetField("Order.CreditCardNumber",     "Credit Card Number",     typeof(string)),
                    new GenericSpreadsheetTargetField("Order.CreditCardExpiration", "Credit Card Expiration", typeof(string)),
                    new GenericSpreadsheetTargetField("Order.CreditCardName",       "Credit Card Name",       typeof(string)),
                });
        }

        /// <summary>
        /// Create the field group for 'Address'
        /// </summary>
        private GenericSpreadsheetTargetFieldGroup CreateAddressGroup()
        {
            return new GenericSpreadsheetTargetFieldGroup("Address",
                new GenericSpreadsheetTargetField[]
                {
                    new GenericSpreadsheetTargetField("Address.BillFirstName",     "Bill First Name", typeof(string)),
                    new GenericSpreadsheetTargetField("Address.BillMiddleName",    "Bill Middle Name", typeof(string)),
                    new GenericSpreadsheetTargetField("Address.BillLastName",      "Bill Last Name", typeof(string)),
                    new GenericSpreadsheetTargetField("Address.BillFullName",      "Bill Full Name", typeof(string)),
                    new GenericSpreadsheetTargetField("Address.BillCompany",       "Bill Company", typeof(string)),
                    new GenericSpreadsheetTargetField("Address.BillLine1",         "Bill Address 1", typeof(string)),
                    new GenericSpreadsheetTargetField("Address.BillLine2",         "Bill Address 2", typeof(string)),
                    new GenericSpreadsheetTargetField("Address.BillLine3",         "Bill Address 3", typeof(string)),
                    new GenericSpreadsheetTargetField("Address.BillCity",          "Bill City",      typeof(string)),
                    new GenericSpreadsheetTargetField("Address.BillStateProv",     "Bill State\\Province",      typeof(string)),
                    new GenericSpreadsheetTargetField("Address.BillPostal",        "Bill Postal Code",      typeof(string)),
                    new GenericSpreadsheetTargetField("Address.BillCountry",       "Bill Country",      typeof(string)),
                    new GenericSpreadsheetTargetField("Address.BillEmail",         "Bill Email",      typeof(string)),
                    new GenericSpreadsheetTargetField("Address.BillPhone",         "Bill Phone",      typeof(string)),
                    new GenericSpreadsheetTargetField("Address.BillFax",           "Bill Fax",      typeof(string)),
                    new GenericSpreadsheetTargetField("Address.BillWebsite",       "Bill Website",      typeof(string)),

                    new GenericSpreadsheetTargetField("Address.ShipFirstName",     "Ship First Name", typeof(string)),
                    new GenericSpreadsheetTargetField("Address.ShipMiddleName",    "Ship Middle Name", typeof(string)),
                    new GenericSpreadsheetTargetField("Address.ShipLastName",      "Ship Last Name", typeof(string)),
                    new GenericSpreadsheetTargetField("Address.ShipFullName",      "Ship Full Name", typeof(string)),
                    new GenericSpreadsheetTargetField("Address.ShipCompany",       "Ship Company", typeof(string)),
                    new GenericSpreadsheetTargetField("Address.ShipLine1",         "Ship Address 1", typeof(string)),
                    new GenericSpreadsheetTargetField("Address.ShipLine2",         "Ship Address 2", typeof(string)),
                    new GenericSpreadsheetTargetField("Address.ShipLine3",         "Ship Address 3", typeof(string)),
                    new GenericSpreadsheetTargetField("Address.ShipCity",          "Ship City",      typeof(string)),
                    new GenericSpreadsheetTargetField("Address.ShipStateProv",     "Ship State\\Province",      typeof(string)),
                    new GenericSpreadsheetTargetField("Address.ShipPostal",        "Ship Postal Code",      typeof(string)),
                    new GenericSpreadsheetTargetField("Address.ShipCountry",       "Ship Country",      typeof(string)),
                    new GenericSpreadsheetTargetField("Address.ShipEmail",         "Ship Email",      typeof(string)),
                    new GenericSpreadsheetTargetField("Address.ShipPhone",         "Ship Phone",      typeof(string)),
                    new GenericSpreadsheetTargetField("Address.ShipFax",           "Ship Fax",      typeof(string)),
                    new GenericSpreadsheetTargetField("Address.ShipWebsite",       "Ship Website",      typeof(string)),
                });
        }

        /// <summary>
        /// Create the field group for 'Items'
        /// </summary>
        private GenericSpreadsheetTargetFieldGroup CreateItemsGroup()
        {
            return new GenericSpreadsheetOrderItemsSchemaGroup("Items",
                CreateItemsGroupFields());
        }

        /// <summary>
        /// Create all the fields for the items group
        /// </summary>
        protected virtual List<GenericSpreadsheetTargetField> CreateItemsGroupFields()
        {
            return new List<GenericSpreadsheetTargetField>
            {
                new GenericSpreadsheetTargetField("Item.Name",                  "Name", typeof(string)),
                new GenericSpreadsheetTargetField("Item.Code",                  "Code", typeof(string)),
                new GenericSpreadsheetTargetField("Item.SKU",                   "SKU",  typeof(string)),
                new GenericSpreadsheetTargetField("Item.Quantity",              "Quantity", typeof(float)),
                new GenericSpreadsheetTargetField("Item.UnitPrice",             "Unit Price", typeof(decimal)),
                new GenericSpreadsheetTargetField("Item.TotalPrice",            "Total Price", typeof(decimal)),
                new GenericSpreadsheetTargetField("Item.UnitWeight",            "Unit Weight", typeof(float)),
                new GenericSpreadsheetTargetField("Item.TotalWeight",           "Total Weight", typeof(float)),
                new GenericSpreadsheetTargetField("Item.Status",                "Status", typeof(string)),
                new GenericSpreadsheetTargetField("Item.Description",           "Description", typeof(string)),
                new GenericSpreadsheetTargetField("Item.Location",              "Location", typeof(string)),
                new GenericSpreadsheetTargetField("Item.UnitCost",              "Unit Cost", typeof(decimal)),
                new GenericSpreadsheetTargetField("Item.TotalCost",             "Total Cost", typeof(decimal)),
                new GenericSpreadsheetTargetField("Item.Image",                 "Image URL", typeof(string)),
                new GenericSpreadsheetTargetField("Item.Thumbnail",             "Thumbnail URL", typeof(string)),
                new GenericSpreadsheetTargetField("Item.UPC",                   "UPC", typeof(string)),
                new GenericSpreadsheetTargetField("Item.ISBN",                  "ISBN", typeof(string)),
                new GenericSpreadsheetTargetField("Item.Attribute.Name",        "Attribute Name", typeof(string))
            };
        }

    }
}
