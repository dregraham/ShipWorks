using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.BigCommerce.DTO;
using ShipWorks.Stores.Platforms.BigCommerce.Enums;
using log4net;

namespace ShipWorks.Stores.Platforms.BigCommerce
{
    /// <summary>
    /// Downloader for BigCommerce stores
    /// </summary>
    public class BigCommerceDownloader : StoreDownloader
    {
        static readonly ILog log = LogManager.GetLogger(typeof(BigCommerceDownloader));
        int totalCount;
        BigCommerceWebClient webClient;
        readonly BigCommerceStoreEntity bigCommerceStore;
        const int MissingCustomerID = 0;

        // provider for status codes
        BigCommerceStatusCodeProvider statusProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="store">Store for which this downloader will operate</param>
        public BigCommerceDownloader(BigCommerceStoreEntity store)
            : base(store)
        {
            bigCommerceStore = store;
            totalCount = 0;
        }

        /// <summary>
        /// The web client used to download
        /// </summary>
        public BigCommerceWebClient WebClient
        {
            get 
            {
                return webClient ??
                       (webClient =
                        new BigCommerceWebClient(bigCommerceStore.ApiUserName, bigCommerceStore.ApiUrl,
                                                 bigCommerceStore.ApiToken, Progress));
            }
        }

