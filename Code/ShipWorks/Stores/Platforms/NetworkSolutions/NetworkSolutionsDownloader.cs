using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using log4net;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Stores.Communication;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Stores.Platforms.NetworkSolutions.WebServices;
using Interapptive.Shared.Business;
using ShipWorks.Stores.Content;
using System.Globalization;

namespace ShipWorks.Stores.Platforms.NetworkSolutions
{
    /// <summary>
    /// Order downloader for NetworkSolutions stores
    /// </summary>
    public class NetworkSolutionsDownloader : StoreDownloader
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(NetworkSolutionsDownloader));

        NetworkSolutionsStatusCodeProvider statusProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public NetworkSolutionsDownloader(StoreEntity store)
            : base(store)
        {
        }

        /// <summary>
        /// Retrieve new orders from NetworkSolutions
        /// </summary>
        protected override void Download()
        {
            try
            {
                Progress.Detail = "Updating status codes...";

                statusProvider = new NetworkSolutionsStatusCodeProvider((NetworkSolutionsStoreEntity)Store);
                statusProvider.UpdateFromOnlineStore();

                Progress.Detail = "Checking for orders...";

                NetworkSolutionsWebClient webClient = new NetworkSolutionsWebClient((NetworkSolutionsStoreEntity)Store);

                // check for cancel
                if (Progress.IsCancelRequested)
                {
                    return;
                }

                // download until NetworkSolutions says no more orders exist to download
                while (webClient.HasMoreOrders)
                {
                    List<OrderType> orders = webClient.GetNextOrders();

                    if (webClient.TotalCount == 0)
                    {
                        Progress.PercentComplete = 100;
                        Progress.Detail = "Done.";
                        return;
                    }

                    // import each order
                    foreach (OrderType order in orders)
                    {
                        // check for cancel
                        if (Progress.IsCancelRequested)
                        {
                            return;
                        }

                        // after the first order pull, we can get the total number of orders available for progress 
                        Progress.Detail = String.Format("Downloading order {0} of {1}...", QuantitySaved + 1, webClient.TotalCount);

                        // import the order
                        LoadOrder(order);

                        // update progress
                        Progress.PercentComplete = Math.Min(100, 100 * QuantitySaved / webClient.TotalCount);
                    }
                }
            }
            catch (NetworkSolutionsException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
            catch (SqlForeignKeyException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Loads a NetworkSolutions order into ShipWorks
        /// </summary>
        private void LoadOrder(OrderType nsOrder)
        {
            if (string.IsNullOrWhiteSpace(nsOrder.OrderNumber))
            {
                log.InfoFormat("Order with OrderId '{0}' did not have an order number.  Skipping it and continuing to the next order.", nsOrder.OrderId);
                return;
            }

            long networkSolutionsOrderId = nsOrder.OrderId;

            NetworkSolutionsOrderEntity order = (NetworkSolutionsOrderEntity)InstantiateOrder(new NetworkSolutionsOrderIdentifier(networkSolutionsOrderId));

            // populate things that can change between downloads
            order.OrderDate = nsOrder.CreateDate;
            order.NetworkSolutionsOrderID = nsOrder.OrderId;
            order.OrderNumber = Convert.ToInt64(nsOrder.OrderNumber);

            // online customer id
            order.OnlineCustomerID = nsOrder.Customer == null ? null : nsOrder.Customer.CustomerId;

            // requested shipping
            order.RequestedShipping = nsOrder.Shipping == null ? string.Empty : nsOrder.Shipping.Name;

            // order status
            if (nsOrder.Status != null)
            {
                order.OnlineStatusCode = nsOrder.Status.OrderStatusId;
                order.OnlineStatus = statusProvider.GetCodeName(nsOrder.Status.OrderStatusId);
            }

            // shipping/billing address information
            LoadAddressInfo(order, nsOrder);

            // the remainder is only to be done on new orders
            if (order.IsNew)
            {
                LoadNotes(order, nsOrder);

                LoadOrderItems(order, nsOrder);

                LoadOrderCharges(order, nsOrder);

                LoadPayments(order, nsOrder);

                order.OrderTotal = OrderUtility.CalculateTotal(order);
            }

            // save the order
            SqlAdapterRetry<SqlException> retryAdapter = new SqlAdapterRetry<SqlException>(5, -5, "NetworkSolutionsDownloader.LoadOrder");
            retryAdapter.ExecuteWithRetry(() => SaveDownloadedOrder(order));
        }

        /// <summary>
        /// Load payment information
        /// </summary>
        private void LoadPayments(NetworkSolutionsOrderEntity order, OrderType nsOrder)
        {
            if (nsOrder.Payment.PaymentMethodSpecified)
            {
                string paymentMethod = nsOrder.Payment.PaymentMethod.ToString();
                CreatePaymentDetail(order, "Payment Type", paymentMethod);

                CreditCardType cc = nsOrder.Payment.CreditCard;
                if (cc != null)
                {
                    string firstName = cc.FirstName;
                    string lastName = cc.LastName;
                    string owner = string.Format("{0} {1}", firstName, lastName).Trim();

                    if (owner.Length > 0)
                    {
                        CreatePaymentDetail(order, "Card Owner", owner);
                    }

                    if (cc.ExpirationSpecified)
                    {
                        CreatePaymentDetail(order, "Card Expires", cc.Expiration.ToShortDateString());
                    }

                    if (cc.IssuerSpecified)
                    {
                        CreatePaymentDetail(order, "Card Type", cc.Issuer.ToString());
                    }

                    if (!string.IsNullOrEmpty(cc.Number))
                    {
                        CreatePaymentDetail(order, "Card Number", cc.Number);
                    }
                }
            }
        }

        /// <summary>
        /// Create a new payment detail entity and populates it
        /// </summary>
        private void CreatePaymentDetail(NetworkSolutionsOrderEntity order, string name, string value)
        {
            OrderPaymentDetailEntity detail = InstantiateOrderPaymentDetail(order);

            detail.Label = name;
            detail.Value = value;
        }

        /// <summary>
        /// Load order charges into ShipWorks
        /// </summary>
        private void LoadOrderCharges(NetworkSolutionsOrderEntity order, OrderType nsOrder)
        {
            decimal amount;

            if (nsOrder.Invoice.Tax != null)
            {
                amount = nsOrder.Invoice.Tax.Amount;
                CreateOrderCharge(order, "Tax", amount);
            }

            if (nsOrder.Invoice.Shipping != null)
            {
                amount = nsOrder.Invoice.Shipping.Value;
                CreateOrderCharge(order, "Shipping", amount);
            }

            if (nsOrder.Invoice.Handling != null)
            {
                amount = nsOrder.Invoice.Handling.Value;
                CreateOrderCharge(order, "Handling", amount);
            }

            if (nsOrder.Invoice.Surcharge != null)
            {
                amount = nsOrder.Invoice.Surcharge.Value;
                CreateOrderCharge(order, "Surcharge", amount);
            }

            // discounts
            if (nsOrder.Invoice.Discount != null)
            {
                amount = nsOrder.Invoice.Discount.Value;
                if (amount > 0)
                {
                    string code = nsOrder.Invoice.DiscountCode;
                    if (string.IsNullOrWhiteSpace(code) && nsOrder.Invoice.OrderDiscountList != null)
                    {
                        var discounts = nsOrder.Invoice.OrderDiscountList.Where(d => d.Applied.Value == amount);
                        if (discounts.Count() == 1)
                        {
                            code = discounts.First().Code;
                        }
                    }

                    CreateOrderCharge(order, "Discount", code, -amount);
                }
            }

            // gift certificates
            if (nsOrder.Invoice.GiftCertificate != null)
            {
                amount = nsOrder.Invoice.GiftCertificate.Value;
                if (amount > 0)
                {
                    CreateOrderCharge(order, "GiftCertificate", nsOrder.Invoice.GiftCertificateCode, -amount);
                }
            }
        }

        /// <summary>
        /// Creates and populates an order charge entity
        /// </summary>
        private void CreateOrderCharge(NetworkSolutionsOrderEntity order, string type, string description, decimal amount)
        {
            OrderChargeEntity charge = InstantiateOrderCharge(order);

            charge.Type = type.ToUpper(CultureInfo.InvariantCulture);
            charge.Amount = amount;

            if (string.IsNullOrWhiteSpace(description))
            {
                description = type;
            }

            charge.Description = description;
        }

        /// <summary>
        /// Creates and populates an order charge entity
        /// </summary>
        private void CreateOrderCharge(NetworkSolutionsOrderEntity order, string type, decimal amount)
        {
            CreateOrderCharge(order, type, type, amount);
        }

        /// <summary>
        /// Load Order Items into ShipWorks
        /// </summary>
        private void LoadOrderItems(NetworkSolutionsOrderEntity order, OrderType nsOrder)
        {
            // Don't crash just because there are no items in this order
            if (nsOrder.Invoice.LineItemList == null)
            {
                return;
            }

            // go through and find all line items on the order
            foreach (LineItemType lineItem in nsOrder.Invoice.LineItemList)
            {
                OrderItemEntity orderItem = InstantiateOrderItem(order);

                orderItem.Code = lineItem.PartNumber;
                orderItem.Name = lineItem.Name;
                orderItem.Quantity = lineItem.QtySold;
                orderItem.UnitPrice = lineItem.UnitPrice.Value;

                decimal pounds = lineItem.Weight.Major;
                decimal ounces = lineItem.Weight.Minor;

                // store in pounds
                orderItem.Weight = Convert.ToDouble(pounds + (ounces / 16.0M));

                LoadOrderItemAttributes(orderItem, lineItem);
            }
        }

        /// <summary>
        /// Loads item variations as order item attributes
        /// </summary>
        private void LoadOrderItemAttributes(OrderItemEntity orderItem, LineItemType lineItem)
        {
            //if (lineItem.SelectedVariationList != null)
            //{
            //    foreach (SelectedVariationType variation in lineItem.SelectedVariationList)
            //    {
            //        OrderItemAttributeEntity attribute = InstantiateOrderItemAttribute(orderItem);

            //        attribute.Name = variation.Group;
            //        attribute.Description = variation.Option;
            //        attribute.UnitPrice = 0;
            //    }
            //}

            IEnumerable<KeyValuePair<string, string>> attributes = BuildVariationList(lineItem.SelectedVariationList)
                .Concat(BuildQuestionAnswerList(lineItem.QuestionList));

            foreach (KeyValuePair<string, string> question in attributes)
            {
                OrderItemAttributeEntity attribute = InstantiateOrderItemAttribute(orderItem);

                attribute.Name = question.Key;
                attribute.Description = question.Value;
                attribute.UnitPrice = 0;
            }
        }

        /// <summary>
        /// Build a list of item variations
        /// </summary>
        private static Dictionary<string, string> BuildVariationList(IEnumerable<SelectedVariationType> variations)
        {
            return variations == null ?
                new Dictionary<string, string>() :
                variations.ToDictionary(x => x.Group, x => x.Option);
        }

        /// <summary>
        /// Build a list of question and their answers
        /// </summary>
        private static Dictionary<string, string> BuildQuestionAnswerList(IEnumerable<QuestionType> questionList)
        {
            return questionList == null ? 
                new Dictionary<string, string>() : 
                questionList.ToDictionary(question => question.Title, BuildAnswerFromQuestion);
        }

        /// <summary>
        /// Build an answer from the given question
        /// </summary>
        private static string BuildAnswerFromQuestion(QuestionType question)
        {
            return question.Items
                .Select(ConvertQuestionItemToBooleanAnswer)
                .Where(x => x.Value)
                .Select(x => x.Answer)
                .Aggregate((x, y) => x + ", " + y);
        }

        /// <summary>
        /// Converts a question item to a boolean answer so it can be handled in a uniform way
        /// </summary>
        private static BooleanAnswerType ConvertQuestionItemToBooleanAnswer(object answer)
        {
            BooleanAnswerType booleanAnswer = answer as BooleanAnswerType;
            if (booleanAnswer != null)
            {
                return booleanAnswer;
            }

            TextAnswerType textAnswer = answer as TextAnswerType;
            return textAnswer != null ? 
                new BooleanAnswerType {Answer = textAnswer.Value, Value = true} : 
                new BooleanAnswerType { Value = false};
        }

        /// <summary>
        /// Load order notes into ShipWorks
        /// </summary>
        private void LoadNotes(NetworkSolutionsOrderEntity order, OrderType nsOrder)
        {
            InstantiateNote(order, nsOrder.Notes, order.OrderDate, NoteVisibility.Public);

            foreach (KeyValuePair<string, string> question in BuildQuestionAnswerList(nsOrder.QuestionList))
            {
                InstantiateNote(order, question.Key + Environment.NewLine + question.Value, order.OrderDate, NoteVisibility.Internal);
            }
        }

        /// <summary>
        /// Populates the shipworks order with address information from the NetSol order.
        /// </summary>
        private void LoadAddressInfo(NetworkSolutionsOrderEntity order, OrderType nsOrder)
        {
            PersonAdapter shipAdapter = new PersonAdapter(order, "Ship");
            PersonAdapter billAdapter = new PersonAdapter(order, "Bill");

            // Fill in the address info
            LoadAddressInfo(shipAdapter, nsOrder.Customer.ShippingAddress);
            LoadAddressInfo(billAdapter, nsOrder.Customer.BillingAddress);

            // email address
            billAdapter.Email = nsOrder.Customer.EmailAddress;

            // fix bad/missing shipping information, take from teh customer record
            if (shipAdapter.FirstName.Length == 0 && shipAdapter.LastName.Length == 0 && shipAdapter.City.Length == 0)
            {
                PersonAdapter.Copy(billAdapter, shipAdapter);
            }
            else
            {
                // if the basic person details look the same, copy the billing email address to shipping
                if (shipAdapter.FirstName == billAdapter.FirstName &&
                    shipAdapter.LastName == billAdapter.LastName &&
                    shipAdapter.Street1 == billAdapter.Street1 &&
                    shipAdapter.PostalCode == billAdapter.PostalCode)
                {
                    shipAdapter.Email = billAdapter.Email;
                }
            }
        }

        /// <summary>
        /// Populate address information
        /// </summary>
        private void LoadAddressInfo(PersonAdapter adapter, AddressType address)
        {
            adapter.NameParseStatus = PersonNameParseStatus.Simple;
            adapter.FirstName = address.FirstName;
            adapter.LastName = address.LastName;
            adapter.Street1 = address.Address1;
            adapter.Street2 = address.Address2;
            adapter.City = address.City;
            adapter.StateProvCode = address.StateProvince;
            adapter.PostalCode = address.PostalCode;
            adapter.CountryCode = address.Country.ToString();
            adapter.Phone = address.Phone;
            adapter.Company = address.Company;
        }
    }
}
