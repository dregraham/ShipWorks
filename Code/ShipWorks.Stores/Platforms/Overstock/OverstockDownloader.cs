using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Administration.Recovery;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Import;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores.Platforms.Overstock
{
    /// <summary>
    /// Downloader for Overstock stores
    /// </summary>
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.Overstock)]
    public class OverstockDownloader : StoreDownloader
    {
        private readonly IOverstockWebClient webClient;
        private readonly OverstockStoreEntity overstockStore;
        private readonly IDownloadStartingPoint downloadStartingPoint;

        /// <summary>
        /// Constructor
        /// </summary>
        public OverstockDownloader(StoreEntity store,
            IStoreTypeManager storeTypeManager,
            IOverstockWebClient webClient,
            IDownloadStartingPoint downloadStartingPoint)
            : base(store, storeTypeManager.GetType(store))
        {
            this.downloadStartingPoint = downloadStartingPoint;
            this.webClient = webClient;
            overstockStore = (OverstockStoreEntity) store;
        }

        /// <summary>
        /// Download
        /// </summary>
        protected override async Task Download(TrackedDurationEvent trackedDurationEvent)
        {
            try
            {
                Progress.Detail = "Checking for orders...";

                // Downloading based on the last modified time
                DateTime? lastModified = await GetOrderStartingPoint().ConfigureAwait(false);
                if (!lastModified.HasValue)
                {
                    lastModified = DateTime.UtcNow.AddDays(-30);
                }

                Progress.Detail = $"Downloading orders...";

                TimeSpan offset = TimeSpan.FromHours(1);

                await DownloadOrders(lastModified.Value, DateTime.UtcNow, offset).ConfigureAwait(false);
            }
            catch (Exception ex) when (ex is OverstockException || ex is SqlForeignKeyException)
            {
                throw new DownloadException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Returns a sorted set of start DateTimes between a date range, split by a given timespan
        /// </summary>
        private IEnumerable<Range<DateTime>> GetDates(DateTime startDateTime, DateTime endDateTime, TimeSpan timeOffset) =>
            Enumerable.Range(0, int.MaxValue)
                .Select(x => timeOffset.MultiplyBy(x))
                .Select(offset => startDateTime.Add(offset).AddSeconds(1))
                .TakeWhile(start => start <= endDateTime)
                .Select(ChangeTimeZoneTo(TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time")))
                .Select(start => start.To(start.Add(timeOffset)));

        /// <summary>
        /// Downloads the orders.
        /// </summary>
        private async Task DownloadOrders(DateTime initialStart, DateTime initialEnd, TimeSpan offset)
        {
            // keep going until none are left
            foreach (var downloadRange in GetDates(initialStart, initialEnd, offset))
            {
                // Check if it has been canceled
                if (Progress.IsCancelRequested)
                {
                    return;
                }

                var result = await webClient.GetOrders(overstockStore, downloadRange);
                IEnumerable<XElement> orderElements = result?.Root?.Elements("list");

                if (orderElements?.Any() == true)
                {
                    await LoadOrders(orderElements).ConfigureAwait(false);
                }
            }

            Progress.Detail = "Done";
        }

        /// <summary>
        /// Imports the orders contained in the iterator
        /// </summary>
        private async Task<bool> LoadOrders(IEnumerable<XElement> orderElements)
        {
            bool anyProcessed = false;

            // go through each order in the batch
            foreach (var order in orderElements)
            {
                // check for cancel again
                if (Progress.IsCancelRequested)
                {
                    return false;
                }

                // Update the status
                Progress.Detail = string.Format("Processing order {0}...", (QuantitySaved + 1));

                anyProcessed |= await LoadOrder(order).ConfigureAwait(false);
            }

            return anyProcessed;
        }

        /// <summary>
        /// Extract the order from the xml
        /// </summary>
        private async Task<bool> LoadOrder(XElement orderElement)
        {
            string salesChannelOrderNumber = orderElement.GetValue("salesChannelOrderNumber");

            GenericResult<OrderEntity> result = await InstantiateOrder(salesChannelOrderNumber).ConfigureAwait(false);
            if (result.Failure)
            {
                return false;
            }

            OverstockOrderEntity order = result.Value as OverstockOrderEntity;
            var isNew = order.IsNew;

            // last modified
            order.OnlineLastModified = DateTime.Parse(orderElement.GetValue("orderDate"));

            // If Parse can tell what timezone it's in, it automatically converts it to local.  We need UTC.
            if (order.OnlineLastModified.Kind == DateTimeKind.Local)
            {
                order.OnlineLastModified = order.OnlineLastModified.ToUniversalTime();
            }

            order.OrderDate = order.OnlineLastModified;
            order.OnlineStatus = orderElement.GetValue("status");
            order.RequestedShipping = orderElement.Element("shippingSpecifications")?.Descendants("shippingServiceLevel")?.First().GetValue("code");

            // Load Address info
            LoadAddressInfo(order, orderElement);

            if (isNew)
            {
                var lineItems = orderElement.Elements("processedSalesOrderLine");

                await LoadNotes(this, order, lineItems).ConfigureAwait(false);
                LoadItems(this, order, lineItems);
            }

            order.WarehouseCode = orderElement.Element("warehouseName")?.GetValue("code");
            order.SalesChannelName = orderElement.GetValue("salesChannelName");
            order.SofsCreatedDate = TimeZoneInfo.ConvertTimeToUtc(orderElement.GetDate("sofsCreatedDate", DateTime.Now));

            // Update the total
            order.OrderTotal = OrderUtility.CalculateTotal(order);

            // Save the downloaded order
            SqlAdapterRetry<SqlException> retryAdapter = new SqlAdapterRetry<SqlException>(5, -5, "OverstockDownloader.LoadOrder");
            await retryAdapter.ExecuteWithRetryAsync(() => SaveDownloadedOrder(order)).ConfigureAwait(false);

            return true;
        }

        /// <summary>
        /// Instantiate the Overstock order 
        /// </summary>
        protected Task<GenericResult<OrderEntity>> InstantiateOrder(string salesChannelOrderNumber)
        {
            AlphaNumericOrderIdentifier orderIdentifier = new AlphaNumericOrderIdentifier(salesChannelOrderNumber);
            return InstantiateOrder(orderIdentifier);
        }

        /// <summary>
        /// Loads Shipping and Billing address information
        /// </summary>
        private static void LoadAddressInfo(OrderEntity order, XElement orderElement)
        {
            LoadAddress(order.ShipPerson, orderElement.Element("shipToAddress"));
            LoadAddress(order.BillPerson, orderElement.Element("shipToAddress"));
        }

        /// <summary>
        /// Loads the Billing or Shipping address detail into the order entity, depending on the 
        /// prefix specified by the caller.
        /// </summary>
        private static void LoadAddress(PersonAdapter person, XElement address)
        {
            // FullName must be sent, or FirstName/MiddleName/LastName
            string fullName = address.GetValue("contactName");

            // parse the name for its parts
            person.ParsedName = PersonName.Parse(fullName);

            person.Company = string.Empty;
            person.Street1 = address.GetValue("address1");
            person.Street2 = address.GetValue("address2");
            person.Street3 = string.Empty;

            person.City = address.GetValue("city");
            person.StateProvCode = Geography.GetStateProvCode(address.GetValue("stateOrProvince"));
            person.PostalCode = address.GetValue("postalCode");
            person.CountryCode = Geography.GetCountryCode(address.GetValue("countryCode"));

            person.Phone = address.GetValue("phone");
            person.Fax = string.Empty;
            person.Email = string.Empty;
            person.Website = string.Empty;
        }

        /// <summary>
        /// Loads notes 
        /// </summary>
        private static async Task LoadNotes(IOrderElementFactory factory, OrderEntity order, IEnumerable<XElement> items)
        {
            foreach (var note in GetNotesFromItems(items))
            {
                await factory.CreateNote(order, note, order.OrderDate, NoteVisibility.Internal).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Load items
        /// </summary>
        private void LoadItems(IOrderElementFactory elementFactory, OrderEntity orderEntity, IEnumerable<XElement> items)
        {
            foreach (var item in items)
            {
                var itemEntity = (OverstockOrderItemEntity) elementFactory.CreateItem(orderEntity);
                itemEntity.SalesChannelLineNumber = item.GetLong("salesChannelLineNumber", 0);
                itemEntity.SKU = item.GetValue("salesChannelSKU");
                itemEntity.Name = item.GetValue("itemName");
                itemEntity.UnitCost = item.GetDecimal("unitCost");
                itemEntity.UnitPrice = item.GetDecimal("itemPrice");
                itemEntity.UPC = item.GetValue("upc");
                itemEntity.Code = item.GetValue("barcode");
                itemEntity.Quantity = item.GetDouble("quantity");
            }
        }

        /// <summary>
        /// Get a list of notes from a list of items
        /// </summary>
        private static IEnumerable<string> GetNotesFromItems(IEnumerable<XElement> items) =>
            items.Select(n => n.GetValue("notes")).Where(x => !x.IsNullOrWhiteSpace());

        /// <summary>
        /// Change the time zone of the given date to the specific time zone
        /// </summary>
        private Func<DateTime, DateTime> ChangeTimeZoneTo(TimeZoneInfo timeZone) =>
            (DateTime input) => TimeZoneInfo.ConvertTimeFromUtc(input, timeZone);

        /// <summary>
        /// Gets the largest SOFS created time we have in our database for non-manual orders for this store.
        /// If no such orders exist, and there is an initial download policy, that policy is applied.  Otherwise null is returned.
        /// </summary>
        private Task<DateTime?> GetOrderStartingPoint() =>
            downloadStartingPoint.CustomDate(Store, OverstockOrderFields.SofsCreatedDate);
    }
}
