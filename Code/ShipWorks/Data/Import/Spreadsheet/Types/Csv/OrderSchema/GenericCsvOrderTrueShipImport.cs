using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using ShipWorks.Data.Import.Spreadsheet;
using ShipWorks.Data.Import.Spreadsheet.OrderSchema;

namespace ShipWorks.Data.Import.Spreadsheet.Types.Csv.OrderSchema
{
    /// <summary>
    /// Utility class to import from the TrueShip map format
    /// </summary>
    public static class GenericCsvOrderTrueShipImport
    {
        static Dictionary<string, string> simpleLinkMappings = new Dictionary<string, string>
            {
                { "BILLED_SHIP_COST",         "Order.ShippingAmount" },
                { "BILLED_TAX",               "Order.TaxAmount" },
                { "BILL_ADDRESS_ADDRESS",     "Address.BillLine1" },
                { "BILL_ADDRESS_APTNO",       "Address.BillLine2" },
                { "BILL_ADDRESS_CITY",        "Address.BillCity" },
                { "BILL_ADDRESS_COMPANY",     "Address.BillCompany" },
                { "BILL_ADDRESS_COUNTRY",     "Address.BillCountry" },
                { "BILL_ADDRESS_EMAIL",       "Address.BillEmail" },
                { "BILL_ADDRESS_FIRST_NAME",  "Address.BillFirstName" },
                { "BILL_ADDRESS_LAST_NAME",   "Address.BillLastName" },
                { "BILL_ADDRESS_PHONE",       "Address.BillPhone" },
                { "BILL_ADDRESS_STATE",       "Address.BillStateProv" },
                { "BILL_ADDRESS_ZIP",         "Address.BillPostal" },
                { "CUSTOMER_NUMBER",          "Order.CustomerNumber" },
                { "MESSAGE",                  "Order.NotePublic" },
                { "NUMERIC_ID",               "Order.Number" },
                { "PAYMENT",                  "Order.PaymentMethod" },
                { "PLACEMENT_TIME",           "Order.DateTime" },
                { "SHIP_TYPE",                "Order.RequestedShipping" },
                { "TO_ADDRESS_ADDRESS",       "Address.ShipLine1" },
                { "TO_ADDRESS_APTNO",         "Address.ShipLine2" },
                { "TO_ADDRESS_CITY",          "Address.ShipCity" },
                { "TO_ADDRESS_COMPANY",       "Address.ShipCompany" },
                { "TO_ADDRESS_COUNTRY",       "Address.ShipCountry" },
                { "TO_ADDRESS_EMAIL",         "Address.ShipEmail" },
                { "TO_ADDRESS_FIRST_NAME",    "Address.ShipFirstName" },
                { "TO_ADDRESS_LAST_NAME",     "Address.ShipLastName" },
                { "TO_ADDRESS_PHONE",         "Address.ShipPhone" },
                { "TO_ADDRESS_STATE",         "Address.ShipStateProv" },
                { "TO_ADDRESS_ZIP",           "Address.ShipPostal" },
            };

        static Dictionary<string, string> repeatLinkMappings = new Dictionary<string, string>
            {
                { "ITEM_DESCRIPTION",  "Item.Description" },
                { "ITEM_PARTNO",       "Item.Name" },
                { "ITEM_PICKLOCATION", "Item.Location" },
                { "ITEM_PRICE",        "Item.UnitPrice" },
                { "ITEM_QTY",          "Item.Quantity" },
                { "ITEM_WEIGHT",       "Item.UnitWeight" }
            };

        /// <summary>
        /// The file filter for displaying in an OpenFileDialog
        /// </summary>
        public static string FileFilter
        {
            get { return "ReadyShipper Map (*.rsmap)|*.rsmap"; }
        }

