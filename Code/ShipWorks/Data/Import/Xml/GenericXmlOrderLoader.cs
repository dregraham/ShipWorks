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

namespace ShipWorks.Data.Import.Xml
{
    /// <summary>
    /// Public core utlity class for downloading generic xml files
    /// </summary>
    public static class GenericXmlOrderLoader
    {
        /// <summary>
        /// Load the data from the given XPath, positioned on an ShipWorks Schema 'Order' element, into the given OrderEntity.
        /// </summary>
        public static void LoadOrder(OrderEntity order, IOrderElementFactory factory, IGenericXmlOrderLoadObserver observer, XPathNavigator xpath)
        {
            order.OrderDate = DateTime.Parse(XPathUtility.Evaluate(xpath, "OrderDate", ""));

            // If Parse can tell what timezone it's in, it automatically converts it to local.  We need UTC.
            if (order.OrderDate.Kind == DateTimeKind.Local)
            {
                order.OrderDate = order.OrderDate.ToUniversalTime();
            }

            // shipping
            order.RequestedShipping = XPathUtility.Evaluate(xpath, "ShippingMethod", "");

            // Load Address info
            LoadAddressInfo(order, xpath);
            
            // Notes
            LoadNotes(factory, order, xpath);

            // only do the remainder for new orders
            if (order.IsNew)
            {
                // Items
                XPathNodeIterator itemNodes = xpath.Select("Items/Item");
                while (itemNodes.MoveNext())
                {
                    LoadItem(factory, observer, order, itemNodes.Current);
                }

                // Charges
                LoadOrderCharges(factory, order, xpath);

                // Credit Card stuff
                string cctype = XPathUtility.Evaluate(xpath, "Payment/CreditCard/Type", "");
                if (cctype.Length > 0)
                {
                    LoadPaymentDetail(factory, order, "Card Owner", XPathUtility.Evaluate(xpath, "Payment/CreditCard/Owner", ""));
                    LoadPaymentDetail(factory, order, "Card Expires", XPathUtility.Evaluate(xpath, "Payment/CreditCard/Expires", ""));
                    LoadPaymentDetail(factory, order, "Card Number", XPathUtility.Evaluate(xpath, "Payment/CreditCard/Number", ""));
                    LoadPaymentDetail(factory, order, "Card Type", XPathUtility.Evaluate(xpath, "Payment/CreditCard/Type", ""));
                }

                // Payment Details
                LoadPaymentDetail(factory, order, "Payment Type", XPathUtility.Evaluate(xpath, "Payment/Method", ""));

                // It's also possible to use name\value pairs in Payment/Detail nodes
                foreach (XPathNavigator detailXPath in xpath.Select("Payment/Detail").Cast<XPathNavigator>().Reverse())
                {
                    LoadPaymentDetail(factory, order, XPathUtility.Evaluate(detailXPath, "@name", ""), XPathUtility.Evaluate(detailXPath, "@value", ""));
                }

                // Update the total
                order.OrderTotal = OrderUtility.CalculateTotal(order);
            }

            // notify listeners that the order was loaded
            if (observer != null)
            {
                observer.OnOrderLoadComplete(order, xpath);
            }
        }

        /// <summary>
        /// Loads notes from the module xml
        /// </summary>
        private static void LoadNotes(IOrderElementFactory factory, OrderEntity order, XPathNavigator xpath)
        {
            XPathNodeIterator notes = xpath.Select("Notes/Note");
            while (notes.MoveNext())
            {
                XPathNavigator noteXPath = notes.Current;

                string noteText = XPathUtility.Evaluate(noteXPath, "text()", "");
                bool publicNote = XPathUtility.EvaluateXsdBoolean(noteXPath, "@public", false);
                string noteDateText = XPathUtility.Evaluate(noteXPath, "@date", "");

                DateTime noteDate = order.OrderDate;
                if (noteDateText.Length > 0)
                {
                    noteDate = DateTime.Parse(noteDateText);
                }

                factory.CreateNote(order, noteText, noteDate, publicNote ? NoteVisibility.Public : NoteVisibility.Internal);
            }
        }

        /// <summary>
        /// Loads payment detail information into the given order
        /// </summary>
        private static void LoadPaymentDetail(IOrderElementFactory factory, OrderEntity order, string label, string value)
        {
            if (label == null || label.Trim().Length == 0)
            {
                return;
            }

            if (value == null || value.Trim().Length == 0)
            {
                return;
            }

            OrderPaymentDetailEntity detail = factory.CreatePaymentDetail(order);

            detail.Label = label;
            detail.Value = value;
        }

