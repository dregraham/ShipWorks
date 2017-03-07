using System;
using System.Linq;
using Interapptive.Shared.Business;
using Interapptive.Shared.Metrics;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Walmart.DTO;

namespace ShipWorks.Stores.Platforms.Walmart
{
    [KeyedComponent(typeof(StoreDownloader), StoreTypeCode.Walmart, ExternallyOwned = true)]
    public class WalmartDownloader : StoreDownloader
    {
        private readonly IWalmartWebClient walmartWebClient;
        private readonly WalmartStoreEntity walmartStore;

        public WalmartDownloader(StoreEntity store, IWalmartWebClient walmartWebClient)
            : base(store)
        {
            this.walmartWebClient = walmartWebClient;
            walmartStore = store as WalmartStoreEntity;
        }

        protected override void Download(TrackedDurationEvent trackedDurationEvent)
        {
            ordersListType ordersList = GetFirstBatch();

            while (ordersList.elements?.Any() ?? false)
            {
                SaveOrders(ordersList);
                ordersList = GetNextBatch(ordersList);
            }
        }

        private ordersListType GetFirstBatch()
        {
            DateTime startTime = GetDownloadStartingPoint();
            return walmartWebClient.GetOrders(walmartStore, startTime);
        }

        private ordersListType GetNextBatch(ordersListType ordersList)
        {
            string nextCursor = ordersList.meta.nextCursor;
            return walmartWebClient.GetOrders(walmartStore, nextCursor);
        }

        private void SaveOrders(ordersListType ordersList)
        {
            foreach (Order downloadedOrder in ordersList.elements)
            {
                LoadOrder(downloadedOrder);
            }
        }

        private void LoadOrder(Order downloadedOrder)
        {
            long orderNumber;
            if (!long.TryParse(downloadedOrder.customerOrderId, out orderNumber))
            {
                throw new WalmartException($"CustomerOrderId '{downloadedOrder.customerOrderId}' could not be converted to an integer");
            }

            WalmartOrderEntity orderToSave =
                (WalmartOrderEntity) InstantiateOrder(new OrderNumberIdentifier(orderNumber));

            orderToSave.CustomerOrderID = downloadedOrder.customerOrderId;
            orderToSave.PurchaseOrderID = downloadedOrder.purchaseOrderId;
            orderToSave.OrderDate = downloadedOrder.orderDate;
            orderToSave.EstimatedDeliveryDate = downloadedOrder.shippingInfo.estimatedDeliveryDate;
            orderToSave.EstimatedShipDate = downloadedOrder.shippingInfo.estimatedShipDate;
            orderToSave.RequestedShipping = downloadedOrder.shippingInfo.methodCode.ToString();

            LoadAddress(downloadedOrder, orderToSave);
            LoadItems(downloadedOrder.orderLines, orderToSave);
            LoadTax(downloadedOrder.orderLines, orderToSave);
            LoadRefunds(downloadedOrder.orderLines, orderToSave);
        }

        private void LoadTax(orderLineType[] orderLines, WalmartOrderEntity orderToSave)
        {
            var taxCharges = orderLines.SelectMany(orderLine => orderLine.charges)
                .Where(c => c.tax != null)
                .GroupBy(c => c.tax.taxName, c => c.tax.taxAmount)
                .Select(taxGroup => new {Name = taxGroup.Key, Value = taxGroup.Sum(g => g.amount)});

            foreach (var taxCharge in taxCharges)
            {
                InstantiateOrderCharge(orderToSave, "Tax", taxCharge.Name, taxCharge.Value);
            }
        }

        private void LoadRefunds(orderLineType[] orderLines, WalmartOrderEntity orderToSave)
        {
            var refunds = orderLines.Select(orderLine => orderLine.refund)
                .Where(refund => refund != null)
                .SelectMany(refund=>refund.refundCharges)
                .GroupBy(c => c.refundReason, c => new { c.charge.chargeAmount, c.charge.tax?.taxAmount.amount})
                .Select(refundGroup => new { Name = refundGroup.Key, TotalRefund =  refundGroup.Sum(group=>group.chargeAmount.amount), TotalTaxRefund = refundGroup.Sum(group=>group.amount.GetValueOrDefault(0)) });

            foreach (var refund in refunds)
            {
                InstantiateOrderCharge(orderToSave, "Refund", refund.Name.ToString(), refund.TotalRefund);
                InstantiateOrderCharge(orderToSave, "Refunded Tax", refund.Name.ToString(), refund.TotalTaxRefund);
            }
        }

        private void LoadItems(orderLineType[] downloadedOrderOrderLines, WalmartOrderEntity orderToSave)
        {
            foreach (orderLineType orderLine in downloadedOrderOrderLines)
            {
                LoadItem(orderLine, orderToSave);
            }
        }

        private void LoadItem(orderLineType orderLine, WalmartOrderEntity orderToSave)
        {
            WalmartOrderItemEntity item = (WalmartOrderItemEntity) InstantiateOrderItem(orderToSave);

            item.LineNumber = orderLine.lineNumber;
            item.Name = orderLine.item.productName;
            item.SKU = orderLine.item.sku;
            item.UnitPrice = orderLine.charges.Sum(c => c.chargeAmount.amount);
            item.Quantity = double.Parse(orderLine.orderLineQuantity.amount);

            orderLineStatusType orderLineStatus = orderLine.orderLineStatuses.SingleOrDefault();
            item.LocalStatus = orderLineStatus?.status.ToString() ?? "Unknown";
        }

        private static void LoadAddress(Order downloadedOrder, WalmartOrderEntity orderToSave)
        {
            postalAddressType downloadedAddress = downloadedOrder.shippingInfo.postalAddress;

            PersonName name = PersonName.Parse(downloadedAddress.name);
            orderToSave.BillFirstName = name.First;
            orderToSave.BillLastName = name.LastWithSuffix;
            orderToSave.BillMiddleName = name.Middle;
            orderToSave.BillNameParseStatus = (int) name.ParseStatus;
            orderToSave.BillUnparsedName = name.UnparsedName;

            orderToSave.BillStreet1 = downloadedAddress.address1;
            orderToSave.BillStreet2 = downloadedAddress.address2;
            orderToSave.BillCity = downloadedAddress.city;
            orderToSave.BillStateProvCode = downloadedAddress.state;
            orderToSave.BillPostalCode = downloadedAddress.postalCode;
            orderToSave.BillCountryCode = downloadedAddress.country;

            orderToSave.BillPhone = downloadedOrder.shippingInfo.phone;
            orderToSave.BillEmail = downloadedOrder.customerEmailId;

            PersonAdapter billAdapter = new PersonAdapter(orderToSave, "Bill");
            PersonAdapter shipAdapter = new PersonAdapter(orderToSave, "Ship");
            PersonAdapter.Copy(billAdapter, shipAdapter);
        }

        private DateTime GetDownloadStartingPoint()
        {
            throw new NotImplementedException();
        }
    }
}
