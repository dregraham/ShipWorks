﻿using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using ShipWorks.Data.Import;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Jet;
using ShipWorks.Stores.Platforms.Jet.DTO;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Jet
{
    public class JetOrderLoaderTest
    {
        private readonly AutoMock mock;
        private readonly JetOrderEntity order;
        private readonly JetOrderLoader testObject;

        public JetOrderLoaderTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            order = new JetOrderEntity();
            testObject = mock.Create<JetOrderLoader>();
        }

        [Fact]
        public void LoadOrder_DelegatesToOrderEntityChangeOrderNumber_WhenOrderIsANumber()
        {
            JetOrderDetailsResult orderDto = GetEmptyJetOrderDetailsResult();
            orderDto.ReferenceOrderId = 1234567890;

            testObject.LoadOrder(order, orderDto, new JetStoreEntity());

            Assert.Equal("1234567890", order.OrderNumberComplete);
            Assert.Equal(1234567890, order.OrderNumber);
        }

        [Fact]
        public void LoadOrder_SetsOrderDate()
        {
            JetOrderDetailsResult orderDto = GetEmptyJetOrderDetailsResult();
            orderDto.OrderPlacedDate = new DateTime(1999, 12, 2);

            testObject.LoadOrder(order, orderDto, new JetStoreEntity());

            Assert.Equal(new DateTime(1999, 12, 2), order.OrderDate);
        }

        [Fact]
        public void LoadOrder_SetsOrderOnlineStatusToAcknowledged()
        {
            JetOrderDetailsResult orderDto = GetEmptyJetOrderDetailsResult();

            testObject.LoadOrder(order, orderDto, new JetStoreEntity());

            Assert.Equal("Acknowledged", order.OnlineStatus);
        }

        [Fact]
        public void LoadOrder_SetsOrderRequestedShipping()
        {
            JetOrderDetailsResult orderDto = GetEmptyJetOrderDetailsResult();
            orderDto.OrderDetail.RequestShippingCarrier = "UPS";
            orderDto.OrderDetail.RequestShippingMethod = "Ground";

            testObject.LoadOrder(order, orderDto, new JetStoreEntity());

            Assert.Equal("UPS Ground", order.RequestedShipping);
        }

        [Fact]
        public void LoadOrder_SetsShipToAddress()
        {
            JetOrderDetailsResult orderDto = GetEmptyJetOrderDetailsResult();
            orderDto.ShippingTo.Recipient.Name = "Mirza Mulaosmanovic";
            orderDto.ShippingTo.Recipient.PhoneNumber = "3148215888";
            orderDto.ShippingTo.Address.Address1 = "16204 Bay Harbour Ct";
            orderDto.ShippingTo.Address.Address2 = "suite 2000";
            orderDto.ShippingTo.Address.City = "Wildwood";
            orderDto.ShippingTo.Address.State = "Mo";
            orderDto.ShippingTo.Address.ZipCode = "63040";
            
            testObject.LoadOrder(order, orderDto, new JetStoreEntity());

            Assert.Equal("Mirza", order.ShipFirstName);
            Assert.Equal("Mulaosmanovic", order.ShipLastName);
            Assert.Equal("16204 Bay Harbour Ct", order.ShipStreet1);
            Assert.Equal("suite 2000", order.ShipStreet2);
            Assert.Equal("Wildwood", order.ShipCity);
            Assert.Equal("Mo", order.ShipStateProvCode);
            Assert.Equal("63040", order.ShipPostalCode);
            Assert.Equal("3148215888", order.ShipPhone);
            Assert.Equal("US", order.ShipCountryCode);
        }

        [Fact]
        public void LoadOrder_SetsBillToAddress()
        {
            JetOrderDetailsResult orderDto = GetEmptyJetOrderDetailsResult();
            orderDto.Buyer.Name = "Mirza Mulaosmanovic";
            orderDto.Buyer.PhoneNumber = "3148215888";

            testObject.LoadOrder(order, orderDto, new JetStoreEntity());

            Assert.Equal("Mirza", order.BillFirstName);
            Assert.Equal("Mulaosmanovic", order.BillLastName);
            Assert.Equal("3148215888", order.BillPhone);
        }

        [Fact]
        public void LoadOrder_DelegatesToItemLoader()
        {
            JetOrderDetailsResult orderDto = GetEmptyJetOrderDetailsResult();
            JetStoreEntity store = new JetStoreEntity();

            testObject.LoadOrder(order, orderDto, store);

            mock.Mock<IJetOrderItemLoader>().Verify(l => l.LoadItems(order, orderDto, store));
        }

        [Fact]
        public void LoadOrder_DelegatesToChargeCalculator()
        {
            JetOrderDetailsResult orderDto = GetEmptyJetOrderDetailsResult();

            testObject.LoadOrder(order, orderDto, new JetStoreEntity());

            mock.Mock<IOrderChargeCalculator>().Verify(l => l.CalculateTotal(order));
        }

        [Fact]
        public void LoadOrder_DelegatesToOrderElementFactoryToCreateShippingCharge()
        {
            JetOrderDetailsResult orderDto = GetEmptyJetOrderDetailsResult();
            orderDto.OrderTotals.ItemPrice.ItemShippingTax = 12.23M;
            orderDto.OrderTotals.ItemPrice.ItemShippingTax = 22.56M;

            testObject.LoadOrder(order, orderDto, new JetStoreEntity());
            
            mock.Mock<IOrderElementFactory>().Setup(f => f.CreateCharge(order, "TAX", "Tax", 34.79M));
        }

        [Fact]
        public void LoadOrder_DelegatesToOrderElementFactoryToCreateTaxCharge()
        {
            JetOrderDetailsResult orderDto = GetEmptyJetOrderDetailsResult();
            orderDto.OrderTotals.ItemPrice.ItemShippingCost = 12.23M;

            testObject.LoadOrder(order, orderDto, new JetStoreEntity());

            mock.Mock<IOrderElementFactory>().Setup(f => f.CreateCharge(order, "SHIPPING", "Shipping", 12.23M));
        }

        private JetOrderDetailsResult GetEmptyJetOrderDetailsResult()
        {
            return new JetOrderDetailsResult()
            {
                OrderDetail = new JetOrderDetail(),
                ShippingTo = new JetShippingTo()
                {
                    Address = new JetAddress(),
                    Recipient = new JetRecipient()
                },
                Buyer = new JetBuyer(),
                OrderTotals = new JetOrderTotals()
                {
                    ItemPrice = new JetOrderItemPrice(),
                    FeeAdjustments = new List<JetFeeAdjustment>()
                }
            };
        }
    }
}