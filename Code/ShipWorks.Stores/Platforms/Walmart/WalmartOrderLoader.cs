using System.Linq;
using System.Net;
using Interapptive.Shared.Business;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Walmart.DTO;

namespace ShipWorks.Stores.Platforms.Walmart
{
    /// <summary>
    /// Populates a Walmart order from a downloaded walmart order.
    /// </summary>
    [Component]
    public class WalmartOrderLoader : IWalmartOrderLoader
    {
        private readonly IOrderChargeCalculator orderChargeCalculator;
        private readonly IOrderRepository orderRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="WalmartOrderLoader"/> class.
        /// </summary>
        public WalmartOrderLoader(IOrderChargeCalculator orderChargeCalculator, IOrderRepository orderRepository)
        {
            this.orderChargeCalculator = orderChargeCalculator;
            this.orderRepository = orderRepository;
        }

        /// <summary>
        /// Loads the order.
        /// </summary>
        public void LoadOrder(Order downloadedOrder, WalmartOrderEntity orderToSave)
        {
            if (orderToSave.IsNew)
            {
                orderToSave.CustomerOrderID = downloadedOrder.customerOrderId;
                orderToSave.PurchaseOrderID = downloadedOrder.purchaseOrderId;
                orderToSave.OrderDate = downloadedOrder.orderDate;
                orderToSave.EstimatedDeliveryDate = downloadedOrder.shippingInfo.estimatedDeliveryDate;
                orderToSave.EstimatedShipDate = downloadedOrder.shippingInfo.estimatedShipDate;
                orderToSave.RequestedShipping = downloadedOrder.shippingInfo.methodCode.ToString();

                LoadAddress(downloadedOrder, orderToSave);
            }
            else
            {
                ClearExistingCharges(orderToSave);
            }

            LoadItems(downloadedOrder.orderLines, orderToSave);
            LoadOtherCharges(downloadedOrder.orderLines, orderToSave);
            LoadTax(downloadedOrder.orderLines, orderToSave);
            LoadRefunds(downloadedOrder.orderLines, orderToSave);

            orderToSave.OrderTotal = orderChargeCalculator.CalculateTotal(orderToSave);
        }


        /// <summary>
        /// Clears the existing charges.
        /// </summary>
        private void ClearExistingCharges(WalmartOrderEntity orderToSave)
        {
            orderRepository.PopulateOrderDetails(orderToSave);
            foreach (OrderChargeEntity charge in orderToSave.OrderCharges)
            {
                charge.Amount = 0;
            }
        }

        /// <summary>
        /// Loads the other charges.
        /// </summary>
        private void LoadOtherCharges(orderLineType[] downloadedOrderOrderLines, WalmartOrderEntity orderToSave)
        {
            var otherCharges = downloadedOrderOrderLines.SelectMany(orderLine => orderLine.charges)
                .Where(charge => charge.chargeType1 != "PRODUCT" && charge.chargeAmount.amount != 0)
                .GroupBy(charge => new { charge.chargeType1, charge.chargeName }, charge => charge.chargeAmount.amount)
                .Select(chargeGroup => new { Type = chargeGroup.Key.chargeType1, Name = chargeGroup.Key.chargeName, Amount = chargeGroup.Sum() });

            foreach (var otherCharge in otherCharges)
            {
                InstantiateOrderCharge(orderToSave, otherCharge.Type, otherCharge.Name, otherCharge.Amount);
            }
        }

        /// <summary>
        /// Loads the tax.
        /// </summary>
        private void LoadTax(orderLineType[] orderLines, WalmartOrderEntity orderToSave)
        {
            var taxCharges = orderLines.SelectMany(orderLine => orderLine.charges)
                .Where(c => c.tax != null && c.tax.taxAmount.amount != 0)
                .GroupBy(c => c.tax.taxName, c => c.tax.taxAmount.amount)
                .Select(taxGroup => new { Name = taxGroup.Key, Value = taxGroup.Sum() });

            foreach (var taxCharge in taxCharges)
            {
                InstantiateOrderCharge(orderToSave, "Tax", taxCharge.Name, taxCharge.Value);
            }
        }

