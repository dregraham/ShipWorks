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
using log4net;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Import;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.Overstock
{
    /// <summary>
    /// Downloader for Overstock stores
    /// </summary>
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.Overstock)]
    public class OverstockDownloader : StoreDownloader
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(OverstockDownloader));
        private int totalCount;
        private readonly IOverstockWebClient webClient;
        private readonly OverstockStoreEntity overstockStore;

        /// <summary>
        /// Constructor
        /// </summary>
        public OverstockDownloader(StoreEntity store, IStoreTypeManager storeTypeManager, IOverstockWebClient webClient)
            : base(store, storeTypeManager.GetType(store))
        {
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
                DateTime? lastModified = await GetOnlineLastModifiedStartingPoint().ConfigureAwait(false);
                if (!lastModified.HasValue)
                {
                    lastModified = DateTime.Now.AddHours(-1);
                }

                DateTime endDateTime = DateTime.Now;

                //await GetOrderCount(lastModified.Value, endDateTime).ConfigureAwait(false);

                //if (totalCount == 0)
                //{
                //    Progress.Detail = "No orders to download.";
                //    Progress.PercentComplete = 100;
                //    return;
                //}

                Progress.Detail = $"Downloading orders...";

                TimeSpan offset = TimeSpan.FromHours(1);

                await DownloadOrders(lastModified.Value, endDateTime, offset).ConfigureAwait(false);

                //foreach (DateTime workingDateTime in GetDates(lastModified.Value, endDateTime, offset))
                //{
                //    await DownloadOrders(workingDateTime, workingDateTime.Add(offset.Add(-TimeSpan.FromSeconds(1)))).ConfigureAwait(false);
                //}
            }
            catch (GenericModuleConfigurationException ex)
            {
                string message =
                    "The ShipWorks module returned invalid configuration information. " +
                    $"Please contact the module developer with the following information.\n\n{ex.Message}";

                throw new DownloadException(message, ex);
            }
            catch (Exception ex) when (ex is GenericStoreException || ex is SqlForeignKeyException)
            {
                throw new DownloadException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Returns a sorted set of start DateTimes between a date range, split by a given timespan
        /// </summary>
        private IEnumerable<Range<DateTime>> GetDates(DateTime startDateTime, DateTime endDateTime, TimeSpan timeOffset) =>
            Enumerable.Range(0, int.MaxValue)
                .Select(x => startDateTime.AddHours(x))
                .TakeWhile(x => x <= endDateTime)
                .Select(x => x.To(x.Add(timeOffset).AddSeconds(-1)));


        //    SortedSet<DateTime> dates = new SortedSet<DateTime>();
        //    DateTime tmp = startDateTime;

        //    while (tmp <= endDateTime)
        //    {
        //        dates.Add(tmp);
        //        tmp += timeOffset;
        //    }

        //    return dates;
        //}

        /// <summary>
        /// Downloads the orders.
        /// </summary>
        private async Task DownloadOrders(DateTime initialStart, DateTime initialEnd, TimeSpan offset)
        {
            // keep going until none are left
            foreach (Range<DateTime> downloadRange in GetDates(initialStart, initialEnd, offset))
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

                // update the status
                //                Progress.PercentComplete = Math.Min(100, 100 * QuantitySaved / totalCount);
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
            order.RequestedShipping = orderElement.Element("shippingSpecifications")?.Descendants("shippingServiceLevel")?.First().GetValue("code");

            // Load Address info
            LoadAddressInfo(order, orderElement);

            if (isNew)
            {
                // Notes
                LoadNotes(this, order, orderElement);

                LoadItems(this, order, orderElement.Elements("processedSalesOrderLine"));
            }
            
            order.WarehouseCode = orderElement.Element("warehouseName")?.GetValue("code");
            order.SalesChannelName = orderElement.GetValue("salesChannelName");

            //// CustomerID
            //LoadCustomerIdentifier(order, xpath);

            // Update the total
            order.OrderTotal = OrderUtility.CalculateTotal(order);

            // Save the downloaded order
            SqlAdapterRetry<SqlException> retryAdapter = new SqlAdapterRetry<SqlException>(5, -5, "GenericModuleDownloader.LoadOrder");
            await retryAdapter.ExecuteWithRetryAsync(() => SaveDownloadedOrder(order)).ConfigureAwait(false);

            return true;
        }

        /// <summary>
        /// Instantiate the Overstock order 
        /// </summary>
        protected Task<GenericResult<OrderEntity>> InstantiateOrder(string salcesChannelOrderNumber)
        {
            AlphaNumericOrderIdentifier orderIdentifier = new AlphaNumericOrderIdentifier(salcesChannelOrderNumber);
            return InstantiateOrder(orderIdentifier);
        }

        /// <summary>
        /// Loads Shipping and Billing address information
        /// </summary>
        private static void LoadAddressInfo(OrderEntity order, XElement orderElement)
        {
            LoadAddress(order.ShipPerson, orderElement.Element("shipToAddress"));
            LoadAddress(order.BillPerson, orderElement.Element("returnAddress"));
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

            //person.ResidentialStatus = ;

            person.Phone = address.GetValue("phone");
            person.Fax = string.Empty;
            person.Email = string.Empty;
            person.Website = string.Empty;
        }

        /// <summary>
        /// Loads notes 
        /// </summary>
        private static void LoadNotes(IOrderElementFactory factory, OrderEntity order, XElement orderElement)
        {
            foreach (var note in orderElement.Elements("processedSalesOrderLine").Select(n => n.GetValue("notes")))
            {
                if (!note.IsNullOrWhiteSpace())
                {
                    factory.CreateNote(order, note, order.OrderDate, NoteVisibility.Internal);
                }
            }
        }

        /// <summary>
        /// Load items
        /// </summary>
        private void LoadItems(IOrderElementFactory elementFactory, OrderEntity orderEntity, IEnumerable<XElement> items)
        {
            foreach (var item in items)
            {
                var itemEntity = elementFactory.CreateItem(orderEntity);
                itemEntity.SKU = item.GetValue("salesChannelSKU");
                itemEntity.Name = item.GetValue("itemName");
                itemEntity.UnitCost = item.GetDecimal("unitCost");
                itemEntity.UnitPrice = item.GetDecimal("itemPrice");
                itemEntity.UPC = item.GetValue("upc");
                itemEntity.Code = item.GetValue("barcode");
            }
        }
    }
}
