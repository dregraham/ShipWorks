using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Business;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Import;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Rakuten.DTO;
using ShipWorks.Stores.Platforms.Rakuten.Enums;

namespace ShipWorks.Stores.Platforms.Rakuten
{
    /// <summary>
    /// Populates a RakutenOrderEntity from a downloaded Rakuten order
    /// </summary>
    [Component(RegistrationType.Self)]
    public class RakutenOrderLoader
    {
        private readonly IOrderChargeCalculator orderChargeCalculator;
        private readonly ILog log;
        private readonly IRakutenStoreEntity store;

        /// <summary>
        /// Initializes a new instance of the <see cref="RakutenOrderLoader"/> class.
        /// </summary>
        /// <param name="orderChargeCalculator">The order charge calculator.</param>
        public RakutenOrderLoader(IOrderChargeCalculator orderChargeCalculator,
            IRakutenStoreEntity store,
            Func<Type, ILog> loggerFactory)
        {
            this.orderChargeCalculator = orderChargeCalculator;
            this.store = store;
            log = loggerFactory(typeof(RakutenOrderLoader));
        }

        /// <summary>
        /// Loads the order
        /// </summary>
        /// <remarks>
        /// Order to save must have store loaded
        /// </remarks>
        public void LoadOrder(RakutenOrderEntity orderToSave, RakutenOrder downloadedOrder,
            List<RakutenProductsResponse> products,
            IOrderElementFactory orderElementFactory)
        {
            MethodConditions.EnsureArgumentIsNotNull(orderToSave.Store, "orderToSave.Store");

            orderToSave.OrderDate = downloadedOrder.OrderDate;
            orderToSave.OnlineLastModified = downloadedOrder.LastModifiedDate;
            orderToSave.RakutenPackageID = downloadedOrder.Shipping.OrderPackageID;

            LoadAddresses(orderToSave, downloadedOrder);
            LoadOrderStatus(orderToSave, downloadedOrder);

            if (orderToSave.IsNew || string.IsNullOrWhiteSpace(orderToSave.RequestedShipping))
            {
                LoadRequestedShipping(orderToSave, downloadedOrder);
            }

            if (orderToSave.IsNew)
            {
                // Load order data
                LoadNotes(orderToSave, downloadedOrder, orderElementFactory);
                LoadItems(orderToSave, downloadedOrder, products, orderElementFactory);
                LoadCharges(orderToSave, downloadedOrder, orderElementFactory);
                LoadPayments(orderToSave, downloadedOrder, orderElementFactory);
                SetOrderTotal(orderToSave, downloadedOrder, orderElementFactory);
            }
        }

        /// <summary>
        /// Loads the order statuses.
        /// </summary>
        private void LoadOrderStatus(RakutenOrderEntity orderToSave, RakutenOrder downloadedOrder)
        {
            var status = EnumHelper.TryParseEnum<RakutenOrderStatus>(downloadedOrder.OrderStatus);
            orderToSave.OnlineStatus = status == null ? EnumHelper.GetDescription(RakutenOrderStatus.Unknown) :
                EnumHelper.GetDescription(status);
        }

        /// <summary>
        /// Set the order total
        /// </summary>
        private void SetOrderTotal(RakutenOrderEntity orderToSave, RakutenOrder downloadedOrder, IOrderElementFactory orderElementFactory)
        {
            orderToSave.OrderTotal = downloadedOrder.OrderTotal;

            decimal calculatedTotal = orderChargeCalculator.CalculateTotal(orderToSave);

            // This should only happen if Rakuten doesn't send us the correct information somewhere
            if (downloadedOrder.OrderTotal != calculatedTotal)
            {
                decimal adjustment = downloadedOrder.OrderTotal - calculatedTotal;

                log.Info($"Order total for {downloadedOrder.OrderNumber} does not match our calculated total, adding an adjustment charge of {adjustment} to compensate for the discrepancy.");

                orderElementFactory.CreateCharge(orderToSave, "ADDITIONAL COST OR DISCOUNT", "Additional Cost or Discount", adjustment);
            }
        }