        /// <summary>
        /// Load all the charges for the order
        /// </summary>
        private static void LoadOrderCharges(IOrderElementFactory factory, OrderEntity order, XPathNavigator xpath)
        {
            XPathNodeIterator totals = xpath.Select("Totals/Total");
            while (totals.MoveNext())
            {
                XPathNavigator xpathTotal = totals.Current.Clone();

                // filter out types we don't want in ShipWorks
                string type = XPathUtility.Evaluate(xpathTotal, "@class", "");
                string impact = XPathUtility.Evaluate(xpathTotal, "@impact", "add");
                decimal value = XPathUtility.Evaluate(xpathTotal, "text()", 0.0M);

                if (impact == "subtract")
                {
                    value = value * -1;
                }

                // only add the charge if it has an impact on the total
                if (impact != "none")
                {
                    LoadCharge(factory, order, type, XPathUtility.Evaluate(xpathTotal, "@name", ""), value);
                }
            }
        }

        /// <summary>
        /// Loads an order charge for the given values for the order
        /// </summary>
        private static void LoadCharge(IOrderElementFactory factory, OrderEntity order, string type, string name, decimal amount)
        {
            OrderChargeEntity charge = factory.CreateCharge(order);

            if (name.Length == 0)
            {
                name = type;
            }

            charge.Type = type.ToUpperInvariant();
            charge.Description = name;
            charge.Amount = amount;
        }

        /// <summary>
        /// Populates the fields of hte OrderItemEntity from the xpathnavigator
        /// </summary>
        private static void LoadItemFields(OrderItemEntity item, XPathNavigator xpath)
        {
            item.Name = XPathUtility.Evaluate(xpath, "Name", "");
            item.Code = XPathUtility.Evaluate(xpath, "Code", "");
            item.SKU = XPathUtility.Evaluate(xpath, "SKU", item.Code);
            item.UPC = XPathUtility.Evaluate(xpath, "UPC", "");
            item.ISBN = XPathUtility.Evaluate(xpath, "ISBN", "");
            item.Description = XPathUtility.Evaluate(xpath, "Description", "");
            item.LocalStatus = XPathUtility.Evaluate(xpath, "Status", "");
            item.Quantity = XPathUtility.Evaluate(xpath, "Quantity", (double) 0.0);
            item.UnitPrice = XPathUtility.Evaluate(xpath, "UnitPrice", 0.0M);
            item.UnitCost = XPathUtility.Evaluate(xpath, "UnitCost", 0.0M);
            item.Weight = XPathUtility.Evaluate(xpath, "Weight", (double) 0.0);
            item.Image = XPathUtility.Evaluate(xpath, "Image", "");
            item.Thumbnail = XPathUtility.Evaluate(xpath, "ThumbnailImage", item.Image);
            item.Location = XPathUtility.Evaluate(xpath, "Location", "");
        }

        /// <summary>
        /// Load the item information into the given order
        /// </summary>
        private static void LoadItem(IOrderElementFactory factory, IGenericXmlOrderLoadObserver observer, OrderEntity order, XPathNavigator xpath)
        {
            OrderItemEntity item = factory.CreateItem(order);

            // load the data
            LoadItemFields(item, xpath);

            // Now load all the item options
            XPathNodeIterator options = xpath.Select("Attributes/Attribute");
            while (options.MoveNext())
            {
                LoadOption(factory, observer, item, options.Current);
            }

            // notify listeners that the item has been loaded
            if (observer != null)
            {
                observer.OnItemLoadComplete(item, xpath);
            }
        }

        /// <summary>
        /// Populates the OrderItemAttributeEntity fields
        /// </summary>
        private static void LoadOptionFields(OrderItemAttributeEntity option, XPathNavigator xpath)
        {
            option.Name = XPathUtility.Evaluate(xpath, "Name", "");
            option.Description = XPathUtility.Evaluate(xpath, "Value", "");
            option.UnitPrice = XPathUtility.Evaluate(xpath, "Price", 0.0M);
        }

        /// <summary>
        /// Load all the options for the given item
        /// </summary>
        private static void LoadOption(IOrderElementFactory factory, IGenericXmlOrderLoadObserver observer, OrderItemEntity item, XPathNavigator xpath)
        {
            OrderItemAttributeEntity option = factory.CreateItemAttribute(item);

            LoadOptionFields(option, xpath);

            // notify listeners that the option has been loaded
            if (observer != null)
            {
                observer.OnItemAttributeLoadComplete(option, xpath);
            }
        }

