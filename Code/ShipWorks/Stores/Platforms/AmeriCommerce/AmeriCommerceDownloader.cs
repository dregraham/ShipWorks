using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Autofac;
using Interapptive.Shared.Business;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.AmeriCommerce.WebServices;

namespace ShipWorks.Stores.Platforms.AmeriCommerce
{
    /// <summary>
    /// Downloads order information from AmeriCommerce
    /// </summary>
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.AmeriCommerce)]
    public class AmeriCommerceDownloader : StoreDownloader
    {
        static readonly ILog log = LogManager.GetLogger(typeof(AmeriCommerceDownloader));

        // total number of orders to be downloaded
        int totalCount = 0;

        // provider for status codes
        AmeriCommerceStatusCodeProvider statusProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmeriCommerceDownloader(StoreEntity store)
            : base(store)
        {

        }

        /// <summary>
        /// Download orders
        /// </summary>
        /// <param name="trackedDurationEvent">The telemetry event that can be used to
        /// associate any store-specific download properties/metrics.</param>
        protected override async Task Download(TrackedDurationEvent trackedDurationEvent)
        {
            try
            {
                Progress.Detail = "Updating status codes...";

                AmeriCommerceStoreEntity americommerceStore = Store as AmeriCommerceStoreEntity;

                // refresh the status codes from AmeriCommerce
                statusProvider = new AmeriCommerceStatusCodeProvider(americommerceStore);
                statusProvider.UpdateFromOnlineStore();

                Progress.Detail = "Checking for orders...";

                DateTime? lastModified = await GetOnlineLastModifiedStartingPoint().ConfigureAwait(false);

                List<OrderTrans> orders;

                // create the web client
                using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
                {
                    // Create the client for connecting to the module
                    IAmeriCommerceWebClient webClient = lifetimeScope.Resolve<IAmeriCommerceWebClient>(TypedParameter.From(americommerceStore));

                    // get orders
                    orders = webClient.GetOrders(lastModified);

                    totalCount = orders.Count;

                    if (totalCount == 0)
                    {
                        Progress.Detail = "No orders to download.";
                        Progress.PercentComplete = 100;
                        return;
                    }

                    Progress.Detail = String.Format("Downloading {0} orders...", totalCount);

                    // cycle through each order, importing
                    for (int i = 0; i < totalCount; i++)
                    {
                        OrderTrans order = orders[i];

                        // check for cancel
                        if (Progress.IsCancelRequested)
                        {
                            return;
                        }

                        Progress.Detail = String.Format("Downloading order {0} of {1}...", i + 1, totalCount);

                        await LoadOrder(webClient, order).ConfigureAwait(false);

                        Progress.PercentComplete = Math.Min(100, 100 * (i + 1) / totalCount);
                    }
                }
            }
            catch (AmeriCommerceException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
            catch (SqlForeignKeyException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Load an AmeriCommerce order
        /// </summary>
        private async Task LoadOrder(IAmeriCommerceWebClient client, OrderTrans orderTrans)
        {
            // first fetch all of the detail data for the order transaction
            orderTrans = client.FillOrderDetail(orderTrans);

            // check for cancel since FillOrderDetail is pretty time-intensive
            if (Progress.IsCancelRequested)
            {
                return;
            }

            // begin pulling into ShipWorks now
            int orderNumber = orderTrans.orderID.GetValue(0);

            GenericResult<OrderEntity> result = await InstantiateOrder(new OrderNumberIdentifier(orderNumber)).ConfigureAwait(false);
            if (result.Failure)
            {
                log.InfoFormat("Skipping order '{0}': {1}.", orderNumber, result.Message);
                return;
            }

            OrderEntity order = result.Value;

            // populate the few properties that are allowed to change between downloads
            order.OrderDate = orderTrans.orderDate.GetValue(DateTime.UtcNow);
            order.OnlineLastModified = orderTrans.EditDate.GetValue(order.OrderDate);

            // custom customer identifier
            order.OnlineCustomerID = orderTrans.customerID.GetValue(0);

            // status codes
            order.OnlineStatusCode = orderTrans.orderStatusID.GetValue(0);
            order.OnlineStatus = statusProvider.GetCodeName((int) order.OnlineStatusCode);

            // shipping
            order.RequestedShipping = orderTrans.shippingMethodName;

            // address information
            LoadAddressInfo(client, order, orderTrans);

            // do the rest only on new orders
            if (order.IsNew)
            {
                await LoadNotes(orderTrans, order).ConfigureAwait(false);

                LoadOrderItems(client, order, orderTrans);

                LoadOrderCharges(order, orderTrans);

                LoadPayments(order, orderTrans);

                order.OrderTotal = OrderUtility.CalculateTotal(order);
            }

            // save it
            SqlAdapterRetry<SqlException> retryAdapter = new SqlAdapterRetry<SqlException>(5, -5, "AmeriCommerceDownloader.LoadOrder");
            await retryAdapter.ExecuteWithRetryAsync(() => SaveDownloadedOrder(order)).ConfigureAwait(false);
        }

        /// <summary>
        /// Loads the various notes from the AmeriCommerce order
        /// </summary>
        private async Task LoadNotes(OrderTrans orderTrans, OrderEntity order)
        {
            string publicComments = orderTrans.CommentsPublic ?? "";
            if (publicComments.Length > 0)
            {
                await InstantiateNote(order, publicComments, order.OrderDate, NoteVisibility.Public).ConfigureAwait(false);
            }

            string instructions = orderTrans.CommentsInstructions ?? "";
            if (instructions.Length > 0)
            {
                await InstantiateNote(order, instructions, order.OrderDate, NoteVisibility.Public).ConfigureAwait(false);
            }

            string comments = orderTrans.comments ?? "";
            if (comments.Length > 0)
            {
                await InstantiateNote(order, comments, order.OrderDate, NoteVisibility.Internal).ConfigureAwait(false);
            }

            string giftMessage = orderTrans.GiftMessage ?? "";
            if (giftMessage.Length > 0)
            {
                await InstantiateNote(order, "Gift Message: " + giftMessage, order.OrderDate, NoteVisibility.Public).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Load payment method information from the AmeriCommerce order
        /// </summary>
        private void LoadPayments(OrderEntity order, OrderTrans orderTrans)
        {
            // number of credit cards
            int cardCounter = 0;
            int paymentCounter = 0;

            // import all payments
            foreach (OrderPaymentTrans paymentTrans in orderTrans.OrderPaymentColTrans)
            {
                // total payment number
                paymentCounter++;

                // cc-only handling
                if (paymentTrans.paymentType == PaymentTypes.CreditCard)
                {
                    cardCounter++;

                    // add CC detail
                    string cardName = paymentTrans.ccname ?? "";
                    string expires = String.Format("{0}/{1}",
                                            paymentTrans.ccexpmonth ?? "",
                                            paymentTrans.ccexpyear ?? "");

                    // decrypt cc number?
                    bool isDeclined = paymentTrans.declined;

                    string prefix = "Card";
                    if (cardCounter > 1)
                    {
                        prefix += " " + cardCounter + " ";
                    }

                    LoadPaymentDetail(order, prefix + " Type", cardName);
                    LoadPaymentDetail(order, prefix + " Expires", expires);

                    // specify declined
                    if (isDeclined)
                    {
                        LoadPaymentDetail(order, prefix + " Decined", "Declined");
                    }
                }

                // payment method
                string title = String.Format("Payment {0}Type", paymentCounter > 1 ? paymentCounter + " " : "");
                LoadPaymentDetail(order, title, paymentTrans.paymentTypeName ?? "");
            }
        }

        /// <summary>
        /// Creates a new payment detail for the order
        /// </summary>
        private void LoadPaymentDetail(OrderEntity order, string label, string value)
        {
            OrderPaymentDetailEntity detail = InstantiateOrderPaymentDetail(order);

            detail.Label = label;
            detail.Value = value;
        }

        /// <summary>
        /// Loads the charges associated with an order into ShipWorks
        /// </summary>
        private void LoadOrderCharges(OrderEntity order, OrderTrans orderTrans)
        {
            decimal additionalFees = orderTrans.AdditionalFees.GetValue(0M);
            if (additionalFees > 0)
            {
                string feeName = orderTrans.AdditionalFeesString ?? "Additional Fee";

                LoadCharge(order, "FEE", feeName, additionalFees);
            }

            decimal discount = orderTrans.discountAdded.GetValue(0M);
            if (discount > 0)
            {
                string discountName = orderTrans.discountString ?? "Discount";

                LoadCharge(order, "DISCOUNT", discountName, -discount);
            }

            decimal shipping = orderTrans.shippingAdded.GetValue(0M);
            if (shipping > 0)
            {
                LoadCharge(order, "SHIPPING", "Shipping", shipping);
            }

            decimal tax = orderTrans.taxAdded.GetValue(0M);
            if (tax > 0)
            {
                LoadCharge(order, "TAX", "Tax", tax);
            }
        }

        /// <summary>
        /// Loads a charge into ShipWorks
        /// </summary>
        private void LoadCharge(OrderEntity order, string type, string description, decimal value)
        {
            OrderChargeEntity charge = InstantiateOrderCharge(order);

            charge.Type = type;
            charge.Description = description;
            charge.Amount = value;
        }

        /// <summary>
        /// Load order items
        /// </summary>
        private void LoadOrderItems(IAmeriCommerceWebClient client, OrderEntity order, OrderTrans orderTrans)
        {
            foreach (OrderItemTrans orderItemTrans in orderTrans.OrderItemColTrans)
            {
                OrderItemEntity item = InstantiateOrderItem(order);

                item.Name = orderItemTrans.itemName ?? "";
                item.Code = String.IsNullOrEmpty(orderItemTrans.ItemNrFull) ? (orderItemTrans.itemNr ?? "") : orderItemTrans.ItemNrFull;
                item.SKU = item.Code;
                item.Quantity = orderItemTrans.quantity.GetValue(1);
                item.UnitPrice = orderItemTrans.price.GetValue(0M);
                item.UnitCost = orderItemTrans.cost.GetValue(0M);

                // URLs come down as relative, so we need to prefix them with the store URL
                item.Thumbnail = orderItemTrans.ItemThumb == null ? string.Empty : ((AmeriCommerceStoreEntity) Store).StoreUrl + orderItemTrans.ItemThumb;
                item.Image = item.Thumbnail;

                item.Weight = client.GetWeightInPounds(orderItemTrans.Weight.GetValue(0M), orderItemTrans.WeightUnitID.GetValue(0));

                LoadVariations(item, orderItemTrans);
                LoadCustomizations(item, orderItemTrans);
            }
        }

        /// <summary>
        /// Loads item customizations
        /// </summary>
        private void LoadCustomizations(OrderItemEntity item, OrderItemTrans orderItemTrans)
        {
            if (orderItemTrans.customizations.Length == 0)
            {
                return;
            }

            // parse the Personalizations and create an OrderItemAttribute for each
            string[] customizations = orderItemTrans.customizations.Split('|');
            foreach (string customization in customizations)
            {
                LoadOrderItemAttribute(item, customization, "");
            }
        }

        /// <summary>
        /// Load product variations
        /// </summary>
        private void LoadVariations(OrderItemEntity item, OrderItemTrans orderItemTrans)
        {
            // parse the variations and create an OrderItemAttribute for each
            string variationsString = orderItemTrans.variations;

            // don't continue if there aren't any variations
            if (variationsString.Length == 0)
            {
                return;
            }

            string splitPattern = @"(?<name>[^:]+): (?<value>[^,]+),?";
            Regex variationRegex = new Regex(splitPattern);

            foreach (Match match in variationRegex.Matches(variationsString))
            {
                string name = match.Groups["name"].Value;
                string value = match.Groups["value"].Value;

                LoadOrderItemAttribute(item, name, value);
            }
        }

        /// <summary>
        /// Creates a new OrderItemAttribute for the provided OrderItem, with the name and description
        /// </summary>
        private void LoadOrderItemAttribute(OrderItemEntity item, string name, string description)
        {
            OrderItemAttributeEntity option = InstantiateOrderItemAttribute(item);

            option.Name = name;
            option.Description = description;
            option.UnitPrice = 0.0M;
        }

        /// <summary>
        /// Loads the appropriate address info from the Americommerce order
        /// </summary>
        private void LoadAddressInfo(IAmeriCommerceWebClient client, OrderEntity order, OrderTrans orderTrans)
        {
            PersonAdapter shipAdapter = new PersonAdapter(order, "Ship");
            PersonAdapter billAdapter = new PersonAdapter(order, "Bill");

            // Fill in the address info
            LoadAddressInfo(client, shipAdapter, client.GetAddress(orderTrans.orderShippingAddressID.GetValue(0)));
            LoadAddressInfo(client, billAdapter, client.GetAddress(orderTrans.orderBillingAddressID.GetValue(0)));

            // email is on the customer record
            CustomerTrans customer = client.GetCustomer(orderTrans.customerID.GetValue(0));
            billAdapter.Email = customer.email;

            // AC only provides the customer level email.  Through correspondence with them they want us always using the customer email as the shipping email.
            shipAdapter.Email = customer.email;

            // fix bad/missing shipping information, take from the customer record
            if (shipAdapter.FirstName.Length == 0 && shipAdapter.LastName.Length == 0)
            {
                // first copy from Billing
                shipAdapter.FirstName = billAdapter.FirstName;
                shipAdapter.LastName = billAdapter.LastName;
                shipAdapter.Company = billAdapter.Company;

                // if it's still not there, copy from the customer
                if (shipAdapter.FirstName.Length == 0 && shipAdapter.LastName.Length == 0)
                {
                    shipAdapter.FirstName = customer.firstName;
                    shipAdapter.LastName = customer.lastName;
                    shipAdapter.Company = customer.Company;
                }
            }

            // both billing and shipping names came down parsed
            shipAdapter.NameParseStatus = PersonNameParseStatus.Simple;
            billAdapter.NameParseStatus = PersonNameParseStatus.Simple;
        }

        /// <summary>
        /// Populates a person adapter with information from an AmeriCommerce order address
        /// </summary>
        private void LoadAddressInfo(IAmeriCommerceWebClient client, PersonAdapter person, OrderAddressTrans address)
        {
            // name
            person.FirstName = address.FirstName;
            person.LastName = address.LastName;
            person.Company = address.Company ?? "";
            person.Street1 = address.Address1 ?? "";
            person.Street2 = address.Address2 ?? "";
            person.City = address.City ?? "";
            person.PostalCode = address.ZipCode ?? "";
            person.Phone = address.Phone;
            person.Fax = address.Fax;

            person.StateProvCode = client.GetStateCode(address.StateID.GetValue(0));
            person.CountryCode = client.GetCountryCode(address.CountryID.GetValue(0));

            if (person.CountryCode != "US" && person.StateProvCode.Length == 0)
            {
                person.StateProvCode = address.NonUSState;
            }
        }
    }
}