        /// <summary>
        /// Loads the items.
        /// </summary>
        private void LoadItems(RakutenOrderEntity orderToSave,
            RakutenOrder downloadedOrder,
            List<RakutenProductsResponse> products,
            IOrderElementFactory orderElementFactory)
        {
            if (downloadedOrder.OrderItems != null)
            {
                foreach (RakutenOrderItem downloadedItem in downloadedOrder.OrderItems)
                {
                    OrderItemEntity itemToSave = orderElementFactory.CreateItem(orderToSave);

                    var product = products.Where(x => x.BaseSKU.Equals(downloadedItem.BaseSKU)).FirstOrDefault();

                    LoadItem(orderToSave, itemToSave, downloadedItem, product, orderElementFactory);
                }
            }
        }

        /// <summary>
        /// Loads the item.
        /// </summary>
        private void LoadItem(RakutenOrderEntity order, OrderItemEntity itemToSave, RakutenOrderItem downloadedItem,
            RakutenProductsResponse product,
            IOrderElementFactory orderElementFactory)
        {
            itemToSave.SKU = downloadedItem.SKU ?? downloadedItem.BaseSKU;
            itemToSave.Quantity = downloadedItem.Quantity;
            itemToSave.UnitPrice = downloadedItem.UnitPrice;

            itemToSave.Name = GetEnglishOrFirst(downloadedItem.Name);

            if (product != null)
            {
                // This can override some of the base values set above
                LoadAdditionalItemInfo(order, itemToSave, product, orderElementFactory);
            }
        }

        /// <summary>
        /// Adds various details to the item
        /// </summary>
        private void LoadAdditionalItemInfo(RakutenOrderEntity order, OrderItemEntity item,
            RakutenProductsResponse product,
            IOrderElementFactory orderElementFactory)
        {
            var details = product.ShopSpecificProductDetails.Where(x => x.ShopKey?.ShopURL?.Equals(store.ShopURL) == true).FirstOrDefault();

            if (details != null)
            {
                LoadItemDetails(order, item, details, orderElementFactory);
            }

            var variant = product.Variants.Where(x => x.SKU?.Equals(item.SKU) == true).FirstOrDefault();

            if (variant != null)
            {
                LoadItemVariant(item, variant, product.VariantAttributeNames, orderElementFactory);
            }
        }

        /// <summary>
        /// Load shop-specific product details
        /// </summary>
        private void LoadItemDetails(RakutenOrderEntity order, OrderItemEntity item,
            RakutenProductDetails details,
            IOrderElementFactory orderElementFactory)
        {
            if (details.InternalNotes?.Any() == true)
            {
                // There will only ever be a single note. This is a dictionary instead
                // of a tuple because newtonsoft can't properly deserialize a tuple
                var text = $"Shipping Note: {details.InternalNotes.Values.FirstOrDefault()}";
                orderElementFactory.CreateNote(order, text, order.OrderDate, NoteVisibility.Public);
            }

            item.Brand = details.Brand;
            item.Description = GetEnglishOrFirst(details.Description);
            item.Image = details.Images?.FirstOrDefault()?.URL ?? string.Empty;
            item.Name = GetEnglishOrFirst(details.Title);

            // The SKU was already set in the LoadItem method, so we can use it here
            details.VariantSpecificInfo.TryGetValue(item.SKU, out RakutenVariantInfo variantInfo);

            // Override the image with the more specific variant image if available
            item.Image = variantInfo?.Images?.FirstOrDefault().URL ?? item.Image;
        }

        /// <summary>
        /// Load variant-specific product details
        /// </summary>
        private void LoadItemVariant(OrderItemEntity item, RakutenProductVariant variant,
            Dictionary<string, Dictionary<string, string>> attributeNames,
            IOrderElementFactory orderElementFactory)
        {
            if (variant.ShippingInfo?.Weight != null)
            {
                if (variant.ShippingInfo.Weight.Unit?.Equals("g", StringComparison.OrdinalIgnoreCase) == true)
                {
                    item.Weight = WeightUtility.Convert(Interapptive.Shared.Enums.WeightUnitOfMeasure.Grams,
                        Interapptive.Shared.Enums.WeightUnitOfMeasure.Pounds, variant.ShippingInfo.Weight.Value);
                }
                else
                {
                    item.Weight = variant.ShippingInfo.Weight.Value;
                }
            }

            if (variant.ShippingInfo?.Dimensions != null)
            {
                // Convert cm to inches
                if (variant.ShippingInfo.Dimensions.Unit?.Equals("cm", StringComparison.OrdinalIgnoreCase) == true)
                {
                    item.Length = variant.ShippingInfo.Dimensions.Length * 0.393701m;
                    item.Width = variant.ShippingInfo.Dimensions.Width * 0.393701m;
                    item.Height = variant.ShippingInfo.Dimensions.Height * 0.393701m;
                }
                else
                {
                    item.Length = variant.ShippingInfo.Dimensions.Length;
                    item.Width = variant.ShippingInfo.Dimensions.Width;
                    item.Height = variant.ShippingInfo.Dimensions.Height;
                }
            }

            item.MPN = variant.ManufacturerPartNumber;

            if (variant.GlobalTradeItems.TryGetValue("UPC", out string upc))
            {
                item.UPC = upc;
            }

            if (variant.GlobalTradeItems.TryGetValue("ISBN", out string isbn))
            {
                item.ISBN = isbn;
            }

            if (attributeNames != null)
            {
                LoadItemAttributes(item, variant, attributeNames, orderElementFactory);
            }
        }

