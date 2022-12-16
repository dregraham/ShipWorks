using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using log4net;
using Newtonsoft.Json;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Platforms.Amazon.DTO;
using ShipWorks.Stores.Platforms.Etsy;
using ShipWorks.Stores.Platforms.ShipEngine.Apollo;

namespace ShipWorks.Stores.Platforms.ShipEngine
{
    public abstract class PlatformDownloader : StoreDownloader
    {
        protected readonly ILog log;

        protected PlatformDownloader(StoreEntity store, StoreType storeType) : base(store, storeType)
        {
            log = LogManager.GetLogger(this.GetType());
        }
        protected List<GiftNote> GetGiftNotes(OrderSourceApiSalesOrder salesOrder)
        {
            var itemNotes = new List<GiftNote>();
            if (salesOrder.Notes != null)
            {
                foreach (var note in salesOrder.Notes)
                {
                    if (note.Type == OrderSourceNoteType.GiftMessage)
                    {
                        itemNotes.Add(GiftNote.FromOrderSourceNote(note));
                    }
                }
            }

            return itemNotes;
        }

        protected static IEnumerable<CouponCode> GetCouponCodes(OrderSourceApiSalesOrder salesOrder)
        {
            return salesOrder.Payment.CouponCodes.Select((c) => JsonConvert.DeserializeObject<CouponCode>(c));
        }

        /// <summary>
        /// Load an order item
        /// </summary>
        protected void LoadOrderItem(OrderSourceSalesOrderItem orderItem, OrderEntity order, IEnumerable<GiftNote> giftNotes, IEnumerable<CouponCode> couponCodes)
        {
            var item = (AmazonOrderItemEntity) InstantiateOrderItem(order);

            // populate the basics
            item.Name = orderItem.Product.Name;
            item.Quantity = orderItem.Quantity;
            item.UnitPrice = orderItem.UnitPrice;
            item.SKU = orderItem.Product.Identifiers.Sku;
            item.Code = item.SKU;

            var fromWeightUnit = PlatformUnitConverter.FromPlatformWeight(orderItem.Product.Weight.Unit);
            var weight = WeightUtility.Convert(fromWeightUnit, WeightUnitOfMeasure.Pounds, (double) orderItem.Product.Weight.Value);
            item.Weight = weight;

            PopulateUrls(orderItem, item);

            if (orderItem.Product?.Dimensions != null)
            {
                var dims = orderItem.Product.Dimensions;
                var fromDimUnit = dims.Unit;

                item.Length = (decimal) PlatformUnitConverter.ConvertDimension(dims.Length, fromDimUnit);
                item.Width = (decimal) PlatformUnitConverter.ConvertDimension(dims.Width, fromDimUnit);
                item.Height = (decimal) PlatformUnitConverter.ConvertDimension(dims.Height, fromDimUnit);
            }

            // amazon-specific fields
            item.AmazonOrderItemCode = orderItem.LineItemId;
            item.ASIN = orderItem.Product.Identifiers?.Asin;

            //Load the gift messages
            foreach(var giftNote in giftNotes)
            {
                if(giftNote.Message.HasValue() || giftNote.Fee > 0)
                {
                    OrderItemAttributeEntity giftAttribute = InstantiateOrderItemAttribute(item);
                    giftAttribute.Name = "Gift Message";
                    giftAttribute.Description = giftNote.Message;
                    giftAttribute.UnitPrice = giftNote.Fee;
                    item.OrderItemAttributes.Add(giftAttribute);
                }

                if (giftNote.GiftWrapLevel.HasValue())
                {
                    OrderItemAttributeEntity levelAttribute = InstantiateOrderItemAttribute(item);
                    levelAttribute.Name = "Gift Wrap Level";
                    levelAttribute.Description = giftNote.GiftWrapLevel;
                    levelAttribute.UnitPrice = 0;
                    item.OrderItemAttributes.Add(levelAttribute);
                }
            }

            //Load any coupon codes
            if (couponCodes != null)
            {
                foreach (var couponSet in couponCodes)
                {
                    foreach (var code in couponSet.Codes)
                    {
                        OrderItemAttributeEntity couponAttribute = InstantiateOrderItemAttribute(item);
                        couponAttribute.Name = "Promotion ID";
                        couponAttribute.Description = code;
                        couponAttribute.UnitPrice = 0;
                        item.OrderItemAttributes.Add(couponAttribute);
                    }
                }
            }

            item.ConditionNote = orderItem.Product?.Details?.FirstOrDefault((d) => d.Name == "Condition")?.Value;

            AddOrderItemCharges(orderItem, order);
        }

        /// <summary>
        /// Populate image urls
        /// </summary>
        private static void PopulateUrls(OrderSourceSalesOrderItem orderItem, AmazonOrderItemEntity item)
        {
            var urls = orderItem.Product?.Urls;

            item.Thumbnail = urls?.ThumbnailUrl ?? string.Empty;
            item.Image = urls?.ImageUrl ?? string.Empty;
        }

        /// <summary>
        /// Add item charges to the order
        /// </summary>
        private void AddOrderItemCharges(OrderSourceSalesOrderItem orderItem, OrderEntity order)
        {
            foreach (var orderItemAdjustment in orderItem.Adjustments)
            {
                AddToCharge(order, orderItemAdjustment.Description, orderItemAdjustment.Description, orderItemAdjustment.Amount);
            }

            foreach (var orderItemShippingCharge in orderItem.ShippingCharges)
            {
                AddToCharge(order, "SHIPPING", orderItemShippingCharge.Description.Replace(" price", string.Empty), orderItemShippingCharge.Amount);
            }
        }

