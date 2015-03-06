using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using System.Xml.XPath;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Content;
using Interapptive.Shared.Business;
using ShipWorks.Stores.Communication;
using Interapptive.Shared.IO.Text.Csv;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Import.Spreadsheet;
using ShipWorks.Data.Import.Spreadsheet.Types.Csv;

namespace ShipWorks.Data.Import.Spreadsheet.OrderSchema
{
    /// <summary>
    /// Public core utlity class for downloading generic CSV files
    /// </summary>
    public class GenericSpreadsheetOrderLoader
    {
        // The order data will be loaded into
        OrderEntity order;

        // The source CSV content
        GenericSpreadsheetReader csv;

        // The factory used to create new order element instances as needed
        IOrderElementFactory factory;

        /// <summary>
        /// Load the data from the given CsvReader, positioned on an order row, into the given OrderEntity.
        /// </summary>
        public void Load(OrderEntity order, GenericSpreadsheetReader csv, IOrderElementFactory factory)
        {
            if (order == null)
            {
                throw new ArgumentNullException("order");
            }

            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }

            if (csv == null)
            {
                throw new ArgumentNullException("csv");
            }

            this.order = order;
            this.csv = csv;
            this.factory = factory;

            DateTime? orderDate = csv.ReadField("Order.DateTime", (DateTime?) null, null, csv.Map.DateSettings.DateTimeFormat, false);

            // If the full date\time wasn't mapped, let's see if the individual parts were
            if (orderDate == null)
            {
                DateTime? datePart = csv.ReadField("Order.Date", (DateTime?) null, null, csv.Map.DateSettings.DateFormat, false);
                DateTime? timePart = csv.ReadField("Order.Time", (DateTime?) null, null, csv.Map.DateSettings.TimeFormat, false);

                if (datePart == null)
                {
                    orderDate = DateTime.UtcNow;
                }
                else
                {
                    orderDate = datePart;

                    if (timePart != null)
                    {
                        DateTime combined = datePart.Value;
                        orderDate = combined.Date.Add(timePart.Value.TimeOfDay);
                    }
                }
            }

            order.OrderDate = orderDate.Value;

            // If Parse can tell what timezone it's in, it automatically converts it to local.  We need UTC.
            if (order.OrderDate.Kind == DateTimeKind.Local)
            {
                order.OrderDate = order.OrderDate.ToUniversalTime();
            }

            // If it's unspecified, we need go based on the settings
            if (order.OrderDate.Kind == DateTimeKind.Unspecified)
            {
                if (csv.Map.DateSettings.TimeZoneAssumption == GenericSpreadsheetTimeZoneAssumption.Local)
                {
                    order.OrderDate = order.OrderDate.ToUniversalTime();
                }
                else
                {
                    order.OrderDate = new DateTime(order.OrderDate.Ticks, DateTimeKind.Utc);
                }
            }

            // Only set customer ID if non blank
            string customerID = csv.ReadField("Order.CustomerNumber", "");
            if (!string.IsNullOrWhiteSpace(customerID))
            {
                order.OnlineCustomerID = customerID;
            }

            // Stuff
            order.LocalStatus = csv.ReadField("Order.LocalStatus", order.LocalStatus ?? "");
            order.OnlineStatus = csv.ReadField("Order.OnlineStatus", order.OnlineStatus ?? "");

            order.RequestedShipping = csv.ReadField("Order.RequestedShipping", order.RequestedShipping ?? "");

            // Load Address info
            LoadAddressInfo();

            // Let derived classes have a shot at loading extra order data
            LoadExtraOrderData(order, csv, factory);

            // only do the remainder for new orders
            if (order.IsNew)
            {
                // Notes
                LoadNotes();

                // Charges
                LoadOrderCharges();

                // Payment details
                LoadPaymentDetails();

                // Items have to come last, since we may be advancing the reader
                LoadItems();
            }

            // Give derived classes a chance to do any additional loading or processing of the fully loaded order
            OnLoadComplete(order, csv, factory);

            // As of right now this should never change unless it's a new order.  Items and Charges (the only things affecting the total), are only 
            // added for new orders, and never updated\edited for existing orders.  If that ever changes we'll need to make sure we have all 
            // items\charges loaded before making the call to calculate
            if (order.IsNew)
            {
                order.OrderTotal = OrderUtility.CalculateTotal(order);
            }
        }

