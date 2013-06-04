using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Import.Spreadsheet.OrderSchema;
using ShipWorks.Data.Import.Spreadsheet;

namespace ShipWorks.Stores.Platforms.BuyDotCom
{
    /// <summary>
    /// Custom order import schema for Buy.com
    /// </summary>
    public class BuyDotComOrderImportSchema : GenericSpreadsheetOrderSchema
    {
        /// <summary>
        /// The schema specific type code
        /// </summary>
        public override string SchemaType
        {
            get { return "BuyDotComOrderImport"; }
        }

        /// <summary>
        /// The version of the schema
        /// </summary>
        public override Version SchemaVersion
        {
            get { return new Version(1, 0, 0, 0); }
        }

        /// <summary>
        /// Create Buy.com specific order item fields within the existing order items group
        /// </summary>
        protected override List<GenericSpreadsheetTargetField> CreateItemsGroupFields()
        {
            var fields = base.CreateItemsGroupFields();

            fields.AddRange(
                    new GenericSpreadsheetTargetField[]
                    {
                        new GenericSpreadsheetTargetField("BuyDotComItem.ReceiptItemID", "Buy.com Receipt Item ID",  typeof(long)),
                        new GenericSpreadsheetTargetField("BuyDotComItem.ListingID",     "Buy.com Listing ID",       typeof(string)),
                        new GenericSpreadsheetTargetField("BuyDotComItem.Shipping",      "Buy.com Shipping",         typeof(decimal)),
                        new GenericSpreadsheetTargetField("BuyDotComItem.Tax",           "Buy.com Tax",              typeof(decimal)),
                        new GenericSpreadsheetTargetField("BuyDotComItem.Commission",    "Buy.com Commission",       typeof(decimal)),
                        new GenericSpreadsheetTargetField("BuyDotComItem.ItemFee",       "Buy.com Item Fee",         typeof(decimal)),
                    }
                );

            return fields;
        }
    }
}