        /// <summary>
        /// Locates an order's charge (or creates it) and adds the value
        /// </summary>
        protected void AddToCharge(OrderEntity order, string chargeType, string name, decimal amount)
        {
            // Don't need to create 0-value charges
            if (amount == 0)
            {
                return;
            }

            var charge = order.OrderCharges.FirstOrDefault(c => string.Compare(c.Type, chargeType.ToUpper(), StringComparison.OrdinalIgnoreCase) == 0);
            if (charge == null)
            {
                // first one, create it
                charge = InstantiateOrderCharge(order);
                charge.Type = chargeType.ToUpper();
                charge.Description = name;
                charge.Amount = 0;
            }

            charge.Amount += amount;
        }

        protected static void LoadAddresses(OrderEntity order, OrderSourceApiSalesOrder salesOrder)
        {
            var shipTo = salesOrder.RequestedFulfillments.FirstOrDefault(x => x?.ShipTo != null)?.ShipTo;
            if (shipTo == null || !order.IsNew)
            {
                return;
            }

            var shipFullName = PersonName.Parse(shipTo.Name ?? string.Empty);
            order.ShipFirstName = shipFullName.First;
            order.ShipMiddleName = shipFullName.Middle;
            order.ShipLastName = shipFullName.LastWithSuffix;
            order.ShipNameParseStatus = (int) shipFullName.ParseStatus;
            order.ShipUnparsedName = shipFullName.UnparsedName;
            order.ShipCompany = shipTo.Company;
            order.ShipPhone = shipTo.Phone ?? string.Empty;

            var shipAddressLines = new List<string>
            {
                shipTo.AddressLine1 ?? string.Empty,
                shipTo.AddressLine2 ?? string.Empty,
                shipTo.AddressLine3 ?? string.Empty
            };
            SetStreetAddress(new PersonAdapter(order, "Ship"), shipAddressLines);

            order.ShipCity = shipTo.City ?? string.Empty;
            order.ShipPostalCode = shipTo.PostalCode ?? string.Empty;
            order.ShipCountryCode = Geography.GetCountryCode(shipTo.CountryCode ?? string.Empty);
            order.ShipStateProvCode = Geography.GetStateProvCode(shipTo.StateProvince ?? string.Empty, order.ShipCountryCode);

            // Platform only provides one email
            order.ShipEmail = salesOrder.Buyer.Email ?? string.Empty;
            order.BillEmail = order.ShipEmail;

            // Bill To
            var billToFullName = PersonName.Parse(salesOrder.BillTo.Name ?? salesOrder.Buyer.Name ?? string.Empty);
            order.BillFirstName = billToFullName.First;
            order.BillMiddleName = billToFullName.Middle;
            order.BillLastName = billToFullName.LastWithSuffix;
            order.BillNameParseStatus = (int) billToFullName.ParseStatus;
            order.BillUnparsedName = billToFullName.UnparsedName;
            order.BillCompany = salesOrder.BillTo.Company;
            order.BillPhone = salesOrder.BillTo.Phone ?? salesOrder.Buyer.Phone ?? string.Empty;

            var billAddressLines = new List<string>
            {
                salesOrder.BillTo.AddressLine1 ?? string.Empty,
                salesOrder.BillTo.AddressLine2 ?? string.Empty,
                salesOrder.BillTo.AddressLine3 ?? string.Empty
            };
            SetStreetAddress(new PersonAdapter(order, "Bill"), billAddressLines);

            order.BillCity = salesOrder.BillTo.City ?? string.Empty;
            order.BillPostalCode = salesOrder.BillTo.PostalCode ?? string.Empty;
            order.BillCountryCode = Geography.GetCountryCode(salesOrder.BillTo.CountryCode ?? string.Empty);
            order.BillStateProvCode = Geography.GetStateProvCode(salesOrder.BillTo.StateProvince ?? string.Empty, order.BillCountryCode);
        }

        /// <summary>
        /// Sets the XXXStreet1 - XXXStreet3 address lines
        /// </summary>
        private static void SetStreetAddress(PersonAdapter address, List<string> addressLines)
        {
            // first get rid of blanks
            addressLines.RemoveAll(s => s.Length == 0);

            var targetLine = 0;
            foreach (var addressLine in addressLines)
            {
                targetLine++;

                switch (targetLine)
                {
                    case 1:
                        address.Street1 = addressLine;
                        break;
                    case 2:
                        address.Street2 = addressLine;
                        break;
                    case 3:
                        address.Street3 = addressLine;
                        break;
                }
            }
        }

        /// <summary>
        /// Loads the order items of an amazon order
        /// </summary>
        protected void LoadOrderItems(OrderSourceRequestedFulfillment fulfillment, OrderEntity order, IEnumerable<GiftNote> giftNotes, IEnumerable<CouponCode> couponCodes)
        {
            foreach (var item in fulfillment.Items)
            {
                var filteredNotes = giftNotes.Where(i => i.OrderItemId == item.LineItemId);
                var filteredCouponCodes = couponCodes.Where((c) => c.OrderItemId == item.LineItemId);
                LoadOrderItem(item, order, filteredNotes, filteredCouponCodes);
            }
        }
    }
}