        /// <summary>
        /// Load the variant-specific attributes
        /// </summary>
        private void LoadItemAttributes(OrderItemEntity item, RakutenProductVariant variant,
            Dictionary<string, Dictionary<string, string>> attributeNames,
            IOrderElementFactory orderElementFactory)
        {
            foreach (KeyValuePair<string, Dictionary<string, string>> productAttributeName in attributeNames)
            {
                var itemAttributeName = GetEnglishOrFirst(productAttributeName.Value);

                // Get the attribute values for this variant
                variant.VariantAttributes.TryGetValue(productAttributeName.Key, out Dictionary<string, string> variantAttribute);

                var itemAttributeValue = GetEnglishOrFirst(variantAttribute);

                // Assuming we have both a name and value, add the attribute to the item
                if (!string.IsNullOrEmpty(itemAttributeName) && !string.IsNullOrEmpty(itemAttributeValue))
                {
                    orderElementFactory.CreateItemAttribute(item, itemAttributeName, itemAttributeValue, 0, false);
                }
            }
        }

        /// <summary>
        /// Loads the charges.
        /// </summary>
        private void LoadCharges(RakutenOrderEntity orderToSave, RakutenOrder downloadedOrder,
            IOrderElementFactory orderElementFactory)
        {
            if (downloadedOrder.Shipping?.ShippingFee != 0)
            {
                orderElementFactory.CreateCharge(orderToSave, "SHIPPING", "Shipping", downloadedOrder.Shipping.ShippingFee);
            }

            if (downloadedOrder.SalesTaxTotal != 0)
            {
                orderElementFactory.CreateCharge(orderToSave, "SALES TAX", "Sales Tax", downloadedOrder.SalesTaxTotal);
            }

            if (downloadedOrder.RecyclingFeeTotal != 0)
            {
                orderElementFactory.CreateCharge(orderToSave, "RECYCLING FEE", "Recycling Fee", downloadedOrder.RecyclingFeeTotal);
            }
        }

        /// <summary>
        /// Loads the notes.
        /// </summary>
        private void LoadNotes(RakutenOrderEntity orderToSave, RakutenOrder downloadedOrder, IOrderElementFactory orderElementFactory)
        {
            if (!string.IsNullOrWhiteSpace(downloadedOrder.MerchantMemo))
            {
                var text = $"Merchant Memo: {downloadedOrder.MerchantMemo}";
                orderElementFactory.CreateNote(orderToSave, text, orderToSave.OrderDate, NoteVisibility.Public);
            }

            if (!string.IsNullOrWhiteSpace(downloadedOrder.ShopperComment) &&
                !downloadedOrder.ShopperComment.Equals("{}"))
            {
                var text = $"Shopper Comment: {downloadedOrder.ShopperComment}";
                orderElementFactory.CreateNote(orderToSave, text, orderToSave.OrderDate, NoteVisibility.Public);
            }

            if (downloadedOrder.CheckoutOptionalInfo?.Any() == true)
            {
                foreach (var info in downloadedOrder.CheckoutOptionalInfo)
                {
                    var formattedInfo = info.FilledInfo.Select(x => $"{x.Title}: {x.InputValue}");
                    string values = string.Join("\n", formattedInfo);
                    string noteText = $"{info.Name} Values:\n\n{values}";

                    orderElementFactory.CreateNote(orderToSave, noteText, orderToSave.OrderDate, NoteVisibility.Public);
                }
            }
        }