        /// <summary>
        /// Allows derived classes a chance to do additional loading or processing of the fully loaded order
        /// </summary>
        protected virtual void OnLoadComplete(OrderEntity order, GenericSpreadsheetReader csv, IOrderElementFactory factory)
        {

        }

        /// <summary>
        /// Allows derived classes a shot at loading extra order data or manipulating already loaded data
        /// </summary>
        protected virtual void LoadExtraOrderData(OrderEntity order, GenericSpreadsheetReader csv, IOrderElementFactory factory)
        {

        }

        /// <summary>
        /// Load the items into the order
        /// </summary>
        private void LoadItems()
        {
            GenericSpreadsheetOrderMapSettings settings = (GenericSpreadsheetOrderMapSettings) csv.Map.TargetSettings;

            // All items are seperate columns on a single line
            if (settings.MultiItemStrategy == GenericSpreadsheetOrderMultipleItemStrategy.SingleLine)
            {
                for (int i = 1; i <= settings.SingleLineCount; i++)
                {
                    LoadLineItem(i);
                }
            }
            else
            {
                // Load the first item, on the same line as the order
                LoadLineItem(1);

                // Get the current order key
                string currentOrderKey = GetCurrentLineKey(settings);

                while (true)
                {
                    // See if we can move to the next record
                    if (csv.NextRecord())
                    {
                        // If it matches our order key settings, load the line
                        if (GetCurrentLineKey(settings) == currentOrderKey)
                        {
                            // Load the item from the current line
                            LoadLineItem(1);
                        }
                        else
                        {
                            // If it doesn't match the order key, back it up, b\c it will need picked up as an actual order
                            csv.PreviousRecord();
                            break;
                        }
                    }
                    else
                    {
                        // No more records, we're done
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Get the order 'key' value of the current line which determines when items repeat accross multiple order lines
        /// </summary>
        private string GetCurrentLineKey(GenericSpreadsheetOrderMapSettings settings)
        {
            if (settings.MultiLineKeyColumns.Count == 0)
            {
                throw new GenericSpreadsheetException("The column that uniquely identifies each order for repeating items has not been set.");
            }

            StringBuilder key = new StringBuilder();

            foreach (string column in settings.MultiLineKeyColumns)
            {
                key.AppendFormat("{0} -- ", csv.ReadColumnText(column));
            }

            return key.ToString();
        }

        /// <summary>
        /// Load the given line item from the CSV
        /// </summary>
        private void LoadLineItem(int itemIndex)
        {
            OrderItemEntity item = factory.CreateItem(order);
            item.Quantity = 1;

            // To know if we set any values
            OrderItemEntity clone = EntityUtility.CloneEntity(item);

            // Apply the suffis the reader should use for all field name requests.
            csv.FieldIdentifierSuffix = string.Format(".{0}", itemIndex);

            try
            {
                item.Name = csv.ReadField("Item.Name", "");
                item.Code = csv.ReadField("Item.Code", "");
                item.SKU = csv.ReadField("Item.SKU", "");
                item.Quantity = csv.ReadField("Item.Quantity", item.Quantity);
                item.LocalStatus = csv.ReadField("Item.Status", "");
                item.Description = csv.ReadField("Item.Description", "");
                item.Location = csv.ReadField("Item.Location", "");
                item.Image = csv.ReadField("Item.Image", "");
                item.Thumbnail = csv.ReadField("Item.Thumbnail", item.Image);

                // Determine unit vs. total price
                decimal? unitPrice = csv.ReadField("Item.UnitPrice", 0m, (decimal?) null);
                if (unitPrice == null)
                {
                    decimal? totalPrice = csv.ReadField("Item.TotalPrice", 0m, (decimal?) null);
                    unitPrice = (totalPrice.HasValue && item.Quantity != 0) ? (totalPrice.Value / (decimal) item.Quantity) : 0m;
                }

                // Determine unit vs. total cost
                decimal? unitCost = csv.ReadField("Item.UnitCost", 0m, (decimal?) null);
                if (unitCost == null)
                {
                    decimal? totalCost = csv.ReadField("Item.TotalCost", 0m, (decimal?) null);
                    unitCost = (totalCost.HasValue && item.Quantity != 0) ? (totalCost.Value / (decimal) item.Quantity) : 0m;
                }

                // Determine unit vs. total weight
                double? unitWeight = csv.ReadField("Item.UnitWeight", 0d, (double?) null);
                if (unitWeight == null)
                {
                    double? totalWeight = csv.ReadField("Item.TotalWeight", 0d, (double?) null);
                    unitWeight = (totalWeight.HasValue && item.Quantity != 0) ? (totalWeight.Value / (double) item.Quantity) : 0d;
                }

                // Set the numerics now that we have the units
                item.UnitPrice = unitPrice.Value;
                item.UnitCost = unitCost.Value;
                item.Weight = unitWeight.Value;

                item.UPC = csv.ReadField("Item.UPC", "");
                item.ISBN = csv.ReadField("Item.ISBN", "");

                // Allow derive classes to load\change their own specific item data
                LoadExtraOrderItemData(item, csv, factory);
            }
            finally
            {
                csv.FieldIdentifierSuffix = "";
            }

            bool anyDifferent = false;

            foreach (EntityField2 field in item.Fields)
            {
                if (!object.Equals(item.GetCurrentFieldValue(field.FieldIndex), clone.GetCurrentFieldValue(field.FieldIndex)))
                {
                    anyDifferent = true;
                }
            }

            if (!anyDifferent)
            {
                order.OrderItems.Remove(item);
            }
        }

        /// <summary>
        /// Allow derived classes a chance to load any extra item data they may need to load
        /// </summary>
        protected virtual void LoadExtraOrderItemData(OrderItemEntity item, GenericSpreadsheetReader csv, IOrderElementFactory factory)
        {

        }

        /// <summary>
        /// Loads notes from the module xml
        /// </summary>
        private void LoadNotes()
        {
            string publicNote = csv.ReadField("Order.NotePublic", "");
            string internalNote = csv.ReadField("Order.NoteInternal", "");

            if (!string.IsNullOrWhiteSpace(publicNote))
            {
                factory.CreateNote(order, publicNote, order.OrderDate, NoteVisibility.Public);
            }

            if (!string.IsNullOrWhiteSpace(internalNote))
            {
                factory.CreateNote(order, internalNote, order.OrderDate, NoteVisibility.Internal);
            }
        }

        /// <summary>
        /// Load the payment details for the order
        /// </summary>
        private void LoadPaymentDetails()
        {
            LoadPaymentDetail(csv.Map.TargetSchema.GetField("Order.CreditCardName",       csv.Map.TargetSettings));
            LoadPaymentDetail(csv.Map.TargetSchema.GetField("Order.CreditCardExpiration", csv.Map.TargetSettings));
            LoadPaymentDetail(csv.Map.TargetSchema.GetField("Order.CreditCardNumber",     csv.Map.TargetSettings));
            LoadPaymentDetail(csv.Map.TargetSchema.GetField("Order.CreditCardType",       csv.Map.TargetSettings));
            LoadPaymentDetail(csv.Map.TargetSchema.GetField("Order.PaymentReference",     csv.Map.TargetSettings));
            LoadPaymentDetail(csv.Map.TargetSchema.GetField("Order.PaymentMethod",        csv.Map.TargetSettings));
        }

        /// <summary>
        /// Loads payment detail information into the given order
        /// </summary>
        private void LoadPaymentDetail(GenericSpreadsheetTargetField targetField)
        {
            string value = csv.ReadField(targetField.Identifier, (string) null);

            if (value == null)
            {
                return;
            }

            // Only pay method get's to stay if blank
            if (value.Trim().Length == 0 && targetField.Identifier != "Order.PaymentMethod")
            {
                return;
            }

            OrderPaymentDetailEntity detail = factory.CreatePaymentDetail(order);

            detail.Label = targetField.DisplayName;
            detail.Value = value;
        }

        /// <summary>
        /// Load all the charges for the order
        /// </summary>
        private void LoadOrderCharges()
        {
            LoadCharge("Tax",       "Order.TaxAmount");
            LoadCharge("Other",     "Order.OtherAmount");
            LoadCharge("Insurance", "Order.InsuranceAmount");
            LoadCharge("Discount",  "Order.DiscountAmount");
            LoadCharge("Handling",  "Order.HandlingAmount");
            LoadCharge("Shipping",  "Order.ShippingAmount");
        }

        /// <summary>
        /// Loads an order charge for the given values for the order
        /// </summary>
        private void LoadCharge(string type, string identifier)
        {
            // Shipping or Tax we will show as charges of zero if mapped at all - even if blank values.  The others of mapped and blank, will not show up
            decimal? value = csv.ReadField(identifier, (identifier == "Order.ShippingAmount" || identifier == "Order.TaxAmount") ? 0m : (decimal?) null, null);

            if (value != null)
            {
                CreateCharge(order, type, value.Value);
            }
        }

        /// <summary>
        /// Create a charge for the order of the given type and value
        /// </summary>
        protected void CreateCharge(OrderEntity order, string type, decimal value)
        {
            OrderChargeEntity charge = factory.CreateCharge(order);

            charge.Type = type.ToUpperInvariant();
            charge.Description = type;
            charge.Amount = value;
        }

        /// <summary>
        /// Loads Shipping and Billing address information
        /// </summary>
        private void LoadAddressInfo()
        {
            LoadAddress("Ship");
            LoadAddress("Bill");
        }

        /// <summary>
        /// Loads the Billing or Shipping address detail into the order entity, depending on the 
        /// prefix specified by the caller.
        /// </summary>
        private void LoadAddress(string prefix)
        {
            // FullName must be sent, or FirstName/MiddleName/LastName
            string fullName = csv.ReadField("Address." + prefix + "FullName", "");
            if (fullName.Length == 0)
            {
                order.SetNewFieldValue(prefix + "NameParseStatus", (int)PersonNameParseStatus.Simple);
                order.SetNewFieldValue(prefix + "FirstName", csv.ReadField("Address." + prefix + "FirstName", ""));
                order.SetNewFieldValue(prefix + "MiddleName", csv.ReadField("Address." + prefix + "MiddleName", ""));
                order.SetNewFieldValue(prefix + "LastName", csv.ReadField("Address." + prefix + "LastName", ""));
            }
            else
            {
                // parse the name for its parts
                PersonName personName = PersonName.Parse(fullName);

                order.SetNewFieldValue(prefix + "NameParseStatus", (int)personName.ParseStatus);
                order.SetNewFieldValue(prefix + "UnparsedName", personName.UnparsedName);
                order.SetNewFieldValue(prefix + "FirstName", personName.First);
                order.SetNewFieldValue(prefix + "MiddleName", personName.Middle);
                order.SetNewFieldValue(prefix + "LastName", personName.Last);
            }

            order.SetNewFieldValue(prefix + "Company", csv.ReadField("Address." + prefix + "Company", ""));
            order.SetNewFieldValue(prefix + "Street1", csv.ReadField("Address." + prefix + "Line1", ""));
            order.SetNewFieldValue(prefix + "Street2", csv.ReadField("Address." + prefix + "Line2", ""));
            order.SetNewFieldValue(prefix + "Street3", csv.ReadField("Address." + prefix + "Line3", ""));

            order.SetNewFieldValue(prefix + "City", csv.ReadField("Address." + prefix + "City", ""));
            order.SetNewFieldValue(prefix + "StateProvCode", Geography.GetStateProvCode(csv.ReadField("Address." + prefix + "StateProv", "")));
            order.SetNewFieldValue(prefix + "PostalCode", csv.ReadField("Address." + prefix + "Postal", ""));
            order.SetNewFieldValue(prefix + "CountryCode", Geography.GetCountryCode(csv.ReadField("Address." + prefix + "Country", "US", "US")));

            order.SetNewFieldValue(prefix + "Phone", csv.ReadField("Address." + prefix + "Phone", ""));
            order.SetNewFieldValue(prefix + "Fax", csv.ReadField("Address." + prefix + "Fax", ""));
            order.SetNewFieldValue(prefix + "Email", csv.ReadField("Address." + prefix + "Email", ""));
            order.SetNewFieldValue(prefix + "Website", csv.ReadField("Address." + prefix + "Website", ""));
        }
    }
}
