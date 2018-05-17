using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Autofac;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Dhl;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.OnTrac.Enums;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Stores.Platforms.Overstock;
using ShipWorks.Stores.Platforms.Overstock.OnlineUpdating;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Overstock
{
    public class OverstockShipmentFactoryTest : IDisposable
    {
        private readonly AutoMock mock;

        public OverstockShipmentFactoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Theory]
        [InlineData(ShipmentTypeCode.UpsOnLineTools, "1z8888", (int) UpsServiceType.UpsSurePost1LbOrGreater, "UPSSP", "FIRSTCLASS")]
        [InlineData(ShipmentTypeCode.UpsOnLineTools, "1z8888", (int) UpsServiceType.UpsMailInnovationsFirstClass, "UPSMI", "FIRSTCLASS")]
        [InlineData(ShipmentTypeCode.UpsOnLineTools, "1z8888", (int) UpsServiceType.UpsMailInnovationsPriority, "UPSMI", "FIRSTCLASS")]
        [InlineData(ShipmentTypeCode.UpsOnLineTools, "1z8888", (int) UpsServiceType.UpsGround, "UPS", "GROUND")]
        [InlineData(ShipmentTypeCode.UpsOnLineTools, "1z8888", (int) UpsServiceType.UpsNextDayAir, "UPS", "NEXT_DAY")]
        [InlineData(ShipmentTypeCode.UpsOnLineTools, "1z8888", (int) UpsServiceType.Ups2DayAir, "UPS", "TWO_DAY")]
        [InlineData(ShipmentTypeCode.UpsOnLineTools, "1z8888", (int) UpsServiceType.Ups3DaySelect, "UPS", "THREE_DAY")]
        [InlineData(ShipmentTypeCode.UpsOnLineTools, "1z8888", (int) UpsServiceType.UpsExpress, "UPS", "GROUND")]
        [InlineData(ShipmentTypeCode.UpsWorldShip, "1z8888", (int) UpsServiceType.UpsGround, "UPS", "GROUND")]
        [InlineData(ShipmentTypeCode.FedEx, "9499922", (int) FedExServiceType.FedExGround, "FEDEX", "GROUND")]
        [InlineData(ShipmentTypeCode.FedEx, "9499922", (int) FedExServiceType.FedExExpressSaver, "FEDEX", "GROUND")]
        [InlineData(ShipmentTypeCode.FedEx, "9499922", (int) FedExServiceType.PriorityOvernight, "FEDEX", "NEXT_DAY")]
        [InlineData(ShipmentTypeCode.FedEx, "9499922", (int) FedExServiceType.FedEx2Day, "FEDEX", "TWO_DAY")]
        [InlineData(ShipmentTypeCode.FedEx, "9499922", (int) FedExServiceType.InternationalFirst, "FEDEX", "FIRSTCLASS")]
        [InlineData(ShipmentTypeCode.FedEx, "9499922", (int) FedExServiceType.InternationalPriority, "FEDEX", "PRIORITY")]
        [InlineData(ShipmentTypeCode.Usps, "9499922", (int) PostalServiceType.FirstClass, "USPSFC", "FIRSTCLASS")]
        [InlineData(ShipmentTypeCode.Usps, "9499922", (int) PostalServiceType.PriorityMail, "USPS", "PRIORITY")]
        [InlineData(ShipmentTypeCode.Express1Usps, "9499922", (int) PostalServiceType.FirstClass, "USPSFC", "FIRSTCLASS")]
        [InlineData(ShipmentTypeCode.Express1Usps, "9499922", (int) PostalServiceType.PriorityMail, "USPS", "PRIORITY")]
        [InlineData(ShipmentTypeCode.Endicia, "9499922", (int) PostalServiceType.FirstClass, "USPSFC", "FIRSTCLASS")]
        [InlineData(ShipmentTypeCode.Endicia, "9499922", (int) PostalServiceType.PriorityMail, "USPS", "PRIORITY")]
        [InlineData(ShipmentTypeCode.Express1Endicia, "9499922", (int) PostalServiceType.FirstClass, "USPSFC", "FIRSTCLASS")]
        [InlineData(ShipmentTypeCode.Express1Endicia, "9499922", (int) PostalServiceType.PriorityMail, "USPS", "PRIORITY")]
        [InlineData(ShipmentTypeCode.PostalWebTools, "9499922", (int) PostalServiceType.FirstClass, "USPSFC", "FIRSTCLASS")]
        [InlineData(ShipmentTypeCode.PostalWebTools, "9499922", (int) PostalServiceType.PriorityMail, "USPS", "PRIORITY")]
        [InlineData(ShipmentTypeCode.OnTrac, "9499922", (int) OnTracServiceType.Ground, "ONTRAC", "GROUND")]
        [InlineData(ShipmentTypeCode.DhlExpress, "9499922", (int) DhlExpressServiceType.ExpressEnvelope, "DHL", "GROUND")]
        public void CreateShipmentDetails_ReturnsCorrectXml(ShipmentTypeCode shipmentTypeCode, string trackingNumber, int service, string expectedCarrier, string expectedServiceLevel)
        {
            DateTime shipmentDate = DateTime.Now;
            ShipmentEntity shipment = CreateShipment(shipmentTypeCode, trackingNumber, service, shipmentDate);


            var orderDetails = GetOrderDetails(shipment);

            OverstockShipmentFactory testObject = new OverstockShipmentFactory();
            var result = testObject.CreateShipmentDetails(orderDetails);

            OverstockOrderEntity order = (OverstockOrderEntity) shipment.Order;
            OverstockOrderItemEntity orderItem1 = (OverstockOrderItemEntity) shipment.Order.OrderItems.First();
            OverstockOrderItemEntity orderItem2 = (OverstockOrderItemEntity) shipment.Order.OrderItems.Last();

            XElement supplierShipmentFirst = result.Descendants("supplierShipment").First();
            XElement supplierShipConfFirst = supplierShipmentFirst.Element("supplierShipConfirmation");
            XElement supplierShipmentLast = result.Descendants("supplierShipment").Last();
            XElement supplierShipConfLast = supplierShipmentLast?.Element("supplierShipConfirmation");

            // First Item
            Assert.Equal(order.SalesChannelName, supplierShipmentFirst.Element("salesChannelName")?.Value);
            Assert.Equal(order.OrderNumberComplete, supplierShipmentFirst.Element("salesChannelOrderNumber")?.Value);
            Assert.Equal(orderItem1.SalesChannelLineNumber.ToString(), supplierShipmentFirst.Element("salesChannelLineNumber")?.Value);
            Assert.Equal(order.WarehouseCode, supplierShipmentFirst.Element("warehouse")?.Element("code")?.Value);
            Assert.Equal(orderItem1.Quantity.ToString(), supplierShipConfFirst.Element("quantity").Value);
            Assert.Equal(expectedCarrier, supplierShipConfFirst?.Element("carrier")?.Element("code")?.Value);
            Assert.Equal(shipment.TrackingNumber, supplierShipConfFirst?.Element("trackingNumber")?.Value);
            Assert.Equal(shipmentDate.ToString("o"), supplierShipConfFirst?.Element("shipDate")?.Value);
            Assert.Equal(expectedServiceLevel, supplierShipConfFirst?.Element("serviceLevel")?.Element("code")?.Value);

            // Second Item
            Assert.Equal(order.SalesChannelName, supplierShipmentLast.Element("salesChannelName")?.Value);
            Assert.Equal(order.OrderNumberComplete, supplierShipmentLast.Element("salesChannelOrderNumber")?.Value);
            Assert.Equal(orderItem2.SalesChannelLineNumber.ToString(), supplierShipmentLast.Element("salesChannelLineNumber")?.Value);
            Assert.Equal(order.WarehouseCode, supplierShipmentLast.Element("warehouse")?.Element("code")?.Value);
            Assert.Equal(orderItem2.Quantity.ToString(), supplierShipConfLast.Element("quantity").Value);
            Assert.Equal(expectedCarrier, supplierShipConfLast?.Element("carrier")?.Element("code")?.Value);
            Assert.Equal(shipment.TrackingNumber, supplierShipConfLast?.Element("trackingNumber")?.Value);
            Assert.Equal(shipmentDate.ToString("o"), supplierShipConfLast?.Element("shipDate")?.Value);
            Assert.Equal(expectedServiceLevel, supplierShipConfLast?.Element("serviceLevel")?.Element("code")?.Value);
        }

        private IEnumerable<OverstockSupplierShipment> GetOrderDetails(ShipmentEntity shipment)
        {
            List<OverstockSupplierShipment> orderDetails = new List<OverstockSupplierShipment>();
            OverstockOrderEntity order = (OverstockOrderEntity) shipment.Order;

            foreach (OrderItemEntity orderItemEntity in shipment.Order.OrderItems)
            {
                OverstockOrderItemEntity overstockOrderItem = (OverstockOrderItemEntity) orderItemEntity;

                OverstockOrderDetail orderDetail = new OverstockOrderDetail(order.OrderNumberComplete, order.SalesChannelName, order.WarehouseCode, order.OrderID, false);

                orderDetails.Add(new OverstockSupplierShipment(orderDetail, shipment, overstockOrderItem));
            }

            return orderDetails;
        }

        private static ShipmentEntity CreateShipment(ShipmentTypeCode shipmentTypeCode, string trackingNumber, int service, DateTime shipmentDate)
        {
            ShipmentEntity shipment = new ShipmentEntity()
            {
                ShipmentTypeCode = shipmentTypeCode,
                TrackingNumber = trackingNumber,
                ShipDate = shipmentDate,
                Ups = new UpsShipmentEntity()
                {
                    Service = service,
                },
                FedEx = new FedExShipmentEntity()
                {
                    Service = service
                },
                Postal = new PostalShipmentEntity()
                {
                    Service = service,
                },
                Other = new OtherShipmentEntity()
                {
                    Carrier = "UPS",
                    Service = "Ground"
                },
                OnTrac = new OnTracShipmentEntity(),
                DhlExpress = new DhlExpressShipmentEntity(),
                Order = new OverstockOrderEntity()
                {
                    SalesChannelName = "OSTK",
                    WarehouseCode = "OSTK-WH",
                }
            };

            shipment.Order.ChangeOrderNumber("1-1");

            shipment.Order.OrderItems.Add(new OverstockOrderItemEntity()
                {
                    SalesChannelLineNumber = 1,
                    Quantity = 1,
                }
            );

            shipment.Order.OrderItems.Add(new OverstockOrderItemEntity()
                {
                    SalesChannelLineNumber = 2,
                    Quantity = 2,
                }
            );
            return shipment;
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