        /// <summary>
        /// Download orders and statuses for the BigCommerce store
        /// </summary>
        protected override void Download()
        {
            try
            {
                // Update the orders statuses from online
                UpdateOrderStatuses();

                Progress.Detail = "Checking for orders...";

                // Get the total number of orders for the range
                BigCommerceWebClientOrderSearchCriteria orderSearchCriteria =
                    GetOrderSearchCriteria(BigCommerceWebClientOrderDateSearchType.CreatedDate);
                totalCount = WebClient.GetOrderCount(orderSearchCriteria);

                if (totalCount != 0)
                {
                    // Download any newly created orders (thier modified dates could be older than the last last modified date SW has)
                    DownloadOrders(orderSearchCriteria);
                }

                if (bigCommerceStore.DownloadModifiedNumberOfDaysBack != 0)
                {
                    // Now Download modified orders
                    Progress.Detail = "Checking for modified orders...";
                    orderSearchCriteria.OrderDateSearchType = BigCommerceWebClientOrderDateSearchType.ModifiedDate;
                    orderSearchCriteria.LastModifiedFromDate = DateTime.UtcNow.AddDays(-bigCommerceStore.DownloadModifiedNumberOfDaysBack);

                    int modifiedCount = WebClient.GetOrderCount(orderSearchCriteria);

                    if (modifiedCount == 0)
                    {
                        Progress.Detail = "No orders to download.";
                        Progress.PercentComplete = 100;
                        return;
                    }

                    // Update the total count to included the modified orders
                    totalCount += modifiedCount;

                    // Update the progress bar for the new count
                    Progress.PercentComplete = QuantitySaved/totalCount;

                    // Download the modified orders
                    DownloadOrders(orderSearchCriteria);
                }

                Progress.PercentComplete = 100;
                Progress.Detail = "Done.";
            }
            catch (BigCommerceWebClientThrottleWaitCancelException)
            {
                // Just a cancel - nothing to do
            }
            catch (BigCommerceException ex)
            {
                log.Error(ex);
                throw new DownloadException(ex.Message, ex);
            }
            catch (SqlForeignKeyException ex)
            {
                log.Error(ex);
                throw new DownloadException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Update the local order status provider
        /// </summary>
        private void UpdateOrderStatuses()
        {
            Progress.Detail = "Updating status codes...";

            // refresh the status codes from BigCommerce
            statusProvider = new BigCommerceStatusCodeProvider((BigCommerceStoreEntity)Store);
            statusProvider.UpdateFromOnlineStore();
        }

        /// <summary>
        /// Download orders for specified order search criteria
        /// </summary>
        /// <param name="orderSearchCriteria"></param>
        private void DownloadOrders(BigCommerceWebClientOrderSearchCriteria orderSearchCriteria)
        {
            int page = 1;
            while (true)
            {
                // Download the next range of orders
                orderSearchCriteria.Page = page;

                try
                {
                    if (!DownloadOrderRange(orderSearchCriteria))
                    {
                        return;
                    }
                }
                catch (BigCommerceMaxIncompleteOrdersReachedException)
                {
                    // If this was caught, we got a page full of InComplete orders, so just move to the next page.
                }

                // Check for cancel
                if (Progress.IsCancelRequested)
                {
                    return;
                }

                page++;
            }
        }

        /// <summary>
        /// Download the next range of orders. 
        /// </summary>
        /// <param name="orderSearchCriteria">The BigCommerceWebClientOrderSearchCriteria for which to download orders.</param>
        /// <returns>True if orders were loaded, false if the user clicks cancel or if no orders were left to download</returns>
        private bool DownloadOrderRange(BigCommerceWebClientOrderSearchCriteria orderSearchCriteria)
        {
            // Check for cancel
            if (Progress.IsCancelRequested)
            {
                return false;
            }

            // Update progress reporter that we are now downloading orders
            Progress.Detail = string.Format("Downloading {0} orders...", 
                orderSearchCriteria.OrderDateSearchType == BigCommerceWebClientOrderDateSearchType.CreatedDate ? "new" : "modified");

            // Download the orders to process
            List<BigCommerceOrder> orders = WebClient.GetOrders(orderSearchCriteria); 

            // Check to see that we received some orders to process (just in case another ShipWorks instance downloaded them before we got to them)
            if (orders.Count == 0)
            {
                Progress.Detail = "No additional orders to download.";
                Progress.PercentComplete = 100;
                return false;
            }

            // Return the result of loading the orders.
            return LoadOrders(orders);
        }

        /// <summary>
        /// Gets the last online modified date from the orders table, and adds 1 second so that we don't processes the already downloaded
        /// order mulitple times.
        /// </summary>
        /// <returns>BigCommerceWebClientOrderSearchCriteria </returns>
        private BigCommerceWebClientOrderSearchCriteria GetOrderSearchCriteria(BigCommerceWebClientOrderDateSearchType orderDateSearchType)
        {
            // Getting last online modified starting point
            DateTime? createdDateStartingPoint = GetOrderDateStartingPoint();

            // If the date has a value, add 1 second, otherwise default to 6 months back
            createdDateStartingPoint = createdDateStartingPoint.HasValue ? createdDateStartingPoint.Value.ToUniversalTime() : DateTime.UtcNow.AddMonths(-6);

            // Set end date to now
            DateTime createdDateEndPoint = DateTime.UtcNow;

            // Getting last online modified starting point
            DateTime? modifiedDateStartingPoint = GetOnlineLastModifiedStartingPoint();

            // If the date has a value, add 1 second, otherwise default to 6 months back
            modifiedDateStartingPoint = modifiedDateStartingPoint.HasValue ? modifiedDateStartingPoint.Value.ToUniversalTime() : DateTime.UtcNow.AddMonths(-6);

            // Set end date to now
            DateTime modifiedDateEndPoint = DateTime.UtcNow;

            BigCommerceWebClientOrderSearchCriteria orderSearchCriteria = 
                new BigCommerceWebClientOrderSearchCriteria(orderDateSearchType, 
                    modifiedDateStartingPoint.Value, modifiedDateEndPoint,
                    createdDateStartingPoint.Value, createdDateEndPoint)
                {
                    PageSize = BigCommerceConstants.OrdersPageSize,
                    Page = 1
                };

            // Create the order search criteria
            return orderSearchCriteria;
        }

        /// <summary>
        /// Load all the orders contained in the list
        /// </summary>
        /// <returns>True, if all orders were loaded.  False if the user pressed cancel</returns>
        private bool LoadOrders(IEnumerable<BigCommerceOrder> orders)
        {
            // Go through each order in the XML Document
            foreach (BigCommerceOrder order in orders)
            {
                // Check for user cancel
                if (Progress.IsCancelRequested)
                {
                    return false;
                }

                // Update the status
                Progress.Detail = string.Format("Processing order {0}...", (QuantitySaved + 1));

                // If the order has at least one shipping address
                if (order.OrderShippingAddresses != null && order.OrderShippingAddresses.Count > 0)
                {
                    // If there are more than one OrderShippingAddress, we'll be creating sub orders
                    bool hasSubOrders = order.OrderShippingAddresses.Count > 1;

                    // Default to not being a sub order, as this will be the parent if there are sub orders
                    bool isSubOrder = false;

                    // We'll use this as the order post fix, so start with 1
                    int shipToAddressIndex = 1;
                    foreach (BigCommerceAddress shipToAddress in order.OrderShippingAddresses.OrderBy(address => address.id))
                    {
                        // If there are mulitple ship to addresses, create the post fix.  Otherwise leave it blank.
                        string orderPostfix = hasSubOrders ? string.Format("-{0}", shipToAddressIndex) : string.Empty;

                        LoadOrder(order, orderPostfix, shipToAddress, isSubOrder, hasSubOrders);

                        // Set the flag to note the next order will be a sub order
                        isSubOrder = true;

                        // Increment the ship to address by one so the post fix is correct
                        shipToAddressIndex++;
                    }
                }
                else
                {
                    // It must be a digital order since no shipping was provided
                    LoadOrder(order, string.Empty, null, false, false);
                }

                //Update the PercentComplete
                Progress.PercentComplete = 100 * QuantitySaved / totalCount;
            }

            return true;
        }

        /// <summary>
        /// Extract and save the order from the XML
        /// </summary>
        private void LoadOrder(BigCommerceOrder order, string orderNumberPostfix, BigCommerceAddress shipToAddress, bool isSubOrder, bool hasSubOrders)
        {
            // Create a BigCommerceOrderIdentifier
            BigCommerceOrderIdentifier bigCommerceOrderIdentifier = new BigCommerceOrderIdentifier(order.id, orderNumberPostfix);

            // Get the order instance.
            OrderEntity orderEntity = InstantiateOrder(bigCommerceOrderIdentifier);

            // If the order does not have sub orders, set the order total.  If it does have sub orders, each order should be calculated based on it's
            // content
            if (!hasSubOrders)
            {
                // Set the total.  It will be calculated and verified later.
                orderEntity.OrderTotal = Convert(order.total_inc_tax, 0m);
            }
            
            // Get the customer
            int onlineCustomerID = (int)order.customer_id;
            orderEntity.OnlineCustomerID = (onlineCustomerID <= MissingCustomerID) ? (int?)null : onlineCustomerID;

            // Requested shipping.  Default to N/A to support all digital orders.
            string requestedShipping = "N/A";
            if (shipToAddress != null)
            {
                requestedShipping = shipToAddress.shipping_method;
            }
            orderEntity.RequestedShipping = requestedShipping;

            // Set order dates.
            SetOrderDates(order, orderEntity);

            //Load Online Status
            LoadStatus(orderEntity, order);

            //Load address info
            LoadAddressInfo(orderEntity, order, shipToAddress);

            // Load any order notes
            LoadOrderNotes(orderEntity, order);

            // Only update the rest for brand new orders
            if (orderEntity.IsNew)
            {
                PopulateNewOrder(order, shipToAddress, isSubOrder, orderEntity, hasSubOrders);
            }

            // If this is a sub order, increment the total count
            if (isSubOrder)
            {
                totalCount++;
            }

            // Save the downloaded order
            SqlAdapterRetry<SqlException> retryAdapter = new SqlAdapterRetry<SqlException>(5, -5, "BigCommerceDownloader.LoadOrder");
            retryAdapter.ExecuteWithRetry(() => SaveDownloadedOrder(orderEntity));
        }
  
        private void PopulateNewOrder(BigCommerceOrder order, BigCommerceAddress shipToAddress, bool isSubOrder, OrderEntity orderEntity, bool hasSubOrders)
        {
            // If the order isn't new, return
            if (!orderEntity.IsNew)
            {
                return;
            }

            // If we have a ship to address, filter out products not in this shipment
            // Digital items come with an order_address_id = 0...  we'll put them on the first order
            var qry = from orderProduct in order.OrderProducts
                      where (shipToAddress != null && orderProduct.order_address_id == shipToAddress.id) || (!isSubOrder && orderProduct.order_address_id == 0)
                      select orderProduct;

            List<BigCommerceProduct> orderProducts = qry.ToList();

            //Iterate through each item
            foreach (BigCommerceProduct orderProduct in orderProducts)
            {
                LoadItem(orderEntity, orderProduct);
            }

            // Only add charges/payments if processing the main order
            if (!isSubOrder)
            {
                //Load all the charges
                LoadOrderCharges(orderEntity, order);

                //Load all payment details
                LoadPaymentDetails(orderEntity, order);

                // Load any store credit 
                LoadStoreCredit(orderEntity, order);
            }

            // If this is a sub order, or it's the first order that has sub orders, calculate the total 
            if (isSubOrder || hasSubOrders)
            {
                decimal total = OrderUtility.CalculateTotal(orderEntity);
                orderEntity.OrderTotal = total;
            }
        }
  
        /// <summary>
        /// Converts and sets BigCommerce order dates to SW dates on the order
        /// </summary>
        /// <param name="order">The BigCommerce order which has the order dates to convert</param>
        /// <param name="orderEntity">The order on which to update the dates</param>
        private static void SetOrderDates(BigCommerceOrder order, OrderEntity orderEntity)
        {
            // Get order date.
            DateTime rfc1123Date;
            if (DateTime.TryParseExact(order.date_created, "ddd, dd MMM yyyy HH:mm:ss zzz", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out rfc1123Date))
            {
                orderEntity.OrderDate = rfc1123Date;
            }
            else
            {
                log.ErrorFormat("Unable to parse order.date_created to an rfc1123 date.  date_created: {0}", order.date_created);
                throw new BigCommerceException("Unable to get order creation date from BigCommerce");
            }

            // Set OnlineLastModified to their last modified date
            if (DateTime.TryParseExact(order.date_modified, "ddd, dd MMM yyyy HH:mm:ss zzz", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out rfc1123Date))
            {
                orderEntity.OnlineLastModified = rfc1123Date;
            }
            else
            {
                log.ErrorFormat("Unable to parse order.date_modified to an rfc1123 date.  date_modified: {0}", order.date_modified);
                throw new BigCommerceException("Unable to get order modification date from BigCommerce");
            }
        }

        /// <summary>
        /// Extract and set the status from the order
        /// </summary>
        private void LoadStatus(OrderEntity orderEntity, BigCommerceOrder order)
        {
            //Add Online Status
            orderEntity.OnlineStatusCode = order.status_id;
            orderEntity.OnlineStatus = order.status;

            // Deleted isn't a BigCommerce status, it's a flag.  But we'll treat it as a status in SW
            if (order.is_deleted)
            {
                if (statusProvider[BigCommerceConstants.OnlineStatusDeletedCode] != null)
                {
                    orderEntity.OnlineStatus = statusProvider[BigCommerceConstants.OnlineStatusDeletedCode];
                }
            }
        }

        /// <summary>
        /// Load the item information into the order
        /// </summary>
        private void LoadItem(OrderEntity order, BigCommerceProduct orderProduct)
        {
            BigCommerceOrderItemEntity item = (BigCommerceOrderItemEntity) InstantiateOrderItem(order);

            // Set item properties
            item.Code = orderProduct.sku;
            item.SKU = orderProduct.sku;
            item.Quantity = orderProduct.quantity;
            item.UnitPrice = Convert(orderProduct.price_ex_tax, 0m);
            item.UnitCost = Convert(orderProduct.cost_price_ex_tax, 0m);

            // Convert weights from store units to pounds
            item.Weight = WeightUtility.Convert((WeightUnitOfMeasure)bigCommerceStore.WeightUnitOfMeasure, 
                                                 WeightUnitOfMeasure.Pounds, 
                                                 Convert<double>(orderProduct.weight, 0));

            item.Name = orderProduct.name;

            // Set the event date and name.  This is usually a delivery date
            DateTime eventDate;
            if (DateTime.TryParse(orderProduct.event_date, out eventDate))
            {
                item.EventDate = eventDate.ToUniversalTime();
            }
            item.EventName = orderProduct.event_name;

            // Set the bin picking number
            item.Location = orderProduct.bin_picking_number;

            // Save the BigCommerce ShipmentID with which this item is associated
            item.OrderAddressID = orderProduct.order_address_id > 0 ? orderProduct.order_address_id : BigCommerceConstants.InvalidOrderAddressID;

            item.OrderProductID = orderProduct.id;

            string productType = orderProduct.type.ToLowerInvariant();
            item.IsDigitalItem = productType == EnumHelper.GetApiValue(BigCommerceProductType.Digital) ||
                                 productType == EnumHelper.GetApiValue(BigCommerceProductType.GiftCertificate);

            // Now load all the item options
            LoadProductAndRelatedObjects(item, orderProduct);

            // Load any gift wrapping messages and cost for the item
            LoadOrderItemGiftWrap(item, orderProduct);
        }

        /// <summary>
        /// Load any gift wrapping messages and cost for the item
        /// </summary>
        /// <param name="item">The order item to add gift wrapping attributes to.</param>
        /// <param name="product">The BigCommerce Product with the gift wrapping information.</param>
        private void LoadOrderItemGiftWrap(OrderItemEntity item, BigCommerceProduct product)
        {
            // BigCommerce makes the user select a gift wrapping option when adding gift wrapping to an item.
            // The gift wrapping option has a name, so we will check that to see if the user selected gift wrapping.
            if (!string.IsNullOrEmpty(product.wrapping_name))
            {
                OrderItemAttributeEntity optionToAdd = InstantiateOrderItemAttribute(item);
                optionToAdd.Name = string.Format("Gift Wrap - {0}", product.wrapping_name);
                optionToAdd.Description = product.wrapping_message;
                optionToAdd.UnitPrice = Convert(product.wrapping_cost_ex_tax, 0m); 
            }
        }

        /// <summary>
        /// Load the option(s) of the given item
        /// </summary>
        private void LoadProductAndRelatedObjects(OrderItemEntity item, BigCommerceProduct product)
        {
            // Add each product option as an SW attribute.
            // BigCommerce doesn't provide pricing at the product option level, so set to $0
            foreach (BigCommerceProductOption productOption in product.product_options)
            { 
                OrderItemAttributeEntity optionToAdd = InstantiateOrderItemAttribute(item);
                optionToAdd.Name = productOption.display_name;
                optionToAdd.Description = productOption.display_value;
                optionToAdd.UnitPrice = 0; 
            }

            // BigCommerce has configurable fields that admins can create their own data to capture when ordering.  For example, engraving text, allow uploading a file, etc...
            // Iterate through each, creating an attribute for each.
            foreach (BigCommerceConfigurableField configurableField in product.configurable_fields)
            {
                OrderItemAttributeEntity optionToAdd = InstantiateOrderItemAttribute(item);
                optionToAdd.Name = configurableField.name;
                optionToAdd.Description = string.IsNullOrWhiteSpace(configurableField.original_filename) ? configurableField.value.ToString() : configurableField.original_filename;
                optionToAdd.UnitPrice = 0; 
            }

            if (product.Image != null)
            {
                item.Image = product.Image; 
            }

            if (product.ThumbnailImage != null)
            {
                item.Thumbnail = product.ThumbnailImage; 
            }
        }

        /// <summary>
        /// Load all the charges for the order
        /// </summary>
        private void LoadOrderCharges(OrderEntity orderEntity, BigCommerceOrder order)
        {
            // Charge - Discount
            decimal discountAmount = Convert(order.discount_amount, 0m);
            if (discountAmount > 0.0m)
            {
                //Load charge for each discount code
                LoadCharge(orderEntity, "Discount", "Discount", -discountAmount);
            }

            // Charge - Gift cert
            decimal giftCertAmount = Convert(order.gift_certificate_amount, 0m);
            if (giftCertAmount > 0.0m)
            {
                //Load charge for each discount code
                LoadCharge(orderEntity, "Gift Certificate", "Gift Certificate", -giftCertAmount);
            }

            //Iterate through each coupon
            if (order.OrderCoupons != null)
            {
                foreach (BigCommerceCoupon coupon in order.OrderCoupons)
                {
                    //Load charge for each coupon code
                    LoadCharge(orderEntity, "Coupon", coupon.code, -coupon.discount);

                    // If the coupon.Type = 4, it is free shipping.  But there will be no shipping charge provided so we would
                    // subtract too much.  So if we are a free shipping coupon, add a shipping charge of the same amount
                    if (coupon.type == "4")
                    {
                        // Now add a shipping charge to match this coupon
                        LoadCharge(orderEntity, "Shipping", "Free Shipping", coupon.discount);
                    }
                }
            }

            // We receive individual tax amounts for different parts of the order.  However, if there's a discount/coupon
            // the parts don't always add up to the real total_tax on the order.  So, only add the total tax.
            decimal totalTax = Convert(order.total_tax, 0m);
            LoadCharge(orderEntity, "Tax", "Total Tax", totalTax);
            
            CreateChargeIfNecessary(orderEntity, order.shipping_cost_ex_tax, "Shipping", orderEntity.RequestedShipping);
            CreateChargeIfNecessary(orderEntity, order.handling_cost_ex_tax, "Handling", string.Empty);
            CreateChargeIfNecessary(orderEntity, order.wrapping_cost_ex_tax, "Wrapping", string.Empty);
        }

        /// <summary>
        /// Creates an extra charge if the charge is set and greater than zero
        /// </summary>
        /// <param name="orderEntity">Order into which the charge will be saved</param>
        /// <param name="charge">Value of the charge</param>
        /// <param name="type">Type of charge</param>
        /// <param name="name">Name of the charge</param>
        private void CreateChargeIfNecessary(OrderEntity orderEntity, string charge, string type, string name)
        {
            // Save the charge cost if it was included
            decimal chargeAmount = Convert(charge, 0m);
            if (chargeAmount > 0.0m)
            {
                LoadCharge(orderEntity, type, name, chargeAmount);
            }
        }

        /// <summary>
        /// Load an order charge for the given values for the order
        /// </summary>
        private void LoadCharge(OrderEntity order, string type, string name, decimal amount)
        {
            //InstantiateOrderCharge
            OrderChargeEntity charge = InstantiateOrderCharge(order);

            //Set charge properties
            if (string.IsNullOrWhiteSpace(name))
            {
                name = type;
            }

            charge.Type = type.ToUpperInvariant();
            charge.Description = name;
            charge.Amount = amount;
        }

        /// <summary>
        /// Load the payment details for the order
        /// </summary>
        private void LoadPaymentDetails(OrderEntity orderEntity, BigCommerceOrder order)
        {
            // Payment details
            string paymentMethod = order.payment_method;
            if (!string.IsNullOrWhiteSpace(paymentMethod))
            {
                //Load payment details
                LoadPaymentDetail(orderEntity, "Payment Type", paymentMethod);
            }
        }

        /// <summary>
        /// Load store credit if needed
        /// </summary>
        private void LoadStoreCredit(OrderEntity orderEntity, BigCommerceOrder bigCommerceOrder)
        {
            // Customers can pay with store credit.  Add a charge to capture this for the full amount.
            decimal total;
            if (decimal.TryParse(bigCommerceOrder.store_credit_amount, out total) && total > 0)
            {
                LoadCharge(orderEntity, "Store Credit", "Store Credit", -total);
            }
        }

        /// <summary>
        /// Load the given payment detail into the order
        /// </summary>
        private void LoadPaymentDetail(OrderEntity order, string label, string value)
        {
            //Instantiate OrderPaymentDetail
            OrderPaymentDetailEntity detail = InstantiateOrderPaymentDetail(order);

            //Set orderPaymentDetail properties
            detail.Label = label;
            detail.Value = value;
        }

        /// <summary>
        /// If the order has any notes, save them
        /// </summary>
        private void LoadOrderNotes(OrderEntity orderEntity, BigCommerceOrder order)
        {
            string orderComment = order.customer_message;
            InstantiateNote(orderEntity, orderComment, DateTime.Now, NoteVisibility.Public, true);

            string orderInternalComment = order.staff_notes;
            InstantiateNote(orderEntity, orderInternalComment, DateTime.Now, NoteVisibility.Internal, true);
        }

        /// <summary>
        /// Load the appropriate address info from the XPath
        /// </summary>
        private static void LoadAddressInfo(OrderEntity orderEntity, BigCommerceOrder order, BigCommerceAddress shipToAddress)
        {
            // Load shipping address info, if it's not "digital"
            // Digital can mean download or gift cert, in which case no shipping address is provided.  This value is only
            // true if ALL items on the order are digital.  If at least 1 item is shipped, it will be false.
            if (!order.order_is_digital && shipToAddress != null)
            {
                LoadAddressInfo(orderEntity, shipToAddress, "Ship");
            }
            
            //Load billing address info
            LoadAddressInfo(orderEntity, order.billing_address.ToAddress(), "Bill");

            //Bill only properties
            orderEntity.BillEmail = order.billing_address.email;

            // BigCommerce doesnt have a ShipTo email, so set it to the bill email
            orderEntity.ShipEmail = orderEntity.BillEmail;
        }

        /// <summary>
        /// Load the address info for the given address type prefix
        /// </summary>
        private static void LoadAddressInfo(OrderEntity order, BigCommerceAddress address, string dbPrefix)
        {
            //See if the NameParts entries exist
            string first = address.first_name;
            string middle = string.Empty;
            string last = address.last_name;

            //Parse person's name
            PersonName fullName = PersonName.Parse(string.Format("{0} {1}", first, last));
            PersonAdapter personAdapter = new PersonAdapter(order, dbPrefix);
            
            personAdapter.UnparsedName = fullName.FullName;
            personAdapter.NameParseStatus = PersonNameParseStatus.Simple;
            personAdapter.FirstName = first;
            personAdapter.MiddleName = middle;
            personAdapter.LastName = last;

            personAdapter.Company = address.company;
            personAdapter.Phone = address.phone;

            personAdapter.Street1 = address.street_1;
            personAdapter.Street2 = address.street_2;

            personAdapter.City = address.city;
            personAdapter.StateProvCode = Geography.GetStateProvCode(address.state);
            personAdapter.PostalCode = address.zip;
            personAdapter.CountryCode = Geography.GetCountryCode(address.country_iso2);
        }

        /// <summary>
        /// Helper method to convert strings to another type.
        /// </summary>
        /// <typeparam name="T">Type to which the string should be converted</typeparam>
        /// <param name="input">The string to convert</param>
        /// <param name="defaultValue">If no converter is found, return this as the default value.</param>
        /// <returns>The value of input converted to T</returns>
        public static T Convert<T>(string input, T defaultValue)
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));
            if (converter != null)
            {
                return (T)converter.ConvertFromString(input);
            }
            return defaultValue;
        }
    }
}