        /// <summary>
        /// Loads the shipping, billing, and email addresses.
        /// </summary>
        private void LoadAddresses(RakutenOrderEntity orderToSave, RakutenOrder downloadedOrder)
        {
            PersonAdapter shipAdapter = new PersonAdapter(orderToSave, "Ship");
            PersonAdapter billAdapter = new PersonAdapter(orderToSave, "Bill");

            LoadAddress(downloadedOrder, shipAdapter);
            LoadAddress(downloadedOrder, billAdapter);

            billAdapter.Email = downloadedOrder.AnonymizedEmailAddress;

            if (shipAdapter.FirstName == billAdapter.FirstName &&
                shipAdapter.LastName == billAdapter.LastName &&
                shipAdapter.City == billAdapter.City &&
                shipAdapter.Street1 == billAdapter.Street1)
            {
                shipAdapter.Email = billAdapter.Email;
            }
        }
        /// <summary>
        /// Loads the billing address.
        /// </summary>
        private void LoadAddress(RakutenOrder downloadedOrder, PersonAdapter adapter)
        {
            var address = adapter.FieldPrefix.Equals("Ship") ? downloadedOrder.Shipping.DeliveryAddress :
                downloadedOrder.Shipping.InvoiceAddress;

            if (address == null)
            {
                return;
            }

            var name = PersonName.Parse(address.Name);
            adapter.NameParseStatus = name.ParseStatus;
            adapter.FirstName = name.First;
            adapter.MiddleName = name.Middle;
            adapter.LastName = name.Last;

            // These are reversed by the API
            adapter.Street1 = address.Address2;
            adapter.Street2 = address.Address1;

            adapter.City = address.CityName;
            adapter.StateProvCode = ParseState(address.StateCode);
            adapter.PostalCode = address.PostalCode;
            adapter.CountryCode = address.CountryCode;
            adapter.Phone = address.PhoneNumber;
        }

        /// <summary>
        /// Remove the prepended country code from the state code
        /// </summary>
        private string ParseState(string stateCode)
        {
            var stateCodeIndex = stateCode.IndexOf("-");

            if (stateCodeIndex == -1 || stateCodeIndex + 1 >= stateCode.Length)
            {
                return stateCode;
            }

            return stateCode.Substring(stateCodeIndex + 1);
        }

        /// <summary>
        /// Loads payment information for the order
        /// </summary>
        private void LoadPayments(RakutenOrderEntity orderToSave, RakutenOrder downloadedOrder, IOrderElementFactory orderElementFactory)
        {
            CreatePaymentDetail(orderElementFactory, orderToSave, "Payment ID", downloadedOrder.Payment.OrderPaymentID);
            CreatePaymentDetail(orderElementFactory, orderToSave, "Payment Status", downloadedOrder.Payment.PaymentStatus);
            CreatePaymentDetail(orderElementFactory, orderToSave, "Payment Amount", downloadedOrder.Payment.PayAmount);
            CreatePaymentDetail(orderElementFactory, orderToSave, "Point Amount", downloadedOrder.Payment.PointAmount);
        }

        /// <summary>
        /// Creates the payment detail if value is not null or whitespace
        /// </summary>
        private void CreatePaymentDetail(IOrderElementFactory orderElementFactory, RakutenOrderEntity orderToSave, string label, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                orderElementFactory.CreatePaymentDetail(orderToSave, label, value);
            }
        }

        /// <summary>
        /// Loads the requested shipping
        /// </summary>
        private static void LoadRequestedShipping(RakutenOrderEntity orderToSave, RakutenOrder downloadedOrder)
        {
            string carrier = downloadedOrder.Shipping?.ShippingMethod;

            if (!string.IsNullOrEmpty(carrier))
            {
                orderToSave.RequestedShipping = carrier;
            }
        }

        /// <summary>
        /// For a given dictionary with language keys, try to get the english string.
        /// If english doesn't exist, get the first
        /// </summary>
        private string GetEnglishOrFirst(Dictionary<string, string> dictionary)
        {
            if (dictionary == null || !dictionary.Any())
            {
                return string.Empty;
            }

            dictionary.TryGetValue("en_US", out string result);
            if (string.IsNullOrWhiteSpace(result))
            {
                return dictionary.Values.FirstOrDefault();
            }

            return result;
        }
    }
}