        /// <summary>
        /// Deserialize a saved TrueShip ReadyShipper map
        /// </summary>
        public static GenericSpreadsheetMap DeserializeTrueShipMap(string mapContent)
        {
            JavaScriptSerializer json = new JavaScriptSerializer();
            var result = json.DeserializeObject(mapContent) as Dictionary<string, object>;

            if (result == null)
            {
                throw new GenericSpreadsheetException("ShipWorks does not support reading the selected map format.");
            }

            try
            {
                string name = (string) result["name"];
                object[] headers = (object[]) result["headers"];
                Dictionary<string, object> auxiliary = (Dictionary<string, object>) result["auxiliary"];
                Dictionary<string, object> simpleLinks = (Dictionary<string, object>) result["simple_links"];
                Dictionary<string, object> repeatLinks = (Dictionary<string, object>) result["repeat_links"];

                GenericCsvMap map = new GenericCsvMap(new GenericSpreadsheetOrderSchema());
                map.SourceSchema = new GenericCsvSourceSchema(headers.Select(h => new GenericSpreadsheetSourceColumn((string) h)), ',', '"', '"', "");
                map.Name = name;
                
                // I'm not sure we really want to import this - I think our "Automatic" may be a better choice
                // map.DateFormat = (string) auxiliary["TIME_FORMAT"];

                // Grab our specific settings
                GenericSpreadsheetOrderMapSettings settings = (GenericSpreadsheetOrderMapSettings) map.TargetSettings;

                // Load the simple mappings
                LoadSimpleLinks(simpleLinks, map);

                // We need to know how many items are supported by the target
                int maxItemCount = DetermineMaxItems(repeatLinks);
                
                // If just one, assume it repeats
                if (maxItemCount <= 1)
                {
                    settings.MultiItemStrategy = GenericSpreadsheetOrderMultipleItemStrategy.MultipleLine;

                    var keyColumn = map.Mappings["Order.Number"];
                    if (keyColumn != null)
                    {
                        settings.MultiLineKeyColumns.Add(keyColumn.SourceColumnName);
                    }
                }
                else
                {
                    settings.MultiItemStrategy = GenericSpreadsheetOrderMultipleItemStrategy.SingleLine;
                    settings.SingleLineCount = maxItemCount;
                }

                // Load the links
                LoadRepeatLinks(repeatLinks, map);

                return map;
            }
            catch (InvalidCastException ex)
            {
                throw new GenericSpreadsheetException("ShipWorks found something unexpected while reading the selected map.", ex);
            }
            catch (KeyNotFoundException ex)
            {
                throw new GenericSpreadsheetException("ShipWorks found something unexpected while reading the selected map.", ex);
            }
        }

        /// <summary>
        /// Load the simple link mappings into the map
        /// </summary>
        private static void LoadSimpleLinks(Dictionary<string, object> simpleLinks, GenericSpreadsheetMap map)
        {
            foreach (var link in simpleLinks)
            {
                string sourceColumn = link.Value as string;

                if (!string.IsNullOrWhiteSpace(sourceColumn))
                {
                    string swIdentifier;
                    if (simpleLinkMappings.TryGetValue(link.Key, out swIdentifier))
                    {
                        map.Mappings.SetMapping(new GenericSpreadsheetFieldMapping(map.TargetSchema.GetField(swIdentifier, map.TargetSettings), sourceColumn));
                    }
                }
            }
        }

        /// <summary>
        /// Load the repeat (item) links into the map
        /// </summary>
        private static void LoadRepeatLinks(Dictionary<string, object> repeatLinks, GenericSpreadsheetMap map)
        {
            foreach (var link in repeatLinks)
            {
                string baseIdentifier;
                if (repeatLinkMappings.TryGetValue(link.Key, out baseIdentifier))
                {
                    object[] fieldValues = (object[]) link.Value;

                    for (int i = 0; i < fieldValues.Length; i++)
                    {
                        string sourceColumn = fieldValues[i] as string;

                        if (!string.IsNullOrWhiteSpace(sourceColumn))
                        {
                            map.Mappings.SetMapping(new GenericSpreadsheetFieldMapping(map.TargetSchema.GetField(string.Format("{0}.{1}", baseIdentifier, i + 1), map.TargetSettings), sourceColumn));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Determine the maximum number of items mapped in the target
        /// </summary>
        private static int DetermineMaxItems(Dictionary<string, object> repeatLinks)
        {
            int maxValidIndex = 1;

            foreach (object[] fieldValues in repeatLinks.Values)
            {
                for (int i = 0; i < fieldValues.Length; i++)
                {
                    if (!string.IsNullOrWhiteSpace((string) fieldValues[i]))
                    {
                        maxValidIndex = Math.Max(maxValidIndex, i + 1);
                    }
                }
            }

            return maxValidIndex;
        }
    }
}