        /// <summary>
        /// Loads Shipping and Billing address information
        /// </summary>
        private static void LoadAddressInfo(OrderEntity order, XPathNavigator xpath)
        {
            LoadAddress(order, xpath, "Ship", "Shipping");
            LoadAddress(order, xpath, "Bill", "Billing");
        }

        /// <summary>
        /// Loads the Billing or Shipping address detail into the order entity, depending on the 
        /// prefix specified by the caller.
        /// </summary>
        private static void LoadAddress(OrderEntity order, XPathNavigator xpath, string dbPrefix, string xmlPrefix)
        {
            // FullName must be sent, or FirstName/MiddleName/LastName
            string fullName = XPathUtility.Evaluate(xpath, xmlPrefix + "Address/FullName", "").Trim();
            if (fullName.Length == 0)
            {
                order.SetNewFieldValue(dbPrefix + "NameParseStatus", (int)PersonNameParseStatus.Simple);
                order.SetNewFieldValue(dbPrefix + "FirstName", XPathUtility.Evaluate(xpath, xmlPrefix + "Address/FirstName", "").Trim());
                order.SetNewFieldValue(dbPrefix + "MiddleName", XPathUtility.Evaluate(xpath, xmlPrefix + "Address/MiddleName", "").Trim());
                order.SetNewFieldValue(dbPrefix + "LastName", XPathUtility.Evaluate(xpath, xmlPrefix + "Address/LastName", "").Trim());
            }
            else
            {
                // parse the name for its parts
                PersonName personName = PersonName.Parse(fullName);

                order.SetNewFieldValue(dbPrefix + "NameParseStatus", (int)personName.ParseStatus);
                order.SetNewFieldValue(dbPrefix + "UnparsedName", personName.UnparsedName.Trim());
                order.SetNewFieldValue(dbPrefix + "FirstName", personName.First.Trim());
                order.SetNewFieldValue(dbPrefix + "MiddleName", personName.Middle.Trim());
                order.SetNewFieldValue(dbPrefix + "LastName", personName.Last.Trim());
            }

            order.SetNewFieldValue(dbPrefix + "Company", XPathUtility.Evaluate(xpath, xmlPrefix + "Address/Company", "").Trim());
            order.SetNewFieldValue(dbPrefix + "Street1", XPathUtility.Evaluate(xpath, xmlPrefix + "Address/Street1", "").Trim());
            order.SetNewFieldValue(dbPrefix + "Street2", XPathUtility.Evaluate(xpath, xmlPrefix + "Address/Street2", "").Trim());
            order.SetNewFieldValue(dbPrefix + "Street3", XPathUtility.Evaluate(xpath, xmlPrefix + "Address/Street3", "").Trim());

            order.SetNewFieldValue(dbPrefix + "City", XPathUtility.Evaluate(xpath, xmlPrefix + "Address/City", "").Trim());
            order.SetNewFieldValue(dbPrefix + "StateProvCode", Geography.GetStateProvCode(XPathUtility.Evaluate(xpath, xmlPrefix + "Address/State", "").Trim()));
            order.SetNewFieldValue(dbPrefix + "PostalCode", XPathUtility.Evaluate(xpath, xmlPrefix + "Address/PostalCode", "").Trim());
            order.SetNewFieldValue(dbPrefix + "CountryCode", Geography.GetCountryCode(XPathUtility.Evaluate(xpath, xmlPrefix + "Address/Country", "").Trim()));

            order.SetNewFieldValue(dbPrefix + "Residential", XPathUtility.EvaluateXsdBoolean(xpath, xmlPrefix + "Address/Residential", true));
            order.SetNewFieldValue(dbPrefix + "Phone", XPathUtility.Evaluate(xpath, xmlPrefix + "Address/Phone", "").Trim());
            order.SetNewFieldValue(dbPrefix + "Fax", XPathUtility.Evaluate(xpath, xmlPrefix + "Address/Fax", "").Trim());
            order.SetNewFieldValue(dbPrefix + "Email", XPathUtility.Evaluate(xpath, xmlPrefix + "Address/Email", "").Trim());
            order.SetNewFieldValue(dbPrefix + "Website", XPathUtility.Evaluate(xpath, xmlPrefix + "Address/Website", "").Trim());
        }
    }
}
