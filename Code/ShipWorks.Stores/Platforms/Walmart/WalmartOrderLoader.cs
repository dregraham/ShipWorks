using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
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
            try
            {
                if (orderToSave.IsNew)
                {
                    long pruchaseOrderId;
                    if (!long.TryParse(downloadedOrder.purchaseOrderId, out pruchaseOrderId))
                    {
                        throw new WalmartException($"PurchaseOrderId '{downloadedOrder.purchaseOrderId}' could not be converted to a number");
                    }
                    orderToSave.OrderNumber = pruchaseOrderId;
                    // orderToSave.PurchaseOrderId is set via the WalmartOrderIdentifier, no need to do it here.
                    orderToSave.CustomerOrderID = downloadedOrder.customerOrderId;
                    orderToSave.OrderDate = downloadedOrder.orderDate;

                    orderToSave.EstimatedDeliveryDate = downloadedOrder.shippingInfo.estimatedDeliveryDate.Year > 1754
                        ? downloadedOrder.shippingInfo.estimatedDeliveryDate
                        : downloadedOrder.orderDate;

                    orderToSave.EstimatedShipDate = downloadedOrder.shippingInfo.estimatedShipDate.Year > 1754
                        ? downloadedOrder.shippingInfo.estimatedShipDate
                        : downloadedOrder.orderDate;

                    orderToSave.RequestedShipping = downloadedOrder.shippingInfo.methodCode.ToString();
                    orderToSave.RequestedShippingMethodCode = orderToSave.RequestedShipping;

                    LoadAddress(downloadedOrder, orderToSave);
                }
                else
                {
                    // Load existing charges and other detail from the database
                    orderRepository.PopulateOrderDetails(orderToSave);
                    ClearExistingCharges(orderToSave);
                }

                IEnumerable<orderLineType> orderLines = downloadedOrder.orderLines ?? Enumerable.Empty<orderLineType>();

                LoadItems(orderLines, orderToSave);
                LoadOtherCharges(orderLines, orderToSave);
                LoadTax(orderLines, orderToSave);
                LoadRefunds(orderLines, orderToSave);

                orderToSave.OrderTotal = orderChargeCalculator.CalculateTotal(orderToSave);
            }
            catch (Exception ex)
            {
                // yes catching a general exception here is heavy handed however we have had
                // 3 instances where the Walmart api has gone down or given us bad data which
                // causes issues when loading orders
                throw new WalmartException($"Error occured while loading Walmart order: {ex.Message}");
            }
        }

        /// <summary>
        /// Clears the existing charges.
        /// </summary>
        private void ClearExistingCharges(WalmartOrderEntity order)
        {
            foreach (OrderChargeEntity charge in order.OrderCharges)
            {
                charge.Amount = 0;
            }
        }

        /// <summary>
        /// Loads the other charges.
        /// </summary>
        private void LoadOtherCharges(IEnumerable<orderLineType> downloadedOrderOrderLines, WalmartOrderEntity orderToSave)
        {
            // Get all the non-zero charges where the type is not "PRODUCT" and create an
            // order charge for each charge type.
            downloadedOrderOrderLines.SelectMany(orderLine => orderLine.charges)
                .Where(charge => (charge?.chargeAmount?.amount ?? 0) != 0 && charge?.chargeType1 != "PRODUCT")
                .GroupBy(charge => new { charge.chargeType1, charge.chargeName }, charge => charge.chargeAmount.amount)
                .ForEach(group => InstantiateOrderCharge(orderToSave, group.Key.chargeType1, group.Key.chargeName, group.Sum()));
        }

        /// <summary>
        /// Loads the tax.
        /// </summary>
        private void LoadTax(IEnumerable<orderLineType> orderLines, WalmartOrderEntity orderToSave)
        {
            // Get all the charges with tax and create an order charge for the total tax of each taxName.
            orderLines.SelectMany(orderLine => orderLine.charges)
                .Where(charge => (charge?.tax?.taxAmount?.amount ?? 0) != 0)
                .GroupBy(c => c.tax.taxName, c => c.tax.taxAmount.amount)
                .ForEach(taxGroup => InstantiateOrderCharge(orderToSave, "Tax", taxGroup.Key, taxGroup.Sum()));
        }

        /// <summary>
        /// Loads the refunds.
        /// </summary>
        private void LoadRefunds(IEnumerable<orderLineType> orderLines, WalmartOrderEntity orderToSave)
        {
            // Get all refunds, group by reason.
            // Create order charges for the total chargeAmount and tax for each refund reason.
            orderLines.Select(orderLine => orderLine.refund)
                .Where(refund => refund != null)
                .SelectMany(refund => refund.refundCharges)
                .GroupBy(c => c.refundReason, c => new { c.charge.chargeAmount, c.charge.tax?.taxAmount.amount })
                .ForEach(refundGroup =>
                {
                    string name = refundGroup.Key.ToString();
                    decimal totalRefund = refundGroup.Sum(group => group.chargeAmount.amount);
                    decimal totalTaxRefund = refundGroup.Sum(group => group.amount.GetValueOrDefault(0));

                    if (totalRefund < 0)
                    {
                        InstantiateOrderCharge(orderToSave, "Refund", name.ToString(), totalRefund);
                    }

                    if (totalTaxRefund < 0)
                    {
                        InstantiateOrderCharge(orderToSave, "Refunded Tax", name.ToString(), totalTaxRefund);
                    }
                });
        }

        /// <summary>
        /// Loads the order items.
        /// </summary>
        /// <remarks>
        /// Creates new items or updates existing items. This method assumes that
        /// a line item will not be deleted and that the price will go to 0 if
        /// the order is canceled.
        /// </remarks>
        public void LoadItems(IEnumerable<orderLineType> downloadedOrderOrderLines, WalmartOrderEntity orderToSave)
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
            item.OnlineStatus = orderLineStatus?.status.ToString() ?? "Unknown";

            item.UnitPrice = orderLine.charges
                .Where(c => c?.chargeType1 == "PRODUCT")
                .Sum(c => c.chargeAmount?.amount ?? 0);

            // Walmart spelled "Canceled" wrong, so I added the correct spelling in case they fix it...
            item.Quantity = (item.OnlineStatus == orderLineStatusValueType.Cancelled.ToString())
                ? 0
                : double.Parse(orderLine.orderLineQuantity.amount);
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
            orderToSave.BillStateProvCode = Geography.GetStateProvCode(downloadedAddress.state);
            orderToSave.BillPostalCode = downloadedAddress.postalCode;
            orderToSave.BillCountryCode = Geography.GetCountryCode(downloadedAddress.country);

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
