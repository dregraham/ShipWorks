using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Stores.Communication;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data;
using ShipWorks.Data.Model.HelperClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using log4net;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Platforms.PayPal.WebServices;
using ShipWorks.Stores.Content;
using System.Data.SqlTypes;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.Win32;

namespace ShipWorks.Stores.Platforms.PayPal
{
    /// <summary>
    /// Order downloader for PayPal.
    /// </summary>
    public class PayPalDownloader : StoreDownloader
    {
        const int maxIntialDownload = 365;

        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(PayPalDownloader));

        /// <summary>
        /// Convenience property for quick access to the specific store entity
        /// </summary>
        private PayPalStoreEntity PayPalStore
        {
            get { return (PayPalStoreEntity)Store; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public PayPalDownloader(StoreEntity store)
            : base(store)
        {
        }

        /// <summary>
        /// Download orders from PayPal
        /// </summary>
        protected override void Download()
        {
            try
            {
                Progress.Detail = "Searching for PayPal Transactions...";

                PayPalWebClient client = new PayPalWebClient(new PayPalAccountAdapter(Store, ""));

                // find the maximum date we can download to
                DateTime rangeEnd = client.GetPayPalTime();

                // get the date range start
                DateTime? startDate = GetOrderDateStartingPoint();
                if (!startDate.HasValue)
                {
                    // need to start from today - maxIntialDownload
                    startDate = rangeEnd.AddDays(-maxIntialDownload);

                    // reset the LastTransactionDate and LastValidTransactionDate fields
                    PayPalStore.LastTransactionDate = SqlDateTime.MinValue.Value;
                    PayPalStore.LastValidTransactionDate = SqlDateTime.MinValue.Value;
                    SaveStore();
                }
                else
                {
                    if (startDate.Value == PayPalStore.LastValidTransactionDate)
                    {
                        // the most recent order has not been deleted from the database,
                        // so we can take the date of the last transaction seen
                        startDate = PayPalStore.LastTransactionDate;
                    }

                    // Add a second so we don't download hte last ones over and over
                    startDate = startDate.Value.AddSeconds(1);
                }

                // safety check, don't let it go back further than we allow
                DateTime rangeCap = rangeEnd.AddDays(-maxIntialDownload);
                if (rangeCap > startDate) startDate = rangeCap;

                DownloadTransactions(client, startDate.Value, rangeEnd);

                Progress.Detail = "Done";
            }
            catch (PayPalException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
            catch (SqlForeignKeyException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Download PayPal transactiosn that occur in the given date range
        /// </summary>
        private void DownloadTransactions(PayPalWebClient client, DateTime rangeStart, DateTime rangeEnd)
        {
            List<PaymentTransactionSearchResultType> transactions = client.GetTransactions(rangeStart, rangeEnd, true);

            if (Progress.IsCancelRequested)
            {
                return;
            }

            if (transactions.Count == 0)
            {
                Progress.Detail = "No orders to download.";
                Progress.PercentComplete = 100;
                return;
            }

            Progress.Detail = string.Format("Downloading {0} PayPal transactions...", transactions.Count);

            int counter = 0;
            foreach (PaymentTransactionSearchResultType transaction in transactions)
            {
                counter++;

                Progress.Detail = string.Format("Processing transaction {0} of {1}...", counter, transactions.Count);

                try
                {
                    LoadTransaction(client, transaction.TransactionID);
                }
                catch (PayPalException ex)
                {
                    // PayPal doesn't document the transaction types we can't get the detials on, so we're just ignoring
                    // transactions that cause these errors
                    if (ex.Errors.Any( e => e.Code == "10007" || e.Code == "10004"))
                    {
                        // just ignoring these errors
                        log.ErrorFormat("Caught and ignoring exception while loading PayPal transaction '{0}'." , transaction.TransactionID);

                        // mark that we saw this transaction
                        if (transaction.Timestamp > PayPalStore.LastTransactionDate)
                        {
                            PayPalStore.LastTransactionDate = transaction.Timestamp;
                            SaveStore();
                        }

                    }
                    else if (ex.Errors.Any(e => e.Code == "10001"))
                    {
                        // known paypal issue where retrying often results in success
                        log.Error("Internal Error reported by PayPal.");
                        throw new DownloadException("PayPal reported an Internal Error. Wait a moment and try to download again.", ex);
                    }
                    else
                    {
                        throw new DownloadException(ex.Message, ex);
                    }
                }

                // update the status, 100 max
                Progress.PercentComplete = Math.Min(100 * counter / transactions.Count, 100);
            }
        }


        /// <summary>
        /// Downloads the specified transaction from PayPal and creates a ShipWorks order
        /// </summary>
        private void LoadTransaction(PayPalWebClient client, string transactionID)
        {
            log.InfoFormat("Preparing to load PayPal transaction '{0}'.", transactionID);

            PaymentTransactionType transaction = client.GetTransaction(transactionID);

            if (ShouldImportTransaction(transaction))
            {
                PayPalOrderEntity order = (PayPalOrderEntity)InstantiateOrder(new PayPalOrderIdentifier(transactionID));

                order.TransactionID = transactionID;
                order.PayPalFee = GetAmount(transaction.PaymentInfo.FeeAmount);
                order.PaymentStatus = (int)PayPalUtility.GetPaymentStatus(transaction.PaymentInfo.PaymentStatus);

                // Load Address info
                LoadAddressInfo(order, transaction);

                // only do the remainder for new orders
                if (order.IsNew)
                {
                    order.OrderDate = transaction.PaymentInfo.PaymentDate.ToUniversalTime();
                    order.RequestedShipping = transaction.PaymentInfo.ShippingMethod == null ? "" : transaction.PaymentInfo.ShippingMethod;

                    // no customer ids
                    order.OnlineCustomerID = null;

                    InstantiateNote(order, transaction.PaymentItemInfo.Memo, order.OrderDate, NoteVisibility.Public);

                    LoadItems(order, transaction);

                    LoadCharges(order, transaction);

                    LoadPayments(order, transaction);

                    order.OrderTotal = OrderUtility.CalculateTotal(order);

                    // assign an order number
                    order.OrderNumber = GetNextOrderNumber();
                }

                SqlAdapterRetry<SqlException> retryAdapter = new SqlAdapterRetry<SqlException>(5, -5, "PayPalDownloader.LoadOrder");
                retryAdapter.ExecuteWithRetry(() => SaveDownloadedOrder(order));

                // update the last valid transaction date field
                if (PayPalStore.LastValidTransactionDate < transaction.PaymentInfo.PaymentDate.ToUniversalTime())
                {
                    log.InfoFormat("Updating the store's LastValidTransactionDate to '{0}'.", transaction.PaymentInfo.PaymentDate.ToUniversalTime());
                    PayPalStore.LastValidTransactionDate = transaction.PaymentInfo.PaymentDate.ToUniversalTime();
                }
            }
            else
            {
                log.InfoFormat("PayPal transaction '{0}' is of type '{1}', which are ignored.", transactionID, transaction.PaymentInfo.TransactionType);
            }

            // update the paypallasttransactiondate field
            if (PayPalStore.LastTransactionDate < transaction.PaymentInfo.PaymentDate.ToUniversalTime())
            {
                log.InfoFormat("Updating the store's LastTransactionDate to '{0}'.", transaction.PaymentInfo.PaymentDate.ToUniversalTime());
                PayPalStore.LastTransactionDate = transaction.PaymentInfo.PaymentDate.ToUniversalTime();
            }

            // save the store if changes were made
            SaveStore();
            
            // if this transaction has a parent transaction, load it too
            if (transaction.PaymentInfo.ParentTransactionID != null && transaction.PaymentInfo.ParentTransactionID.Length > 0)
            {
                log.InfoFormat("PayPay transaction '{0}' has a parent transaction '{0}' that must also be downloaded.", transactionID, transaction.PaymentInfo.ParentTransactionID);
                LoadTransaction(client, transaction.PaymentInfo.ParentTransactionID);
            }
        }

        /// <summary>
        /// Loads payment information from the paypal transaction
        /// </summary>
        private void LoadPayments(PayPalOrderEntity order, PaymentTransactionType transaction)
        {
            string paymentType = "";
            switch (transaction.PaymentInfo.PaymentType)
            {
                case PaymentCodeType.echeck:
                    paymentType = "eCheck";
                    break;
                case PaymentCodeType.instant:
                    paymentType = "instant";
                    break;
                case PaymentCodeType.none:
                default:
                    break;
            }

            if (paymentType.Length > 0)
            {
                OrderPaymentDetailEntity detail = InstantiateOrderPaymentDetail(order);
                detail.Label = "Payment Type";
                detail.Value = paymentType;
            }
        }

        /// <summary>
        /// Creates a ShipWorks Order Charge for an order
        /// </summary>
        private void LoadCharge(PayPalOrderEntity order, string type, string description, decimal value)
        {
            OrderChargeEntity charge = InstantiateOrderCharge(order);

            charge.Type = type;
            charge.Description = description;
            charge.Amount = value;
        }

        /// <summary>
        /// Loads order charges from the PayPal transaction
        /// </summary>
        private void LoadCharges(PayPalOrderEntity order, PaymentTransactionType transaction)
        {
            // Tax
            decimal taxAmount = GetAmount(transaction.PaymentInfo.TaxAmount);
            if (taxAmount > 0)
            {
                LoadCharge(order, "TAX", "Tax", taxAmount);
            }

            // calculate the "other" amount, which includes shipping and other (discounts, etc)
            decimal otherAmount = GetAmount(transaction.PaymentInfo.GrossAmount) - taxAmount;
            foreach (OrderItemEntity orderItem in order.OrderItems)
            {
                otherAmount = otherAmount - (orderItem.UnitPrice * (decimal)orderItem.Quantity);
            }

            // other
            if (otherAmount > 0)
            {
                LoadCharge(order, "OTHER", "Other", otherAmount);
            }
        }

        /// <summary>
        /// Loads Order Items from the PayPal transaction
        /// </summary>
        private void LoadItems(PayPalOrderEntity order, PaymentTransactionType transaction)
        {
            if (transaction.PaymentItemInfo.PaymentItem != null)
            {
                foreach (PaymentItemType paymentItem in transaction.PaymentItemInfo.PaymentItem)
                {
                    // SendMoney transactions don't inlclude items
                    if (paymentItem.Name.Length == 0 && paymentItem.Amount == null)
                    {
                        continue;
                    }

                    // create the order item 
                    OrderItemEntity item = InstantiateOrderItem(order);

                    // fill it 
                    item.Name = paymentItem.Name;
                    item.Code = paymentItem.Number;
                    item.Quantity = Parse(paymentItem.Quantity, 1);
                    item.UnitPrice = GetAmount(paymentItem.Amount) / Math.Max(1, Convert.ToDecimal(item.Quantity));

                    if (paymentItem.Options != null)
                    {
                        LoadOptions(item, paymentItem.Options);
                    }
                }
            }
        }

        /// <summary>
        /// Loads Order Item Options from the paypal transaction
        /// </summary>
        private void LoadOptions(OrderItemEntity item, OptionType[] options)
        {
            foreach (OptionType optionType in options)
            {
                OrderItemAttributeEntity option = InstantiateOrderItemAttribute(item);

                option.Name = optionType.name;
                option.Description = optionType.value;
                option.UnitPrice = 0;
            }
        }

        /// <summary>
        /// Loads billing and shipping address information from the PayPal transaction
        /// </summary>
        private void LoadAddressInfo(PayPalOrderEntity order, PaymentTransactionType transaction)
        {
            // address verified
            order.AddressStatus = (int)PayPalUtility.GetAddressStatus(transaction.PayerInfo.Address.AddressStatus);

            // general Bill info
            order.BillCompany = transaction.PayerInfo.PayerBusiness;
            order.BillEmail = transaction.PayerInfo.Payer;
            order.BillPhone = transaction.PayerInfo.ContactPhone;
            order.BillFirstName = transaction.PayerInfo.PayerName.FirstName;
            order.BillLastName = transaction.PayerInfo.PayerName.LastName;
            order.BillNameParseStatus = (int)PersonNameParseStatus.Simple;

            // Bill address
            order.BillStreet1 = transaction.PayerInfo.Address.Street1;
            order.BillStreet2 = transaction.PayerInfo.Address.Street2;
            order.BillCity = transaction.PayerInfo.Address.CityName;
            order.BillStateProvCode = Geography.GetStateProvCode(transaction.PayerInfo.Address.StateOrProvince);
            order.BillPostalCode = transaction.PayerInfo.Address.PostalCode;

            if (transaction.PayerInfo.Address.CountrySpecified)
            {
                order.BillCountryCode = transaction.PayerInfo.Address.Country.ToString(); // enum w/ 2-char codes
            }
            else
            {
                // default it to US
                order.BillCountryCode = "US";
            }
            
            // copy billing to shipping
            PersonAdapter.Copy(order, "Bill", order, "Ship");

            // see if there is a shipping name, if so, use it
            if (!String.IsNullOrEmpty(transaction.PayerInfo.Address.Name))
            {
                PersonName personName = PersonName.Parse(transaction.PayerInfo.Address.Name);
                order.ShipFirstName = personName.First;
                order.ShipMiddleName = personName.Middle;
                order.ShipLastName = personName.Last;
                order.ShipNameParseStatus = (int)personName.ParseStatus;
                order.ShipUnparsedName = personName.UnparsedName;

                // if billing/shipping name isn't the same, don't carry BillCompany over
                if (string.Compare(order.ShipFirstName, order.BillFirstName, StringComparison.OrdinalIgnoreCase) != 0 ||
                    string.Compare(order.ShipLastName, order.ShipLastName, StringComparison.OrdinalIgnoreCase) != 0)
                {
                    order.ShipCompany = "";
                }
            }
        }

        /// <summary>
        /// Determines if a certain transaction should be imported
        /// </summary>
        private bool ShouldImportTransaction(PaymentTransactionType transaction)
        {
            PaymentTransactionCodeType code = transaction.PaymentInfo.TransactionType;
            switch (code)
            {
                case PaymentTransactionCodeType.credit:
                case PaymentTransactionCodeType.masspay:
                case PaymentTransactionCodeType.merchtpmt:
                case PaymentTransactionCodeType.none:
                case PaymentTransactionCodeType.subscrcancel:
                case PaymentTransactionCodeType.subscreot:
                case PaymentTransactionCodeType.subscrfailed:
                case PaymentTransactionCodeType.subscrmodify:
                case PaymentTransactionCodeType.subscrpayment:
                    return false;

                default:
                    return true;
            }
        }

        /// <summary>
        /// Converts a PayPal Amount Type to a value
        /// </summary>
        private decimal GetAmount(BasicAmountType amountType)
        {
            if (amountType == null || amountType.Value.Length == 0)
            {
                return 0M;
            }
            else
            {
                try
                {
                    return Convert.ToDecimal(amountType.Value);
                }
                catch(FormatException)
                {
                    return 0M;
                }
            }
        }

        /// <summary>
        /// Parses a double string into its typed value
        /// </summary>
        private double Parse(string toParse, double defaultValue)
        {
            double temp = 0;
            if (double.TryParse(toParse, out temp))
            {
                return temp;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Saves changes to the store if necessary
        /// </summary>
        private void SaveStore()
        {
            if (PayPalStore.IsDirty)
            {
                using (SqlAdapter adapter = new SqlAdapter())
                {
                    adapter.SaveAndRefetch(PayPalStore);
                }
            }
        }
    }
}
