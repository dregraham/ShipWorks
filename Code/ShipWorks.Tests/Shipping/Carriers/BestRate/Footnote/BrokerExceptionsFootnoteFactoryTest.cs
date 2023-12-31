﻿using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.BestRate.Footnote;
using ShipWorks.Shipping.Carriers.OnTrac;
using ShipWorks.Shipping.Editing.Rating;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.BestRate.Footnote
{
    public class BrokerExceptionsFootnoteFactoryTest
    {
        private BrokerExceptionsRateFootnoteFactory testObject;

        [Fact]
        public void Constructor_ThrowsInvalidOperationException_WhenCollectionOfBrokerExceptionsIsEmpty()
        {
            Assert.Throws<InvalidOperationException>(() => new BrokerExceptionsRateFootnoteFactory(ShipmentTypeCode.Other, new List<BrokerException>()));
        }

        [Fact]
        public void CreateFootnote_ReturnsBrokerExceptionsRateFootnoteControl()
        {
            List<BrokerException> brokerExceptions = new List<BrokerException>
            {
                new BrokerException(new ShippingException("broker exception 1"), BrokerExceptionSeverityLevel.Information, new OnTracShipmentType()),
                new BrokerException(new ShippingException("broker exception 2"), BrokerExceptionSeverityLevel.Warning, new OnTracShipmentType()),
                new BrokerException(new ShippingException("broker exception 3"), BrokerExceptionSeverityLevel.Error, new OnTracShipmentType())
            };

            testObject = new BrokerExceptionsRateFootnoteFactory(ShipmentTypeCode.Other, brokerExceptions);

            RateFootnoteControl footnote = testObject.CreateFootnote(null);

            Assert.IsAssignableFrom<BrokerExceptionsRateFootnoteControl>(footnote);
        }

        [Fact]
        public void CreateFootnote_ReturnsBrokerExceptionsRateFootnoteControl_WithBrokerExceptions()
        {
            List<BrokerException> brokerExceptions = new List<BrokerException>
            {
                new BrokerException(new ShippingException("broker exception 1"), BrokerExceptionSeverityLevel.Information, new OnTracShipmentType()),
                new BrokerException(new ShippingException("broker exception 2"), BrokerExceptionSeverityLevel.Warning, new OnTracShipmentType()),
                new BrokerException(new ShippingException("broker exception 3"), BrokerExceptionSeverityLevel.Error, new OnTracShipmentType())
            };

            testObject = new BrokerExceptionsRateFootnoteFactory(ShipmentTypeCode.Other, brokerExceptions);

            BrokerExceptionsRateFootnoteControl footnote = testObject.CreateFootnote(null) as BrokerExceptionsRateFootnoteControl;

            Assert.Equal(3, footnote.BrokerExceptions.Count());
        }
    }
}
