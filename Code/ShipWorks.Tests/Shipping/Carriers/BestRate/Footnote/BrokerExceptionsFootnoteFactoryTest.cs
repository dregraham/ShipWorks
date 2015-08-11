using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.BestRate.Footnote;
using ShipWorks.Shipping.Carriers.OnTrac;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Tests.Shipping.Carriers.BestRate.Footnote
{
    public class BrokerExceptionsFootnoteFactoryTest
    {
        private BrokerExceptionsRateFootnoteFactory testObject;

        [Fact]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Constructor_ThrowsInvalidOperationException_WhenCollectionOfBrokerExceptionsIsEmpty_Test()
        {
            testObject = new BrokerExceptionsRateFootnoteFactory(new BestRateShipmentType(), new List<BrokerException>());
        }

        [Fact]
        public void CreateFootnote_ReturnsBrokerExceptionsRateFootnoteControl_Test()
        {
            List<BrokerException> brokerExceptions = new List<BrokerException>
            {
                new BrokerException(new ShippingException("broker exception 1"), BrokerExceptionSeverityLevel.Information, new OnTracShipmentType()),
                new BrokerException(new ShippingException("broker exception 2"), BrokerExceptionSeverityLevel.Warning, new OnTracShipmentType()),
                new BrokerException(new ShippingException("broker exception 3"), BrokerExceptionSeverityLevel.Error, new OnTracShipmentType())
            };

            testObject = new BrokerExceptionsRateFootnoteFactory(new BestRateShipmentType(), brokerExceptions);

            RateFootnoteControl footnote = testObject.CreateFootnote(null);

            Assert.IsInstanceOfType(footnote, typeof(BrokerExceptionsRateFootnoteControl));
        }

        [Fact]
        public void CreateFootnote_ReturnsBrokerExceptionsRateFootnoteControl_WithBrokerExceptions_Test()
        {
            List<BrokerException> brokerExceptions = new List<BrokerException>
            {
                new BrokerException(new ShippingException("broker exception 1"), BrokerExceptionSeverityLevel.Information, new OnTracShipmentType()),
                new BrokerException(new ShippingException("broker exception 2"), BrokerExceptionSeverityLevel.Warning, new OnTracShipmentType()),
                new BrokerException(new ShippingException("broker exception 3"), BrokerExceptionSeverityLevel.Error, new OnTracShipmentType())
            };
                
            testObject = new BrokerExceptionsRateFootnoteFactory(new BestRateShipmentType(), brokerExceptions);

            BrokerExceptionsRateFootnoteControl footnote = testObject.CreateFootnote(null) as BrokerExceptionsRateFootnoteControl;

            Assert.AreEqual(3, footnote.BrokerExceptions.Count());
        }
    }
}