        /// <summary>
        /// Loads the refunds.
        /// </summary>
        private void LoadRefunds(orderLineType[] orderLines, WalmartOrderEntity orderToSave)
        {
            var refunds = orderLines.Select(orderLine => orderLine.refund)
                .Where(refund => refund != null)
                .SelectMany(refund => refund.refundCharges)
                .GroupBy(c => c.refundReason, c => new { c.charge.chargeAmount, c.charge.tax?.taxAmount.amount })
                .Select(refundGroup => new { Name = refundGroup.Key, TotalRefund = refundGroup.Sum(group => group.chargeAmount.amount), TotalTaxRefund = refundGroup.Sum(group => group.amount.GetValueOrDefault(0)) });

            foreach (var refund in refunds)
            {
                if (refund.TotalRefund < 0)
                {
                    InstantiateOrderCharge(orderToSave, "Refund", refund.Name.ToString(), refund.TotalRefund);
                }

                if (refund.TotalTaxRefund < 0)
                {
                    InstantiateOrderCharge(orderToSave, "Refunded Tax", refund.Name.ToString(), refund.TotalTaxRefund);
                }
            }
        }

        /// <summary>
        /// Loads the order items.
        /// </summary>
        /// <remarks>
        /// Creates new items or updates existing items. This method assumes that
        /// a line item will not be deleted and that the price will go to 0 if
        /// the order is canceled.
        /// </remarks>
        private void LoadItems(orderLineType[] downloadedOrderOrderLines, WalmartOrderEntity orderToSave)
        {
            foreach (orderLineType orderLine in downloadedOrderOrderLines)
            {
                LoadItem(orderLine, orderToSave);
            }
        }

        /// <summary>
        /// Loads the order item.
        /// </summary>
        /// <remarks>
        /// If the item exists, populate price, quantity, and status. If it does not exist, create it and populate everything.
        /// </remarks>
        private void LoadItem(orderLineType orderLine, WalmartOrderEntity orderToSave)
        {
            WalmartOrderItemEntity item = FindOrCreateOrderItem(orderLine, orderToSave);

            orderLineStatusType orderLineStatus = orderLine.orderLineStatuses.SingleOrDefault();
            item.LocalStatus = orderLineStatus?.status.ToString() ?? "Unknown";

            item.UnitPrice = orderLine.charges.Where(c => c.chargeType1 == "PRODUCT").Sum(c => c.chargeAmount.amount);
            // Walmart spelled "Canceled" wrong, so I added the correct spelling in case they fix it...
            item.Quantity = (item.LocalStatus == "Cancelled" || item.LocalStatus == "Canceled") ? 0 : double.Parse(orderLine.orderLineQuantity.amount);
        }

        /// <summary>
        /// Finds the or create order item.
        /// </summary>
        private static WalmartOrderItemEntity FindOrCreateOrderItem(orderLineType orderLine, WalmartOrderEntity orderToSave)
        {
            WalmartOrderItemEntity item = orderToSave.OrderItems.Cast<WalmartOrderItemEntity>().FirstOrDefault(orderItem => orderItem.LineNumber == orderLine.lineNumber);

            if (item == null)
            {
                item = new WalmartOrderItemEntity(orderToSave)
                {
                    LineNumber = orderLine.lineNumber,
                    Name = WebUtility.HtmlDecode(WebUtility.HtmlDecode(orderLine.item.productName)),
                    SKU = orderLine.item.sku
                };
            }

            return item;
        }

        /// <summary>
        /// Loads the address into the order
        /// </summary>
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

        /// <summary>
        /// Create a new order charge based on the given order, type, description and amount
        /// </summary>
        private void InstantiateOrderCharge(OrderEntity order, string type, string description, decimal amount)
        {
            OrderChargeEntity charge =
                order.OrderCharges.FirstOrDefault(orderCharge => orderCharge.Type == type && orderCharge.Description == description) ??
                    new OrderChargeEntity()
                    {
                        Order = order,
                        Type = type,
                        Description = description
                    };

            charge.Amount = amount;
        }
    }
}
